namespace DataWork.Services.Models.Project
{
    using Common.Mapping;
    using Data.Models;

    public class ProjectNameServiceModel : IMapFrom<ProjectName>
    {
        public int Id { get; set; }

        public string Title { get; set; }
    }
}
