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

        public int LeaveType { get; set; }

        public int Status { get; set; }

        public bool IsHalfDay { get; set; }

        public int HalfDayShift { get; set; }

        public bool IsPaid { get; set; }

        public Guid ManagerId { get; set; }

        public string ManagerName { get; set; }

        public DateTime DateOfRequest { get; set; }

        public int LeaveDays { get; set; }
    }
}
