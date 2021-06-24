namespace TicketManagement.WebMVC.Models
{
    /// <summary>
    /// Error view model.
    /// </summary>
    public class ErrorViewModel
    {
        /// <summary>
        /// Gets or sets RequestId.
        /// </summary>
        public string RequestId { get; set; }

        /// <summary>
        /// Gets ShowRequestId.
        /// </summary>
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
