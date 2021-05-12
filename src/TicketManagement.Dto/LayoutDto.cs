using System.Collections.Generic;
using TicketManagement.Dto.Interfaces;

namespace TicketManagement.Dto
{
    /// <summary>
    /// Layout data transfer object class.
    /// </summary>
    public class LayoutDto : IDtoEntity
    {
        /// <summary>
        /// Gets or sets Id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets VenueId.
        /// </summary>
        public int VenueId { get; set; }

        /// <summary>
        /// Gets or sets Description.
        /// </summary>
        public string Description { get; set; }

        public IEnumerable<EventDto> Events { get; set; }
    }
}
