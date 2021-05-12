using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TicketManagement.WebMVC.ViewModels
{
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
