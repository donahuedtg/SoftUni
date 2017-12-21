namespace DataWork.Web.Areas.Administrator.ViewModels
{
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Services.Administrator.Models;
    using System.Collections.Generic;

    public class AdminUsersListViewModel
    {
        public IEnumerable<AdminUserListServiceModel> UserList { get; set; }

        public string RoleName { get; set; }

        public IEnumerable<SelectListItem> RolesList { get; set; }

    }
}
