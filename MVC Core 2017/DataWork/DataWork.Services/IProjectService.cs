namespace DataWork.Services
{
    using Services.Models.Project;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IProjectService
    {
        Task<IEnumerable<ProjectNameServiceModel>> GetAllForUser(string userId);

        Task<int> Create(string userId, string projectName);

        Task<bool> IsExist(string userId, string projectName);

        Task<bool> IsExist(string userId, int projectNameId);
    }
}
