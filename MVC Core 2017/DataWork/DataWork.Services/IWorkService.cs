namespace DataWork.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using DataWork.Services.Models.Work;

    public interface IWorkService
    {
        Task<IEnumerable<ListWorkServiceModel>> GetAllForUser(string userId, int year);

        Task Create(string userId, string description, DateTime workDate, int projectNameId);

        Task<bool> IsExist(string userId, int id);

        Task<EditWorkServiceModel> FindById(int id);

        Task Edit(string userId, int id, string description, DateTime workDate, int projectNameId);
    }
}
