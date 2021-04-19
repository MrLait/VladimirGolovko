using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using TicketManagement.BusinessLogic.Infrastructure;
using TicketManagement.BusinessLogic.Services;
using TicketManagement.DataAccess.Ado;
using TicketManagement.DataAccess.Domain.Models;
using TicketManagement.DataAccess.Repositories.AdoRepositories;
using TicketManagement.Dto;

namespace TicketManagement.IntegrationTests.BusinessLogic
{
    [TestFixture]
    internal class EventSeatServiceTests : TestDatabaseLoader
    {
        private AdoUsingParametersRepository<EventSeat> _eventSeatRepository;
        private AdoDbContext _adoDbContext;

        [OneTimeSetUp]
        public void InitRepositories()
        {
            _eventSeatRepository = new AdoUsingParametersRepository<EventSeat>(MainConnectionString);
            _adoDbContext = new AdoDbContext(MainConnectionString);
        }

        [Test]
        public void UpdateState_WhenEventSeatExist_ShouldUpdateStateInLastEventSeat()
        {
            // Arrange
            var eventSeatLast = _eventSeatRepository.GetAll().Last();
            EventSeat expected = new EventSeat
            {
                Id = eventSeatLast.Id,
                EventAreaId = eventSeatLast.EventAreaId,
                Number = eventSeatLast.Number,
                Row = eventSeatLast.Row,
                State = eventSeatLast.State + 1,
            };

            var eventSeatsService = new EventSeatService(_adoDbContext);

            // Act
            eventSeatsService.UpdateState(new EventSeatDto
            {
                Id = eventSeatLast.Id,
                EventAreaId = expected.EventAreaId,
                Number = expected.Number,
                Row = expected.Row,
                State = expected.State,
            });
            var actual = _eventSeatRepository.GetAll().Last();

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void UpdateState_WhenEventSeatEmpty_ShouldThrowValidationException()
        {
            // Arrange
            var eventSeatService = new EventSeatService(_adoDbContext);

            // Act & Assert
            Assert.Throws<ValidationException>(() => eventSeatService.UpdateState(null));
        }

        [Test]
        public void UpdateState_WhenIdEqualZero_ShouldThrowValidationException()
        {
            // Arrange
            var eventSeatService = new EventSeatService(_adoDbContext);

            // Act & Assert
            Assert.Throws<ValidationException>(() => eventSeatService.UpdateState(new EventSeatDto { Id = 0 }));
        }

        [Test]
        public void UpdateState_WhenIdEqualLeesThanZero_ShouldThrowValidationException()
        {
            // Arrange
            var eventSeatService = new EventSeatService(_adoDbContext);

            // Act & Assert
            Assert.Throws<ValidationException>(() => eventSeatService.UpdateState(new EventSeatDto { Id = -1 }));
        }

        [Test]
        public void UpdateState_WhenStateLeesThanZero_ShouldThrowValidationException()
        {
            // Arrange
            var eventSeatService = new EventSeatService(_adoDbContext);

            // Act & Assert
            Assert.Throws<ValidationException>(() => eventSeatService.UpdateState(new EventSeatDto { Id = 1, State = -1 }));
        }

        [Test]
        public void GetAll_WhenEventSeatsExist_ShouldReturnEventSeats()
        {
            // Arrange
            var expected = _eventSeatRepository.GetAll();
            var eventSeatsService = new EventSeatService(_adoDbContext);

            // Act
            var actual = eventSeatsService.GetAll();

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void GetById_WhenEventSeatExist_ShouldReturnLastEventSeat()
        {
            // Arrange
            var expected = _eventSeatRepository.GetAll().Last();
            var expectedId = expected.Id;
            var eventSeatsService = new EventSeatService(_adoDbContext);

            // Act
            var actual = eventSeatsService.GetByID(expectedId);

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void GetByID_WhenIdEqualZero_ShouldThrowValidationException()
        {
            // Arrange
            var eventSeatsService = new EventSeatService(_adoDbContext);

            // Act & Assert
            Assert.Throws<ValidationException>(() => eventSeatsService.GetByID(0));
        }

        [Test]
        public void GetByID_WhenIdEqualLeesThanZero_ShouldThrowValidationException()
        {
            // Arrange
            var eventSeatsService = new EventSeatService(_adoDbContext);

            // Act & Assert
            Assert.Throws<ValidationException>(() => eventSeatsService.GetByID(-1));
        }
    }
}
