using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using Microsoft.FeatureManagement;
using TicketManagement.WebMVC.Constants;

namespace TicketManagement.WebMVC.Infrastructure.Filters
{
    public class RedirectToReactAppAttribute : ActionFilterAttribute
    {
        private readonly IFeatureManager _featureManager;
        private readonly IOptions<ApiOptions> _appSettings;

        /// <summary>
        /// Initializes a new instance of the <see cref="RedirectToReactAppAttribute"/> class.
        /// </summary>
        /// <param name="featureManager">Feature manager.</param>
        /// <param name="appSettings">App settings.</param>
        public RedirectToReactAppAttribute(IFeatureManager featureManager, IOptions<ApiOptions> appSettings)
        {
            _featureManager = featureManager;
            _appSettings = appSettings;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var isEnable = _featureManager.IsEnabledAsync(FeatureFlags.RedirectToReactApp).Result;
            if (isEnable)
            {
                var reactAppUrlAddress = _appSettings?.Value.ReactAppAddress;
                context.Result = new RedirectResult(reactAppUrlAddress, false);
            }
        }
    }
}
