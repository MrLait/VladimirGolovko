using System.Collections.Generic;

namespace TicketManagement.WebMVC.Clients.EventFlowClient.PurchaseHistory
{
    public class PurchaseHistoryModel
    {
        public List<PurchaseHistoryItem> Items { get; init; } = new List<PurchaseHistoryItem>();

        public string UserId { get; set; }
    }
}
