using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TicketManagement.BusinessLogic.Interfaces;
using TicketManagement.WebMVC.ViewModels.EventSeatViewModels;

namespace TicketManagement.WebMVC.Controllers
{
    public class EventSeatController : Controller
    {
        private readonly IEventSeatService _eventSeatService;

        public EventSeatController(IEventSeatService eventSeatService)
        {
            _eventSeatService = eventSeatService;
        }

        public async Task<IActionResult> Index(int id)
        {
            var eventSeatDto = (await _eventSeatService.GetAllAsync()).Where(x => x.EventAreaId == id);
            var vm = new IndexViewModel
            {
                EvenSeatItems = eventSeatDto,
            };
            return View(vm);
        }

        [HttpPost]
        public IActionResult Create(string name)
        {
            return View();
        }

        public IActionResult GetEventAreaId(int id)
        {
            if (id != 0)
            {
                return RedirectToAction("Index", "EventSeat", id);
            }

            return RedirectToAction("Index", "EventHomePage");
        }
    }
}
