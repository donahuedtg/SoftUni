namespace DataWork.Web.Areas.Administrator.Controllers
{
    using Data.Models;
    using ViewModels;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.EntityFrameworkCore;
    using Services;
    using Services.Administrator;
    using Services.Administrator.Models;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using BindModels;
    using Web.Controllers;
    using System;

    public class AdminController : BaseController
    {
        
        private readonly IAdminService adminService;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<User> userManager;
        private readonly ILeaveService leaveService;

        public AdminController(IAdminService adminService, RoleManager<IdentityRole> roleManager, UserManager<User> userManager, ILeaveService leaveService)
        {
            this.adminService = adminService;
            this.roleManager = roleManager;
            this.userManager = userManager;
            this.leaveService = leaveService;
        }

        public async Task<IActionResult> Index()
        {
            if (!User.Identity.IsAuthenticated || !User.IsInRole(GlobalConstString.AdministratorRole))
            {
                return RedirectToActionCustom(nameof(AccountController.Login), nameof(AccountController));
            }

            try
            {
                AdminNewUserViewModel model = new AdminNewUserViewModel
                {
                    Count = (await this.adminService.AllUser()).Count(x => x.IsSetLeaveDaysForCurrYear == false)
                };

                return View(model);
            }
            catch
            {
                this.AddErrorMessage(GlobalConstString.LoadError);
                return RedirectToActionCustom(nameof(HomeController.Error), nameof(HomeController));
            }
        }

        public async Task<IActionResult> List()
        {
            if (!User.Identity.IsAuthenticated || !User.IsInRole(GlobalConstString.AdministratorRole))
            {
                return RedirectToActionCustom(nameof(AccountController.Login), nameof(AccountController));
            }

            try
            {
                IEnumerable<AdminUserListServiceModel> userList = await this.adminService.AllUser();

                IEnumerable<SelectListItem> rolesList = await this.roleManager
                    .Roles
                    .Select(x => new SelectListItem
                    {
                        Value = x.Name,
                        Text = x.Name
                    })
                    .ToListAsync();

                AdminUsersListViewModel model = new AdminUsersListViewModel
                {
                    UserList = userList,
                    RolesList = rolesList
                };


                return View(model);
            }
            catch
            {
                this.AddErrorMessage(GlobalConstString.LoadError);
                return RedirectToActionCustom(nameof(AdminController.Index), nameof(AdminController));
            }
        }

        [HttpPost]
        public async Task<IActionResult> SetRole(AdminAddUserToRoleBindModel bind)
        {
            if (!User.Identity.IsAuthenticated || !User.IsInRole(GlobalConstString.AdministratorRole))
            {
                return RedirectToActionCustom(nameof(AccountController.Login), nameof(AccountController));
            }

            try
            {
                bool isRoleExist = await this.roleManager.RoleExistsAsync(bind.RoleName);
                User user = await this.userManager.FindByIdAsync(bind.UserId);
                bool isUserExist = user != null;

                if (!isRoleExist || !isUserExist)
                {
                    this.AddErrorMessage("User or Role not exist");
                    return RedirectToAction(nameof(List));
                }

                if (!ModelState.IsValid)
                {
                    this.AddErrorMessage("User or Role not exist");
                    return RedirectToAction(nameof(List));
                }

                await this.userManager.AddToRoleAsync(user, bind.RoleName);

                this.AddSuccessMessage($"User {user.UserName} added to the role {bind.RoleName}");
                return RedirectToAction(nameof(List));
            }
            catch
            {
                this.AddErrorMessage(GlobalConstString.LoadError);
                return RedirectToAction(nameof(List));
            }

        }

        [HttpPost]
        public async Task<IActionResult> SetLeaveDays(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                this.AddErrorMessage(GlobalConstString.NotFound);
                return RedirectToAction(nameof(List));
            }

            if (!User.Identity.IsAuthenticated || !User.IsInRole(GlobalConstString.AdministratorRole))
            {
                return RedirectToActionCustom(nameof(AccountController.Login), nameof(AccountController));
            }

            try
            {
                await this.leaveService.SetLeaveDaysForCurrYear(userId);

                this.AddSuccessMessage($"{string.Format(GlobalConstString.AddDaysSuccess, DateTime.UtcNow.Year)}");
                return RedirectToAction(nameof(List));
            }
            catch
            {
                this.AddErrorMessage(GlobalConstString.AddDaysError);
                return RedirectToAction(nameof(List));
            }
        }

        [HttpPost]
        public async Task<IActionResult> SetLeaveDaysForAll()
        {
            if (!User.Identity.IsAuthenticated || !User.IsInRole(GlobalConstString.AdministratorRole))
            {
                return RedirectToActionCustom(nameof(AccountController.Login), nameof(AccountController));
            }

            try
            {
                bool isUserChanged = await this.leaveService.SetLeaveDaysForAll();

                if (!isUserChanged)
                {
                    this.AddWarningMessage(GlobalConstString.AddDaysToAllNoUserFound);
                }
                else
                {
                    this.AddSuccessMessage($"{string.Format(GlobalConstString.AddDaysToAll, DateTime.UtcNow.Year)}");
                }
                
                return RedirectToAction(nameof(List));
            }
            catch
            {
                this.AddErrorMessage(GlobalConstString.AddDaysError);
                return RedirectToAction(nameof(List));
            }
        }
    }
}
