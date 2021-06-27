using System.Collections.Generic;

namespace TicketManagement.WebMVC.Clients.EventFlowClient.PurchaseHistory
{
    /// <summary>
    /// Purchase history model.
    /// </summary>
    public class PurchaseHistoryModel
    {
        /// <summary>
        /// Gets or init items.
        /// </summary>
        public List<PurchaseHistoryItem> Items { get; init; } = new ();

        /// <summary>
        /// Gets or init user id.
        /// </summary>
        public string UserId { get; set; }
    }
}
