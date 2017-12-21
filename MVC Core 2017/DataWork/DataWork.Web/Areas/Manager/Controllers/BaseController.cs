namespace DataWork.Web.Areas.Manager.Controllers
{
    using DataWork.Web.Controllers;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Area(GlobalConstString.ManagerAreaName)]
    [Authorize(Roles = GlobalConstString.ManagerRole)]
    public class BaseController : MainController
    {
    }
}
