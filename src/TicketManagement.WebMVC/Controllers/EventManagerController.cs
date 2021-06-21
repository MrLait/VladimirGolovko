using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using TicketManagement.Dto;
using TicketManagement.WebMVC.Clients.EventFlowClient.Event;
using TicketManagement.WebMVC.Clients.EventFlowClient.EventArea;
using TicketManagement.WebMVC.Clients.EventFlowClient.EventManager;
using TicketManagement.WebMVC.Infrastructure.ExceptionsMessages;
using TicketManagement.WebMVC.Models;
using TicketManagement.WebMVC.ViewModels.EventViewModels;

namespace TicketManagement.WebMVC.Controllers
{
    [Authorize(Roles = UserRoles.EventManager)]
    public class EventManagerController : Controller
    {
        private readonly IEventClient _eventClient;
        private readonly IEventAreaClient _eventAreaClient;
        private readonly IEventManagerClient _eventManagerClient;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<EventManagerController> _localizer;

        public EventManagerController(
            IEventClient eventClient,
            IEventManagerClient eventManagerClient,
            IEventAreaClient eventAreaClient,
            IMapper mapper,
            IStringLocalizer<EventManagerController> localizer)
        {
            _eventAreaClient = eventAreaClient;
            _eventManagerClient = eventManagerClient;
            _eventClient = eventClient;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<IActionResult> Index()
        {
            var eventCatalog = await _eventClient.GetAllAsync();
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
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                var eventDto = _mapper.Map<EventViewModel, EventDto>(model);

                if (model.Id == 0)
                {
                    await _eventManagerClient.CreateEvent(eventDto);
                    var lastAddedEvent = await _eventClient.GetLastAsync();
                    model = _mapper.Map<EventDto, EventViewModel>(lastAddedEvent);
                    var eventsDto = (await _eventAreaClient.GetAllByEventIdAsync(lastAddedEvent.Id)).ToList();
                    model.EventAreaItems = _mapper.Map<List<EventAreaDto>, List<EventAreaItem>>(eventsDto);
                    return View(model);
                }

                var eventAreas = _mapper.Map<List<EventAreaItem>, List<EventAreaDto>>(model.EventAreaItems);
                await _eventAreaClient.UpdatePricesAsync(eventAreas);
                return RedirectToAction("Index", "EventManager");
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
        }

        [HttpGet]
        public async Task<IActionResult> UpdateEventAsync(int id)
        {
            var eventItem = await _eventClient.GetByIDAsync(id);
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
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var eventDto = _mapper.Map<EventViewModel, EventDto>(model);
            await _eventClient.UpdateEventAsync(eventDto);
            return RedirectToAction("Index", "EventManager");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateLayoutId(EventViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var eventDto = _mapper.Map<EventViewModel, EventDto>(model);
            await _eventClient.UpdateLayoutIdAsync(eventDto);
            return RedirectToAction("Index", "EventManager");
        }

        [HttpGet]
        public async Task<IActionResult> DeleteEventAsync(int id)
        {
            try
            {
                await _eventClient.DeleteByIdAsync(id);
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
