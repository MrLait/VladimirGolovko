using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using TicketManagement.Services.EventFlow.API.Infrastructure.Exceptions;
using TicketManagement.Services.EventFlow.API.Infrastructure.Services.Interfaces;

namespace TicketManagement.Services.EventFlow.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class EventAreaController : ControllerBase
    {
        private readonly IEventAreaService _eventAreaService;
        private readonly IEventSeatService _eventSeatService;
        ////private readonly IBasketService _basketService;
        ////private readonly IIdentityParser<ApplicationUser> _identityParser;

        public EventAreaController(IEventAreaService eventAreaService, IEventSeatService eventSeatService)
            ////IBasketService basketService,
            ////IIdentityParser<ApplicationUser> identityParser
        {
            _eventAreaService = eventAreaService;
            _eventSeatService = eventSeatService;
            ////_basketService = basketService;
            ////_identityParser = identityParser;
        }

        [HttpGet("getAllByEventId")]
        public IActionResult GetAllByEventId(int id)
        {
            try
            {
                var eventAreaDto = _eventAreaService.GetByEventId(id);

                for (int i = 0; i < eventAreaDto.Count(); i++)
                {
                    var eventSeatDto = _eventSeatService.GetByEventAreaId(eventAreaDto.ToList()[i]);
                    eventAreaDto.ToList()[i].EvenSeats = eventSeatDto;
                }

                return Ok(eventAreaDto);
            }
            catch (ValidationException)
            {
                return RedirectToAction("Index", "EventHomePage");
            }
        }

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

        ////public IActionResult GetViewEventArea(EventDto dto)
        ////{
        ////    if (dto is null)
        ////    {
        ////        return RedirectToAction("Index", "EventHomePage");
        ////    }

        ////    if (dto.Id != 0)
        ////    {
        ////        return RedirectToAction("Index", "EventArea", dto);
        ////    }

        ////    return RedirectToAction("Index", "EventHomePage");
        ////}
    }
}
