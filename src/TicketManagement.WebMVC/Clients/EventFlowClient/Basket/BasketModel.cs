using System;
using System.Collections.Generic;
using System.Linq;

namespace TicketManagement.WebMVC.Clients.EventFlowClient.Basket
{
    /// <summary>
    /// Basket model.
    /// </summary>
    public class BasketModel
    {
        /// <summary>
        /// Gets or init basket items.
        /// </summary>
        public List<BasketItem> Items { get; init; } = new ();

        /// <summary>
        /// Gets or init user id.
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Gets total price.
        /// </summary>
        public decimal TotalPrice => Math.Round(Items.Sum(x => x.Price), 2);
    }
}
