using System;
using System.IO;
using NUnit.Framework;
using TicketManagement.IntegrationTests.Services;

namespace TicketManagement.IntegrationTests
{
    internal class TestDatabaseLoader
    {
        private const string ScriptCreateDatabaseName = "TicketManagement.Database_Create.sql";
        private readonly DatabaseHelper _databaseHelper = new ();

        public TestDatabaseLoader()
        {
            DatabaseConnectionFactory = new DatabaseConnectionFactory();
            DefaultDatabaseName = DatabaseConnectionFactory.DefaultDatabaseName;
            SnapshotDatabaseName = DatabaseConnectionFactory.SnapshotDatabaseName;
            DefaultConnectionString = DatabaseConnectionFactory.CreateIntegrationTestDefaultConnection();
            MasterConnectionString = DatabaseConnectionFactory.CreateIntegrationTestMasterConnection();
            SnapshotConnectionString = DatabaseConnectionFactory.CreateIntegrationTestSnapshotConnection();
    }

        public DatabaseConnectionFactory DatabaseConnectionFactory { get; set; }

        public string DefaultDatabaseName { get; set; }

        public string SnapshotDatabaseName { get; set; }

        public string DefaultConnectionString { get; set; }

        public string MasterConnectionString { get; set; }

        public string SnapshotConnectionString { get; set; }

        [OneTimeSetUp]
        public void Init()
        {
            string scriptPath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\Scripts", ScriptCreateDatabaseName));
            _databaseHelper.CreateDatabase(scriptPath, MasterConnectionString);
            string snapshotsDirectoryPath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\Snapshots"));
            _databaseHelper.CreateSnapshot(SnapshotDatabaseName, DefaultDatabaseName, snapshotsDirectoryPath, DefaultConnectionString);
        }

        [TearDown]
        public void TearDown()
        {
            _databaseHelper.SetOfflineWithRollBack(DefaultDatabaseName, MasterConnectionString);
            _databaseHelper.SetOnlineWithRollBack(DefaultDatabaseName, MasterConnectionString);
            _databaseHelper.RestorDatabaseSnapshot(DefaultDatabaseName, SnapshotDatabaseName, SnapshotConnectionString);
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            _databaseHelper.DropDatabase(SnapshotDatabaseName, MasterConnectionString);
            _databaseHelper.DropDatabase(DefaultDatabaseName, MasterConnectionString);
        }
    }
}
