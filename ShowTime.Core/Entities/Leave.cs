using ShowTime.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowTime.Core.Entities
{
    public class Leave
    {
        public Guid Id { get; set; }

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

        public bool IsPaid { get; set; }    

    }
}
