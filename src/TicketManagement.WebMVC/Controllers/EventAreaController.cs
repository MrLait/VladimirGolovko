using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
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

        public EventAreaController(IEventAreaService eventAreaService, IEventSeatService eventSeatService, IBasketService basketService)
        {
            _eventAreaService = eventAreaService;
            _eventSeatService = eventSeatService;
            _basketService = basketService;
        }

        public async Task<IActionResult> Index(EventDto dto)
        {
            var eventAreaDto = (await _eventAreaService.GetAllAsync()).Where(x => x.EventId == dto.Id);

            var vm = new IndexViewModel
            {
                EvenAreatItems = eventAreaDto,
            };

            for (int i = 0; i < eventAreaDto.Count(); i++)
            {
                List<EventSeatDto> eventSeatDto = (await _eventSeatService.GetAllAsync()).Where(x => x.EventAreaId == eventAreaDto.ToList()[i].Id).ToList();
                vm.EvenAreatItems.ToList()[i].EvenSeats = eventSeatDto;
            }

            return View(vm);
        }

        public async Task<IActionResult> AddToBasketAsync(EventAreaDto eventAreaDto, int itemId, States itemState)
        {
            var user = Parse(HttpContext.User);
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

            var user = Parse(HttpContext.User);
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

        public ApplicationUser Parse(IPrincipal principal)
        {
            // Pattern matching 'is' expression
            // assigns "claims" if "principal" is a "ClaimsPrincipal"
            if (principal is ClaimsPrincipal claims)
            {
                return new ApplicationUser
                {
                    Email = claims.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value ?? "",
                    Id = claims.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value ?? "",
                    PhoneNumber = claims.Claims.FirstOrDefault(x => x.Type == ClaimTypes.MobilePhone)?.Value ?? "",
                    UserName = claims.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value ?? "",
                };
            }

            throw new ArgumentException(message: "The principal must be a ClaimsPrincipal", paramName: nameof(principal));
        }
    }
}
