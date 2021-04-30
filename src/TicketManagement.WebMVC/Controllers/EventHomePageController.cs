using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TicketManagement.BusinessLogic.Interfaces;
using TicketManagement.WebMVC.ViewModels.EventViewModels;

namespace TicketManagement.WebMVC.Controllers
{
    public class EventHomePageController : Controller
    {
        private readonly IEventService _eventService;

        public EventHomePageController(IEventService eventService)
        {
            _eventService = eventService;
        }

        public async Task<IActionResult> IndexAsync()
        {
            var eventCatalog = await _eventService.GetAllAsync();
            var vm = new IndexViewModel
            {
                EventItems = eventCatalog,
            };
            return View(vm);
        }
    }
}
