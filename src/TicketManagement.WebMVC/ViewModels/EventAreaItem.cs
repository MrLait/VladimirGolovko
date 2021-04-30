using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketManagement.Dto;

namespace TicketManagement.WebMVC.ViewModels
{
    public class EventAreaItem
    {
        public IEnumerable<EventSeatDto> EvenSeatItems { get; set; }
    }
}
