using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TicketManagement.Dto;
using TicketManagement.WebMVC.Clients.EventFlowClient.EventArea;
using TicketManagement.WebMVC.ViewModels.EventAreaViewModels;

namespace TicketManagement.WebMVC.Controllers
{
    public class EventAreaController : Controller
    {
        private readonly IEventAreaClient _evenAreaClient;

        public EventAreaController(IEventAreaClient evenAreaClient)
        {
            _evenAreaClient = evenAreaClient;
        }

        public async Task<IActionResult> Index(EventDto dto)
        {
            var evntArea = await _evenAreaClient.GetAllByEventIdAsync(dto.Id);
            var vm = new IndexViewModel { EvenAreatItems = evntArea };
            return View(vm);
        }

        public IActionResult GetViewEventArea(EventDto dto)
        {
            if (dto is null)
            {
                return RedirectToAction("Index", "EventHomePage");
            }

            if (dto.Id != 0)
            {
                return RedirectToAction("Index", "EventArea", dto);
            }

            return RedirectToAction("Index", "EventHomePage");
        }
    }
}