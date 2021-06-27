using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using TicketManagement.Services.EventFlow.API.Clients.IdentityClient;

namespace TicketManagement.Services.EventFlow.API.Infrastructure.HealthChecks
{
    /// <summary>
    /// User api health check class.
    /// </summary>
    internal class UserApiHealthCheck : IHealthCheck
    {
        public const string UserApiHealthCheckName = "user_api_health_check";
        private const string UserApiHealthyText = "User API is healthy";
        private const string UserApiUnhealthyText = "User API is unhealthy. Reason: ";
        private readonly IUserClient _userClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserApiHealthCheck"/> class.
        /// </summary>
        /// <param name="userClient">User client.</param>
        public UserApiHealthCheck(IUserClient userClient)
        {
            _userClient = userClient;
        }

        /// <summary>
        /// Runs the health check, returning the status of the component being checked.
        /// </summary>
        /// <param name="context">A context object associated with the current execution.</param>
        /// <param name="cancellationToken">A that can be used to cancel the health check.</param>
        /// <returns>A that completes when the health check has finished, yielding the status of the component being checked.</returns>
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new ())
        {
            try
            {
                await _userClient.HealthCheck(cancellationToken);
                return HealthCheckResult.Healthy(UserApiHealthyText);
            }
            catch (HttpRequestException)
            {
                return HealthCheckResult.Unhealthy(UserApiUnhealthyText);
            }
        }
    }
}