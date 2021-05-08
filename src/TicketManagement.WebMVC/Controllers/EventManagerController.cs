using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TicketManagement.BusinessLogic.Interfaces;
using TicketManagement.Dto;
using TicketManagement.WebMVC.Models;
using TicketManagement.WebMVC.ViewModels;
using TicketManagement.WebMVC.ViewModels.EventViewModels;

namespace TicketManagement.WebMVC.Controllers
{
    [Authorize(Roles = "eventManager")]
    public class EventManagerController : Controller
    {
        private readonly IEventService _eventService;
        private readonly IEventAreaService _eventAreaService;
        private readonly IMapper _mapper;

        public EventManagerController(UserManager<ApplicationUser> applicationUser, IEventService eventService, IEventAreaService eventAreaService, IMapper mapper)
        {
            _eventService = eventService;
            _eventAreaService = eventAreaService;
            _mapper = mapper;
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
            if (ModelState.IsValid)
            {
                var eventDto = new EventDto
                {
                    Name = model.Name,
                    Description = model.Description,
                    LayoutId = model.LayoutId,
                    StartDateTime = model.StartDateTime,
                    EndDateTime = model.EndDateTime,
                    ImageUrl = model.ImageUrl,
                };
                if (model.Id == 0)
                {
                    await _eventService.CreateAsync(eventDto);
                    var lastAddedEvent = _eventService.Last();
                    model.Id = lastAddedEvent.Id;
                    model.Name = lastAddedEvent.Name;
                    model.Description = lastAddedEvent.Description;
                    model.LayoutId = lastAddedEvent.LayoutId;
                    model.StartDateTime = lastAddedEvent.StartDateTime;
                    model.EndDateTime = lastAddedEvent.EndDateTime;
                    model.ImageUrl = lastAddedEvent.ImageUrl;

                    var eventsDto = _eventAreaService.GetByEventId(lastAddedEvent).ToList();
                    model.EventAreaItems = _mapper.Map<List<EventAreaDto>, List<EventAreaItem>>(eventsDto);
                    return View(model);
                }

                var testMap = _mapper.Map<List<EventAreaItem>, List<EventAreaDto>>(model.EventAreaItems);
                await _eventAreaService.UpdatePriceAsync(testMap);
                return RedirectToAction("Index", "EventManager");
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

            EventViewModel model = new EventViewModel
            {
                Id = id,
                Name = eventItem.Name,
                Description = eventItem.Description,
                EndDateTime = eventItem.EndDateTime,
                ImageUrl = eventItem.ImageUrl,
                LayoutId = eventItem.LayoutId,
                StartDateTime = eventItem.StartDateTime,
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateEvent(EventViewModel model)
        {
            if (ModelState.IsValid)
            {
                var eventDto = new EventDto
                {
                    Id = model.Id,
                    Name = model.Name,
                    Description = model.Description,
                    LayoutId = model.LayoutId,
                    StartDateTime = model.StartDateTime,
                    EndDateTime = model.EndDateTime,
                    ImageUrl = model.ImageUrl,
                };

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
                var eventDto = new EventDto
                {
                    Id = model.Id,
                    Name = model.Name,
                    Description = model.Description,
                    LayoutId = model.LayoutId,
                    StartDateTime = model.StartDateTime,
                    EndDateTime = model.EndDateTime,
                    ImageUrl = model.ImageUrl,
                };

                await _eventService.UpdateLayoutIdAsync(eventDto);
                return RedirectToAction("Index", "EventManager");
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteEventAsync(int id)
        {
            await _eventService.DeleteAsync(new EventDto { Id = id });

            return RedirectToAction("Index", "EventManager");
        }
    }
}
