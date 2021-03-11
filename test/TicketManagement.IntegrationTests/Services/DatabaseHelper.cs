using System;
using System.IO;
using System.Text;
using Microsoft.Data.SqlClient;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using TicketManagement.IntegrationTests.Services.Interfaces;

namespace TicketManagement.IntegrationTests.Services
{
    internal class DatabaseHelper : IDatabaseHelper, IReadFromFile, IExecuteScript
    {
        public void CreateSnapshot(string snapshotName, string databaseName, string snapshotsDirectoryPath, string sqlConnectionString)
        {
            string script =
                "USE master; " + Environment.NewLine +
                $"CREATE DATABASE {snapshotName} ON " + Environment.NewLine +
                "(" + Environment.NewLine +
                $"NAME = '{databaseName}', " + Environment.NewLine +
                $"FILENAME = '{snapshotsDirectoryPath}\\{snapshotName}.ss'" + Environment.NewLine +
                ")" + Environment.NewLine +
                $"AS SNAPSHOT OF \"{databaseName}\"";

            ExecuteScript(sqlConnectionString, script);
        }

        public void SetOfflineWithRollBack(string databaseName, string masterSqlConnectionString)
        {
            string script =
                "USE master; " + Environment.NewLine +
                $"ALTER DATABASE \"{databaseName}\" " + Environment.NewLine +
                "SET OFFLINE WITH ROLLBACK IMMEDIATE";

            ExecuteScript(masterSqlConnectionString, script);
        }

        public void CreateDatabase(string scriptPath, string masterConnectionString)
        {
            string script = ReadFromFile(scriptPath);
            ExecuteScript(masterConnectionString, script);
        }

        public void SetOnlineWithRollBack(string databaseName, string masterSqlConnectionString)
        {
            string script =
                $"ALTER DATABASE \"{databaseName}\" " + Environment.NewLine +
                "SET ONLINE WITH ROLLBACK IMMEDIATE";

            ExecuteScript(masterSqlConnectionString, script);
        }

        public void RestorDatabaseSnapshot(string databaseName, string databaseSnapshotName, string sqlConnectionString)
        {
            string script =
               "USE master; " + Environment.NewLine +
               $"RESTORE DATABASE \"{databaseName}\" FROM " + Environment.NewLine +
               $"DATABASE_SNAPSHOT = '{databaseSnapshotName}';" + Environment.NewLine +
               "GO";

            ExecuteScript(sqlConnectionString, script);
        }

        public void DropDatabase(string databaseName, string sqlConnectionString)
        {
            string script =
               $"DROP DATABASE \"{databaseName}\";";

            ExecuteScript(sqlConnectionString, script);
        }

        public string ReadFromFile(string path)
        {
            string script = string.Empty;

            if (!File.Exists(path))
            {
                // Create the file.
                using FileStream fs = File.Create(path);
                byte[] info =
                    new UTF8Encoding(true).GetBytes("This is some text in the file.");

                // Add some information to the file.
                fs.Write(info, 0, info.Length);
            }

            // Open the stream and read it back.
            using StreamReader sr = File.OpenText(path);
            string s = string.Empty;
            while ((s = sr.ReadLine()) != null)
            {
                script += s + Environment.NewLine;
            }

            return script;
        }

        public void ExecuteScript(string sqlConnectionString, string script)
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
