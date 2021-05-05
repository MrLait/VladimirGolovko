using System.Collections.Generic;
using TicketManagement.Dto.Interfaces;

namespace TicketManagement.Dto
{
    /// <summary>
    /// EventArea data transfer object class.
    /// </summary>
    public class EventAreaDto : IDtoEntity
    {
        /// <summary>
        /// Gets or sets id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets EventId.
        /// </summary>
        public int EventId { get; set; }

        /// <summary>
        /// Gets or sets Description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets CoordX.
        /// </summary>
        public int CoordX { get; set; }

        /// <summary>
        /// Gets or sets CoordY.
        /// </summary>
        public int CoordY { get; set; }

        /// <summary>
        /// Gets or sets Price.
        /// </summary>
        public decimal Price { get; set; }

        public IEnumerable<EventSeatDto> EvenSeats { get; set; }
    }
}
