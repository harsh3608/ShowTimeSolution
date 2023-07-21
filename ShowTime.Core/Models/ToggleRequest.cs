using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowTime.Core.Models
{
    public class ToggleRequest
    {
        public Guid LeaveId { get; set; }

        public int value { get; set; }
    }
}
