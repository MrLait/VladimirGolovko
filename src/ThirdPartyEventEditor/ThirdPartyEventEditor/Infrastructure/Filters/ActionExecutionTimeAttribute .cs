using System;
using System.Web.Mvc;
using ClassicMvc.Infrastructure.Loggers;

namespace ClassicMvc.Infrastructure.Filters
{
    public class ActionExecutionTimeAttribute : ActionFilterAttribute
    {
        private static ILogger _logger = new FileLogger();

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            filterContext.HttpContext.Items["ActionName"] = filterContext.HttpContext.Request.RawUrl;
            filterContext.HttpContext.Items["StartTime"] = DateTime.UtcNow;
            base.OnActionExecuting(filterContext);
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);
            DateTime startTime = (DateTime)filterContext.HttpContext.Items["StartTime"];

            _logger.LogExecutionTimeActionAsync(
                controllerName: (string)filterContext.RouteData.Values["controller"],
                actionName: (string)filterContext.RouteData.Values["action"],
                executionTime: (DateTime.UtcNow - startTime).TotalMilliseconds.ToString());
        }

        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            base.OnResultExecuted(filterContext);
            DateTime startTime = (DateTime)filterContext.HttpContext.Items["StartTime"];
            _logger.LogExecutionTimeActionResultAsync(
                controllerName: (string)filterContext.RouteData.Values["controller"],
                actionName: (string)filterContext.RouteData.Values["action"],
                executionTime: (DateTime.UtcNow - startTime).TotalMilliseconds.ToString());
        }
    }
}