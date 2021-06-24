using System.Collections.Generic;
using TicketManagement.Dto.Interfaces;

namespace TicketManagement.Dto
{
    /// <summary>
    /// Venue data transfer object class.
    /// </summary>
    public class VenueDto : IDtoEntity
    {
        /// <summary>
        /// Gets or sets Id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets Description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets Address.
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Gets or sets Phone.
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// Gets or sets Layouts.
        /// </summary>
        public IEnumerable<LayoutDto> Layouts { get; set; }
    }
}
