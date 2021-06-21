using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TicketManagement.WebMVC.Models;

namespace TicketManagement.WebMVC.Clients.IdentityClient.Profile
{
    public interface IProfileClient
    {
        Task<decimal> GetBalanceAsync(string userId, CancellationToken cancellationToken = default);

        Task UpdateBalanceAsync(string userId, decimal balance, CancellationToken cancellationToken = default);

        Task<ApplicationUser> GetUserProfileAsync(string userId, CancellationToken cancellationToken = default);

        Task EditFirstNameAsync(string userId, string firstName, CancellationToken cancellationToken = default);

        Task EditSurnameAsync(string userId, string surname, CancellationToken cancellationToken = default);

        Task EditEmailAsync(string userId, string email, CancellationToken cancellationToken = default);

        Task EditPasswordAsync(string userId, string oldPassword, string newPassword, CancellationToken cancellationToken = default);

        Task EditTimeZoneOffsetAsync(string userId, string timeZoneOffset, CancellationToken cancellationToken = default);

        Task DepositeAsync(string userId, decimal balance, CancellationToken cancellationToken = default);

        Task SetLanguageAsync(string userId, string culture, CancellationToken cancellationToken = default);
    }

    internal class ProfileClient : IProfileClient
    {
        private readonly HttpClient _httpClient;

        public ProfileClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<decimal> GetBalanceAsync(string userId, CancellationToken cancellationToken = default)
        {
            var address = string.Format(IdentityApiRequestUries.ProfileGetBalance, userId);
            var result = await _httpClient.GetStringAsync(address, cancellationToken);
            var balance = JsonConvert.DeserializeObject<decimal>(result);
            return balance;
        }

        public async Task UpdateBalanceAsync(string userId, decimal balance, CancellationToken cancellationToken = default)
        {
            var address = string.Format(IdentityApiRequestUries.ProfileUpdateBalance);
            var model = new { userId, balance };
            var json = JsonConvert.SerializeObject(model);
            await PutAsync(json, address);
        }

        public async Task<ApplicationUser> GetUserProfileAsync(string userId, CancellationToken cancellationToken = default)
        {
            var address = string.Format(IdentityApiRequestUries.ProfileGetUserProfile, userId);
            var result = await _httpClient.GetStringAsync(address, cancellationToken);
            var user = JsonConvert.DeserializeObject<ApplicationUser>(result);
            return user;
        }

        public async Task SetLanguageAsync(string userId, string culture, CancellationToken cancellationToken = default)
        {
            var address = string.Format(IdentityApiRequestUries.ProfileSetLanguage);
            var model = new { userId, culture };
            var json = JsonConvert.SerializeObject(model);
            await PutAsync(json, address);
        }

        public async Task DepositeAsync(string userId, decimal balance, CancellationToken cancellationToken = default)
        {
            var address = string.Format(IdentityApiRequestUries.ProfileDeposite);
            var model = new { userId, balance };
            var json = JsonConvert.SerializeObject(model);
            await PutAsync(json, address);
        }

        public async Task EditEmailAsync(string userId, string email, CancellationToken cancellationToken = default)
        {
            var address = string.Format(IdentityApiRequestUries.ProfileEditEmail);
            var model = new { userId, email };
            var json = JsonConvert.SerializeObject(model);
            await PutAsync(json, address);
        }

        public async Task EditFirstNameAsync(string userId, string firstName, CancellationToken cancellationToken = default)
        {
            var address = string.Format(IdentityApiRequestUries.ProfileEditFirstName);
            var model = new { userId, firstName };
            var json = JsonConvert.SerializeObject(model);
            await PutAsync(json, address);
        }

        public async Task EditPasswordAsync(string userId, string oldPassword, string newPassword, CancellationToken cancellationToken = default)
        {
            var address = string.Format(IdentityApiRequestUries.ProfileEditPassword);
            var model = new { userId, oldPassword, newPassword };
            var json = JsonConvert.SerializeObject(model);
            await PutAsync(json, address);
        }

        public async Task EditSurnameAsync(string userId, string surname, CancellationToken cancellationToken = default)
        {
            var address = string.Format(IdentityApiRequestUries.ProfileEditSurname);
            var model = new { userId, surname };
            var json = JsonConvert.SerializeObject(model);
            await PutAsync(json, address);
        }

        public async Task EditTimeZoneOffsetAsync(string userId, string timeZoneOffset, CancellationToken cancellationToken = default)
        {
            var address = string.Format(IdentityApiRequestUries.ProfileEditTimeZoneOffset);
            var model = new { userId, timeZoneOffset };
            var json = JsonConvert.SerializeObject(model);
            await PutAsync(json, address);
        }

        private async Task PutAsync(string json, string address)
        {
            StringContent queryString = new StringContent(json, Encoding.UTF8, "application/json");
            var message = await _httpClient.PutAsync(address, queryString);
            message.EnsureSuccessStatusCode();
        }
    }
}