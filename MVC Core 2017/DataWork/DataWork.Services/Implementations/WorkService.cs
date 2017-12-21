namespace DataWork.Services.Implementations
{
    using AutoMapper.QueryableExtensions;
    using Data;
    using Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Services.Models.Work;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System;

    public class WorkService : IWorkService
    {
        private readonly DataWorkDbContext db;

        public WorkService(DataWorkDbContext db)
        {
            this.db = db;
        }


        public async Task<IEnumerable<ListWorkServiceModel>> GetAllForUser(string userId, int year)
        {
            return await this.db
                .Works
                .Where(x => x.UserId == userId && x.WorkDate.Year == year)
                .ProjectTo<ListWorkServiceModel>()
                .OrderByDescending(m => m.Id)
                .ThenBy(m => m.WorkDate)
                .ToListAsync();
        }

        public async Task Create(string userId, string description, DateTime workDate, int projectNameId)
        {
            Work work = new Work
            {
                UserId = userId,
                Description = description,
                WorkDate = workDate,
                ProjectNameId = projectNameId,
                TimeStamp = DateTime.UtcNow
            };

            this.db.Works.Add(work);
            await this.db.SaveChangesAsync();
        }

        public async Task<bool> IsExist(string userId, int id)
        {
            return await this.db
                .Works
                .AnyAsync(x => x.UserId == userId && x.Id == id);
        }

        public async Task<EditWorkServiceModel> FindById(int id)
        {
            return await this.db
                .Works
                .Where(x => x.Id == id)
                .ProjectTo<EditWorkServiceModel>()
                .FirstOrDefaultAsync();
        }

        public async Task Edit(string userId, int id, string description, DateTime workDate, int projectNameId)
        {
            Work work = await this.db
                .Works
                .Where(x => x.UserId == userId && x.Id == id)
                .FirstOrDefaultAsync();

            work.Description = description;
            work.ProjectNameId = projectNameId;
            work.WorkDate = workDate;
            work.TimeStamp = DateTime.UtcNow;

            await this.db.SaveChangesAsync();
        }
    }
}
