using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicketManagement.WebMVC.Clients.EventFlowClient.Event;
using TicketManagement.WebMVC.ViewModels.EventViewModels;

namespace TicketManagement.WebMVC.Controllers
{
    [AllowAnonymous]
    public class EventHomePageController : Controller
    {
        private readonly IEventClient _eventClient;

        public EventHomePageController(IEventClient eventClient)
        {
            _eventClient = eventClient;
        }

        public async Task<IActionResult> IndexAsync()
        {
            var eventCatalog = await _eventClient.GetAllAsync();
            var vm = new IndexViewModel
            {
                EventItems = eventCatalog,
            };

            return View(vm);
        }
    }
}
