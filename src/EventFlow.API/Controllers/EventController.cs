using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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

        /// <summary>
        /// Get all events.
        /// </summary>
        /// <returns>Returns events.</returns>
        [HttpGet("getAll")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetAll()
        {
            var events = _eventService.GetAll();
            return Ok(events);
        }

        /// <summary>
        /// Get last event.
        /// </summary>
        /// <returns>Return event.</returns>
        [HttpGet("getLast")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetLast()
        {
            var eventDto = _eventService.Last();
            return Ok(eventDto);
        }

        /// <summary>
        /// Get event by id.
        /// </summary>
        /// <param name="id">Event id.</param>
        /// <returns>Returns event.</returns>
        [HttpGet("getByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByIDAsync(int id)
        {
            var eventDto = await _eventService.GetByIDAsync(id);
            return Ok(eventDto);
        }

        /// <summary>
        /// Update event.
        /// </summary>
        /// <param name="eventDto">Event dto.</param>
        /// <returns>Return status code.</returns>
        [HttpPut("updateEvent")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateEvent([FromBody] EventDto eventDto)
        {
            await _eventService.UpdateAsync(eventDto);
            return Ok();
        }

        /// <summary>
        /// Update layout id.
        /// </summary>
        /// <param name="eventDto">Event dto.</param>
        /// <returns>Return status code.</returns>
        [HttpPut("updateLayoutId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateLayoutId([FromBody] EventDto eventDto)
        {
            await _eventService.UpdateLayoutIdAsync(eventDto);
            return Ok();
        }

        /// <summary>
        /// Delete event by id.
        /// </summary>
        /// <param name="id">Event id.</param>
        /// <returns>Return status code.</returns>
        [HttpDelete("deleteById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteById(int id)
        {
            await _eventService.DeleteAsync(new EventDto { Id = id });
            return Ok();
        }
    }
}
