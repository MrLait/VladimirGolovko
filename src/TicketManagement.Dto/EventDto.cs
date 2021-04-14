using System;
using TicketManagement.Dto.Interfaces;

namespace TicketManagement.Dto
{
    /// <summary>
    /// Event data transfer object class.
    /// </summary>
    internal class EventDto : IDtoEntity
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
        /// Gets or sets DateTime.
        /// </summary>
        public DateTime DateTime { get; set; }

        /// <summary>
        /// Gets or sets Price.
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Gets or sets State.
        /// </summary>
        public int State { get; set; }
    }
}
