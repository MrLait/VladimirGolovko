using System.Web.Mvc;
using ClassicMvc.Infrastructure.Filters;

namespace ThirdPartyEventEditor
{
    public class FilterConfig
    {
        protected FilterConfig()
        {
        }

        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new ControllerExceptionFilter());
        }
    }
}
