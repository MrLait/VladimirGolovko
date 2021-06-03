using System.Linq;
using Microsoft.Extensions.Configuration;

namespace TicketManagement.IntegrationTests
{
    /// <summary>
    /// Class with database connection string.
    /// </summary>
    public class DatabaseConnectionFactory : Startup
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseConnectionFactory"/> class.
        /// </summary>
        public DatabaseConnectionFactory()
            : base()
        {
            IntegrationTestDefaultConnection = Configuration.GetConnectionString("IntegrationTestDefaultConnection");
            IntegrationTestSnapshotConnection = Configuration.GetConnectionString("IntegrationTestSnapshotConnection");
            IntegrationTestMasterConnection = Configuration.GetConnectionString("IntegrationTestMasterConnection");

            DefaultDatabaseName = GetDatabaseName(IntegrationTestDefaultConnection);
            SnapshotDatabaseName = GetDatabaseName(IntegrationTestSnapshotConnection);
        }

        public string IntegrationTestDefaultConnection { get; }

        public string DefaultDatabaseName { get; private set; }

        public string IntegrationTestSnapshotConnection { get; }

        public string SnapshotDatabaseName { get; private set; }

        public string IntegrationTestMasterConnection { get; }

        public string CreateIntegrationTestDefaultConnection() => IntegrationTestDefaultConnection;

        public string CreateIntegrationTestSnapshotConnection() => IntegrationTestSnapshotConnection;

        public string CreateIntegrationTestMasterConnection() => IntegrationTestMasterConnection;

        private static string GetDatabaseName(string databaseConnection)
        {
            return databaseConnection.Split(';')
                                     .Where(x => x.Contains("Initial Catalog = "))
                                     .Select(x => x.Substring("Initial Catalog = ".ToArray().Length))
                                     .FirstOrDefault();
        }
    }
}
