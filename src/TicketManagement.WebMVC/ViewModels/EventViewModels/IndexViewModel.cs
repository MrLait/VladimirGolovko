using System.Collections.Generic;
using TicketManagement.Dto;

namespace TicketManagement.WebMVC.ViewModels.EventViewModels
{
    public class IndexViewModel
    {
        public IEnumerable<EventDto> EventItems { get; set; }
    }
}
