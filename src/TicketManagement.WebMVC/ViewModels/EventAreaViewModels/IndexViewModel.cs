using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketManagement.Dto;

namespace TicketManagement.WebMVC.ViewModels.EventAreaViewModels
{
    public record IndexViewModel
    {
        public IEnumerable<EventAreaDto> EvenAreatItems { get; init; }
    }
}
