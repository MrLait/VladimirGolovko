namespace TicketManagement.WebMVC.Clients.EventFlowClient
{
    /// <summary>
    /// Event flow api request uris.
    /// </summary>
    public static class EventFlowApiRequestUris
    {
        /// <summary>
        /// Event get all url.
        /// </summary>
        public const string EventGetAll = "/Event";

        /// <summary>
        /// Event get last url.
        /// </summary>
        public const string EventGetLast = "/Event/get-last";

        /// <summary>
        /// Event update event url.
        /// </summary>
        public const string EventUpdateEvent = "/Event";

        /// <summary>
        /// {0} - event id.
        /// </summary>
        public const string EventGetById = "/Event/get-by-id?id={0}";

        /// <summary>
        /// {0} - event id.
        /// </summary>
        public const string EventDeleteById = "/Event?id={0}";

        /// <summary>
        /// {0} - event id.
        /// </summary>
        public const string EventAreaGetAllByEventId = "/EventArea?id={0}";

        /// <summary>
        /// updatePrices url.
        /// </summary>
        public const string EventAreaUpdatePrices = "EventArea";

        /// <summary>
        /// {0} - user id.
        /// </summary>
        public const string BasketGetAllByUserId = "/Basket?id={0}";

        /// <summary>
        /// Add item to basket url.
        /// </summary>
        public const string BasketAddToBasket = "/Basket";

        /// <summary>
        /// {0} - user id.
        /// {1} - item id.
        /// </summary>
        public const string BasketRemoveFromBasket = "/Basket?userId={0}&itemId={1}";

        /// <summary>
        /// {0} - user id.
        /// </summary>
        public const string BasketDeleteAllByUserId = "/Basket/delete-all-by-user-id?id={0}";

        /// <summary>
        /// Add item to purchase history url.
        /// </summary>
        public const string PurchaseHistoryAddItem = "/PurchaseHistory";

        /// <summary>
        /// {0} - user id.
        /// </summary>
        public const string PurchaseHistoryGetAllByUserId = "/PurchaseHistory?userId={0}";

        /// <summary>
        /// Create event url.
        /// </summary>
        public const string EventManagerCreateEvent = "EventManager";
    }
}
