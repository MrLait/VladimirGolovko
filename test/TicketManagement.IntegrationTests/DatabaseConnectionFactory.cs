using System.IO;
using System.Linq;
using System.Reflection;
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
            DefaultConnection = Configuration.GetConnectionString("MainConnection");
            SnapshotConnection = Configuration.GetConnectionString("SnapshotConnection");
            MasterConnection = Configuration.GetConnectionString("MasterConnection");

            DefaultDatabaseName = GetDatabaseName(DefaultConnection);
            SnapshotDatabaseName = GetDatabaseName(SnapshotConnection);
        }

        public string DefaultConnection { get; }

        public string DefaultDatabaseName { get; private set; }

        public string SnapshotConnection { get; }

        public string SnapshotDatabaseName { get; private set; }

        public string MasterConnection { get; }

        public string CreateDefaultConnection() => DefaultConnection;

        public string CreateSnapshotConnection() => SnapshotConnection;

        public string CreateMasterConnection() => MasterConnection;

        private static string GetDatabaseName(string databaseConnection)
        {
            return databaseConnection.Split(';')
                                     .Where(x => x.Contains("Initial Catalog = "))
                                     .Select(x => x.Substring("Initial Catalog = ".ToArray().Length))
                                     .FirstOrDefault();
        }
    }
}
