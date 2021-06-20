using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TicketManagement.WebMVC.Models;

namespace TicketManagement.WebMVC.Clients.IdentityClient.Profile
{
    public interface IProfileClient
    {
        public Task<decimal> GetBalanceAsync(string userId, CancellationToken cancellationToken = default);

        public Task UpdateBalanceAsync(string userId, decimal balance, CancellationToken cancellationToken = default);

        public Task<ApplicationUser> GetUserProfile(string userId, CancellationToken cancellationToken = default);

        public Task EditFirstName(string userId, string firstName, CancellationToken cancellationToken = default);

        public Task EditSurname(string userId, string surname, CancellationToken cancellationToken = default);

        public Task EditEmail(string userId, string email, CancellationToken cancellationToken = default);

        public Task EditPassword(string userId, string oldPassword, string newPassword, CancellationToken cancellationToken = default);

        public Task EditTimeZoneOffset(string userId, string timeZoneOffset, CancellationToken cancellationToken = default);

        public Task Deposite(string userId, decimal balance, CancellationToken cancellationToken = default);

        public Task SetLanguage(string userId, string culture, CancellationToken cancellationToken = default);
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
            var address = string.Format(IdentityApiRequestUries.ProfileUpdateBalance, userId, balance);
            var message = await _httpClient.GetAsync(address, cancellationToken);
            message.EnsureSuccessStatusCode();
            ////var table = new { userId, balance };
            ////string json = JsonConvert.SerializeObject(table);
            ////var uri = IdentityApiRequestUries.UpdateBalance;
            ////StringContent queryString = new StringContent(json, System.Text.Encoding.UTF8);
            ////var message = await _httpClient.PutAsync(uri, queryString, cancellationToken);
        }

        public async Task<ApplicationUser> GetUserProfile(string userId, CancellationToken cancellationToken = default)
        {
            var address = string.Format(IdentityApiRequestUries.ProfileGetUserProfile, userId);
            var result = await _httpClient.GetStringAsync(address, cancellationToken);
            var user = JsonConvert.DeserializeObject<ApplicationUser>(result);
            return user;
        }

        public async Task SetLanguage(string userId, string culture, CancellationToken cancellationToken = default)
        {
            var address = string.Format(IdentityApiRequestUries.ProfileSetLanguage, userId, culture);
            var message = await _httpClient.GetAsync(address, cancellationToken);
            message.EnsureSuccessStatusCode();
        }

        public async Task Deposite(string userId, decimal balance, CancellationToken cancellationToken = default)
        {
            var address = string.Format(IdentityApiRequestUries.ProfileDeposite, userId, balance);
            var message = await _httpClient.GetAsync(address, cancellationToken);
            message.EnsureSuccessStatusCode();
        }

        public async Task EditEmail(string userId, string email, CancellationToken cancellationToken = default)
        {
            var address = string.Format(IdentityApiRequestUries.ProfileEditEmail, userId, email);
            var message = await _httpClient.GetAsync(address, cancellationToken);
            message.EnsureSuccessStatusCode();
        }

        public async Task EditFirstName(string userId, string firstName, CancellationToken cancellationToken = default)
        {
            var address = string.Format(IdentityApiRequestUries.ProfileEditFirstName, userId, firstName);
            var message = await _httpClient.GetAsync(address, cancellationToken);
            message.EnsureSuccessStatusCode();
        }

        public async Task EditPassword(string userId, string oldPassword, string newPassword, CancellationToken cancellationToken = default)
        {
            var address = string.Format(IdentityApiRequestUries.ProfileEditPassword, userId, oldPassword, newPassword);
            var message = await _httpClient.GetAsync(address, cancellationToken);
            message.EnsureSuccessStatusCode();
        }

        public async Task EditSurname(string userId, string surname, CancellationToken cancellationToken = default)
        {
            var address = string.Format(IdentityApiRequestUries.ProfileEditSurname, userId, surname);
            var message = await _httpClient.GetAsync(address, cancellationToken);
            message.EnsureSuccessStatusCode();
        }

        public async Task EditTimeZoneOffset(string userId, string timeZoneOffset, CancellationToken cancellationToken = default)
        {
            var address = string.Format(IdentityApiRequestUries.ProfileEditTimeZoneOffset, userId, timeZoneOffset);
            var message = await _httpClient.GetAsync(address, cancellationToken);
            message.EnsureSuccessStatusCode();
        }
    }
}