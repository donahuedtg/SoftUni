namespace DataWork.Data.Models
{
    using System;

    public class LeaveDay
    {
        public int Id { get; set; }

        public int CurrentYear { get; set; }

        public int LeaveDayForYear { get; set; }

        public int RemainLeaveDay { get; set; }

        public DateTime TimeStamp { get; set; }

        public string UserId { get; set; }

        public User User { get; set; }
    }
}
