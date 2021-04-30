using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketManagement.Dto;

namespace TicketManagement.WebMVC.ViewModels.EventSeatViewModels
{
    public class IndexViewModel
    {
        public IEnumerable<EventSeatDto> EvenSeatItems { get; set; }
    }
}
