namespace TicketManagement.BusinessLogic.Infrastructure
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

        /// <summary>
        /// {0} - Price.
        /// </summary>
        public const string PriceIsNegative = "Price can't be negative. The current price is: '{0}'.";

        /// <summary>
        /// {0} - State.
        /// </summary>
        public const string StateIsNegative = "State can't be negative. The current state is: '{0}'.";

        /// <summary>
        /// {0} - DateTime.
        /// </summary>
        public const string EventDateTimeValidation = "The Event with date time: '{0}' - can't be created in the past.";

        /// <summary>
        /// {0} - Description.
        /// {1} - DateTime.
        /// </summary>
        public const string EventForTheSameVenueInTheSameDateTime = "Event with description: '{0}' for the Venue with the dateTime: '{1}' - already exist";

        /// <summary>
        /// {0} - Description.
        /// </summary>
        public const string ThereAreNoSeatsInTheEvent = "Event with description: '{0}' - cannot exist without seats.";

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

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionMessages"/> class.
        /// </summary>
        protected ExceptionMessages()
        {
        }
    }
}
