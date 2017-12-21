namespace DataWork.Web.Areas.Administrator.Controllers
{
    using Web.Controllers;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;


    [Area(GlobalConstString.AdministratorAreaName)]
    [Authorize(Roles = GlobalConstString.AdministratorRole)]
    public class BaseController : MainController
    {
    }
}
