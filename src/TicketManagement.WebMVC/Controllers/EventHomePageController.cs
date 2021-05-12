using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicketManagement.BusinessLogic.Interfaces;
using TicketManagement.WebMVC.ViewModels.EventViewModels;

namespace TicketManagement.WebMVC.Controllers
{
    [AllowAnonymous]
    public class EventHomePageController : Controller
    {
        private readonly IEventService _eventService;

        public EventHomePageController(IEventService eventService)
        {
            _eventService = eventService;
        }

        public IActionResult Index()
        {
            var eventCatalog = _eventService.GetAll();
            var vm = new IndexViewModel
            {
                EventItems = eventCatalog,
            };

            return View(vm);
        }
    }
}
