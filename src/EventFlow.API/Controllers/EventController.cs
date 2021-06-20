using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TicketManagement.Dto;
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

        [HttpGet("getByID")]
        public async Task<IActionResult> GetByIDAsync(int id)
        {
            var eventDto = await _eventService.GetByIDAsync(id);
            return Ok(eventDto);
        }

        [HttpPost("updateEvent")]
        public async Task<IActionResult> UpdateEvent([FromBody] EventDto eventDto)
        {
            await _eventService.UpdateAsync(eventDto);
            return Ok();
        }

        [HttpPost("updateLayoutId")]
        public async Task<IActionResult> UpdateLayoutId([FromBody] EventDto eventDto)
        {
            await _eventService.UpdateLayoutIdAsync(eventDto);
            return Ok();
        }

        [HttpDelete("deleteById")]
        public async Task<IActionResult> DeleteById(int id)
        {
            await _eventService.DeleteAsync(new EventDto { Id = id });
            return Ok();
        }
    }
}
