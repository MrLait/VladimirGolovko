using System;

namespace TicketManagement.WebMVC.ViewModels.EventViewModels
{
    public class EventViewModel
    {
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets name column in table.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets description column in table.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets layoutId column in table.
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
        /// Gets or sets image url column in table.
        /// </summary>
        public string ImageUrl { get; set; }
    }
}
