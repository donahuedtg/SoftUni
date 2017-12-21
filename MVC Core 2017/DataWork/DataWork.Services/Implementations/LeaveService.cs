namespace DataWork.Services.Implementations
{
    using AutoMapper.QueryableExtensions;
    using Data;
    using Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Services.Models.Leave;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class LeaveService : ILeaveService
    {
        private readonly int currYear;
        private readonly DataWorkDbContext db;

        public LeaveService(DataWorkDbContext db)
        {
            this.db = db;
            this.currYear = DateTime.UtcNow.Year;
        }

        public async Task<IEnumerable<ListLeaveServiceModel>> GetAllForYear(string userId, int year)
        {
            return await this.db
                .Leaves
                .Where(x => x.UserId == userId && x.CurrentYear == year)
                .ProjectTo<ListLeaveServiceModel>()
                .OrderByDescending(m => m.Id)
                .ToListAsync();
        }

        public async Task<IEnumerable<ListLeaveServiceModel>> GetAllForApprove()
        {
            return await this.db
                .Leaves
                .Where(x => x.CurrentYear == this.currYear && x.LeaveStatus == LeaveStatus.Send)
                .ProjectTo<ListLeaveServiceModel>()
                .OrderByDescending(m => m.Id)
                .ToListAsync();
        }

        public async Task<LeaveDayDetailsServiceModel> GetLeaveDayDetailsForYear(string userId, int year)
        {
            return await this.db
                .LeaveDays
                .Where(x => x.UserId == userId && x.CurrentYear == year)
                .ProjectTo<LeaveDayDetailsServiceModel>()
                .FirstOrDefaultAsync();
        }

        public async Task<int> RemainingDaysForYear(string userId)
        {
            return await this.db
                .LeaveDays
                .Where(x => x.UserId == userId && x.CurrentYear == this.currYear)
                .Select(x => x.RemainLeaveDay)
                .FirstOrDefaultAsync();
        }

        public async Task Create(string userId, LeaveType leaveTypeId, DateTime startDate, DateTime endDate, int totalLeaveDays)
        {
            Leave leave = new Leave
            {
                UserId = userId,
                LeaveType = leaveTypeId,
                StartDate = startDate,
                EndDate = endDate,
                TotalLeaveDays = totalLeaveDays,
                TimeStamp = DateTime.UtcNow,
                LeaveStatus = LeaveStatus.Create,
                CurrentYear = this.currYear
            };

            this.db.Leaves.Add(leave);

            if (leaveTypeId == LeaveType.Paid)
            {
                LeaveDay leaveDay = await this.GetLeaveDayData(userId);

                if (leaveDay == null)
                {
                    throw new ArgumentNullException("User not leave day for current year");
                }

                int remainingDays = leaveDay.RemainLeaveDay - totalLeaveDays;

                if (remainingDays < 0)
                {
                    throw new ArgumentException("User not enough days remaining");
                }

                leaveDay.RemainLeaveDay = remainingDays;
            }

            await this.db.SaveChangesAsync();

        }

        public async Task Edit(string userId, int id, LeaveType leaveTypeId, DateTime startDate, DateTime endDate, int totalLeaveDays)
        {
            int oldTotalLeaveDays = 0;

            Leave leave = await this.GetLeaveData(userId, id);

            LeaveType oldLeaveType = leave.LeaveType;
            leave.LeaveType = leaveTypeId;
            leave.StartDate = startDate;
            leave.EndDate = endDate;

            if (leave.TotalLeaveDays != totalLeaveDays)
            {
                oldTotalLeaveDays = leave.TotalLeaveDays;
                leave.TotalLeaveDays = totalLeaveDays;
            }
            
            leave.CurrentYear = this.currYear;
            leave.TimeStamp = DateTime.UtcNow;

            LeaveDay leaveDay = await this.GetLeaveDayData(userId);

            if (leaveDay == null)
            {
                throw new ArgumentNullException("User not leave day for current year");
            }

            //if old is paid
            if (oldLeaveType == LeaveType.Paid)
            {
                //if new is paid
                if (leaveTypeId == LeaveType.Paid)
                {
                    leaveDay.RemainLeaveDay = leaveDay.RemainLeaveDay + oldTotalLeaveDays;

                    int remainingDays = leaveDay.RemainLeaveDay - totalLeaveDays;

                    if (remainingDays < 0)
                    {
                        throw new ArgumentException("User not enough days remaining");
                    }

                    leaveDay.RemainLeaveDay = remainingDays;
                }
                else
                {
                    leaveDay.RemainLeaveDay = leaveDay.RemainLeaveDay + oldTotalLeaveDays;
                }
            }
            else //old is unpaid
            {
                //if new is paid
                if (leaveTypeId == LeaveType.Paid)
                {
                    int remainingDays = leaveDay.RemainLeaveDay - totalLeaveDays;

                    if (remainingDays < 0)
                    {
                        throw new ArgumentException("User not enough days remaining");
                    }

                    leaveDay.RemainLeaveDay = remainingDays;
                }

            }

            await this.db.SaveChangesAsync();


        }

        public async Task Delete(string userId, int id)
        {
            Leave leave = await this.GetLeaveData(userId, id);

            if (leave == null)
            {
                throw new ArgumentNullException("Record not found");
            }

            LeaveType leaveType = leave.LeaveType;

            if (leaveType == LeaveType.Paid)
            {
                int oldTotalLeaveDays = leave.TotalLeaveDays;
                LeaveDay leaveDay = await this.GetLeaveDayData(userId);

                if (leaveDay == null)
                {
                    throw new ArgumentNullException("User not leave day for current year");
                }

                int days = leaveDay.RemainLeaveDay + oldTotalLeaveDays;

                leaveDay.RemainLeaveDay = days;
            }

            this.db.Leaves.Remove(leave);
            await this.db.SaveChangesAsync();

        }

        public async Task Change(string userId, int id, LeaveStatus leaveStatus)
        {
            Leave leave = await this.GetLeaveData(userId, id);
            if (leave == null)
            {
                throw new ArgumentNullException("Record not found");
            }

            leave.LeaveStatus = leaveStatus;
            leave.TimeStamp = DateTime.UtcNow;

            if (leaveStatus == LeaveStatus.Denied && leave.LeaveType == LeaveType.Paid)
            {
                int currTotalLeaveDays = leave.TotalLeaveDays;
                LeaveDay leaveDay = await this.GetLeaveDayData(userId);

                int returnDays = leaveDay.RemainLeaveDay + currTotalLeaveDays;
                leaveDay.RemainLeaveDay = returnDays;
                leaveDay.TimeStamp = DateTime.UtcNow;
            }

            await this.db.SaveChangesAsync();
        }

        public async Task<bool> IsExist(int id, LeaveStatus leaveStatus)
        {
            return await this.db
                .Leaves
                .AnyAsync(x => x.Id == id && x.LeaveStatus == leaveStatus);
        }

        public async Task<bool> IsExist(string userId, int id, LeaveStatus leaveStatus)
        {
            return await this.db
                .Leaves
                .AnyAsync(x => x.UserId == userId && x.Id == id && x.LeaveStatus == leaveStatus);
        }

        public async Task<EditLeaveServiceModel> GetById(string userId, int id)
        {
            return await this.db
                .Leaves
                .Where(x => x.UserId == userId && x.Id == id)
                .ProjectTo<EditLeaveServiceModel>()
                .FirstOrDefaultAsync();
        }

        public async Task<string> GetUserIdById(int id)
        {
            return await this.db
                .Leaves
                .Where(x => x.Id == id)
                .Select(x => x.UserId)
                .FirstOrDefaultAsync();
        }

        public async Task<int> GetLeaveCountForApprove()
        {
            return await this.db
                .Leaves
                .Where(x => x.CurrentYear == this.currYear && x.LeaveStatus == LeaveStatus.Send)
                .CountAsync();
        }

        public async Task<IEnumerable<int>> GetYearDropDown()
        {
            return await this.db
                .LeaveDays
                .Select(x => x.CurrentYear)
                .Distinct()
                .ToListAsync();
        }

        public async Task SetLeaveDaysForCurrYear(string userId)
        {
            LeaveDay leaveDay = this.CreateLeaveDayData(userId);

            this.db.LeaveDays.Add(leaveDay);

            await this.db.SaveChangesAsync();
        }


        public async Task<bool> SetLeaveDaysForAll()
        {
            bool isChanged = false;
            IEnumerable<string> userListId = await this.db
                .Users
                .Select(x => x.Id)
                .ToListAsync();

            foreach (var userId in userListId)
            {
                bool isExistLeaveDay = await this.db
                    .LeaveDays
                    .AnyAsync(x => x.UserId == userId && x.CurrentYear == this.currYear);

                    
                if (!isExistLeaveDay)
                {
                    isChanged = true;

                    LeaveDay leaveDay = this.CreateLeaveDayData(userId);

                    this.db.LeaveDays.Add(leaveDay);
                }
            }

            if (isChanged)
            {
                await this.db.SaveChangesAsync();
                return true;
            }

            return false;
            
        }

        private async Task<Leave> GetLeaveData(string userId, int id)
        {
            return await this.db
                .Leaves
                .Where(x => x.UserId == userId && x.Id == id)
                .FirstOrDefaultAsync();
        }

        private async Task<LeaveDay> GetLeaveDayData(string userId)
        {
            return await this.db
                .LeaveDays
                .Where(x => x.UserId == userId && x.CurrentYear == this.currYear)
                .FirstOrDefaultAsync();

        }

        private LeaveDay CreateLeaveDayData(string userId)
        {
            return new LeaveDay
            {
                CurrentYear = DateTime.UtcNow.Year,
                UserId = userId,
                LeaveDayForYear = 20,
                RemainLeaveDay = 20,
                TimeStamp = DateTime.UtcNow,
            };
        }


    }
}
