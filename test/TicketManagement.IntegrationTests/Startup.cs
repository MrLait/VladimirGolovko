using System.IO;
using System.Reflection;
using Microsoft.Extensions.Configuration;

namespace TicketManagement.IntegrationTests
{
    /// <summary>
    /// Configuration with json file.
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Startup"/> class.
        /// </summary>
        /// <param name="configuration">Configuration.</param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Startup"/> class.
        /// </summary>
        protected Startup()
        {
            var jsonPath = GetAppSettingsJsonPath();
            var configurationBuilder = new ConfigurationBuilder().AddJsonFile(jsonPath);
            Configuration = configurationBuilder.Build();
        }

        /// <summary>
        /// Configuration.
        /// </summary>
        protected IConfiguration Configuration { get; }

        private static string GetAppSettingsJsonPath()
        {
            var path = Assembly.GetAssembly(typeof(DatabaseConnectionFactory))?.Location;
            var jsonPath = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(path) ?? string.Empty, @"..\..\..\", "appsettings.json"));
            return jsonPath;
        }
    }
}
