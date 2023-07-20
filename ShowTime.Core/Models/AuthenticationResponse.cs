using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowTime.Core.Models
{
    public class AuthenticationResponse
    {
        public string PersonName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string UserType { get; set; }
        public string Token { get; set; } = string.Empty;
        public DateTime Expiration { get; set; }
        public Guid ManagerId { get; set; }
        public string ManagerName { get; set; }

    }
}
