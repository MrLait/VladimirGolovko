using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicketManagement.Dto;
using TicketManagement.Services.EventFlow.API.Infrastructure.Services.Interfaces;
using TicketManagement.Services.Identity.Domain.Models;

namespace TicketManagement.Services.EventFlow.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class EventManagerController : ControllerBase
    {
        private readonly IEventService _eventService;
        private readonly IEventAreaService _eventAreaService;
        ////private readonly IMapper _mapper;

        public EventManagerController(IEventService eventService, IEventAreaService eventAreaService)
            //////, IMapper mapper)
        {
            _eventService = eventService;
            _eventAreaService = eventAreaService;
            ////_mapper = mapper;
        }

        [HttpPost("createEvent")]
        public async Task<IActionResult> CreateEvent([FromBody] EventDto model)
        {
            if (ModelState.IsValid)
            {
                var eventDto = model;
                if (model.Id == 0)
                {
                    await _eventService.CreateAsync(eventDto);
                    return Ok();
                }

                ////var eventAreas = _mapper.Map<List<EventAreaItem>, List<EventAreaDto>>(model.EventAreaItems);
                ////await _eventAreaService.UpdatePriceAsync(eventAreas);
                ////return RedirectToAction("Index", "EventManager");
            }

            return Ok(model);
        }

            ////    else
            ////    {
            ////        ViewData["PriceRequired"] = _localizer["PriceRequired"];
            ////    }
            ////}
            ////catch (ValidationException ve)
            ////{
            ////    if (ve.Message == ExceptionMessages.PriceIsZero)
            ////    {
            ////        ViewData["PriceRequired"] = _localizer["PriceRequired"];
            ////    }

            ////    if (ve.Message == ExceptionMessages.PriceIsNegative)
            ////    {
            ////        ViewData["PriceRequired"] = _localizer["PriceRequired"];
            ////    }

            ////    if (ve.Message == ExceptionMessages.CantBeCreatedInThePast)
            ////    {
            ////        ModelState.AddModelError("StartDateTime", _localizer["The event can't be created in the past"]);
            ////    }

            ////    return View(model);
            ////}

            ////return View(model);

            ////[HttpGet]
            ////public async Task<IActionResult> UpdateEventAsync(int id)
            ////{
            ////    var eventItem = await _eventService.GetByIDAsync(id);
            ////    if (eventItem == null)
            ////    {
            ////        return NotFound();
            ////    }

            ////    var model = _mapper.Map<EventDto, EventViewModel>(eventItem);
            ////    return View(model);
            ////}

            ////[HttpPost]
            ////public async Task<IActionResult> UpdateEvent(EventViewModel model)
            ////{
            ////    if (ModelState.IsValid)
            ////    {
            ////        var eventDto = _mapper.Map<EventViewModel, EventDto>(model);

            ////        await _eventService.UpdateAsync(eventDto);
            ////        return RedirectToAction("Index", "EventManager");
            ////    }

            ////    return View(model);
            ////}

            ////[HttpPost]
            ////public async Task<IActionResult> UpdateLayoutId(EventViewModel model)
            ////{
            ////    if (ModelState.IsValid)
            ////    {
            ////        var eventDto = _mapper.Map<EventViewModel, EventDto>(model);

            ////        await _eventService.UpdateLayoutIdAsync(eventDto);
            ////        return RedirectToAction("Index", "EventManager");
            ////    }

            ////    return View(model);
            ////}

            ////[HttpGet]
            ////public async Task<IActionResult> DeleteEventAsync(int id)
            ////{
            ////    try
            ////    {
            ////        await _eventService.DeleteAsync(new EventDto { Id = id });
            ////        return RedirectToAction("Index", "EventManager");
            ////    }
            ////    catch (ValidationException ex)
            ////    {
            ////        if (ex.Message == ExceptionMessages.SeatsHaveAlreadyBeenPurchased)
            ////        {
            ////            return Content(_localizer["SeatsHaveAlreadyBeenPurchased"]);
            ////        }

            ////        return RedirectToAction("Index", "EventManager");
            ////    }
            ////}
        }
}
