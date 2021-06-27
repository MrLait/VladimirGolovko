namespace TicketManagement.WebMVC.ViewModels.BasketViewModels
{
    /// <summary>
    /// Basket component view model.
    /// </summary>
    public class BasketComponentViewModel
    {
        /// <summary>
        /// Gets or sets ItemsCount.
        /// </summary>
        public int ItemsCount { get; set; }

        /// <summary>
        /// Disabled.
        /// </summary>
        public string Disabled => (ItemsCount == 0) ? "is-disabled" : "";
    }
}
