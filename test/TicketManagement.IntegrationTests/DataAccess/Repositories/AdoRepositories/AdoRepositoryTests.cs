﻿namespace TicketManagement.IntegrationTests.DataAccess.Repositories.AdoRepositories
{
    using System;
    using System.IO;
    using NUnit.Framework;
    using TicketManagement.IntegrationTests.Services;

    internal class AdoRepositoryTests
    {
        private const string _databaseName = "TicketManagement.Database";
        private const string _databaseSnapshotName = "TicketManagement_Database";
        private readonly string _masterConnectionString = new DatabaseConnectionBase("master").DbConnString;
        private readonly string _mainConnectionString = new DatabaseConnectionBase(_databaseName).DbConnString;
        private readonly string _snapshotConnectionString = new DatabaseConnectionBase(_databaseSnapshotName).DbConnString;

        private readonly DatabaseHelper _databaseHelper = new DatabaseHelper();

        public string MainConnectionString => _mainConnectionString;

        [OneTimeSetUp]
        public void Init()
        {
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
        }
    }
}
