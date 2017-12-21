namespace DataWork.Web.Controllers
{
    using DataWork.Data.Models;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Services;
    using System.Threading.Tasks;

    public class ProjectsController : MainController
    {
        private readonly IProjectService projectService;
        private readonly UserManager<User> userManager;

        public ProjectsController(IProjectService projectService, UserManager<User> userManager)
        {
            this.projectService = projectService;
            this.userManager = userManager;
        }

        [HttpPost]
        public async Task<JsonResult> AddProject(string projectName)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Json(0);
            }

            if (string.IsNullOrWhiteSpace(projectName))
            {
                return Json(0);
            }

            string userId = this.userManager.GetUserId(this.User);

            bool isExist = await this.projectService.IsExist(userId, projectName);

            if (isExist)
            {
                return Json(0);
            }

            return Json(await this.projectService.Create(userId, projectName));
        }

        [HttpPost]
        public async Task<JsonResult> ReloadProjectDropDownList()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Json(0);
            }

            string userId = this.userManager.GetUserId(this.User);

            return Json(await this.projectService.GetAllForUser(userId));
        }
    }
}
