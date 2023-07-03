using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowTime.Core.Models
{
    public class PunchAddRequest
    {
        public Guid Id { get; set; }

        public DateTime PunchDateTime { get; set; }

        public Guid UserId { get; set; }

        public string? UserName { get; set; }

        public bool PunchStatus { get; set; }
    }
}
