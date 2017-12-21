namespace DataWork.Services.Models.Leave
{
    using Common.Mapping;
    using Data.Models;
    using System;
    using AutoMapper;

    public class ListLeaveServiceModel : IMapFrom<Leave>, IHaveCustomMapping
    {
        public int Id { get; set; }
        public int CurrentYear { get; set; }
        public string FullName { get; set; }
        public DateTime TimeStamp { get; set; }
        public LeaveType LeaveType { get; set; }
        public LeaveStatus LeaveStatus { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int TotalLeaveDays { get; set; }

        public void ConfigureMapping(Profile profile)
        {
            profile
                .CreateMap<Leave, ListLeaveServiceModel>()
                .ForMember(x => x.FullName, src => src.MapFrom(u => u.User.UserName));
        }
    }
}
