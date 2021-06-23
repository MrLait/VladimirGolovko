using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TicketManagement.WebMVC.Clients.IdentityClient.AccountUser
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
        {
            var url = IdentityApiRequestUris.Register;
            string json = JsonConvert.SerializeObject(userModel);
            StringContent queryString = new StringContent(json, Encoding.UTF8, "application/json");

            var result = await _httpClient.PostAsync(url, queryString, cancellationToken);

            try
            {
                result.EnsureSuccessStatusCode();
                return await result.Content.ReadAsStringAsync(cancellationToken);
            }
            catch (HttpRequestException)
            {
                var errorMessages = await result.Content.ReadAsStringAsync(cancellationToken);
                throw new HttpRequestException(errorMessages);
            }
        }

        public async Task<string> Login(LoginModel userModel, CancellationToken cancellationToken = default)
        {
            var url = IdentityApiRequestUris.Login;
            string json = JsonConvert.SerializeObject(userModel);
            StringContent queryString = new StringContent(json, Encoding.UTF8, "application/json");

            var result = await _httpClient.PostAsync(url, queryString, cancellationToken);

            try
            {
                result.EnsureSuccessStatusCode();
                return await result.Content.ReadAsStringAsync(cancellationToken);
            }
            catch (HttpRequestException)
            {
                var errorMessages = await result.Content.ReadAsStringAsync(cancellationToken);
                throw new HttpRequestException(errorMessages);
            }
        }

        public async Task ValidateToken(string token, CancellationToken cancellationToken = default)
        {
            var address = string.Format(IdentityApiRequestUris.ValidateToken, token);
            var message = await _httpClient.GetAsync(address, cancellationToken);
            message.EnsureSuccessStatusCode();
        }
    }
}
