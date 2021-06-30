using System.Collections.Generic;
using System.Linq;
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
    /// Event area controller api.
    /// </summary>
    [Route("[controller]")]
    [Authorize]
    [ApiController]
    public class EventAreaController : ControllerBase
    {
        private readonly IEventAreaService _eventAreaService;
        private readonly IEventSeatService _eventSeatService;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventAreaController"/> class.
        /// </summary>
        /// <param name="eventAreaService">Event area <see cref="IEventAreaService"/> service.</param>
        /// <param name="eventSeatService">Event seat <see cref="IEventSeatService"/> service.</param>
        public EventAreaController(IEventAreaService eventAreaService, IEventSeatService eventSeatService)
        {
            _eventAreaService = eventAreaService;
            _eventSeatService = eventSeatService;
        }

        /// <summary>
        /// Get all event are by event id.
        /// </summary>
        /// <param name="id">Event id.</param>
        /// <returns>Returns event areas dto or <see cref="ValidationException"/>.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult GetAllByEventId(int id)
        {
            try
            {
                var eventAreaDtoList = _eventAreaService.GetByEventId(id).ToList();
                foreach (var item in eventAreaDtoList)
                {
                    var eventSeatDto = _eventSeatService.GetByEventAreaId(item);
                    item.EvenSeats = eventSeatDto;
                }

                return Ok(eventAreaDtoList);
            }
            catch (ValidationException e)
            {
                var validationExceptionSerialized = JsonConvert.SerializeObject(e);
                return BadRequest(validationExceptionSerialized);
            }
        }

        /// <summary>
        /// Update prices.
        /// </summary>
        /// <param name="eventAreaDto">Event areas dto.</param>
        /// <returns>Returns status codes.</returns>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = UserRoles.EventManager)]
        public async Task<IActionResult> UpdatePrices([FromBody] List<EventAreaDto> eventAreaDto)
        {
            try
            {
                await _eventAreaService.UpdatePriceAsync(eventAreaDto);
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
