namespace DataWork.Services.Models.Leave
{
    using Common.Mapping;
    using Data.Models;


    public class LeaveDayDetailsServiceModel : IMapFrom<LeaveDay>
    {
        public int CurrentYear { get; set; }

        public int LeaveDayForYear { get; set; }

        public int RemainLeaveDay { get; set; }
    }
}
