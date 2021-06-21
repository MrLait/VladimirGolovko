using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TicketManagement.Dto;
using TicketManagement.Services.EventFlow.API.Infrastructure.Services.Interfaces;

namespace TicketManagement.Services.EventFlow.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class EventManagerController : ControllerBase
    {
        private readonly IEventService _eventService;

        public EventManagerController(IEventService eventService)
        {
            _eventService = eventService;
        }

        /// <summary>
        /// Create event.
        /// </summary>
        /// <param name="model">Event dto.</param>
        /// <returns>Return status code.</returns>
        [HttpPost("createEvent")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateEvent([FromBody] EventDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var eventDto = model;
            await _eventService.CreateAsync(eventDto);
            return Ok();
        }
    }
}
