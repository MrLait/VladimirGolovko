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
        public Task CreateEvent(EventDto eventDto, CancellationToken cancellationToken = default);
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

////private async Task<string> AuthorizeInternal(RegisterModel userModel, string path, CancellationToken cancellationToken)
////{
////    var form = new MultipartFormDataContent
////            {
////                { new StringContent(userModel.Language), nameof(RegisterModel.Language) },
////                { new StringContent(userModel.FirstName), nameof(RegisterModel.FirstName) },
////                { new StringContent(userModel.Surname), nameof(RegisterModel.Surname) },
////                { new StringContent(userModel.TimeZoneOffset), nameof(RegisterModel.TimeZoneOffset) },
////                { new StringContent(userModel.UserName), nameof(RegisterModel.UserName) },
////                { new StringContent(userModel.Email), nameof(RegisterModel.Email) },
////                { new StringContent(userModel.Password), nameof(RegisterModel.Password) },
////                { new StringContent(userModel.PasswordConfirm), nameof(RegisterModel.PasswordConfirm) },
////            };
////    var result = await _httpClient.PostAsync(path, form, cancellationToken);
////    result.EnsureSuccessStatusCode();
////    return await result.Content.ReadAsStringAsync(cancellationToken);
////}