using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using TicketManagement.Services.Identity.API.Settings;

namespace TicketManagement.Services.Identity.API.Infrastructure.Services
{
    /// <summary>
    /// Jwt token service.
    /// </summary>
    public class JwtTokenService : IJwtTokenService
    {
        private readonly JwtTokenSettings _settings;

        /// <summary>
        /// Initializes a new instance of the <see cref="JwtTokenService"/> class.
        /// </summary>
        /// <param name="options">Options.</param>
        public JwtTokenService(IOptions<JwtTokenSettings> options)
        {
            _settings = options.Value;
        }

        /// <summary>
        /// Get token.
        /// </summary>
        /// <param name="user">Identity user.</param>
        /// <param name="roles">Roles.</param>
        /// <returns>Token.</returns>
        public string GetToken(IdentityUser user, IEnumerable<string> roles)
        {
            var roleClaims = roles.Select(role => new Claim(ClaimTypes.Role, role)).ToList();
            var userClaims = new List<Claim>
            {
                new (ClaimTypes.Name, user.UserName),
                new (ClaimTypes.NameIdentifier, user.Id),
            };
            userClaims.AddRange(roleClaims);
            var jwt = new JwtSecurityToken(
                issuer: _settings.JwtIssuer,
                audience: _settings.JwtAudience,
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.JwtSecretKey)),
                    SecurityAlgorithms.HmacSha256),
                claims: userClaims);
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            return encodedJwt;
        }

        /// <summary>
        /// Validate token.
        /// </summary>
        /// <param name="token">Token.</param>
        /// <returns>Bool state.</returns>
        public bool ValidateToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                tokenHandler.ValidateToken(
                token,
                new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = _settings.JwtIssuer,
                    ValidateAudience = true,
                    ValidAudience = _settings.JwtAudience,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.JwtSecretKey)),
                    ValidateLifetime = false,
                },
                out var _);
            }
            catch
            {
                return false;
            }

            return true;
        }
    }
}
