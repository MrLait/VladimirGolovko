using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.FeatureManagement;
using Microsoft.FeatureManagement.Mvc;
using TicketManagement.WebMVC.Clients.EventFlowClient.Event;
using TicketManagement.WebMVC.Constants;
using TicketManagement.WebMVC.ViewModels.EventViewModels;

namespace TicketManagement.WebMVC.Controllers
{
    /// <summary>
    /// Event home page controller.
    /// </summary>
    [AllowAnonymous]
    public class EventHomePageController : Controller
    {
        private readonly IEventClient _eventClient;
        private readonly IFeatureManager _featureManager;
        private readonly IOptions<ApiOptions> _appSettings;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventHomePageController"/> class.
        /// </summary>
        /// <param name="eventClient">Event client.</param>
        /// <param name="featureManager">Feature manager.</param>
        /// <param name="appSettings">App settings.</param>
        public EventHomePageController(IEventClient eventClient, IFeatureManager featureManager, IOptions<ApiOptions> appSettings)
        {
            _eventClient = eventClient;
            _featureManager = featureManager;
            _appSettings = appSettings;
        }

        /// <summary>
        /// Index view.
        /// </summary>
        public async Task<IActionResult> IndexAsync()
        {
            var eventCatalog = await _eventClient.GetAllAsync();
            var vm = new IndexViewModel
            {
                EventItems = eventCatalog,
            };
            var test = _appSettings?.Value.ReactAppAddress;
            var isEnabled = await _featureManager.IsEnabledAsync(FeatureFlags.RedirectToReactApp);

            return isEnabled ? Redirect(test) : View(vm);
        }
    }
}
