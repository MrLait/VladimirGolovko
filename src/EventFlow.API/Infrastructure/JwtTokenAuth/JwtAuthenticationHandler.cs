using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TicketManagement.Services.EventFlow.API.Clients.IdentityClient;

namespace TicketManagement.Services.EventFlow.API.Infrastructure.JwtTokenAuth
{
    /// <summary>
    /// Jwt authentication handler.
    /// </summary>
    public class JwtAuthenticationHandler : AuthenticationHandler<JwtAuthenticationOptions>
    {
        private readonly IUserClient _userClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="JwtAuthenticationHandler"/> class.
        /// </summary>
        /// <param name="options">Jwt authentication options.</param>
        /// <param name="logger">Logger factory.</param>
        /// <param name="encoder">Url encoder.</param>
        /// <param name="clock">System clock.</param>
        /// <param name="userClient">User client.</param>
        public JwtAuthenticationHandler(
            IOptionsMonitor<JwtAuthenticationOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            IUserClient userClient)
            : base(options, logger, encoder, clock)
        {
            _userClient = userClient;
        }

        /// <inheritdoc />
        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey(JwtAuthenticationConstants.Authorization))
            {
                return AuthenticateResult.Fail(JwtAuthenticationConstants.Unauthorized);
            }

            var token = Request.Headers[JwtAuthenticationConstants.Authorization].ToString()[JwtAuthenticationConstants.Bearer.Length..];
            try
            {
                await _userClient.ValidateToken(token);
            }
            catch (HttpRequestException)
            {
                return AuthenticateResult.Fail(JwtAuthenticationConstants.Unauthorized);
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(token);
            var identity = new ClaimsIdentity(jwtToken.Claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);
            return AuthenticateResult.Success(ticket);
        }
    }
}
