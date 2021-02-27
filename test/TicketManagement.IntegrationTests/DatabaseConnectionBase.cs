using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketManagement.IntegrationTests
{
    public class DatabaseConnectionBase
    {
        public DatabaseConnectionBase(string databaseName)
        {
            var dataSource = "Data Source=DESKTOP-CAMEADT;";
            var initialCatalog = $"Initial Catalog = {databaseName};";
            DbConnString =dataSource + initialCatalog + "Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        }

        /// <summary>
        /// Connection string to database.
        /// </summary>
        public string DbConnString { get; }
    }
}
