using ShowTime.Core.Enums;

namespace ShowTime.Core.Models
{
    public class LeaveAddRequest
    {
        public Guid UserId { get; set; }

        public string Username { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string Reason { get; set; }

        public string LeaveType { get; set; }

        public bool IsApproved { get; set; }

        public bool IsRejected { get; set; }

        public bool IsHalfDay { get; set; }

        public string HalfDayShift { get; set; }

        public bool IsPaid { get; set; }

        public Guid ManagerId { get; set; }

        public string ManagerName { get; set; }

        public DateTime DateOfRequest { get; set; }

        public int LeaveDays { get; set; }
    }
}
