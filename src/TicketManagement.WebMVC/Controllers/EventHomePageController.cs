using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicketManagement.WebMVC.Clients.EventFlowClient.Event;
using TicketManagement.WebMVC.ViewModels.EventViewModels;

namespace TicketManagement.WebMVC.Controllers
{
    /// <summary>
    /// Event home page controller.
    /// </summary>
    [AllowAnonymous]
    public class EventHomePageController : Controller
    {
        private readonly IEventClient _eventClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventHomePageController"/> class.
        /// </summary>
        /// <param name="eventClient">Event client.</param>
        public EventHomePageController(IEventClient eventClient)
        {
            _eventClient = eventClient;
        }

        /// <summary>
        /// Index view.
        /// </summary>
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
