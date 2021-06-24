namespace TicketManagement.Services.Identity.API.Models
{
    /// <summary>
    /// Deposit model.
    /// </summary>
    public class DepositModel
    {
        /// <summary>
        /// Gets or sets UserId.
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Gets or sets Balance.
        /// </summary>
        public decimal Balance { get; set; }
    }
}
