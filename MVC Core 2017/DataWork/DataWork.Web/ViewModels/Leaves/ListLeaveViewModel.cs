namespace DataWork.Web.ViewModels.Leaves
{

    using Services.Models.Leave;
    using System.Collections.Generic;

    public class ListLeaveViewModel : YearListDropDownVIewModel
    {
        public IEnumerable<ListLeaveServiceModel> LeavesList { get; set; }
        public LeaveDayDetailsServiceModel LeaveDayDetails { get; set; }
    }
}
