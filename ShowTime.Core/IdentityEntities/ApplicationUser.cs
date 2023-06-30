using Microsoft.AspNetCore.Identity;

namespace ShowTime.Core.IdentityEntities
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string? PersonName { get; set; }

        public string? Gender { get; set; }

        public string? UserType { get; set; }

        public string? JobRole { get; set; }
    }
}
