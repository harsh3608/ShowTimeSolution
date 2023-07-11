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

        public LeaveTypeOptions LeaveType { get; set; }

        public bool IsApproved { get; set; }

        public bool IsRejected { get; set; }

        public bool IsHalfDay { get; set; }

        public HalfDayShiftOptions HalfDayShift { get; set; }
    }
}
