using System.Collections.Generic;

namespace TicketManagement.WebMVC.ViewModels
{
    public class LayoutItem
    {
        /// <summary>
        /// Id column in table.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// VenueId column in table.
        /// </summary>
        public int VenueId { get; set; }

        /// <summary>
        /// Description column in table.
        /// </summary>
        public string Description { get; set; }

        public List<VenueItem> VenueItems { get; set; }
    }
}
