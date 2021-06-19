using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using RestEase;

namespace TicketManagement.WebMVC.Clients.IdentityClient
{
    public interface IUserClient
    {
        public Task HealthCheck(CancellationToken cancellationToken = default);

        public Task<string> Register(RegisterModel userModel, CancellationToken cancellationToken = default);

        public Task<string> Login(LoginModel userModel, CancellationToken cancellationToken = default);

        public Task ValidateToken(string token, CancellationToken cancellationToken = default);
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
            => await AuthorizeInternal(userModel, IdentityApiRequestUries.Register, cancellationToken);

        public async Task<string> Login(LoginModel userModel, CancellationToken cancellationToken = default)
            => await AuthorizeInternal(userModel, IdentityApiRequestUries.Login, cancellationToken);

        public async Task ValidateToken(string token, CancellationToken cancellationToken = default)
        {
            var address = string.Format(IdentityApiRequestUries.ValidateToken, token);
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
    ////public interface IUserRestClient
    ////{
    ////    [Get("/health/live")]
    ////    public Task HealthCheck(CancellationToken cancellationToken = default);

    ////    [Post("AccountUser/register")]
    ////    public Task<string> Register([Body] HttpContent content, CancellationToken cancellationToken = default);

    ////    [Post("AccountUser/login")]
    ////    public Task<string> Login([Body] HttpContent content, CancellationToken cancellationToken = default);

    ////    [Get("AccountUser/validate")]
    ////    public Task ValidateToken([Query] string token, CancellationToken cancellationToken = default);
    ////}

    ////public static class UserClientExtensions
    ////{
    ////    public static async Task<string> Register(this IUserRestClient userClient, RegisterModel userModel, CancellationToken cancellationToken = default)
    ////    {
    ////        var form = new MultipartFormDataContent
    ////        {
    ////            { new StringContent(userModel.Language), nameof(RegisterModel.Language) },
    ////            { new StringContent(userModel.FirstName), nameof(RegisterModel.FirstName) },
    ////            { new StringContent(userModel.Surname), nameof(RegisterModel.Surname) },
    ////            { new StringContent(userModel.TimeZoneOffset), nameof(RegisterModel.TimeZoneOffset) },
    ////            { new StringContent(userModel.UserName), nameof(RegisterModel.UserName) },
    ////            { new StringContent(userModel.Email), nameof(RegisterModel.Email) },
    ////            { new StringContent(userModel.Password), nameof(RegisterModel.Password) },
    ////            { new StringContent(userModel.PasswordConfirm), nameof(RegisterModel.PasswordConfirm) },
    ////        };
    ////        var result = await userClient.Register(form, cancellationToken);
    ////        return result;
    ////    }

    ////    public static async Task<string> Login(this IUserRestClient userClient, LoginModel userModel, CancellationToken cancellationToken = default)
    ////    {
    ////        var form = new MultipartFormDataContent
    ////        {
    ////            { new StringContent(userModel.Email), nameof(LoginModel.Email) },
    ////            { new StringContent(userModel.Password), nameof(LoginModel.Password) },
    ////            { new StringContent(userModel.RememberMe.ToString()), nameof(LoginModel.RememberMe) },
    ////        };
    ////        var result = await userClient.Login(form, cancellationToken);
    ////        return result;
    ////    }

    ////    public static async Task ValidateToken(this IUserRestClient userClient, string token, CancellationToken cancellationToken = default)
    ////    {
    ////        await userClient.ValidateToken(token, cancellationToken);
    ////    }
    ////}
}
