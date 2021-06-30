namespace TicketManagement.WebMVC
{
    /// <summary>
    /// Api options.
    /// </summary>
    public class ApiOptions
    {
        /// <summary>
        /// Event flow api address name in appsettings.
        /// </summary>
        public const string EventFlowApiAddressName = "EventFlowApiAddress";

        /// <summary>
        /// Identity api address name in appsettings.
        /// </summary>
        public const string IdentityApiAddressName = "IdentityApiAddress";

        /// <summary>
        /// Event flow api address property.
        /// </summary>
        public string EventFlowApiAddress { get; set; }

        /// <summary>
        /// User api address property.
        /// </summary>
        public string IdentityApiAddress { get; set; }
    }
}