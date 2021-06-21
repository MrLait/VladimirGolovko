using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace TicketManagement.Services.EventFlow.API.Clients.IdentityClient
{
    public interface IUserClient
    {
        Task HealthCheck(CancellationToken cancellationToken = default);

        Task<string> Register(RegisterModel userModel, CancellationToken cancellationToken = default);

        Task<string> Login(LoginModel userModel, CancellationToken cancellationToken = default);

        Task ValidateToken(string token, CancellationToken cancellationToken = default);
    }

    internal class UserClient : IUserClient
    {
        private readonly HttpClient _httpClient;

        public UserClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task HealthCheck(CancellationToken cancellationToken = default)
        {
            var result = await _httpClient.GetAsync("/health/live", cancellationToken);
            result.EnsureSuccessStatusCode();
        }

        public async Task<string> Register(RegisterModel userModel, CancellationToken cancellationToken = default)
            => await AuthorizeInternal(userModel, "AccountUser/register", cancellationToken);

        public async Task<string> Login(LoginModel userModel, CancellationToken cancellationToken = default)
            => await AuthorizeInternal(userModel, "AccountUser/login", cancellationToken);

        public async Task ValidateToken(string token, CancellationToken cancellationToken = default)
        {
            var address = $"AccountUser/validate?token={token}";
            var message = await _httpClient.GetAsync(address, cancellationToken);
            message.EnsureSuccessStatusCode();
        }

        private async Task<string> AuthorizeInternal(RegisterModel userModel, string path, CancellationToken cancellationToken)
        {
            var form = new MultipartFormDataContent
            {
                { new StringContent(userModel.Language), nameof(RegisterModel.Language) },
                { new StringContent(userModel.FirstName), nameof(RegisterModel.FirstName) },
                { new StringContent(userModel.Surname), nameof(RegisterModel.Surname) },
                { new StringContent(userModel.TimeZoneOffset), nameof(RegisterModel.TimeZoneOffset) },
                { new StringContent(userModel.UserName), nameof(RegisterModel.UserName) },
                { new StringContent(userModel.Email), nameof(RegisterModel.Email) },
                { new StringContent(userModel.Password), nameof(RegisterModel.Password) },
                { new StringContent(userModel.PasswordConfirm), nameof(RegisterModel.PasswordConfirm) },
            };

            var result = await _httpClient.PostAsync(path, form, cancellationToken);
            result.EnsureSuccessStatusCode();
            return await result.Content.ReadAsStringAsync(cancellationToken);
        }

        private async Task<string> AuthorizeInternal(LoginModel userModel, string path, CancellationToken cancellationToken)
        {
            var form = new MultipartFormDataContent
            {
                { new StringContent(userModel.Email), nameof(LoginModel.Email) },
                { new StringContent(userModel.Password), nameof(LoginModel.Password) },
                { new StringContent(userModel.RememberMe.ToString()), nameof(LoginModel.RememberMe) },
            };
            var result = await _httpClient.PostAsync(path, form, cancellationToken);
            result.EnsureSuccessStatusCode();
            return await result.Content.ReadAsStringAsync(cancellationToken);
        }
    }
}