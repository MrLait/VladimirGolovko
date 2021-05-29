using System;
using System.Configuration;
using System.IO;
using System.Threading.Tasks;

namespace ClassicMvc.Infrastructure.Loggers
{
    public class FileLogger : StreamFileIO, ILogger
    {
        private readonly string _filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ConfigurationManager.AppSettings["LogDetails"]);

        public async Task LogErrorAsync(string message)
        {
            try
            {
                await WriteAsync(_filePath, $"{DateTime.Now} - Error message: {message}");
            }
            catch (DirectoryNotFoundException e)
            {
                throw new DirectoryNotFoundException(e.Message);
            }
            catch (FileNotFoundException e)
            {
                throw new FileNotFoundException(e.Message);
            }
        }

        public async Task LogErrorAsync(Exception exception, string message)
        {
            try
            {
                await WriteAsync(_filePath, $"{DateTime.Now} - ExceptionMessage: {exception.Message}{Environment.NewLine}Error message: {message}");
            }
            catch (DirectoryNotFoundException e)
            {
                throw new DirectoryNotFoundException(e.Message);
            }
            catch (FileNotFoundException e)
            {
                throw new FileNotFoundException(e.Message);
            }
        }

        public async Task LogErrorAsync(Exception exception, string controllerName, string actionName, string message)
        {
            try
            {
                await WriteAsync(_filePath,
#pragma warning disable SA1118 // Parameter should not span multiple lines
    $@"{DateTime.Now} - ExceptionMessage: {exception.Message}
    Controller name: {controllerName}
    ActionName: {actionName}
    Error message: {message}");
#pragma warning restore SA1118 // Parameter should not span multiple lines
            }
            catch (DirectoryNotFoundException e)
            {
                throw new DirectoryNotFoundException(e.Message);
            }
            catch (FileNotFoundException e)
            {
                throw new FileNotFoundException(e.Message);
            }
        }

        public async Task LogExecutionTimeActionAsync(string controllerName, string actionName, string executionTime)
        {
            try
            {
                await WriteAsync(_filePath,
#pragma warning disable SA1118 // Parameter should not span multiple lines
    $@"{DateTime.Now} - Action execution time message:
    Controller name: {controllerName}
    ActionName: {actionName}
    Execution time: {executionTime} ms");
#pragma warning restore SA1118 // Parameter should not span multiple lines
            }
            catch (DirectoryNotFoundException e)
            {
                throw new DirectoryNotFoundException(e.Message);
            }
            catch (FileNotFoundException e)
            {
                throw new FileNotFoundException(e.Message);
            }
        }

        public async Task LogExecutionTimeActionResultAsync(string controllerName, string actionName, string executionTime)
        {
            try
            {
                await WriteAsync(_filePath,
#pragma warning disable SA1118 // Parameter should not span multiple lines
    $@"{DateTime.Now} - Action result execution time message:
    Controller name: {controllerName}
    ActionName: {actionName}
    Execution time: {executionTime} ms");
#pragma warning restore SA1118 // Parameter should not span multiple lines
            }
            catch (DirectoryNotFoundException e)
            {
                throw new DirectoryNotFoundException(e.Message);
            }
            catch (FileNotFoundException e)
            {
                throw new FileNotFoundException(e.Message);
            }
        }

        public async Task LogInformationAsync(string message)
        {
            try
            {
                await WriteAsync(_filePath, $"{DateTime.Now} - Info message: {message}");
            }
            catch (DirectoryNotFoundException e)
            {
                throw new DirectoryNotFoundException(e.Message);
            }
            catch (FileNotFoundException e)
            {
                throw new FileNotFoundException(e.Message);
            }
        }
    }
}