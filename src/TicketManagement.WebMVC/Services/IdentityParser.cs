using System;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using TicketManagement.WebMVC.Models;

namespace TicketManagement.WebMVC.Services
{
    /// <summary>
    /// Identity parser.
    /// </summary>
    public class IdentityParser : IIdentityParser<ApplicationUser>
    {
        private const string Surname = "Surname";
        private const string Language = "Language";
        private const string TimeZoneOffset = "TimeZoneOffset";
        private const string ClaimPrincipleException = "The principal must be a ClaimsPrincipal";

        /// <inheritdoc/>
        public ApplicationUser Parse(IPrincipal principal)
        {
                return new ()
                {
                    Email = GetClaimValue(ClaimTypes.Email, principal),
                    Id = GetClaimValue(ClaimTypes.NameIdentifier, principal),
                    UserName = GetClaimValue(ClaimTypes.Name, principal),
                    Surname = GetClaimValue(Surname, principal),
                    TimeZoneOffset = GetClaimValue(TimeZoneOffset, principal),
                    Language = GetClaimValue(Language, principal),
                };
        }

        private static string GetClaimValue(string claimType, IPrincipal principal)
        {
            if (principal is ClaimsPrincipal claims)
            {
                return claims.Claims.FirstOrDefault(x => x.Type == claimType)?.Value ?? "";
            }

            throw new ArgumentException(message: ClaimPrincipleException, paramName: nameof(principal));
        }
    }
}
