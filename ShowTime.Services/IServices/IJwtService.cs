using ShowTime.Core.IdentityEntities;
using ShowTime.Core.Models;
using System.Security.Claims;

namespace ShowTime.Services.IServices
{
    public interface IJwtService
    {
        AuthenticationResponse CreateJwtToken(ApplicationUser user);
        ClaimsPrincipal? GetPrincipalFromJwtToken(string? token);
    }
}
