using System.Collections.Generic;

namespace TicketManagement.Services.EventFlow.API.Models
{
    /// <summary>
    /// Purchase history model.
    /// </summary>
    public class PurchaseHistoryModel
    {
        /// <summary>
        /// Gets or sets PurchaseHistoryItem.
        /// </summary>
        public List<PurchaseHistoryItem> Items { get; } = new ();

        /// <summary>
        /// Gets or sets UserId.
        /// </summary>
        public string UserId { get; init; }
    }
}
