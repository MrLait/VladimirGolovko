using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using TicketManagement.BusinessLogic.Infrastructure;
using TicketManagement.BusinessLogic.Interfaces;
using TicketManagement.Dto;
using TicketManagement.WebMVC.ViewModels.EventViewModels;

namespace TicketManagement.WebMVC.Controllers
{
    [Authorize(Roles = "eventManager")]
    public class EventManagerController : Controller
    {
        private readonly IEventService _eventService;
        private readonly IEventAreaService _eventAreaService;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<EventManagerController> _localizer;

        public EventManagerController(IEventService eventService, IEventAreaService eventAreaService, IMapper mapper,
            IStringLocalizer<EventManagerController> localizer)
        {
            _eventService = eventService;
            _eventAreaService = eventAreaService;
            _mapper = mapper;
            _localizer = localizer;
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

        [HttpGet]
        public IActionResult CreateEvent()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateEvent(EventViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var eventDto = _mapper.Map<EventViewModel, EventDto>(model);
                    if (model.Id == 0)
                    {
                        await _eventService.CreateAsync(eventDto);
                        var lastAddedEvent = _eventService.Last();
                        model = _mapper.Map<EventDto, EventViewModel>(lastAddedEvent);
                        var eventsDto = _eventAreaService.GetByEventId(lastAddedEvent).ToList();
                        model.EventAreaItems = _mapper.Map<List<EventAreaDto>, List<EventAreaItem>>(eventsDto);
                        return View(model);
                    }

                    var eventAreas = _mapper.Map<List<EventAreaItem>, List<EventAreaDto>>(model.EventAreaItems);
                    await _eventAreaService.UpdatePriceAsync(eventAreas);
                    return RedirectToAction("Index", "EventManager");
                }
                else
                {
                    ViewData["PriceRequired"] = _localizer["PriceRequired"];
                }
            }
            catch (ValidationException ve)
            {
                if (ve.Message == ExceptionMessages.PriceIsZero)
                {
                    ViewData["PriceRequired"] = _localizer["PriceRequired"];
                }

                if (ve.Message == ExceptionMessages.PriceIsNegative)
                {
                    ViewData["PriceRequired"] = _localizer["PriceRequired"];
                }

                if (ve.Message == ExceptionMessages.CantBeCreatedInThePast)
                {
                    ModelState.AddModelError("StartDateTime", _localizer["The event can't be created in the past"]);
                }

                return View(model);
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> UpdateEventAsync(int id)
        {
            var eventItem = await _eventService.GetByIDAsync(id);
            if (eventItem == null)
            {
                return NotFound();
            }

            var model = _mapper.Map<EventDto, EventViewModel>(eventItem);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateEvent(EventViewModel model)
        {
            if (ModelState.IsValid)
            {
                var eventDto = _mapper.Map<EventViewModel, EventDto>(model);

                await _eventService.UpdateAsync(eventDto);
                return RedirectToAction("Index", "EventManager");
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateLayoutId(EventViewModel model)
        {
            if (ModelState.IsValid)
            {
                var eventDto = _mapper.Map<EventViewModel, EventDto>(model);

                await _eventService.UpdateLayoutIdAsync(eventDto);
                return RedirectToAction("Index", "EventManager");
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteEventAsync(int id)
        {
            try
            {
                await _eventService.DeleteAsync(new EventDto { Id = id });
                return RedirectToAction("Index", "EventManager");
            }
            catch (ValidationException ex)
            {
                if (ex.Message == ExceptionMessages.SeatsHaveAlreadyBeenPurchased)
                {
                    return Content(_localizer["SeatsHaveAlreadyBeenPurchased"]);
                }

                return RedirectToAction("Index", "EventManager");
            }
        }
    }
}
