namespace DataWork.Services.Implementations
{
    using AutoMapper.QueryableExtensions;
    using Data;
    using Data.Models;
    using Services.Models.Project;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Linq;
    using Microsoft.EntityFrameworkCore;

    public class ProjectService : IProjectService
    {
        private readonly DataWorkDbContext db;

        public ProjectService(DataWorkDbContext db)
        {
            this.db = db;
        }

        public async Task<IEnumerable<ProjectNameServiceModel>> GetAllForUser(string userId)
        {
            return await this.db
                .ProjectNames
                .Where(x => x.UserId == userId)
                .ProjectTo<ProjectNameServiceModel>()
                .OrderBy(m => m.Id)
                .ToListAsync();
        }

        public async Task<int> Create(string userId, string projectName)
        {
            try
            {
                ProjectName project = new ProjectName
                {
                    Title = projectName,
                    UserId = userId
                };

                this.db.ProjectNames.Add(project);
                await this.db.SaveChangesAsync();

                return 1;
            }
            catch
            {
                return 0;
            }
        }

        public async Task<bool> IsExist(string userId, string projectName)
        {
            return await this.db
                .ProjectNames
                .AnyAsync(x => x.UserId == userId && x.Title.ToLower().Trim() == projectName.ToLower().Trim());
        }

        public async Task<bool> IsExist(string userId, int projectNameId)
        {
            return await this.db
                .ProjectNames
                .AnyAsync(x => x.UserId == userId && x.Id == projectNameId);
        }
    }
}
