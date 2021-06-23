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
        public const string EventGetAll = "/Event/getAll";

        /// <summary>
        /// Event get last url.
        /// </summary>
        public const string EventGetLast = "/Event/getLast";

        /// <summary>
        /// Event update event url.
        /// </summary>
        public const string EventUpdateEvent = "/Event/updateEvent";

        /// <summary>
        /// Event update layout id url.
        /// </summary>
        public const string EventUpdateLayoutId = "/Event/UpdateLayoutId";

        /// <summary>
        /// {0} - event id.
        /// </summary>
        public const string EventGetById = "/Event/getById?id={0}";

        /// <summary>
        /// {0} - event id.
        /// </summary>
        public const string EventDeleteById = "/Event/DeleteById?id={0}";

        /// <summary>
        /// {0} - event id.
        /// </summary>
        public const string EventAreaGetAllByEventId = "/EventArea/getAllByEventId?id={0}";

        /// <summary>
        /// updatePrices url.
        /// </summary>
        public const string EventAreaUpdatePrices = "EventArea/updatePrices";

        /// <summary>
        /// {0} - user id.
        /// </summary>
        public const string BasketGetAllByUserId = "/Basket/getAllByUserId?id={0}";

        /// <summary>
        /// Add item to basket url.
        /// </summary>
        public const string BasketAddToBasket = "/Basket/addToBasket";

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
        /// Add item to purchase history url.
        /// </summary>
        public const string PurchaseHistoryAddItem = "/PurchaseHistory/addItem";

        /// <summary>
        /// {0} - user id.
        /// </summary>
        public const string PurchaseHistoryGetAllByUserId = "/PurchaseHistory/getAllByUserId?userId={0}";

        /// <summary>
        /// Create event url.
        /// </summary>
        public const string EventManagerCreateEvent = "EventManager/createEvent";
    }
}
