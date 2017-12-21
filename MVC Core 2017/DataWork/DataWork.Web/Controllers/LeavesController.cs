namespace DataWork.Web.Controllers
{
    using Data.Models;
    using Services.Models.Leave;
    using BindModels;
    using BindModels.Leaves;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Services;
    using System.Threading.Tasks;
    using ViewModels.Leaves;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    [Authorize(Roles = GlobalConstString.WorkerRole)]
    public class LeavesController : MainController
    {

        private readonly ILeaveService leaveService;
        private readonly UserManager<User> userManager;

        public LeavesController(ILeaveService leaveService, UserManager<User> userManager)
        {
            this.leaveService = leaveService;
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
                return RedirectToActionCustom(nameof(AccountController.Login), nameof(AccountController));
            }

            try
            {
                string userId = this.userManager.GetUserId(this.User);

                ListLeaveViewModel model = new ListLeaveViewModel
                {
                    LeavesList = await this.leaveService.GetAllForYear(userId, year),
                    LeaveDayDetails = await this.leaveService.GetLeaveDayDetailsForYear(userId, year),
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

        public IActionResult Create()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToActionCustom(nameof(AccountController.Login), nameof(AccountController));
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateLeaveBindModel bind)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToActionCustom(nameof(AccountController.Login), nameof(AccountController));
            }

            try
            {
                if (!ModelState.IsValid)
                {
                    return View(bind);
                }

                if (bind.StartDate > bind.EndDate)
                {
                    this.AddErrorMessage("End date must be after or equals to start date");
                    return View(bind);
                }

                string userId = this.userManager.GetUserId(this.User);

                if (bind.LeaveTypeId == LeaveType.Paid)
                {
                    //check if user has remaining leave days for curr year
                    int remainDays = await this.leaveService.RemainingDaysForYear(userId);

                    if (remainDays == 0 || remainDays < bind.TotalLeaveDays)
                    {
                        this.AddErrorMessage("Not enough days remaining");
                        return View(bind);
                    }
                }

                await this.leaveService.Create(userId, bind.LeaveTypeId, bind.StartDate, bind.EndDate, bind.TotalLeaveDays);


                this.AddSuccessMessage(GlobalConstString.SaveSuccess);
                return RedirectToAction(nameof(List));

            }
            catch
            {
                this.AddErrorMessage(GlobalConstString.SaveError);
                return View(bind);
            }

        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || id < 0 || id > int.MaxValue)
            {
                this.AddErrorMessage(GlobalConstString.NotFound);
                return RedirectToAction(nameof(List));
            }

            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToActionCustom(nameof(AccountController.Login), nameof(AccountController));
            }


            try
            {
                string userId = this.userManager.GetUserId(this.User);

                bool isExist = await this.leaveService.IsExist(userId, id.Value, LeaveStatus.Create);
                if (!isExist)
                {
                    this.AddErrorMessage(GlobalConstString.NotFound);
                    return RedirectToAction(nameof(List));
                }

                EditLeaveServiceModel data = await this.leaveService.GetById(userId, id.Value);

                EditLeaveBindModel model = new EditLeaveBindModel
                {
                    Id = data.Id,
                    LeaveTypeId = data.LeaveType,
                    StartDate = data.StartDate,
                    EndDate = data.EndDate,
                    TotalLeaveDays = data.TotalLeaveDays
                    
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
        public async Task<IActionResult> Edit(EditLeaveBindModel bind)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToActionCustom(nameof(AccountController.Login), nameof(AccountController));
            }


            try
            {
                string userId = this.userManager.GetUserId(this.User);

                if (bind.Id < 0 || bind.Id > int.MaxValue)
                {
                    this.AddErrorMessage(GlobalConstString.NotFound);
                    return RedirectToAction(nameof(List));
                }

                bool isExist = await this.leaveService.IsExist(userId, bind.Id, LeaveStatus.Create);
                if (!isExist)
                {
                    this.AddErrorMessage(GlobalConstString.NotFound);
                    return RedirectToAction(nameof(List));
                }

                await this.leaveService.Edit(userId, bind.Id, bind.LeaveTypeId, bind.StartDate, bind.EndDate, bind.TotalLeaveDays);

                this.AddSuccessMessage(GlobalConstString.SaveSuccess);
                return RedirectToAction(nameof(List));
            }
            catch
            {
                this.AddErrorMessage(GlobalConstString.SaveError);
                return RedirectToAction(nameof(List));
            }
        }


        [HttpPost]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || id < 0 || id > int.MaxValue)
            {
                this.AddErrorMessage(GlobalConstString.NotFound);
                return RedirectToAction(nameof(List));
            }

            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToActionCustom(nameof(AccountController.Login), nameof(AccountController));
            }

            try
            {
                string userId = this.userManager.GetUserId(this.User);

                bool isExist = await this.leaveService.IsExist(userId, id.Value, LeaveStatus.Create);
                if (!isExist)
                {
                    this.AddErrorMessage(GlobalConstString.NotFound);
                    return RedirectToAction(nameof(List));
                }

                await this.leaveService.Delete(userId, id.Value);

                this.AddSuccessMessage(GlobalConstString.DeleteSuccess);
                return RedirectToAction(nameof(List));
            }
            catch
            {
                this.AddErrorMessage(GlobalConstString.DeleteError);
                return RedirectToAction(nameof(List));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Change(int? id)
        {
            if (id == null || id < 0 || id > int.MaxValue)
            {
                this.AddErrorMessage(GlobalConstString.NotFound);
                return RedirectToAction(nameof(List));
            }

            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToActionCustom(nameof(AccountController.Login), nameof(AccountController));
            }

            try
            {
                string userId = this.userManager.GetUserId(this.User);

                bool isExist = await this.leaveService.IsExist(userId, id.Value, LeaveStatus.Create);
                if (!isExist)
                {
                    this.AddErrorMessage(GlobalConstString.NotFound);
                    return RedirectToAction(nameof(List));
                }

                await this.leaveService.Change(userId, id.Value, LeaveStatus.Send);

                this.AddSuccessMessage(GlobalConstString.ChangeSuccess);
                return RedirectToAction(nameof(List));
            }
            catch
            {
                this.AddErrorMessage(GlobalConstString.ChangeError);
                return RedirectToAction(nameof(List));
            }
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
