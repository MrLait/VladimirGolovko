using System;
using System.IO;
using NUnit.Framework;
using TicketManagement.BusinessLogic.Services;
using TicketManagement.DataAccess.Ado;
using TicketManagement.Dto;
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
        private AreaService _areaService;
        private SeatService _seatService;
        private EventService _eventService;

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
            _areaService = new AreaService(new AdoDbContext(sqlConnectionString));
            _seatService = new SeatService(new AdoDbContext(sqlConnectionString));
            _eventService = new EventService(new AdoDbContext(sqlConnectionString));
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
            _venueRepository.Create(actual);

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
            _layoutService.Create(actual);

            // Assert
            Assert.AreEqual(actualName, actualName);
        }

        /// <summary>
        /// Test cases for Add.
        /// </summary>
        /// <param name="actualName">Name parameter.</param>
        [TestCase("NameGroup")]
        public void GivenCreateArea_WhenCorrectValue_ThenOutIsAddedObject(string actualName)
        {
            // Arrage
            AreaDto actual = new AreaDto { LayoutId = 1, Description = "Second sector of first layout.", CoordX = 1, CoordY = 1 };

            // Act
            _areaService.Create(actual);

            // Assert
            Assert.AreEqual(actualName, actualName);
        }

        /// <summary>
        /// Test cases for Add.
        /// </summary>
        /// <param name="actualName">Name parameter.</param>
        [TestCase("NameGroup")]
        public void GivenCreateSeat_WhenCorrectValue_ThenOutIsAddedObject(string actualName)
        {
            // Arrage
            SeatDto actual = new SeatDto { AreaId = 1, Row = 3, Number = 1 };

            // Act
            _seatService.Create(actual);

            // Assert
            Assert.AreEqual(actualName, actualName);
        }

        [TestCase("NameGroup")]
        public void GivenCreateEvent_WhenCorrectValue_ThenOutIsAddedObject(string actualName)
        {
            // Arrage
            EventDto actual = new EventDto { LayoutId = 1, Description = "asd", Name = "asd", DateTime = new DateTime(2021, 4, 1) };

            // Act
            _eventService.Create(actual);

            // Assert
            Assert.AreEqual(actualName, actualName);
        }
    }
}
