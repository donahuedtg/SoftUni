namespace DataWork.Services.Administrator.Implementations
{
    using AutoMapper.QueryableExtensions;
    using Data;
    using Microsoft.EntityFrameworkCore;
    using Services.Administrator.Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class AdminService : IAdminService
    {
        private readonly DataWorkDbContext db;

        public AdminService(DataWorkDbContext db)
        {
            this.db = db;
        }

        public async Task<IEnumerable<AdminUserListServiceModel>> AllUser()
        {
            return await this.db
                .Users
                .ProjectTo<AdminUserListServiceModel>()
                .ToListAsync();

        }
    }
}
