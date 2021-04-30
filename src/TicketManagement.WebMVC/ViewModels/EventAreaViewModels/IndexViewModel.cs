using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketManagement.Dto;

namespace TicketManagement.WebMVC.ViewModels.EventAreaViewModels
{
    public class IndexViewModel
    {
        public IEnumerable<EventAreaDto> EvenAreatItems { get; set; }
    }
}
