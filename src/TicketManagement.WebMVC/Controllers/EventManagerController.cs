using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using TicketManagement.Dto;
using TicketManagement.WebMVC.Clients.EventFlowClient.Event;
using TicketManagement.WebMVC.Clients.EventFlowClient.EventArea;
using TicketManagement.WebMVC.Clients.EventFlowClient.EventManager;
using TicketManagement.WebMVC.Infrastructure.ExceptionsMessages;
using TicketManagement.WebMVC.Models;
using TicketManagement.WebMVC.ViewModels.EventViewModels;

namespace TicketManagement.WebMVC.Controllers
{
    /// <summary>
    /// Event manager controller.
    /// </summary>
    [Authorize(Roles = UserRoles.EventManager)]
    public class EventManagerController : Controller
    {
        private readonly IEventClient _eventClient;
        private readonly IEventAreaClient _eventAreaClient;
        private readonly IEventManagerClient _eventManagerClient;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<EventManagerController> _localizer;
        private readonly ILogger<EventManagerController> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventManagerController"/> class.
        /// </summary>
        /// <param name="eventClient">Event client.</param>
        /// <param name="eventManagerClient">Event manager client.</param>
        /// <param name="eventAreaClient">Event area client.</param>
        /// <param name="mapper">Mapper.</param>
        /// <param name="localizer">Localizer.</param>
        /// <param name="logger">Logger.</param>
        public EventManagerController(
            IEventClient eventClient,
            IEventManagerClient eventManagerClient,
            IEventAreaClient eventAreaClient,
            IMapper mapper,
            IStringLocalizer<EventManagerController> localizer,
            ILogger<EventManagerController> logger)
        {
            _eventAreaClient = eventAreaClient;
            _eventManagerClient = eventManagerClient;
            _eventClient = eventClient;
            _mapper = mapper;
            _localizer = localizer;
            _logger = logger;
        }

        /// <summary>
        /// View index.
        /// </summary>
        public async Task<IActionResult> Index()
        {
            var eventCatalog = await _eventClient.GetAllAsync();
            var vm = new IndexViewModel
            {
                EventItems = eventCatalog,
            };

            return View(vm);
        }

        /// <summary>
        /// Create event view.
        /// </summary>
        [HttpGet]
        public IActionResult CreateEvent()
        {
            return View();
        }

        /// <summary>
        /// Create event view.
        /// </summary>
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
                ModelValidation(ve);

                _logger.LogError("{DateTime} {Error} ", DateTime.UtcNow, ve);
                return View(model);
            }
        }

        /// <summary>
        /// Update event view.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> UpdateEvent(int id)
        {
            var eventItem = await _eventClient.GetByIdAsync(id);
            if (eventItem == null)
            {
                return NotFound();
            }

            var model = _mapper.Map<EventDto, EventViewModel>(eventItem);
            return View(model);
        }

        /// <summary>
        /// Update event view.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> UpdateEvent(EventViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var eventDto = _mapper.Map<EventViewModel, EventDto>(model);
                ////await _eventClient.UpdateEventAsync(eventDto);

                await _eventClient.UpdateEventAsync(eventDto);
                return RedirectToAction("Index", "EventManager");
            }
            catch (ValidationException ve)
            {
                ModelValidation(ve);
                _logger.LogError("{DateTime} {Error} ", DateTime.UtcNow, ve);
                return View(model);
            }
        }

        /// <summary>
        /// Delete event get action.
        /// </summary>
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

                _logger.LogError("{DateTime} {Error} ", DateTime.UtcNow, ex);
                return RedirectToAction("Index", "EventManager");
            }
        }

        private void ModelValidation(ValidationException ve)
        {
            switch (ve.Message)
            {
                case ExceptionMessages.PriceIsZero:
                    ViewData["PriceRequired"] = _localizer["Price can't equal zero."];
                    break;
                case ExceptionMessages.PriceIsNegative:
                    ViewData["PriceRequired"] = _localizer["Price can't negative."];
                    break;
                case ExceptionMessages.CantBeCreatedInThePast:
                    ModelState.AddModelError("StartDateTime", _localizer["The event can't be created in the past."]);
                    break;
                case ExceptionMessages.StartDataTimeBeforeEndDataTime:
                    ModelState.AddModelError("StartDateTime", _localizer["The beginning of the event cannot be after the end of the event."]);
                    break;
                case ExceptionMessages.ThereIsNoSuchLayout:
                    ModelState.AddModelError("StartDateTime", _localizer["There is no such Layout."]);
                    break;
                case ExceptionMessages.ThereAreNoSeatsInTheEvent:
                    ModelState.AddModelError(string.Empty, _localizer["There are no seats in the event."]);
                    break;
                case ExceptionMessages.EventForTheSameVenueInTheSameDateTime:
                    ModelState.AddModelError(string.Empty, _localizer["Event for the Venue with the dateTime already exist."]);
                    break;
                case ExceptionMessages.SeatsHaveAlreadyBeenPurchased:
                    Content(_localizer["SeatsHaveAlreadyBeenPurchased"]);
                    break;
                default:
                    break;
            }
        }
    }
}
