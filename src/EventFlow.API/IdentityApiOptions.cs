namespace TicketManagement.Services.EventFlow.API
{
    /// <summary>
    /// User api Options.
    /// </summary>
    public class IdentityApiOptions
    {
        /// <summary>
        /// Identity api address name in appsettings.
        /// </summary>
        public const string IdentityApiAddressName = "IdentityApiAddress";

        /// <summary>
        /// Identity api address.
        /// </summary>
        public string IdentityApiAddress { get; set; }
    }
}