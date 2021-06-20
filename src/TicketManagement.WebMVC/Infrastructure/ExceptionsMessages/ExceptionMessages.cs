namespace TicketManagement.WebMVC.Infrastructure.ExceptionsMessages
{
    /// <summary>
    /// Exception messages conainer.
    /// </summary>
    internal class ExceptionMessages
    {
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

        public const string PriceIsNegative = "Price can't negative.";

        public const string PriceIsZero = "Price can't equal zero.";

        /// <summary>
        /// {0} - State.
        /// </summary>
        public const string StateIsNegative = "State can't be negative. The current state is: '{0}'.";

        /// <summary>
        /// {0} - DateTime.
        /// </summary>
        public const string EventDateTimeValidation = "The Event with date time: '{0}' - can't be created in the past.";

        public const string EventForTheSameVenueInTheSameDateTime = "Event for the Venue with the dateTime already exist";

        public const string ThereAreNoSeatsInTheEvent = "The Event cannot exist without seats.";

        /// <summary>
        /// {0} - Description.
        /// </summary>
        public const string LayoutForTheVenueExist = "The Layout for this Venue with the description: '{0}' - already exists.";

        /// <summary>
        /// {0} - Row.
        /// {1} - Number.
        /// </summary>
        public const string SeatForTheAreaExis = "The Seat for this Area with parameters: Row = '{0}' and Number = '{1}' - already exists.";

        /// <summary>
        /// {0} - Description.
        /// </summary>
        public const string VenueExist = "The Venue with a description: '{0}' - already exists.";

        public const string SeatsHaveAlreadyBeenPurchased = "Unable to delete because seats have already been purchased for the event.";

        public const string StartDataTimeBeforeEndDataTime = "The beginning of the event cannot be after the end of the event.";

        public const string CantBeCreatedInThePast = "The Event can't be created in the past.";

        public const string ThereIsNoSuchLayout = "There is no such Layout.";

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionMessages"/> class.
        /// </summary>
        protected ExceptionMessages()
        {
        }
    }
}
