using System;

namespace TicketManagement.WebMVC.Clients.Basket
{
    public class BasketItem
    {
        public string Id { get; set; }

        public string EventName { get; set; }

        public string EventAreaDescription { get; set; }

        public int Row { get; set; }

        public int NumberOfSeat { get; set; }

        public DateTime EventDateTimeStart { get; set; }

        public DateTime EventDateTimeEnd { get; set; }

        public decimal Price { get; set; }

        public string PictureUrl { get; set; }
    }
}
