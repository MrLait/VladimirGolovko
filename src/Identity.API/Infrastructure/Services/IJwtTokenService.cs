using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace TicketManagement.Services.Identity.API.Infrastructure.Services
{
    public interface IJwtTokenService
    {
        /// <summary>
        /// Get token.
        /// </summary>
        /// <param name="user">Identity user.</param>
        /// <param name="roles">Roles.</param>
        /// <returns>Token.</returns>
        string GetToken(IdentityUser user, IEnumerable<string> roles);

        /// <summary>
        /// Validate token.
        /// </summary>
        /// <param name="token">Token.</param>
        /// <returns>Bool state.</returns>
        bool ValidateToken(string token);
    }
}
