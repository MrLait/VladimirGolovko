using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        [HttpGet("getAllByEventId")]
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
            catch (ValidationException)
            {
                return RedirectToAction("Index", "EventHomePage");
            }
        }

        [HttpPost("updatePrices")]
        public async Task<IActionResult> UpdatePrices([FromBody] List<EventAreaDto> eventAreaDto)
        {
            try
            {
                await _eventAreaService.UpdatePriceAsync(eventAreaDto);
                return Ok();
            }
            catch (ValidationException)
            {
                return BadRequest();
            }
        }
    }
}
