using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TicketManagement.WebMVC.Models;

namespace TicketManagement.WebMVC.Clients.IdentityClient.Profile
{
    /// <summary>
    /// Profile client.
    /// </summary>
    public interface IProfileClient
    {
        /// <summary>
        /// Get balance.
        /// </summary>
        /// <param name="userId">User id.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Balance.</returns>
        Task<decimal> GetBalanceAsync(string userId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Update Balance.
        /// </summary>
        /// <param name="userId">User id.</param>
        /// <param name="balance">Balance.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        Task UpdateBalanceAsync(string userId, decimal balance, CancellationToken cancellationToken = default);

        /// <summary>
        /// GetUser profile.
        /// </summary>
        /// <param name="userId">User id.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Application user.</returns>
        Task<ApplicationUser> GetUserProfileAsync(string userId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Edit first name.
        /// </summary>
        /// <param name="userId">User id.</param>
        /// <param name="firstName">First name.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        Task EditFirstNameAsync(string userId, string firstName, CancellationToken cancellationToken = default);

        /// <summary>
        /// Edit surname.
        /// </summary>
        /// <param name="userId">User id.</param>
        /// <param name="surname">Surname.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        Task EditSurnameAsync(string userId, string surname, CancellationToken cancellationToken = default);

        /// <summary>
        /// Edit email.
        /// </summary>
        /// <param name="userId">User id.</param>
        /// <param name="email">email.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        Task EditEmailAsync(string userId, string email, CancellationToken cancellationToken = default);

        /// <summary>
        /// Edit password.
        /// </summary>
        /// <param name="userId">User id.</param>
        /// <param name="oldPassword">oldPassword.</param>
        /// <param name="newPassword">newPassword.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        Task EditPasswordAsync(string userId, string oldPassword, string newPassword, CancellationToken cancellationToken = default);

        /// <summary>
        /// Edit time zone offset.
        /// </summary>
        /// <param name="userId">User id.</param>
        /// <param name="timeZoneOffset">Time zone offset.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        Task EditTimeZoneOffsetAsync(string userId, string timeZoneOffset, CancellationToken cancellationToken = default);

        /// <summary>
        /// Deposit.
        /// </summary>
        /// <param name="userId">User id.</param>
        /// <param name="balance">Balance.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        Task DepositAsync(string userId, decimal balance, CancellationToken cancellationToken = default);

        /// <summary>
        /// Set language.
        /// </summary>
        /// <param name="userId">User id.</param>
        /// <param name="culture">Culture.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
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
            var address = string.Format(IdentityApiRequestUris.ProfileGetBalance, userId);
            var result = await _httpClient.GetStringAsync(address, cancellationToken);
            var balance = JsonConvert.DeserializeObject<decimal>(result);
            return balance;
        }

        public async Task UpdateBalanceAsync(string userId, decimal balance, CancellationToken cancellationToken = default)
        {
            var address = string.Format(IdentityApiRequestUris.ProfileUpdateBalance);
            var model = new { userId, balance };
            var json = JsonConvert.SerializeObject(model);
            await PutAsync(json, address);
        }

        public async Task<ApplicationUser> GetUserProfileAsync(string userId, CancellationToken cancellationToken = default)
        {
            var address = string.Format(IdentityApiRequestUris.ProfileGetUserProfile, userId);
            var result = await _httpClient.GetStringAsync(address, cancellationToken);
            var user = JsonConvert.DeserializeObject<ApplicationUser>(result);
            return user;
        }

        public async Task SetLanguageAsync(string userId, string culture, CancellationToken cancellationToken = default)
        {
            var address = string.Format(IdentityApiRequestUris.ProfileSetLanguage);
            var model = new { userId, culture };
            var json = JsonConvert.SerializeObject(model);
            await PutAsync(json, address);
        }

        public async Task DepositAsync(string userId, decimal balance, CancellationToken cancellationToken = default)
        {
            var address = string.Format(IdentityApiRequestUris.ProfileDeposit);
            var model = new { userId, balance };
            var json = JsonConvert.SerializeObject(model);
            await PutAsync(json, address);
        }

        public async Task EditEmailAsync(string userId, string email, CancellationToken cancellationToken = default)
        {
            var address = string.Format(IdentityApiRequestUris.ProfileEditEmail);
            var model = new { userId, email };
            var json = JsonConvert.SerializeObject(model);
            await PutAsync(json, address);
        }

        public async Task EditFirstNameAsync(string userId, string firstName, CancellationToken cancellationToken = default)
        {
            var address = string.Format(IdentityApiRequestUris.ProfileEditFirstName);
            var model = new { userId, firstName };
            var json = JsonConvert.SerializeObject(model);
            await PutAsync(json, address);
        }

        public async Task EditPasswordAsync(string userId, string oldPassword, string newPassword, CancellationToken cancellationToken = default)
        {
            var address = string.Format(IdentityApiRequestUris.ProfileEditPassword);
            var model = new { userId, oldPassword, newPassword };
            var json = JsonConvert.SerializeObject(model);
            await PutAsync(json, address);
        }

        public async Task EditSurnameAsync(string userId, string surname, CancellationToken cancellationToken = default)
        {
            var address = string.Format(IdentityApiRequestUris.ProfileEditSurname);
            var model = new { userId, surname };
            var json = JsonConvert.SerializeObject(model);
            await PutAsync(json, address);
        }

        public async Task EditTimeZoneOffsetAsync(string userId, string timeZoneOffset, CancellationToken cancellationToken = default)
        {
            var address = string.Format(IdentityApiRequestUris.ProfileEditTimeZoneOffset);
            var model = new { userId, timeZoneOffset };
            var json = JsonConvert.SerializeObject(model);
            await PutAsync(json, address);
        }

        private async Task PutAsync(string json, string address)
        {
            var queryString = new StringContent(json, Encoding.UTF8, "application/json");
            var message = await _httpClient.PutAsync(address, queryString);
            message.EnsureSuccessStatusCode();
        }
    }
}