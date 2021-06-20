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
        ////private readonly IEventAreaService _eventAreaService;
        ////private readonly IEventSeatService _eventSeatService;
        ////private readonly IBasketService _basketService;
        ////private readonly IIdentityParser<ApplicationUser> _identityParser;

        public EventAreaController(IEventAreaClient evenAreaClient)
            ////IEventAreaService eventAreaService, IEventSeatService eventSeatService, IBasketService basketService,
            ////IIdentityParser<ApplicationUser> identityParser)
        {
            _evenAreaClient = evenAreaClient;
            ////_eventAreaService = eventAreaService;
            ////_eventSeatService = eventSeatService;
            ////_basketService = basketService;
            ////_identityParser = identityParser;
        }

        public async Task<IActionResult> Index(EventDto dto)
        {
            var evntArea = await _evenAreaClient.GetAllByEventIdAsync(dto.Id);
            var vm = new IndexViewModel { EvenAreatItems = evntArea };
            return View(vm);
        }

        ////public IActionResult Index(EventDto dto)
        ////{
        ////    try
        ////    {
        ////        var eventAreaDto = _eventAreaService.GetByEventId(dto);

        ////        var vm = new IndexViewModel { EvenAreatItems = eventAreaDto };

        ////        for (int i = 0; i < eventAreaDto.Count(); i++)
        ////        {
        ////            var eventSeatDto = _eventSeatService.GetByEventAreaId(eventAreaDto.ToList()[i]);
        ////            vm.EvenAreatItems.ToList()[i].EvenSeats = eventSeatDto;
        ////        }

        ////        return View(vm);
        ////    }
        ////    catch (ValidationException)
        ////    {
        ////        return RedirectToAction("Index", "EventHomePage");
        ////    }
        ////}

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
