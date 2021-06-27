using System.Collections.Generic;
using TicketManagement.Dto;

namespace TicketManagement.WebMVC.ViewModels.EventViewModels
{
    /// <summary>
    /// Index view model.
    /// </summary>
    public class IndexViewModel
    {
        /// <summary>
        /// Gets or sets EventItems.
        /// </summary>
        public IEnumerable<EventDto> EventItems { get; init; }
    }
}
