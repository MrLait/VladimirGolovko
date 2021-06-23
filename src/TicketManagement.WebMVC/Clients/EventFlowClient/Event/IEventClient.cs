using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TicketManagement.Dto;
using TicketManagement.WebMVC.Infrastructure.ExceptionsMessages;

namespace TicketManagement.WebMVC.Clients.EventFlowClient.Event
{
    /// <summary>
    /// Event client.
    /// </summary>
    public interface IEventClient
    {
        /// <summary>
        /// Get all.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token.</param>
        Task<List<EventDto>> GetAllAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Get last.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token.</param>
        Task<EventDto> GetLastAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Get by id.
        /// </summary>
        /// <param name="id">Event id.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        Task<EventDto> GetByIdAsync(int id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Update event.
        /// </summary>
        /// <param name="eventDto">Event dto.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        Task UpdateEventAsync(EventDto eventDto, CancellationToken cancellationToken = default);

        /// <summary>
        /// Delete by id.
        /// </summary>
        /// <param name="id">Event id.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
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

            var result = new HttpResponseMessage();
            try
            {
                result = await _httpClient.DeleteAsync(uri, cancellationToken);
                result.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException)
            {
                var validationException = JsonConvert.DeserializeObject<ValidationException>(result.Content.ReadAsStringAsync(cancellationToken).Result) ?? new ValidationException();
                throw new ValidationException(validationException.Message);
            }
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
            var queryString = new StringContent(json, Encoding.UTF8, "application/json");

            var result = new HttpResponseMessage();
            try
            {
                result = await _httpClient.PutAsync(url, queryString, cancellationToken);
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