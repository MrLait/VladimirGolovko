namespace TicketManagement.Services.EventFlow.API.Models
{
    /// <summary>
    /// Add to purchase history model.
    /// </summary>
    public class AddToPurchaseHistoryModel
    {
        /// <summary>
        /// Gets or sets UserId.
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Gets or sets ItemId.
        /// </summary>
        public int ItemId { get; set; }
    }
}
