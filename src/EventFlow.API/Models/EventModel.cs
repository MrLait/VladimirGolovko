using System.Collections.Generic;
using TicketManagement.Dto;

namespace TicketManagement.Services.EventFlow.API.Models
{
    public class EventModel
    {
        public IEnumerable<EventDto> EventItems { get; set; }
    }
}
