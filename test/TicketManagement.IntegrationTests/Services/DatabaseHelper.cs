namespace TicketManagement.IntegrationTests.Services
{
    using System;
    using Microsoft.Data.SqlClient;
    using Microsoft.SqlServer.Management.Common;
    using Microsoft.SqlServer.Management.Smo;
    using TicketManagement.IntegrationTests.Services.Interfaces;

    internal class DatabaseHelper : IDatabaseHelper
    {
        public void CreateSnapshot(string snapshotName, string databaseName, string snapshotsDirectoryPath, string sqlConnectionString)
        {
            string script =
                $"USE master; " + Environment.NewLine +
                $"CREATE DATABASE {snapshotName} ON " + Environment.NewLine +
                $"(" + Environment.NewLine +
                $"NAME = '{databaseName}', " + Environment.NewLine +
                $"FILENAME = '{snapshotsDirectoryPath}\\{snapshotName}.ss'" + Environment.NewLine +
                $")" + Environment.NewLine +
                $"AS SNAPSHOT OF \"{databaseName}\"";

            ExecuteScript(sqlConnectionString, script);
        }

        public void SetOfflineWithRollBack(string databaseName, string masterSqlConnectionString)
        {
            string script =
                $"USE master; " + Environment.NewLine +
                $"ALTER DATABASE \"{databaseName}\" " + Environment.NewLine +
                $"SET OFFLINE WITH ROLLBACK IMMEDIATE";

            ExecuteScript(masterSqlConnectionString, script);
        }

        public void SetOnlineWithRollBack(string databaseName, string masterSqlConnectionString)
        {
            string script =
                $"ALTER DATABASE \"{databaseName}\" " + Environment.NewLine +
                $"SET ONLINE WITH ROLLBACK IMMEDIATE";

            ExecuteScript(masterSqlConnectionString, script);
        }

        public void RestorDatabaseSnapshot(string databaseName, string databaseSnapshotName, string sqlConnectionString)
        {
            string script =
               $"USE master; " + Environment.NewLine +
               $"RESTORE DATABASE \"{databaseName}\" FROM " + Environment.NewLine +
               $"DATABASE_SNAPSHOT = '{databaseSnapshotName}';" + Environment.NewLine +
               $"GO";

            ExecuteScript(sqlConnectionString, script);
        }

        public void DropDatabase(string databaseName, string sqlConnectionString)
        {
            string script =
               $"DROP DATABASE {databaseName};";

            ExecuteScript(sqlConnectionString, script);
        }

        private static void ExecuteScript(string sqlConnectionString, string script)
        {
            using SqlConnection sqlConnection = new SqlConnection(sqlConnectionString);
            ServerConnection serverConnection = null;
            try
            {
                serverConnection = new ServerConnection(sqlConnection);
                Server server = new Server(serverConnection);
                server.ConnectionContext.ExecuteNonQuery(script);
            }
            finally
            {
                serverConnection?.Disconnect();
            }
        }
    }
}
