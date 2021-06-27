using System;

namespace TicketManagement.Services.EventFlow.API.Models
{
    /// <summary>
    /// Basket item.
    /// </summary>
    public class BasketItem
    {
        /// <summary>
        /// Gets or sets Id.
        /// </summary>
        public string Id { get; init; }

        /// <summary>
        /// Gets or sets EventName.
        /// </summary>
        public string EventName { get; init; }

        /// <summary>
        /// Gets or sets EventAreaDescription.
        /// </summary>
        public string EventAreaDescription { get; init; }

        /// <summary>
        /// Gets or sets Row.
        /// </summary>
        public int Row { get; init; }

        /// <summary>
        /// Gets or sets NumberOfSeat.
        /// </summary>
        public int NumberOfSeat { get; init; }

        /// <summary>
        /// Gets or sets EventDateTimeStart.
        /// </summary>
        public DateTime EventDateTimeStart { get; init; }

        /// <summary>
        /// Gets or sets EventDateTimeEnd.
        /// </summary>
        public DateTime EventDateTimeEnd { get; init; }

        /// <summary>
        /// Gets or sets Price.
        /// </summary>
        public decimal Price { get; init; }

        /// <summary>
        /// Gets or sets PictureUrl.
        /// </summary>
        public string PictureUrl { get; init; }
    }
}
