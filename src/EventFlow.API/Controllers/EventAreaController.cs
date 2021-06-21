using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TicketManagement.Dto;
using TicketManagement.Services.EventFlow.API.Infrastructure.Exceptions;
using TicketManagement.Services.EventFlow.API.Infrastructure.Services.Interfaces;

namespace TicketManagement.Services.EventFlow.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class EventAreaController : ControllerBase
    {
        private readonly IEventAreaService _eventAreaService;
        private readonly IEventSeatService _eventSeatService;

        public EventAreaController(IEventAreaService eventAreaService, IEventSeatService eventSeatService)
        {
            _eventAreaService = eventAreaService;
            _eventSeatService = eventSeatService;
        }

        /// <summary>
        /// Get all event are by event id.
        /// </summary>
        /// <param name="id">Event id.</param>
        /// <returns>Returns event areas dto.</returns>
        [HttpGet("getAllByEventId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult GetAllByEventId(int id)
        {
            try
            {
                var eventAreaDto = _eventAreaService.GetByEventId(id);

                for (int i = 0; i < eventAreaDto.Count(); i++)
                {
                    var eventSeatDto = _eventSeatService.GetByEventAreaId(eventAreaDto.ToList()[i]);
                    eventAreaDto.ToList()[i].EvenSeats = eventSeatDto;
                }

                return Ok(eventAreaDto);
            }
            catch (ValidationException e)
            {
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        /// Update prices.
        /// </summary>
        /// <param name="eventAreaDto">Event areas dto.</param>
        /// <returns>Returns status codes.</returns>
        [HttpPut("updatePrices")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdatePrices([FromBody] List<EventAreaDto> eventAreaDto)
        {
            try
            {
                await _eventAreaService.UpdatePriceAsync(eventAreaDto);
                return Ok();
            }
            catch (ValidationException e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
