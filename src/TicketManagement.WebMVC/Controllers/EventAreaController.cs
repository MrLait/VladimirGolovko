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
            //////var token = Request.Cookies["secret_jwt_key"].ToString();
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

        ////public async Task<IActionResult> AddToBasketAsync(EventAreaDto eventAreaDto, int itemId, States itemState)
        ////{
        ////    try
        ////    {
        ////        var user = _identityParser.Parse(HttpContext.User);
        ////        await _basketService.AddAsync(user, itemId);
        ////        await _eventSeatService.UpdateStateAsync(new EventSeatDto { Id = itemId, State = States.Booked });

        ////        return RedirectToAction("Index", "EventArea", new EventDto { Id = eventAreaDto.Id });
        ////    }
        ////    catch (ValidationException ve)
        ////    {
        ////        ModelState.AddModelError("", ve.Message);
        ////        return RedirectToAction("Index", "EventHomePage");
        ////    }
        ////}

        ////public async Task<IActionResult> RemoveFromBasketAsync(EventAreaDto eventAreaDto, int itemId, States itemState)
        ////{
        ////    if (itemState == States.Purchased)
        ////    {
        ////        return RedirectToAction("Index", "EventArea", new EventDto { Id = eventAreaDto.Id });
        ////    }

        ////    try
        ////    {
        ////        var user = _identityParser.Parse(HttpContext.User);
        ////        await _basketService.DeleteAsync(new Basket { ProductId = itemId, UserId = user.Id });
        ////        await _eventSeatService.UpdateStateAsync(new EventSeatDto { Id = itemId, State = States.Available });

        ////        return RedirectToAction("Index", "EventArea", new EventDto { Id = eventAreaDto.Id });
        ////    }
        ////    catch (ValidationException ve)
        ////    {
        ////        ModelState.AddModelError("", ve.Message);
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
