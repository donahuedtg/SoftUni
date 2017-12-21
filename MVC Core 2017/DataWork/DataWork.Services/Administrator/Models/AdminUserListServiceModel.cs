namespace DataWork.Services.Administrator.Models
{
    using AutoMapper;
    using Common.Mapping;
    using Data.Models;
    using System;
    using System.Linq;

    public class AdminUserListServiceModel : IMapFrom<User>, IHaveCustomMapping
    {
        public string Id { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public bool IsSetLeaveDaysForCurrYear { get; set; }

        public void ConfigureMapping(Profile profile)
        {
            profile
                .CreateMap<User, AdminUserListServiceModel>()
                .ForMember(x => x.IsSetLeaveDaysForCurrYear,
                                src => src.MapFrom(x => x.LeaveDays
                                                        .Any(ld => ld.UserId == x.Id && 
                                                            ld.CurrentYear == DateTime.UtcNow.Year)));
        }
    }
}
