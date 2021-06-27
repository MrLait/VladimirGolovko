using System;
using System.Collections.Generic;
using System.Linq;

namespace TicketManagement.Services.EventFlow.API.Models
{
    /// <summary>
    /// Basket model.
    /// </summary>
    public class BasketModel
    {
        /// <summary>
        /// Gets or sets basket items.
        /// </summary>
        public List<BasketItem> Items { get; } = new ();

        /// <summary>
        /// Gets or sets UserId.
        /// </summary>
        public string UserId { get; init; }

        /// <summary>
        /// Gets TotalPrice.
        /// </summary>
        public decimal TotalPrice => Math.Round(Items.Sum(x => x.Price), 2);
    }
}
