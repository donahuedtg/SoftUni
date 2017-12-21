namespace DataWork.Services.Models.Leave
{
    using Common.Mapping;
    using Data.Models;
    using System;

    public class EditLeaveServiceModel : IMapFrom<Leave>
    {
        public int Id { get; set; }

        public LeaveType LeaveType { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public int TotalLeaveDays { get; set; }
    }
}
