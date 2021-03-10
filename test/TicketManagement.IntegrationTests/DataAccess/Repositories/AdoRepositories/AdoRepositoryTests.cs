using System;
using System.IO;
using NUnit.Framework;
using TicketManagement.IntegrationTests.Services;

namespace TicketManagement.IntegrationTests.DataAccess.Repositories.AdoRepositories
{
    internal class AdoRepositoryTests
    {
        private const string _databaseName = "TicketManagement.Database";
        private const string _databaseSnapshotName = "TicketManagement_Database";
        private const string _scriptCreateDatabaseName = "TicketManagement.Database_Create.sql";
        private readonly string _masterConnectionString = new DatabaseConnectionBase("master").DbConnString;
        private readonly string _mainConnectionString = new DatabaseConnectionBase(_databaseName).DbConnString;
        private readonly string _snapshotConnectionString = new DatabaseConnectionBase(_databaseSnapshotName).DbConnString;

        private readonly DatabaseHelper _databaseHelper = new DatabaseHelper();

        public string MainConnectionString => _mainConnectionString;

        [OneTimeSetUp]
        public void Init()
        {
            string scriptPath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\..\\Scripts", _scriptCreateDatabaseName));
            _databaseHelper.CreateDatabase(scriptPath, _masterConnectionString);
            string snapshotsDirectoryPath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\..\\Snapshots"));
            _databaseHelper.CreateSnapshot(_databaseSnapshotName, _databaseName, snapshotsDirectoryPath, MainConnectionString);
        }

        [TearDown]
        public void TearDown()
        {
            _databaseHelper.SetOfflineWithRollBack(_databaseName, _masterConnectionString);
            _databaseHelper.SetOnlineWithRollBack(_databaseName, _masterConnectionString);
            _databaseHelper.RestorDatabaseSnapshot(_databaseName, _databaseSnapshotName, _snapshotConnectionString);
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            _databaseHelper.DropDatabase(_databaseSnapshotName, _masterConnectionString);
            _databaseHelper.DropDatabase(_databaseName, _masterConnectionString);
        }
    }
}
