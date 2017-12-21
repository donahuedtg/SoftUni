namespace DataWork.Web.Controllers
{
    using BindModels.Works;
    using Web.ViewModels.Works;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Threading.Tasks;
    using Services;
    using Microsoft.AspNetCore.Identity;
    using Data.Models;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using System.Collections.Generic;
    using System.Linq;
    using Services.Models.Work;
    using Web.BindModels;

    [Authorize(Roles = GlobalConstString.WorkerRole)]
    public class WorksController : MainController
    {
        private readonly ILeaveService leaveService;
        private readonly IWorkService workService;
        private readonly IProjectService projectService;
        private readonly UserManager<User> userManager;

        public WorksController(ILeaveService leaveService, IWorkService workService, IProjectService projectService, UserManager<User> userManager)
        {
            this.leaveService = leaveService;
            this.workService = workService;
            this.projectService = projectService;
            this.userManager = userManager;
        }

        public async Task<IActionResult> List(YearBindModel bind)
        {
            int year = DateTime.UtcNow.Year;

            if (bind.YearId != null && 2000 < bind.YearId && bind.YearId < 2050)
            {
                year = bind.YearId.Value;
            }

            if (!User.Identity.IsAuthenticated)
            {
                return this.RedirectToActionCustom(nameof(AccountController.Login), nameof(AccountController));
            }

            try
            {
                string userId = this.userManager.GetUserId(this.User);

                ListWorkViewModel model = new ListWorkViewModel
                {
                    WorksList = await this.workService.GetAllForUser(userId, year),
                    YearId = year,
                    YearsList = await this.FillYearListDropDown()
                };

                return View(model);
            }
            catch
            {
                this.AddErrorMessage(GlobalConstString.LoadError);
                return RedirectToActionCustom(nameof(HomeController.Index), nameof(HomeController));
            }
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return this.RedirectToActionCustom(nameof(AccountController.Login), nameof(AccountController));
            }

            try
            {
                string userId = this.userManager.GetUserId(this.User);

                CreateWorkBindModel model = new CreateWorkBindModel
                {
                    ProjectNameList = await this.FillProjectNameDropDown(userId)
                };

                return View(model);
            }
            catch
            {
                this.AddErrorMessage(GlobalConstString.LoadError);
                return RedirectToActionCustom(nameof(HomeController.Index), nameof(HomeController));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateWorkBindModel bind)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return this.RedirectToActionCustom(nameof(AccountController.Login), nameof(AccountController));
            }

            try
            {
                string userId = this.userManager.GetUserId(this.User);

                if (!ModelState.IsValid)
                {
                    bind.ProjectNameList = await this.FillProjectNameDropDown(userId);
                    return View(bind);
                }

                bool isExistProjectNameId = await this.projectService.IsExist(userId, bind.ProjectNameId);
                if (!isExistProjectNameId)
                {
                    this.AddErrorMessage(GlobalConstString.RequiredProjectMessage);
                    bind.ProjectNameList = await this.FillProjectNameDropDown(userId);
                    return View(bind);
                }

                await this.workService.Create(userId, bind.Description, bind.WorkDate.Value, bind.ProjectNameId);

                this.AddSuccessMessage(GlobalConstString.SaveSuccess);
                return RedirectToActionCustom(nameof(WorksController.List), nameof(WorksController));
            }
            catch
            {
                this.AddErrorMessage(GlobalConstString.SaveError);
                return RedirectToAction(nameof(List));
            }

        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || id < 0 || id > int.MaxValue)
            {
                this.AddErrorMessage(GlobalConstString.NotFound);
                return RedirectToAction(nameof(List));
            }

            if (!User.Identity.IsAuthenticated)
            {
                return this.RedirectToActionCustom(nameof(AccountController.Login), nameof(AccountController));
            }

            try
            {
                string userId = this.userManager.GetUserId(this.User);

                bool isExist = await this.workService.IsExist(userId, id.Value);

                if (!isExist)
                {
                    this.AddErrorMessage(GlobalConstString.NotFound);
                    return RedirectToAction(nameof(List));
                }

                EditWorkServiceModel data = await this.workService.FindById(id.Value);

                if (data == null)
                {
                    this.AddErrorMessage(GlobalConstString.NotFound);
                    return RedirectToAction(nameof(List));
                }

                EditWorkBindModel model = new EditWorkBindModel
                {
                    Id = data.Id,
                    Description = data.Description,
                    WorkDate = data.WorkDate,
                    ProjectNameId = data.ProjectNameId,
                    ProjectNameList = await this.FillProjectNameDropDown(userId)
                };

                return View(model);
            }
            catch
            {
                this.AddErrorMessage(GlobalConstString.LoadError);
                return RedirectToActionCustom(nameof(HomeController.Index), nameof(HomeController));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditWorkBindModel bind)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return this.RedirectToActionCustom(nameof(AccountController.Login), nameof(AccountController));
            }

            try
            {
                string userId = this.userManager.GetUserId(this.User);

                if (!ModelState.IsValid)
                {
                    bind.ProjectNameList = await this.FillProjectNameDropDown(userId);
                    return View(bind);
                }

                if (bind.Id < 0 || bind.Id > int.MaxValue)
                {
                    this.AddErrorMessage(GlobalConstString.NotFound);
                    return RedirectToAction(nameof(List));
                }

                bool isExistProjectNameId = await this.projectService.IsExist(userId, bind.ProjectNameId);
                if (!isExistProjectNameId)
                {
                    this.AddErrorMessage(GlobalConstString.RequiredProjectMessage);
                    bind.ProjectNameList = await this.FillProjectNameDropDown(userId);
                    return View(bind);
                }

                bool isExist = await this.workService.IsExist(userId, bind.Id);
                if (!isExist)
                {
                    this.AddErrorMessage(GlobalConstString.NotFound);
                    bind.ProjectNameList = await this.FillProjectNameDropDown(userId);
                    return View(bind);
                }

                await this.workService.Edit(userId, bind.Id, bind.Description, bind.WorkDate.Value, bind.ProjectNameId);

                this.AddSuccessMessage(GlobalConstString.SaveSuccess);
                return RedirectToActionCustom(nameof(WorksController.List), nameof(WorksController));
            }
            catch
            {
                this.AddErrorMessage(GlobalConstString.SaveError);
                return RedirectToAction(nameof(List));
            }

        }


        private async Task<IEnumerable<SelectListItem>> FillProjectNameDropDown(string userId)
        {
            return (await this.projectService
                .GetAllForUser(userId))
                .Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Title
                });

        }

        private async Task<IEnumerable<SelectListItem>> FillYearListDropDown()
        {
            return (await this.leaveService
                .GetYearDropDown())
                .Select(x => new SelectListItem
                {
                    Text = x.ToString(),
                    Value = x.ToString()
                });
        }
    }
}
