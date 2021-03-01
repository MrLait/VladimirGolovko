using System;
using System.IO;
using NUnit.Framework;
using TicketManagement.BusinessLogic.DTO;
using TicketManagement.BusinessLogic.Services;
using TicketManagement.DataAccess.Ado;
using TicketManagement.DataAccess.Domain.Models;
using TicketManagement.DataAccess.Repositories.AdoRepositories;
using TicketManagement.IntegrationTests.Services;

namespace TicketManagement.IntegrationTests.BusinessLogic.Services
{
    [TestFixture]
    public class VenueServiceTests
    {
        /// <summary>
        /// Field with group repository.
        /// </summary>
        private VenueService _venueRepository;
        private LayoutService _layoutService;

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

            _venueRepository = new VenueService(new AdoDbContext(sqlConnectionString));
            _layoutService = new LayoutService(new AdoDbContext(sqlConnectionString));
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
        public void GivenCreate_WhenCorrectValue_ThenOutIsAddedObject(string actualName)
        {
            // Arrage
            VenueDto actual = new VenueDto { Address = "Addr1 asd new", Description = "Gomel Regional Drama Thea21ter", Phone = "375123" };

            // Act
            _venueRepository.CreateVenue(actual);

            // Assert
            Assert.AreEqual(actualName, actualName);
        }

        /// <summary>
        /// Test cases for Add.
        /// </summary>
        /// <param name="actualName">Name parameter.</param>
        [TestCase("NameGroup")]
        public void GivenCreateLayout_WhenCorrectValue_ThenOutIsAddedObject(string actualName)
        {
            // Arrage
            LayoutDto actual = new LayoutDto { Description = "Layo2ut for concerts.", VenueId = 1 };

            // Act
            _layoutService.CreateLayout(actual);

            // Assert
            Assert.AreEqual(actualName, actualName);
        }
    }
}
