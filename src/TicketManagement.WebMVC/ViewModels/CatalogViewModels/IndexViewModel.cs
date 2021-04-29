using System.Collections.Generic;
using TicketManagement.Dto;

namespace TicketManagement.WebMVC.ViewModels.CatalogViewModels
{
    public class IndexViewModel
    {
        public IEnumerable<EventDto> EventItems { get; set; }
    }
}
