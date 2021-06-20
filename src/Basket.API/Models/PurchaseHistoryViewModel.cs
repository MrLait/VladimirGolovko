using System.Collections.Generic;

namespace TicketManagement.Services.Basket.API.Models
{
    public class PurchaseHistoryViewModel
    {
        public List<PurchaseHistoryItem> Items { get; init; } = new List<PurchaseHistoryItem>();

        public string UserId { get; set; }
    }
}
