namespace DataWork.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class Leave
    {
        public int Id { get; set; }

        public int CurrentYear { get; set; }

        public LeaveType LeaveType { get; set; }

        public LeaveStatus LeaveStatus { get; set; }

        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        public int TotalLeaveDays { get; set; }

        public DateTime TimeStamp { get; set; }

        public string UserId { get; set; }

        public User User { get; set; }

    }
}
