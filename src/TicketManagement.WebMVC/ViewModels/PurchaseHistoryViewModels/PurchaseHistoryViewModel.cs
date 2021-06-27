using System.Collections.Generic;

namespace TicketManagement.WebMVC.ViewModels.PurchaseHistoryViewModels
{
    /// <summary>
    /// Purchase history view model.
    /// </summary>
    public class PurchaseHistoryViewModel
    {
        /// <summary>
        /// Gets or sets purchase history items.
        /// </summary>
        public List<PurchaseHistoryItem> Items { get; init; } = new ();

        /// <summary>
        /// Gets or sets user id.
        /// </summary>
        public string UserId { get; set; }
    }
}
