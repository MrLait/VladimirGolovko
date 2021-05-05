using System;
using System.Collections.Generic;
using System.Linq;

namespace TicketManagement.WebMVC.ViewModels.BasketViewModels
{
    public class BasketViewModel
    {
        public List<BasketItem> Items { get; init; } = new List<BasketItem>();

        public string UserId { get; set; }

        public decimal TotalPrice => Math.Round(Items.Sum(x => x.Price), 2);
    }
}
