namespace DataWork.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class ProjectName
    {
        public int Id { get; set; }

        [MinLength(DataConstants.TitleMinLength)]
        [MaxLength(DataConstants.TitleMaxLength)]
        public string Title { get; set; }

        public string UserId { get; set; }

        public User User { get; set; }

        public List<Work> Works { get; set; } = new List<Work>();

    }
}
