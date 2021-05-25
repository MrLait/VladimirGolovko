using System.Linq;
using System.Web.Mvc;
using ClassicMvc.Infrastructure.Loggers;

namespace ClassicMvc.Infrastructure.Filters
{
    public class ControllerExceptionFilter : IExceptionFilter
    {
        private static ILogger _logger = new FileLogger();

        public void OnException(ExceptionContext filterContext)
        {
            var controller = filterContext.Controller as Controller;
            var controllerName = (string)filterContext.RouteData.Values["controller"];
            var actionName = (string)filterContext.RouteData.Values["action"];
            var exception = filterContext.Exception;
            var model = new HandleErrorInfo(exception, controllerName, actionName);

            var view = new ViewResult
            {
                ViewName = "~/Views/Shared/Error.cshtml",
                ViewData = new ViewDataDictionary(),
            };
            view.ViewData.Model = model;

            var viewData = controller.ViewData;
            if (viewData != null && viewData.Count > 0)
            {
                viewData.ToList().ForEach(view.ViewData.Add);
            }

            filterContext.Result = new ViewResult
            {
                ViewName = "~/Views/Shared/Error.cshtml",
            };

            filterContext.ExceptionHandled = true;
            view.ExecuteResult(filterContext);

            _logger.LogErrorAsync(exception, controllerName, actionName, filterContext.Exception.Message);
        }
    }
}