namespace DataWork.Services.Models.Work
{
    using Data.Models;
    using Common.Mapping;
    using System;
    using AutoMapper;

    public class ListWorkServiceModel : IMapFrom<Work>, IHaveCustomMapping
    {
        public int Id { get; set; }
        public DateTime WorkDate { get; set; }
        public string ProjectName { get; set; }
        public string Description { get; set; }
        public string FullName { get; set; }
        public DateTime TimeStamp { get; set; }

        public void ConfigureMapping(Profile profile)
        {
            profile
                .CreateMap<Work, ListWorkServiceModel>()
                .ForMember(x => x.FullName, src => src.MapFrom(x => x.User.UserName))
                .ForMember(x => x.ProjectName, src => src.MapFrom(x => x.ProjectName.Title));
        }
    }
}
