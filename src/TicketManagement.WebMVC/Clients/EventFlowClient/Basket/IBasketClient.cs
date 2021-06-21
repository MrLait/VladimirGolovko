using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TicketManagement.WebMVC.Clients.EventFlowClient.Basket
{
    public interface IBasketClient
    {
        Task<BasketModel> GetAllByUserIdAsync(string id, CancellationToken cancellationToken = default);

        Task AddToBasketAsync(string userId, int itemId, CancellationToken cancellationToken = default);

        Task RemoveFromBasketAsync(string userId, int itemId, CancellationToken cancellationToken = default);

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
            var address = string.Format(EventFlowApiRequestUries.BasketAddToBasket);
            var model = new { userId, itemId };
            var json = JsonConvert.SerializeObject(model);
            await PostAsync(json, address);
        }

        public async Task RemoveFromBasketAsync(string userId, int itemId, CancellationToken cancellationToken = default)
        {
            var address = string.Format(EventFlowApiRequestUries.BasketRemoveFromBasket, userId, itemId);
            var message = await _httpClient.DeleteAsync(address, cancellationToken);
            message.EnsureSuccessStatusCode();
        }

        public async Task<BasketModel> GetAllByUserIdAsync(string id, CancellationToken cancellationToken = default)
        {
            var address = string.Format(EventFlowApiRequestUries.BasketGetAllByUserId, id);
            var result = await _httpClient.GetStringAsync(address, cancellationToken);
            var eventAreas = JsonConvert.DeserializeObject<BasketModel>(result);
            return eventAreas;
        }

        public async Task DeleteAllByUserIdAsync(string userId, CancellationToken cancellationToken = default)
        {
            var address = string.Format(EventFlowApiRequestUries.BasketDeleteAllByUserId, userId);
            var message = await _httpClient.DeleteAsync(address, cancellationToken);
            message.EnsureSuccessStatusCode();
        }

        private async Task PostAsync(string json, string address)
        {
            StringContent queryString = new StringContent(json, Encoding.UTF8, "application/json");
            var message = await _httpClient.PostAsync(address, queryString);
            message.EnsureSuccessStatusCode();
        }
    }
}