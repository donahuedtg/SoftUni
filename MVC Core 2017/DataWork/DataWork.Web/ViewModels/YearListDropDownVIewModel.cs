namespace DataWork.Web.ViewModels
{
    using Microsoft.AspNetCore.Mvc.Rendering;
    using System.Collections.Generic;

    public abstract class YearListDropDownVIewModel
    {
        public int YearId { get; set; }
        public IEnumerable<SelectListItem> YearsList { get; set; }
    }
}
