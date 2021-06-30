using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TicketManagement.Dto;
using TicketManagement.Services.EventFlow.API.Infrastructure.Exceptions;
using TicketManagement.Services.EventFlow.API.Infrastructure.Services.Interfaces;
using TicketManagement.Services.Identity.Domain.Models;

namespace TicketManagement.Services.EventFlow.API.Controllers
{
    /// <summary>
    /// Event controller api.
    /// </summary>
    [Route("[controller]")]
    [Authorize]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly IEventService _eventService;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventController"/> class.
        /// </summary>
        /// <param name="eventService">Event service.</param>
        public EventController(IEventService eventService)
        {
            _eventService = eventService;
        }

        /// <summary>
        /// Get all events.
        /// </summary>
        /// <returns>Returns events.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [AllowAnonymous]
        public IActionResult GetAll()
        {
            var events = _eventService.GetAll();
            return Ok(events);
        }

        /// <summary>
        /// Get last event.
        /// </summary>
        /// <returns>Return event.</returns>
        [HttpGet("get-last")]
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
        [HttpGet("get-by-id")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var eventDto = await _eventService.GetByIdAsync(id);
            return Ok(eventDto);
        }

        /// <summary>
        /// Update event.
        /// </summary>
        /// <param name="eventDto">Event dto.</param>
        /// <returns>Return status code.</returns>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = UserRoles.EventManager)]
        public async Task<IActionResult> UpdateEvent([FromBody] EventDto eventDto)
        {
            try
            {
                await _eventService.UpdateAsync(eventDto);
                return Ok();
            }
            catch (ValidationException e)
            {
                var validationExceptionSerialized = JsonConvert.SerializeObject(e);
                return BadRequest(validationExceptionSerialized);
            }
        }

        /// <summary>
        /// Delete event by id.
        /// </summary>
        /// <param name="id">Event id.</param>
        /// <returns>Return status code.</returns>
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = UserRoles.EventManager)]
        public async Task<IActionResult> DeleteById(int id)
        {
            try
            {
                await _eventService.DeleteAsync(new EventDto { Id = id });
                return Ok();
            }
            catch (ValidationException e)
            {
                var validationExceptionSerialized = JsonConvert.SerializeObject(e);
                return BadRequest(validationExceptionSerialized);
            }
        }
    }
}
