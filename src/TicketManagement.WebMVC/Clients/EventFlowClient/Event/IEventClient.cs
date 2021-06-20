using System.Collections.Generic;
using System.Net.Http;
using System.Text;
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

        public Task<EventDto> GetByIDAsync(int id, CancellationToken cancellationToken = default);

        public Task UpdateEventAsync(EventDto eventDto, CancellationToken cancellationToken = default);

        public Task UpdateLayoutIdAsync(EventDto eventDto, CancellationToken cancellationToken = default);

        public Task DeleteByIdAsync(int id, CancellationToken cancellationToken = default);
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
            var uri = string.Format(EventFlowApiRequestUries.EventDeleteById, id);
            var result = await _httpClient.DeleteAsync(uri, cancellationToken);
            result.EnsureSuccessStatusCode();
        }

        public async Task<List<EventDto>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var result = await _httpClient.GetStringAsync(EventFlowApiRequestUries.EventGetAll, cancellationToken);
            var events = JsonConvert.DeserializeObject<List<EventDto>>(result);
            return events;
        }

        public async Task<EventDto> GetByIDAsync(int id, CancellationToken cancellationToken = default)
        {
            var uri = string.Format(EventFlowApiRequestUries.EventGetByID, id);
            var result = await _httpClient.GetStringAsync(uri, cancellationToken);
            var eventDto = JsonConvert.DeserializeObject<EventDto>(result);
            return eventDto;
        }

        public async Task<EventDto> GetLastAsync(CancellationToken cancellationToken = default)
        {
            var result = await _httpClient.GetStringAsync(EventFlowApiRequestUries.EventGetLast, cancellationToken);
            var eventDto = JsonConvert.DeserializeObject<EventDto>(result);
            return eventDto;
        }

        public async Task UpdateEventAsync(EventDto eventDto, CancellationToken cancellationToken = default)
        {
            var url = EventFlowApiRequestUries.EventUpdateEvent;
            string json = JsonConvert.SerializeObject(eventDto);
            StringContent queryString = new StringContent(json, Encoding.UTF8, "application/json");
            var result = await _httpClient.PostAsync(url, queryString, cancellationToken);
            result.EnsureSuccessStatusCode();
        }

        public async Task UpdateLayoutIdAsync(EventDto eventDto, CancellationToken cancellationToken = default)
        {
            var url = EventFlowApiRequestUries.EventUpdateLayoutId;
            string json = JsonConvert.SerializeObject(eventDto);
            StringContent queryString = new StringContent(json, Encoding.UTF8, "application/json");
            var result = await _httpClient.PostAsync(url, queryString, cancellationToken);
            result.EnsureSuccessStatusCode();
        }
    }
}