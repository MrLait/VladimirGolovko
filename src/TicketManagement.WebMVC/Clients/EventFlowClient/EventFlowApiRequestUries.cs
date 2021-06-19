namespace TicketManagement.WebMVC.Clients.EventFlowClient
{
    public static class EventFlowApiRequestUries
    {
        public const string EventGetAll = "/Event/getAll";

        /// <summary>
        /// {0} - event id.
        /// </summary>
        public const string EventAreaGetAllByEventId = "/EventArea/getAllByEventId?id={0}";

        /// <summary>
        /// {0} - user id.
        /// </summary>
        public const string BasketGetAllByUserId = "/Basket/getAllByUserId?id={0}";

        /// <summary>
        /// {0} - user id.
        /// {1} - item id.
        /// </summary>
        public const string BasketRemoveFromBasket = "/Basket/removeFromBasket?userId={0}&itemId={1}";

        /// <summary>
        /// {0} - user id.
        /// </summary>
        public const string BasketDeleteAllByUserId = "/Basket/deleteAllByUserId?id={0}";

        /// <summary>
        /// {0} - user id.
        /// {1} - item id.
        /// </summary>
        public const string PurchaseHistoryAddItem = "/PurchaseHistory/addItem?userId={0}&itemId={1}";

        /// <summary>
        /// {0} - user id.
        /// </summary>
        public const string PurchaseHistoryGetAllByUserId = "/PurchaseHistory/getAllByUserId?userId={0}";
    }
}
