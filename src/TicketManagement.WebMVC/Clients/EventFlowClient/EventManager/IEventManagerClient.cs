using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TicketManagement.Dto;
using TicketManagement.WebMVC.Infrastructure.ExceptionsMessages;

namespace TicketManagement.WebMVC.Clients.EventFlowClient.EventManager
{
    /// <summary>
    /// Event manager client.
    /// </summary>
    public interface IEventManagerClient
    {
        /// <summary>
        /// Create event.
        /// </summary>
        /// <param name="eventDto">Event dto.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
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
            var url = EventFlowApiRequestUris.EventManagerCreateEvent;
            var json = JsonConvert.SerializeObject(eventDto);
            var queryString = new StringContent(json, Encoding.UTF8, "application/json");

            var result = new HttpResponseMessage();
            try
            {
                result = await _httpClient.PostAsync(url, queryString, cancellationToken);
                result.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException)
            {
                var validationException = JsonConvert.DeserializeObject<ValidationException>(result.Content.ReadAsStringAsync(cancellationToken).Result) ?? new ValidationException();
                throw new ValidationException(validationException.Message);
            }
        }
    }
}