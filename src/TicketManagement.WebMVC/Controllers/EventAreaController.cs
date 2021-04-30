using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TicketManagement.BusinessLogic.Interfaces;
using TicketManagement.Dto;
using TicketManagement.WebMVC.ViewModels.EventAreaViewModels;

namespace TicketManagement.WebMVC.Controllers
{
    public class EventAreaController : Controller
    {
        private readonly IEventAreaService _eventAreaService;
        private readonly IEventSeatService _eventSeatService;

        public EventAreaController(IEventAreaService eventAreaService, IEventSeatService eventSeatService)
        {
            _eventAreaService = eventAreaService;
            _eventSeatService = eventSeatService;
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

        public IActionResult AddToCart(int eventId, int seatId)
        {
            return RedirectToAction("Index", "EventArea", new EventDto { Id = eventId });
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
