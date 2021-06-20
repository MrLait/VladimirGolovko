using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RestEase;
using TicketManagement.Dto;

namespace TicketManagement.WebMVC.Clients.EventFlowClient.Event
{
    public interface IEventClient
    {
        public Task<List<EventDto>> GetAllAsync(CancellationToken cancellationToken = default);

        public Task<EventDto> GetLastAsync(CancellationToken cancellationToken = default);
    }

    internal class EventClient : IEventClient
    {
        private readonly HttpClient _httpClient;

        public EventClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<EventDto>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var result = await _httpClient.GetStringAsync(EventFlowApiRequestUries.EventGetAll, cancellationToken);
            var events = JsonConvert.DeserializeObject<List<EventDto>>(result);
            return events;
        }

        public async Task<EventDto> GetLastAsync(CancellationToken cancellationToken = default)
        {
            var result = await _httpClient.GetStringAsync(EventFlowApiRequestUries.EventGetLast, cancellationToken);
            var eventDto = JsonConvert.DeserializeObject<EventDto>(result);
            return eventDto;
        }
    }
}