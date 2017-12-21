namespace DataWork.Web.Controllers
{
    using Areas.Administrator.Controllers;
    using Areas.Manager.Controllers;
    using Microsoft.AspNetCore.Mvc;
    using Models;
    using System.Diagnostics;

    public class HomeController : MainController
    {
        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.IsInRole(GlobalConstString.AdministratorRole))
                {
                    return RedirectToActionCustom(nameof(AdminController.Index), nameof(AdminController), GlobalConstString.AdministratorAreaName);
                }

                if (User.IsInRole(GlobalConstString.ManagerRole))
                {
                    return RedirectToActionCustom(nameof(HeadsController.Index), nameof(HeadsController), GlobalConstString.ManagerAreaName);
                }
            }


            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
