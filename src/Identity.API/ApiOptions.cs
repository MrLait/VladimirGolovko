namespace TicketManagement.Services.Identity.API
{
    /// <summary>
    /// User api Options.
    /// </summary>
    public class ApiOptions
    {
        /// <summary>
        /// React app address name in appsettings.
        /// </summary>
        public const string ReactAppAddressName = "ReactAppAddress";

        /// <summary>
        /// React app address.
        /// </summary>
        public string ReactAppAddress { get; set; }
    }
}