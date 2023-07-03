using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowTime.Core.DTO
{
    public class PunchDTO
    {
        public DateTime PunchDateTime { get; set; }

        public Guid UserId { get; set; }

        public string? UserName { get; set; }

        public string? PunchStatus { get; set; }
    }
}
