using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowTime.Core.DTO
{
    public class DobDTO
    {
        public Guid UserId { get; set; }
        public string PersonName { get; set; }
        public DateTime DateOfBirth{ get; set; }
        public string email { get; set; }
    }
}
