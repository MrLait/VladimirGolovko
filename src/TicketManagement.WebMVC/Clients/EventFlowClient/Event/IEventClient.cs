using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TicketManagement.Dto;

namespace TicketManagement.WebMVC.Clients.EventFlowClient.Event
{
    public interface IEventClient
    {
        Task<List<EventDto>> GetAllAsync(CancellationToken cancellationToken = default);

        Task<EventDto> GetLastAsync(CancellationToken cancellationToken = default);

        Task<EventDto> GetByIdAsync(int id, CancellationToken cancellationToken = default);

        Task UpdateEventAsync(EventDto eventDto, CancellationToken cancellationToken = default);

        Task UpdateLayoutIdAsync(EventDto eventDto, CancellationToken cancellationToken = default);

        Task DeleteByIdAsync(int id, CancellationToken cancellationToken = default);
    }

    internal class EventClient : IEventClient
    {
        private readonly HttpClient _httpClient;

        public EventClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task DeleteByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var uri = string.Format(EventFlowApiRequestUris.EventDeleteById, id);
            var result = await _httpClient.DeleteAsync(uri, cancellationToken);
            result.EnsureSuccessStatusCode();
        }

        public async Task<List<EventDto>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var result = await _httpClient.GetStringAsync(EventFlowApiRequestUris.EventGetAll, cancellationToken);
            var events = JsonConvert.DeserializeObject<List<EventDto>>(result);
            return events;
        }

        public async Task<EventDto> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var uri = string.Format(EventFlowApiRequestUris.EventGetById, id);
            var result = await _httpClient.GetStringAsync(uri, cancellationToken);
            var eventDto = JsonConvert.DeserializeObject<EventDto>(result);
            return eventDto;
        }

        public async Task<EventDto> GetLastAsync(CancellationToken cancellationToken = default)
        {
            var result = await _httpClient.GetStringAsync(EventFlowApiRequestUris.EventGetLast, cancellationToken);
            var eventDto = JsonConvert.DeserializeObject<EventDto>(result);
            return eventDto;
        }

        public async Task UpdateEventAsync(EventDto eventDto, CancellationToken cancellationToken = default)
        {
            var url = EventFlowApiRequestUris.EventUpdateEvent;
            string json = JsonConvert.SerializeObject(eventDto);
            StringContent queryString = new StringContent(json, Encoding.UTF8, "application/json");
            var result = await _httpClient.PutAsync(url, queryString, cancellationToken);
            result.EnsureSuccessStatusCode();
        }

        public async Task UpdateLayoutIdAsync(EventDto eventDto, CancellationToken cancellationToken = default)
        {
            var url = EventFlowApiRequestUris.EventUpdateLayoutId;
            string json = JsonConvert.SerializeObject(eventDto);
            StringContent queryString = new StringContent(json, Encoding.UTF8, "application/json");
            var result = await _httpClient.PutAsync(url, queryString, cancellationToken);
            result.EnsureSuccessStatusCode();
        }
    }
}