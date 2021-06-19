using System.Collections.Generic;

namespace TicketManagement.Services.EventFlow.API.Models
{
    public class PurchaseHistoryModel
    {
        public List<PurchaseHistoryItem> Items { get; init; } = new List<PurchaseHistoryItem>();

        public string UserId { get; set; }
    }
}
