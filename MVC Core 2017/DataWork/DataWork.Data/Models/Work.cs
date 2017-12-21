namespace DataWork.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class Work
    {
        public int Id { get; set; }

        [MinLength(DataConstants.DescriptionMinLength)]
        [MaxLength(DataConstants.DescriptionMaxLength)]
        public string Description { get; set; }

        [DataType(DataType.Date)]
        public DateTime WorkDate { get; set; }

        public DateTime TimeStamp { get; set; }

        public int ProjectNameId { get; set; }

        public ProjectName ProjectName { get; set; }

        public string UserId { get; set; }

        public User User { get; set; }


    }
}
