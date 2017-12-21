namespace DataWork.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    public class MainController : Controller
    {

        protected RedirectToActionResult RedirectToActionCustom(string actionName, string controllerName, string areaName = "")
        {
            return RedirectToAction(actionName, controllerName.Replace(nameof(Controller), string.Empty), new { area = areaName });
        }

        protected void AddErrorMessage(string message)
        {
            TempData[GlobalConstString.Error] = message;
        }

        protected void AddSuccessMessage(string message)
        {
            TempData[GlobalConstString.Success] = message;
        }

        protected void AddWarningMessage(string message)
        {
            TempData[GlobalConstString.Warning] = message;
        }

    }
}
