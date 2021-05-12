using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TicketManagement.Dto;

namespace TicketManagement.WebMVC.ViewModels.EventViewModels
{
    public class EventAreaItem
    {
        /// <summary>
        /// Gets or sets id.
        /// </summary>
        public int Id { get; init; }

        /// <summary>
        /// Gets or sets EventId.
        /// </summary>
        public int EventId { get; init; }

        /// <summary>
        /// Gets or sets Description.
        /// </summary>
        public string Description { get; init; }

        /// <summary>
        /// Gets or sets CoordX.
        /// </summary>
        public int CoordX { get; init; }

        /// <summary>
        /// Gets or sets CoordY.
        /// </summary>
        public int CoordY { get; init; }

        /// <summary>
        /// Gets or sets Price.
        /// </summary>
        [Required(ErrorMessage = "PriceRequired")]
        [Display(Name = "Price")]
        public decimal Price { get; init; }

        public IEnumerable<EventSeatItem> EvenSeatItems { get; set; }

        public List<EventSeatDto> EvenSeats { get; set; }
    }
}
