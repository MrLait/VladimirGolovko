namespace TicketManagement.IntegrationTests.Services.Interfaces
{
    internal interface IDatabaseHelper
    {
        void CreateSnapshot(string snapshotName, string databaseName, string snapshotsDirectoryPath, string sqlConnectionString);

        void RestorDatabaseSnapshot(string databaseName, string databaseSnapshotName, string sqlConnectionString);

        void SetOfflineWithRollBack(string databaseName, string masterSqlConnectionString);

        void SetOnlineWithRollBack(string databaseName, string masterSqlConnectionString);

        void DropDatabase(string databaseName, string sqlConnectionString);
    }
}
