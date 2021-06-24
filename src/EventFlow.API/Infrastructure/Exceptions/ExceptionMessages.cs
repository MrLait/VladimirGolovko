namespace TicketManagement.Services.EventFlow.API.Infrastructure.Exceptions
{
    /// <summary>
    /// Exception messages container.
    /// </summary>
    internal static class ExceptionMessages
    {
        /// <summary>
        /// Object reference not set to an instance of an object.
        /// </summary>
        public const string NullReference = "Object reference not set to an instance of an object";

        /// <summary>
        /// {0} - Description.
        /// </summary>
        public const string AreaForTheLayoutExist = "The Area for this Layout with the description: '{0}' - already exists.";

        /// <summary>
        /// {0} - Id.
        /// </summary>
        public const string IdIsNegative = "Id can't be negative. The current Id is: '{0}'.";

        /// <summary>
        /// {0} - Id.
        /// </summary>
        public const string IdIsZero = "Id can't be Zero. The current Id is: '{0}'.";

        /// <summary>
        /// Price can't negative.
        /// </summary>
        public const string PriceIsNegative = "Price can't negative.";

        /// <summary>
        /// Price can't equal zero.
        /// </summary>
        public const string PriceIsZero = "Price can't equal zero.";

        /// <summary>
        /// {0} - State.
        /// </summary>
        public const string StateIsNegative = "State can't be negative. The current state is: '{0}'.";

        /// <summary>
        /// {0} - DateTime.
        /// </summary>
        public const string EventDateTimeValidation = "The Event with date time: '{0}' - can't be created in the past.";

        /// <summary>
        /// Event for the Venue with the dateTime already exist.
        /// </summary>
        public const string EventForTheSameVenueInTheSameDateTime = "Event for the Venue with the dateTime already exist.";

        /// <summary>
        /// The Event cannot exist without seats.
        /// </summary>
        public const string ThereAreNoSeatsInTheEvent = "There are no seats in the event.";

        /// <summary>
        /// {0} - Description.
        /// </summary>
        public const string LayoutForTheVenueExist = "The Layout for this Venue with the description: '{0}' - already exists.";

        /// <summary>
        /// {0} - Row.
        /// {1} - Number.
        /// </summary>
        public const string SeatForTheAreaExist = "The Seat for this Area with parameters: Row = '{0}' and Number = '{1}' - already exists.";

        /// <summary>
        /// {0} - Description.
        /// </summary>
        public const string VenueExist = "The Venue with a description: '{0}' - already exists.";

        /// <summary>
        /// Unable to delete because seats have already been purchased for the event.
        /// </summary>
        public const string SeatsHaveAlreadyBeenPurchased = "Unable to delete because seats have already been purchased for the event.";

        /// <summary>
        /// The beginning of the event cannot be after the end of the event.
        /// </summary>
        public const string StartDataTimeBeforeEndDataTime = "The beginning of the event cannot be after the end of the event.";

        /// <summary>
        /// The Event can't be created in the past.
        /// </summary>
        public const string CantBeCreatedInThePast = "The event can't be created in the past.";

        /// <summary>
        /// There is no such Layout.
        /// </summary>
        public const string ThereIsNoSuchLayout = "There is no such Layout.";

        /// <summary>
        /// Product in another user busket.
        /// </summary>
        public const string ProductInAnotherUserBusket = "Product in another user busket.";
    }
}
