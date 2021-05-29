using System;
using System.Threading.Tasks;

namespace ClassicMvc.Infrastructure.Loggers
{
    public interface ILogger
    {
        Task LogInformationAsync(string message);

        Task LogExecutionTimeActionAsync(string controllerName, string actionName, string executionTime);

        Task LogExecutionTimeActionResultAsync(string controllerName, string actionName, string executionTime);

        Task LogErrorAsync(string message);

        Task LogErrorAsync(Exception exception, string message);

        Task LogErrorAsync(Exception exception, string controllerName, string actionName, string message);
    }
}
