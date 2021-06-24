namespace TicketManagement.WebMVC.ViewModels
{
    /// <summary>
    /// Venue item.
    /// </summary>
    public class VenueItem
    {
        /// <summary>
        /// Gets or sets id column in table.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets description column in table.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets address column in table.
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Gets or sets phone column in table.
        /// </summary>
        public string Phone { get; set; }
    }
}
