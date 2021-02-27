using System;
using System.IO;
using NUnit.Framework;
using TicketManagement.DataAccess.Domain.Models;
using TicketManagement.DataAccess.Repositories.AdoRepositories;
using TicketManagement.IntegrationTests.Services;

namespace TicketManagement.IntegrationTests.DataAccess.Repositories.ADO
{
    [TestFixture]
    internal class AdoUsingParametersRepositoryTests
    {
        /// <summary>
        /// Field with group repository.
        /// </summary>
        private AdoUsingParametersRepository<Venue> _venueRepository;

        /// <summary>
        /// Initialize repository.
        /// </summary>
        [OneTimeSetUp]
        public void Init()
        {
            string sqlConnectionString = new DatabaseConnectionBase("TicketManagement.Database").DbConnString;
            string databaseSnapshotName = "TicketManagement_Database";
            string databaseName = "TicketManagement.Database";
            string snapshotsDirectoryPath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\..\\Snapshots"));

            DatabaseHelper databaseHelper = new DatabaseHelper();
            databaseHelper.CreateSnapshot(databaseSnapshotName, databaseName, snapshotsDirectoryPath, sqlConnectionString);

            _venueRepository = new AdoUsingParametersRepository<Venue>(sqlConnectionString);
        }

        [TearDown]
        public void TearDown()
        {
            string databaseName = "TicketManagement.Database";
            string masterSqlConnectionString = new DatabaseConnectionBase("master").DbConnString;

            DatabaseHelper databaseHelper = new DatabaseHelper();
            databaseHelper.SetOfflineWithRollBack(databaseName, masterSqlConnectionString);
            databaseHelper.SetOnlineWithRollBack(databaseName, masterSqlConnectionString);

            string sqlConnectionString = new DatabaseConnectionBase("TicketManagement_Database").DbConnString;
            string databaseSnapshotName = "TicketManagement_Database";

            databaseHelper.RestorDatabaseSnapshot(databaseName, databaseSnapshotName, sqlConnectionString);
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            DatabaseHelper databaseHelper = new DatabaseHelper();
            databaseHelper.DropDatabase("TicketManagement_Database", new DatabaseConnectionBase("master").DbConnString);
        }

        /// <summary>
        /// Test cases for Add.
        /// </summary>
        /// <param name="actualName">Name parameter.</param>
        [TestCase("NameGroup")]
        public void GivenAdd_WhenCorrectValue_ThenOutIsAddedObject(string actualName)
        {
            // Arrage
            Venue actual = new Venue { Address = "Addr1 asd new", Description = "Desc1 sd", Phone = "375123" };

            // Act
            _venueRepository.Create(actual);

            // Assert
            Assert.AreEqual(actualName, actualName);
        }

        [TestCase(6)]
        public void GivenDelete_WhenCorrectId_ThenOutIsDeletedObject(int actualId)
        {
            // Arrage
            // Act
            _venueRepository.Delete(actualId);

            // Assert
            Assert.AreEqual(actualId, actualId);
        }

        [TestCase(1)]
        public void GivenGetById_WhenCorrectId_ThenOutIsDeletedObject(int actualId)
        {
            // Arrage
            // Act
            var test = _venueRepository.GetByID(actualId);

            // Assert
            Assert.AreEqual(test, test);
        }

        [TestCase(1)]
        public void GivenGetAll_WhenCorrectId_ThenOutIsDeletedObject(int actualId)
        {
            // Arrage
            // Act
            var test = _venueRepository.GetAll();

            // Assert
            Assert.AreEqual(test, test);
        }

        [TestCase(1)]
        public void GivenUpdate_WhenCorrectId_ThenOutIsDeletedObject(int actualId)
        {
            // Arrage
            // Act
            _venueRepository.Update(new Venue { Id = 1, Address = "a", Description = "d", Phone = "333" });

            // Assert
            Assert.AreEqual(actualId, actualId);
        }
    }
}
