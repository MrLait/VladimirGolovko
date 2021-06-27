using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TicketManagement.Dto;
using TicketManagement.WebMVC.Infrastructure.ExceptionsMessages;

namespace TicketManagement.WebMVC.Clients.EventFlowClient.EventArea
{
    /// <summary>
    /// Event area client.
    /// </summary>
    public interface IEventAreaClient
    {
        /// <summary>
        /// Get all by event id.
        /// </summary>
        /// <param name="id">Event id.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        Task<List<EventAreaDto>> GetAllByEventIdAsync(int id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Update prices.
        /// </summary>
        /// <param name="eventAreaDtos">Event area dto list.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
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
            var address = string.Format(EventFlowApiRequestUris.EventAreaGetAllByEventId, id);
            var result = await _httpClient.GetStringAsync(address, cancellationToken);
            var eventAreas = JsonConvert.DeserializeObject<List<EventAreaDto>>(result);
            return eventAreas;
        }

        public async Task UpdatePricesAsync(List<EventAreaDto> eventAreaDtos, CancellationToken cancellationToken = default)
        {
            var url = EventFlowApiRequestUris.EventAreaUpdatePrices;
            var json = JsonConvert.SerializeObject(eventAreaDtos);
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