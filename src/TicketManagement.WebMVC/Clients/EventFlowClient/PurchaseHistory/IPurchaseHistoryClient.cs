using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TicketManagement.WebMVC.Clients.EventFlowClient.PurchaseHistory
{
    public interface IPurchaseHistoryClient
    {
        public Task AddItemAsync(string userId, string itemId, CancellationToken cancellationToken = default);

        public Task<PurchaseHistoryModel> GetAllByUserIdAsync(string id, CancellationToken cancellationToken = default);
    }

    internal class PurchaseHistoryClient : IPurchaseHistoryClient
    {
        private readonly HttpClient _httpClient;

        public PurchaseHistoryClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task AddItemAsync(string userId, string itemId, CancellationToken cancellationToken = default)
        {
            var address = string.Format(EventFlowApiRequestUries.PurchaseHistoryAddItem, userId, itemId);
            var message = await _httpClient.GetAsync(address, cancellationToken);
            message.EnsureSuccessStatusCode();
        }

        public async Task<PurchaseHistoryModel> GetAllByUserIdAsync(string id, CancellationToken cancellationToken = default)
        {
            var address = string.Format(EventFlowApiRequestUries.PurchaseHistoryGetAllByUserId, id);
            var result = await _httpClient.GetStringAsync(address, cancellationToken);
            var purchaseHistory = JsonConvert.DeserializeObject<PurchaseHistoryModel>(result);
            return purchaseHistory;
        }
    }
}