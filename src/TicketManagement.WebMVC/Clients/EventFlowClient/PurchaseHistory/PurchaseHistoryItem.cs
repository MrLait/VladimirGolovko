using System;

namespace TicketManagement.WebMVC.Clients.EventFlowClient.PurchaseHistory
{
    /// <summary>
    /// Purchase history item.
    /// </summary>
    public class PurchaseHistoryItem
    {
        /// <summary>
        /// Gets or init id.
        /// </summary>
        public string Id { get; init; }

        /// <summary>
        /// Gets or init event name.
        /// </summary>
        public string EventName { get; init; }

        /// <summary>
        /// Gets or init event area description.
        /// </summary>
        public string EventAreaDescription { get; init; }

        /// <summary>
        /// Gets or init row.
        /// </summary>
        public int Row { get; init; }

        /// <summary>
        /// Gets or init number of seat.
        /// </summary>
        public int NumberOfSeat { get; init; }

        /// <summary>
        /// Gets or init Event date time start.
        /// </summary>
        public DateTime EventDateTimeStart { get; init; }

        /// <summary>
        /// Gets or init Event date time end.
        /// </summary>
        public DateTime EventDateTimeEnd { get; init; }

        /// <summary>
        /// Gets or init price.
        /// </summary>
        public decimal Price { get; init; }

        /// <summary>
        /// Gets or init picture url.
        /// </summary>
        public string PictureUrl { get; init; }
    }
}
