namespace DataWork.Web.Areas.Manager.Controllers
{
    using Data.Models;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Services;
    using System.Threading.Tasks;
    using ViewModels.Heads;
    using Web.Controllers;

    public class HeadsController : BaseController
    {
        private readonly ILeaveService leaveService;
        private readonly UserManager<User> userManager;

        public HeadsController(ILeaveService leaveService, UserManager<User> userManager)
        {
            this.leaveService = leaveService;
            this.userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            if (!User.Identity.IsAuthenticated || !User.IsInRole(GlobalConstString.ManagerRole))
            {
                return RedirectToActionCustom(nameof(AccountController.Login), nameof(AccountController));
            }

            try
            {
                HeadsLeaveForApproveViewModel model = new HeadsLeaveForApproveViewModel
                {
                    Count = await this.leaveService.GetLeaveCountForApprove()
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
            if (!User.Identity.IsAuthenticated || !User.IsInRole(GlobalConstString.ManagerRole))
            {
                return RedirectToActionCustom(nameof(AccountController.Login), nameof(AccountController));
            }

            try
            {
                HeadsListLeaveViewModel model = new HeadsListLeaveViewModel
                {
                    LeavesList = await this.leaveService.GetAllForApprove()
                };

                return View(model);
            }
            catch
            {
                this.AddErrorMessage(GlobalConstString.LoadError);
                return RedirectToActionCustom(nameof(HeadsController.Index), nameof(HeadsController));
            }

        }

        [HttpPost]
        public async Task<IActionResult> Change(int? id, LeaveStatus leaveStatus)
        {
            if (id == null || id < 0 || id > int.MaxValue || (leaveStatus != LeaveStatus.Approve && leaveStatus != LeaveStatus.Denied))
            {
                this.AddErrorMessage(GlobalConstString.NotFound);
                return RedirectToAction(nameof(List));
            }

            if (!User.Identity.IsAuthenticated || !User.IsInRole(GlobalConstString.ManagerRole))
            {
                return RedirectToActionCustom(nameof(AccountController.Login), nameof(AccountController));
            }

            try
            {
                //string userId = this.userManager.GetUserId(this.User);

                bool isExist = await this.leaveService.IsExist(id.Value, LeaveStatus.Send);
                if (!isExist)
                {
                    this.AddErrorMessage(GlobalConstString.NotFound);
                    return RedirectToAction(nameof(List));
                }

                string leaveUserId = await this.leaveService.GetUserIdById(id.Value);
                if (leaveUserId == null)
                {
                    this.AddErrorMessage(GlobalConstString.NotFound);
                    return RedirectToAction(nameof(List));
                }

                await this.leaveService.Change(leaveUserId, id.Value, leaveStatus);

                this.AddSuccessMessage($"{string.Format(GlobalConstString.ChangeHeadSuccess, leaveStatus)}");
                return RedirectToAction(nameof(List));
            }
            catch
            {
                this.AddErrorMessage($"{string.Format(GlobalConstString.ChangeHeadError, leaveStatus)}");
                return RedirectToAction(nameof(List));
            }
        }
    }
}
