namespace DataWork.Web.ViewModels.Works
{
    using Services.Models.Work;
    using System.Collections.Generic;

    public class ListWorkViewModel : YearListDropDownVIewModel
    {
        public IEnumerable<ListWorkServiceModel> WorksList { get; set; }

    }
}
