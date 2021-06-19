using System;

namespace TicketManagement.Services.EventFlow.API.Models
{
    public class PurchaseHistoryItem
    {
        public string Id { get; init; }

        public string EventName { get; init; }

        public string EventAreaDescription { get; init; }

        public int Row { get; init; }

        public int NumberOfSeat { get; init; }

        public DateTime EventDateTimeStart { get; init; }

        public DateTime EventDateTimeEnd { get; init; }

        public decimal Price { get; init; }

        public string PictureUrl { get; init; }
    }
}
