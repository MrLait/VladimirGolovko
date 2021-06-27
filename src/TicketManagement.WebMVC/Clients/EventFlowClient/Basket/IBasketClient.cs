using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TicketManagement.WebMVC.Clients.EventFlowClient.Basket
{
    /// <summary>
    /// Basket client.
    /// </summary>
    public interface IBasketClient
    {
        /// <summary>
        /// Get all by user id.
        /// </summary>
        /// <param name="id">User id.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Returns basket model.</returns>
        Task<BasketModel> GetAllByUserIdAsync(string id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Add to basket.
        /// </summary>
        /// <param name="userId">User id.</param>
        /// <param name="itemId">Item id.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        Task AddToBasketAsync(string userId, int itemId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Remove from basket.
        /// </summary>
        /// <param name="userId">User id.</param>
        /// <param name="itemId">Item id.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        Task RemoveFromBasketAsync(string userId, int itemId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Delete all by user id.
        /// </summary>
        /// <param name="userId">User id.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        Task DeleteAllByUserIdAsync(string userId, CancellationToken cancellationToken = default);
    }

    internal class BasketClient : IBasketClient
    {
        private readonly HttpClient _httpClient;

        public BasketClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task AddToBasketAsync(string userId, int itemId, CancellationToken cancellationToken = default)
        {
            var address = string.Format(EventFlowApiRequestUris.BasketAddToBasket);
            var model = new { userId, itemId };
            var json = JsonConvert.SerializeObject(model);
            await PostAsync(json, address);
        }

        public async Task RemoveFromBasketAsync(string userId, int itemId, CancellationToken cancellationToken = default)
        {
            var address = string.Format(EventFlowApiRequestUris.BasketRemoveFromBasket, userId, itemId);
            var message = await _httpClient.DeleteAsync(address, cancellationToken);
            message.EnsureSuccessStatusCode();
        }

        public async Task<BasketModel> GetAllByUserIdAsync(string id, CancellationToken cancellationToken = default)
        {
            var address = string.Format(EventFlowApiRequestUris.BasketGetAllByUserId, id);
            var result = await _httpClient.GetStringAsync(address, cancellationToken);
            var eventAreas = JsonConvert.DeserializeObject<BasketModel>(result);
            return eventAreas;
        }

        public async Task DeleteAllByUserIdAsync(string userId, CancellationToken cancellationToken = default)
        {
            var address = string.Format(EventFlowApiRequestUris.BasketDeleteAllByUserId, userId);
            var message = await _httpClient.DeleteAsync(address, cancellationToken);
            message.EnsureSuccessStatusCode();
        }

        private async Task PostAsync(string json, string address)
        {
            var queryString = new StringContent(json, Encoding.UTF8, "application/json");
            var message = await _httpClient.PostAsync(address, queryString);
            message.EnsureSuccessStatusCode();
        }
    }
}