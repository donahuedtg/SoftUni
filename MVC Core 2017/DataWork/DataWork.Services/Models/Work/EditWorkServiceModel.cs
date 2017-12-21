namespace DataWork.Services.Models.Work
{
    using Common.Mapping;
    using Data.Models;
    using System;

    public class EditWorkServiceModel : IMapFrom<Work>
    {
        public int Id { get; set; }

        public int ProjectNameId { get; set; }

        public DateTime WorkDate { get; set; }

        public string Description { get; set; }
    }
}
