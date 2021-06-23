using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TicketManagement.WebMVC.Clients.EventFlowClient.PurchaseHistory
{
    /// <summary>
    /// Purchase history client.
    /// </summary>
    public interface IPurchaseHistoryClient
    {
        /// <summary>
        /// Add item.
        /// </summary>
        /// <param name="userId">User id.</param>
        /// <param name="itemId">Item id.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        Task AddItemAsync(string userId, string itemId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Get all by user id.
        /// </summary>
        /// <param name="id">User id.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Purchase history model.</returns>
        Task<PurchaseHistoryModel> GetAllByUserIdAsync(string id, CancellationToken cancellationToken = default);
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
            var address = string.Format(EventFlowApiRequestUris.PurchaseHistoryAddItem);
            var model = new { userId, itemId };
            var json = JsonConvert.SerializeObject(model);
            await PostAsync(json, address);
        }

        public async Task<PurchaseHistoryModel> GetAllByUserIdAsync(string id, CancellationToken cancellationToken = default)
        {
            var address = string.Format(EventFlowApiRequestUris.PurchaseHistoryGetAllByUserId, id);
            var result = await _httpClient.GetStringAsync(address, cancellationToken);
            var purchaseHistory = JsonConvert.DeserializeObject<PurchaseHistoryModel>(result);
            return purchaseHistory;
        }

        private async Task PostAsync(string json, string address)
        {
            StringContent queryString = new StringContent(json, Encoding.UTF8, "application/json");
            var message = await _httpClient.PostAsync(address, queryString);
            message.EnsureSuccessStatusCode();
        }
    }
}