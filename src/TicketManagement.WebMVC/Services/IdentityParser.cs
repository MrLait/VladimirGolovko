using System;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using TicketManagement.WebMVC.Models;

namespace TicketManagement.WebMVC.Services
{
    public class IdentityParser : IIdentityParser<ApplicationUser>
    {
        public ApplicationUser Parse(IPrincipal principal)
        {
                return new ApplicationUser
                {
                    Email = GetClaimValue(ClaimTypes.Email.ToString(), principal),
                    Id = GetClaimValue(ClaimTypes.NameIdentifier.ToString(), principal),
                    UserName = GetClaimValue(ClaimTypes.Name.ToString(), principal),
                    Surname = GetClaimValue("Surname", principal),
                    TimeZoneOffset = GetClaimValue("TimeZoneOffset", principal),
                    Language = GetClaimValue("Language", principal),
                };
        }

        private static string GetClaimValue(string claimType, IPrincipal principal)
        {
            if (principal is ClaimsPrincipal claims)
            {
                return claims.Claims.FirstOrDefault(x => x.Type == claimType)?.Value ?? "";
            }

            throw new ArgumentException(message: "The principal must be a ClaimsPrincipal", paramName: nameof(principal));
        }
    }
}
