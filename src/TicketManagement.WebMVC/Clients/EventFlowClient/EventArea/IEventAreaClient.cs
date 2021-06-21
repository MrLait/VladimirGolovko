using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TicketManagement.Dto;

namespace TicketManagement.WebMVC.Clients.EventFlowClient.EventArea
{
    public interface IEventAreaClient
    {
        Task<List<EventAreaDto>> GetAllByEventIdAsync(int id, CancellationToken cancellationToken = default);

        Task UpdatePricesAsync(List<EventAreaDto> eventAreaDtos, CancellationToken cancellationToken = default);
    }

    internal class EventAreaClient : IEventAreaClient
    {
        private readonly HttpClient _httpClient;

        public EventAreaClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<EventAreaDto>> GetAllByEventIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var address = string.Format(EventFlowApiRequestUries.EventAreaGetAllByEventId, id);
            var result = await _httpClient.GetStringAsync(address, cancellationToken);
            var eventAreas = JsonConvert.DeserializeObject<List<EventAreaDto>>(result);
            return eventAreas;
        }

        public async Task UpdatePricesAsync(List<EventAreaDto> eventAreaDtos, CancellationToken cancellationToken = default)
        {
            var url = EventFlowApiRequestUries.EventAreaUpdatePrices;
            string json = JsonConvert.SerializeObject(eventAreaDtos);
            StringContent queryString = new StringContent(json, Encoding.UTF8, "application/json");
            var result = await _httpClient.PutAsync(url, queryString, cancellationToken);
            result.EnsureSuccessStatusCode();
        }
    }
}