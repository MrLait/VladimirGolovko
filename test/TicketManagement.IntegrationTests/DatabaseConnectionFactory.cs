using System.Linq;
using Microsoft.Extensions.Configuration;

namespace TicketManagement.IntegrationTests
{
    /// <summary>
    /// Class with database connection string.
    /// </summary>
    public class DatabaseConnectionFactory : Startup
    {
        private readonly string _integrationTestDefaultConnection;
        private readonly string _integrationTestSnapshotConnection;
        private readonly string _integrationTestMasterConnection;

        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseConnectionFactory"/> class.
        /// </summary>
        public DatabaseConnectionFactory()
        {
            _integrationTestDefaultConnection = Configuration.GetConnectionString("IntegrationTestDefaultConnection");
            _integrationTestSnapshotConnection = Configuration.GetConnectionString("IntegrationTestSnapshotConnection");
            _integrationTestMasterConnection = Configuration.GetConnectionString("IntegrationTestMasterConnection");

            DefaultDatabaseName = GetDatabaseName(_integrationTestDefaultConnection);
            SnapshotDatabaseName = GetDatabaseName(_integrationTestSnapshotConnection);
        }

        public string DefaultDatabaseName { get; }

        public string SnapshotDatabaseName { get; }

        public string CreateIntegrationTestDefaultConnection() => _integrationTestDefaultConnection;

        public string CreateIntegrationTestSnapshotConnection() => _integrationTestSnapshotConnection;

        public string CreateIntegrationTestMasterConnection() => _integrationTestMasterConnection;

        private static string GetDatabaseName(string databaseConnection)
        {
            return databaseConnection.Split(';')
                                     .Where(x => x.Contains("Initial Catalog = "))
                                     .Select(x => x["Initial Catalog = ".ToArray().Length..])
                                     .FirstOrDefault();
        }
    }
}
