namespace TicketManagement.Services.EventFlow.API.Models
{
    /// <summary>
    /// Add to basket model.
    /// </summary>
    public class AddToBasketModel
    {
        /// <summary>
        /// Gets or sets user id.
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Gets or init item id.
        /// </summary>
        public int ItemId { get; set; }
    }
}
