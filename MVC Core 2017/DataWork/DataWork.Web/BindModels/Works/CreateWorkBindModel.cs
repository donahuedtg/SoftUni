namespace DataWork.Web.BindModels.Works
{
    using DataWork.Data;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class CreateWorkBindModel
    {
        [Display(Name = "Project Name")]
        public int ProjectNameId { get; set; }
        public IEnumerable<SelectListItem> ProjectNameList { get; set; }

        [Required]
        [Display(Name = "Work Date")]
        [DataType(DataType.Date)]
        public DateTime? WorkDate { get; set; }

        [Required]
        [MinLength(DataConstants.DescriptionMinLength)]
        [MaxLength(DataConstants.DescriptionMaxLength)]
        public string Description { get; set; }
    }
}
