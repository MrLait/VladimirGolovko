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
            if (principal is ClaimsPrincipal claims)
            {
                return new ApplicationUser
                {
                    Email = claims.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value ?? "",
                    Id = claims.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value ?? "",
                    UserName = claims.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value ?? "",
                };
            }

            throw new ArgumentException(message: "The principal must be a ClaimsPrincipal", paramName: nameof(principal));
        }
    }
}
