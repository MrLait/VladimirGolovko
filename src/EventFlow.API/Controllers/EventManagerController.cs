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
    /// Event manager controller api.
    /// </summary>
    [Route("[controller]")]
    [Authorize(Roles = UserRoles.EventManager)]
    [ApiController]
    public class EventManagerController : ControllerBase
    {
        private readonly IEventService _eventService;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventManagerController"/> class.
        /// </summary>
        /// <param name="eventService">Event service.</param>
        public EventManagerController(IEventService eventService)
        {
            _eventService = eventService;
        }

        /// <summary>
        /// Create event.
        /// </summary>
        /// <param name="model">Event dto.</param>
        /// <returns>Return status code or validation exception.</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateEvent([FromBody] EventDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                await _eventService.CreateAsync(model);
                return Ok(model);
            }
            catch (ValidationException e)
            {
                var validationExceptionSerialized = JsonConvert.SerializeObject(e);
                return BadRequest(validationExceptionSerialized);
            }
        }
    }
}
