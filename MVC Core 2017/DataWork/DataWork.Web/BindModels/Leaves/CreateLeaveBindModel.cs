namespace DataWork.Web.BindModels.Leaves
{
    using Data.Models;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class CreateLeaveBindModel : IValidatableObject
    {
        [Display(Name = GlobalConstString.LeaveTypeDisplayName)]
        public LeaveType LeaveTypeId { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = GlobalConstString.StartDateDisplayName)]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = GlobalConstString.EndDateDisplayName)]
        public DateTime EndDate { get; set; }

        [Display(Name = GlobalConstString.TotalLeaveDaysDisplayName)]
        public int TotalLeaveDays { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (this.StartDate > this.EndDate)
            {
                yield return new ValidationResult("End date must be after or equals to Start date", new[] { nameof(StartDate) });
            }

            if (this.TotalLeaveDays <= 0)
            {
                yield return new ValidationResult("Total leave days must be a positive number.", new[] { nameof(TotalLeaveDays) });
            }

            int countDays = 0;
            DateTime start = this.StartDate;
            DateTime end = this.EndDate;

            while (start <= end)
            {
                if (start.DayOfWeek != DayOfWeek.Saturday && start.DayOfWeek != DayOfWeek.Sunday)
                {
                    countDays++;
                }

                start = start.AddDays(1);
            }

            if (countDays < this.TotalLeaveDays)
            {
                yield return new ValidationResult("Total leave days do not equals to working days.", new[] { nameof(TotalLeaveDays) });
            }
        }
    }
}
