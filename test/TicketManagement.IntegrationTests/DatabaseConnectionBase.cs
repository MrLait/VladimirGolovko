namespace TicketManagement.IntegrationTests
{
    public class DatabaseConnectionBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseConnectionBase"/> class.
        /// </summary>
        /// <param name="databaseName">Database name.</param>
        public DatabaseConnectionBase(string databaseName)
        {
            var dataSource = "Data Source=DESKTOP-CAMEADT;";
            var initialCatalog = $"Initial Catalog = {databaseName};";
            DbConnString = dataSource + initialCatalog + "Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        }

        /// <summary>
        /// Gets connection string to database.
        /// </summary>
        public string DbConnString { get; }
    }
}
