using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TicketManagement.WebMVC.Clients.IdentityClient.Profile
{
    public interface IProfileClient
    {
        public Task<decimal> GetBalanceAsync(string userId, CancellationToken cancellationToken = default);

        public Task UpdateBalanceAsync(string userId, decimal balance, CancellationToken cancellationToken = default);
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
            var address = string.Format(IdentityApiRequestUries.GetBalance, userId);
            var result = await _httpClient.GetStringAsync(address, cancellationToken);
            var balance = JsonConvert.DeserializeObject<decimal>(result);
            return balance;
        }

        public async Task UpdateBalanceAsync(string userId, decimal balance, CancellationToken cancellationToken = default)
        {
            var address = string.Format(IdentityApiRequestUries.UpdateBalance, userId, balance);
            var message = await _httpClient.GetAsync(address, cancellationToken);
            message.EnsureSuccessStatusCode();
            ////var table = new { userId, balance };
            ////string json = JsonConvert.SerializeObject(table);
            ////var uri = IdentityApiRequestUries.UpdateBalance;
            ////StringContent queryString = new StringContent(json, System.Text.Encoding.UTF8);
            ////var message = await _httpClient.PutAsync(uri, queryString, cancellationToken);
        }
        ////{
        ////    var address = string.Format(EventFlowApiRequestUries.BasketGetAllByUserId, id);
        ////    var result = await _httpClient.GetStringAsync(address, cancellationToken);
        ////    var eventAreas = JsonConvert.DeserializeObject<BasketModel>(result);
        ////    return eventAreas;
        ////}
    }
}