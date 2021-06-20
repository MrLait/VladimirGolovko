using System;
using System.Collections.Generic;
using System.Linq;

namespace TicketManagement.Services.Basket.API.Models
{
    public class BasketViewModel
    {
        public List<BasketItem> Items { get; init; } = new List<BasketItem>();

        public string UserId { get; set; }

        public decimal TotalPrice => Math.Round(Items.Sum(x => x.Price), 2);
    }
}
