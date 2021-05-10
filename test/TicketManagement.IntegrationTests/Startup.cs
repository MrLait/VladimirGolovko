using System.IO;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TicketManagement.DataAccess.Ado;
using TicketManagement.DataAccess.Interfaces;

namespace TicketManagement.IntegrationTests
{
    /// <summary>
    /// Configuration with json file.
    /// </summary>
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public Startup()
        {
            string jsonPath = GetAppSettingsJsonPath();
            var configurationBuilder = new ConfigurationBuilder().AddJsonFile(jsonPath);
            Configuration = configurationBuilder.Build();
        }

        public IConfiguration Configuration { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IDbContext, EfDbContext>();

            var connectionString = Configuration.GetConnectionString("TestConnection2");
            services.AddDbContext<EfDbContext>(options =>
            {
                options.UseSqlServer(connectionString)
                .EnableSensitiveDataLogging();
            });
        }

        private static string GetAppSettingsJsonPath()
        {
            var path = Assembly.GetAssembly(typeof(DatabaseConnectionFactory)).Location;
            var jsonPath = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(path), @"..\..\..\", "appsettings.json"));
            return jsonPath;
        }
    }
}
