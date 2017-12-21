namespace DataWork.Services.Administrator
{
    using Services.Administrator.Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IAdminService
    {
        Task<IEnumerable<AdminUserListServiceModel>> AllUser();

    }
}
