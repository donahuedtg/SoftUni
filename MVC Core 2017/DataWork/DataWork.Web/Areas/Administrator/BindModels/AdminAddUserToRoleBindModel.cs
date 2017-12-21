namespace DataWork.Web.Areas.Administrator.BindModels
{
    using System.ComponentModel.DataAnnotations;

    public class AdminAddUserToRoleBindModel
    {
        [Required]
        public string UserId { get; set; }

        [Required]
        public string RoleName { get; set; }
    }
}
