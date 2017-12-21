namespace DataWork.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Microsoft.AspNetCore.Identity;
    
    public class User : IdentityUser
    {
        [MinLength(DataConstants.IdNumberMinLength)]
        [MaxLength(DataConstants.IdNumberMaxLength)]
        public string IdNumber { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public List<ProjectName> ProjectNames { get; set; } = new List<ProjectName>();

        public List<Work> Works { get; set; } = new List<Work>();

        public List<LeaveDay> LeaveDays { get; set; } = new List<LeaveDay>();

        public List<Leave> Leaves { get; set; } = new List<Leave>();
    }
}
