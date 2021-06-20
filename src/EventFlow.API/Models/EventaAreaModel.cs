using System.Collections.Generic;
using TicketManagement.Dto;

namespace TicketManagement.Services.EventFlow.API.Models
{
    public record EventaAreaModel
    {
        public IEnumerable<EventAreaDto> EvenAreatItems { get; init; }
    }
}
