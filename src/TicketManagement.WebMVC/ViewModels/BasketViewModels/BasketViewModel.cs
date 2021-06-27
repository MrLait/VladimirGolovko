using System;
using System.Collections.Generic;
using System.Linq;

namespace TicketManagement.WebMVC.ViewModels.BasketViewModels
{
    /// <summary>
    /// Basket view model.
    /// </summary>
    public class BasketViewModel
    {
        /// <summary>
        /// Gets or sets BasketItem.
        /// </summary>
        public List<BasketItem> Items { get; init; } = new ();

        /// <summary>
        /// Gets or sets UserId.
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Gets or sets TotalPrice.
        /// </summary>
        public decimal TotalPrice => Math.Round(Items.Sum(x => x.Price), 2);
    }
}
