using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TicketManagement.Dto;

namespace TicketManagement.WebMVC.Clients.EventFlowClient.EventArea
{
    public interface IEventAreaClient
    {
        public Task<List<EventAreaDto>> GetAllByEventIdAsync(int id, CancellationToken cancellationToken = default);
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
    }
}