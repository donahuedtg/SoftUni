namespace DataWork.Services
{
    using Data.Models;
    using Services.Models.Leave;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ILeaveService
    {
        Task<IEnumerable<ListLeaveServiceModel>> GetAllForYear(string userId, int year);

        Task<IEnumerable<ListLeaveServiceModel>> GetAllForApprove();

        Task<LeaveDayDetailsServiceModel> GetLeaveDayDetailsForYear(string userId, int year);

        Task<int> RemainingDaysForYear(string userId);

        Task Create(string userId, LeaveType leaveTypeId, DateTime startDate, DateTime endDate, int totalLeaveDays);

        Task Edit(string userId, int id, LeaveType leaveTypeId, DateTime startDate, DateTime endDate, int totalLeaveDays);

        Task Delete(string userId, int id);

        Task Change(string userId, int id, LeaveStatus leaveStatus);

        Task<bool> IsExist(int id, LeaveStatus leaveStatus);

        Task<bool> IsExist(string userId, int id, LeaveStatus leaveStatus);

        Task<EditLeaveServiceModel> GetById(string userId, int id);

        Task<string> GetUserIdById(int id);

        Task<int> GetLeaveCountForApprove();

        Task<IEnumerable<int>> GetYearDropDown();

        Task SetLeaveDaysForCurrYear(string userId);

        Task<bool> SetLeaveDaysForAll();
    }
}
