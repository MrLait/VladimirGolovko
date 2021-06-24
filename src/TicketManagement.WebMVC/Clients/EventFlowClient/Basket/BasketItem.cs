using System;

namespace TicketManagement.WebMVC.Clients.EventFlowClient.Basket
{
    /// <summary>
    /// Basket item.
    /// </summary>
    public class BasketItem
    {
        /// <summary>
        /// Gets or init id.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or init id.
        /// </summary>
        public string EventName { get; set; }

        /// <summary>
        /// Gets or init id.
        /// </summary>
        public string EventAreaDescription { get; set; }

        /// <summary>
        /// Gets or init id.
        /// </summary>
        public int Row { get; set; }

        /// <summary>
        /// Gets or init id.
        /// </summary>
        public int NumberOfSeat { get; set; }

        /// <summary>
        /// Gets or init id.
        /// </summary>
        public DateTime EventDateTimeStart { get; set; }

        /// <summary>
        /// Gets or init id.
        /// </summary>
        public DateTime EventDateTimeEnd { get; set; }

        /// <summary>
        /// Gets or init id.
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Gets or init id.
        /// </summary>
        public string PictureUrl { get; set; }
    }
}
