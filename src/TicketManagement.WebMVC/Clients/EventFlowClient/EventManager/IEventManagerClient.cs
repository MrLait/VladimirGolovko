using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TicketManagement.Dto;

namespace TicketManagement.WebMVC.Clients.EventFlowClient.EventManager
{
    public interface IEventManagerClient
    {
        Task CreateEvent(EventDto eventDto, CancellationToken cancellationToken = default);
    }

    internal class EventManagerClient : IEventManagerClient
    {
        private readonly HttpClient _httpClient;

        public EventManagerClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task CreateEvent(EventDto eventDto, CancellationToken cancellationToken = default)
        {
            var url = EventFlowApiRequestUries.EventManagerCreateEvent;
            string json = JsonConvert.SerializeObject(eventDto);
            StringContent queryString = new StringContent(json, Encoding.UTF8, "application/json");
            var result = await _httpClient.PostAsync(url, queryString, cancellationToken);
            result.EnsureSuccessStatusCode();
        }
    }
}