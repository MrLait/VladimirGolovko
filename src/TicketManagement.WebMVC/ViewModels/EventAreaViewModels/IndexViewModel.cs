using System.Collections.Generic;
using TicketManagement.Dto;

namespace TicketManagement.WebMVC.ViewModels.EventAreaViewModels
{
    /// <summary>
    /// Index view model.
    /// </summary>
    public record IndexViewModel
    {
        /// <summary>
        /// Gets or sets EventAreasItems.
        /// </summary>
        public IEnumerable<EventAreaDto> EventAreasItems { get; init; }
    }
}
