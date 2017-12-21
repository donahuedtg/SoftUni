namespace DataWork.Web.Areas.Manager.ViewModels.Heads
{
    using Services.Models.Leave;
    using System.Collections.Generic;

    public class HeadsListLeaveViewModel
    {
        public IEnumerable<ListLeaveServiceModel> LeavesList { get; set; }
    }
}
