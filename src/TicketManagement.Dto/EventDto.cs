using System;
using TicketManagement.Dto.Interfaces;

namespace TicketManagement.Dto
{
    /// <summary>
    /// Event data transfer object class.
    /// </summary>
    public class EventDto : IDtoEntity
    {
        /// <summary>
        /// Gets or sets id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets Name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets Description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets LayoutId.
        /// </summary>
        public int LayoutId { get; set; }

        /// <summary>
        /// Gets or sets start date time column in table.
        /// </summary>
        public DateTime StartDateTime { get; set; }

        /// <summary>
        /// Gets or sets end date time column in table.
        /// </summary>
        public DateTime EndDateTime { get; set; }

        /// <summary>
        /// Gets or sets min price.
        /// </summary>
        public decimal PriceFrom { get; set; }

        /// <summary>
        /// Gets or sets max price.
        /// </summary>
        public decimal PriceTo { get; set; }

        /// <summary>
        /// Gets or sets State.
        /// </summary>
        public int State { get; set; }

        /// <summary>
        /// Gets or sets image url column in table.
        /// </summary>
        public string ImageUrl { get; set; }

        /// <summary>
        /// Gets or sets available seats.
        /// </summary>
        public int AvailableSeats { get; set; }
    }
}
