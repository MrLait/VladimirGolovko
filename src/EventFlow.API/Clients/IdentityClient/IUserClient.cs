using System.Threading;
using System.Threading.Tasks;

namespace TicketManagement.Services.EventFlow.API.Clients.IdentityClient
{
    /// <summary>
    /// User client.
    /// </summary>
    public interface IUserClient
    {
        /// <summary>
        /// User health check.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token parameter.</param>
        Task HealthCheck(CancellationToken cancellationToken = default);

        /// <summary>
        /// Register.
        /// </summary>
        /// <param name="userModel">User model.</param>
        /// <param name="cancellationToken">Cancellation token parameter.</param>
        Task<string> Register(RegisterModel userModel, CancellationToken cancellationToken = default);

        /// <summary>
        /// Login.
        /// </summary>
        /// <param name="userModel">User model.</param>
        /// <param name="cancellationToken">Cancellation token parameter.</param>
        Task<string> Login(LoginModel userModel, CancellationToken cancellationToken = default);

        /// <summary>
        /// Validate token.
        /// </summary>
        /// <param name="token">Token.</param>
        /// <param name="cancellationToken">Cancellation token parameter.</param>
        Task ValidateToken(string token, CancellationToken cancellationToken = default);
    }
}