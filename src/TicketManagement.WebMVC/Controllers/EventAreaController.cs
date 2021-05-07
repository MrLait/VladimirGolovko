using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TicketManagement.BusinessLogic.Interfaces;
using TicketManagement.DataAccess.Domain.Models;
using TicketManagement.DataAccess.Enums;
using TicketManagement.Dto;
using TicketManagement.WebMVC.Models;
using TicketManagement.WebMVC.Services;
using TicketManagement.WebMVC.ViewModels.EventAreaViewModels;

namespace TicketManagement.WebMVC.Controllers
{
    public class EventAreaController : Controller
    {
        private readonly IEventAreaService _eventAreaService;
        private readonly IEventSeatService _eventSeatService;
        private readonly IBasketService _basketService;
        private readonly IIdentityParser<ApplicationUser> _identityParser;

        public EventAreaController(IEventAreaService eventAreaService, IEventSeatService eventSeatService, IBasketService basketService,
            IIdentityParser<ApplicationUser> identityParser)
        {
            _eventAreaService = eventAreaService;
            _eventSeatService = eventSeatService;
            _basketService = basketService;
            _identityParser = identityParser;
        }

        public IActionResult Index(EventDto dto)
        {
            var eventAreaDto = _eventAreaService.GetByEventId(dto);

            var vm = new IndexViewModel
            {
                EvenAreatItems = eventAreaDto,
            };

            for (int i = 0; i < eventAreaDto.Count(); i++)
            {
                var eventSeatDto = _eventSeatService.GetByEventAreaId(eventAreaDto.ToList()[i]);
                vm.EvenAreatItems.ToList()[i].EvenSeats = eventSeatDto;
            }

            return View(vm);
        }

        public async Task<IActionResult> AddToBasketAsync(EventAreaDto eventAreaDto, int itemId, States itemState)
        {
            var user = _identityParser.Parse(HttpContext.User);
            await _basketService.AddAsync(user, itemId);
            await _eventSeatService.UpdateStateAsync(new EventSeatDto { Id = itemId, State = States.Booked });

            return RedirectToAction("Index", "EventArea", new EventDto { Id = eventAreaDto.Id });
        }

        public async Task<IActionResult> RemoveFromBasketAsync(EventAreaDto eventAreaDto, int itemId, States itemState)
        {
            if (itemState == States.Purchased)
            {
                return RedirectToAction("Index", "EventArea", new EventDto { Id = eventAreaDto.Id });
            }

            var user = _identityParser.Parse(HttpContext.User);
            await _basketService.DeleteAsync(new Basket { ProductId = itemId, UserId = user.Id });
            await _eventSeatService.UpdateStateAsync(new EventSeatDto { Id = itemId, State = States.Available });

            return RedirectToAction("Index", "EventArea", new EventDto { Id = eventAreaDto.Id });
        }

        public IActionResult GetViewEventArea(EventDto dto)
        {
            if (dto.Id != 0)
            {
                return RedirectToAction("Index", "EventArea", dto);
            }

            return RedirectToAction("Index", "EventHomePage");
        }

        public ActionResult GetEventSeats()
        {
            return PartialView("_GetEventSeats");
        }
    }
}
