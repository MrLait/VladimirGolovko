using Microsoft.AspNetCore.Mvc;
using TicketManagement.Services.EventFlow.API.Infrastructure.Services.Interfaces;

namespace TicketManagement.Services.EventFlow.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly IEventService _eventService;

        public EventController(IEventService eventService)
        {
            _eventService = eventService;
        }

        [HttpGet("getAll")]
        public IActionResult GetAll()
        {
            var events = _eventService.GetAll();
            return Ok(events);
        }

        [HttpGet("getLast")]
        public IActionResult GetLast()
        {
            var eventDto = _eventService.Last();
            return Ok(eventDto);
        }
    }
}
