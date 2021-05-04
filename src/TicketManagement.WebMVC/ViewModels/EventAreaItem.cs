using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketManagement.Dto;

namespace TicketManagement.WebMVC.ViewModels
{
    public record EventAreaItem
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
        public decimal Price { get; init; }

        public IEnumerable<EventSeatItem> EvenSeatItems { get; init; }
        public List<EventSeatDto> EvenSeats { get; internal set; }
    }
}
