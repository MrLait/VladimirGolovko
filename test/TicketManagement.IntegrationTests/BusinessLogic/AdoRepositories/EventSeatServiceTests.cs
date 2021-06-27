using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using TicketManagement.DataAccess.DbContexts;
using TicketManagement.DataAccess.Domain.Enums;
using TicketManagement.DataAccess.Domain.Models;
using TicketManagement.DataAccess.Repositories.AdoRepositories;
using TicketManagement.Dto;
using TicketManagement.Services.EventFlow.API.Infrastructure.Exceptions;
using TicketManagement.Services.EventFlow.API.Infrastructure.Services;

namespace TicketManagement.IntegrationTests.BusinessLogic.AdoRepositories
{
    [TestFixture]
    internal class EventSeatServiceTests : TestDatabaseLoader
    {
        private AdoUsingParametersRepository<EventSeat> _eventSeatRepository;
        private AdoDbContext _adoDbContext;

        [OneTimeSetUp]
        public void InitRepositories()
        {
            _eventSeatRepository = new AdoUsingParametersRepository<EventSeat>(DefaultConnectionString);
            _adoDbContext = new AdoDbContext(DefaultConnectionString);
        }

        [Test]
        public async Task UpdateStateAsync_WhenEventSeatExist_ShouldUpdateStateInLastEventSeat()
        {
            // Arrange
            var eventSeatLast = _eventSeatRepository.GetAllAsQueryable().Last();
            var expected = new EventSeat
            {
                Id = eventSeatLast.Id,
                EventAreaId = eventSeatLast.EventAreaId,
                Number = eventSeatLast.Number,
                Row = eventSeatLast.Row,
                State = eventSeatLast.State + 1,
            };

            var eventSeatsService = new EventSeatService(_adoDbContext);

            // Act
            await eventSeatsService.UpdateStateAsync(new EventSeatDto
            {
                Id = eventSeatLast.Id,
                EventAreaId = expected.EventAreaId,
                Number = expected.Number,
                Row = expected.Row,
                State = (States)expected.State,
            });
            var actual = _eventSeatRepository.GetAllAsQueryable().Last();

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void UpdateStateAsync_WhenEventSeatEmpty_ShouldThrowValidationException()
        {
            // Arrange
            var eventSeatService = new EventSeatService(_adoDbContext);

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () => await eventSeatService.UpdateStateAsync(null));
        }

        [Test]
        public void UpdateStateAsync_WhenIdEqualZero_ShouldThrowValidationException()
        {
            // Arrange
            var eventSeatService = new EventSeatService(_adoDbContext);

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () => await eventSeatService.UpdateStateAsync(new EventSeatDto { Id = 0 }));
        }

        [Test]
        public void GetAll_WhenEventSeatsExist_ShouldReturnEventSeats()
        {
            // Arrange
            var expected = _eventSeatRepository.GetAllAsQueryable();
            var eventSeatsService = new EventSeatService(_adoDbContext);

            // Act
            var actual = eventSeatsService.GetAll();

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public async Task GetByIdAsync_WhenEventSeatExist_ShouldReturnLastEventSeat()
        {
            // Arrange
            var expected = _eventSeatRepository.GetAllAsQueryable().Last();
            var expectedId = expected.Id;
            var eventSeatsService = new EventSeatService(_adoDbContext);

            // Act
            var actual = await eventSeatsService.GetByIdAsync(expectedId);

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void GetByIDAsync_WhenIdEqualZero_ShouldThrowValidationException()
        {
            // Arrange
            var eventSeatsService = new EventSeatService(_adoDbContext);

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () => await eventSeatsService.GetByIdAsync(0));
        }

        [Test]
        public void GetByIDAsync_WhenIdEqualLeesThanZero_ShouldThrowValidationException()
        {
            // Arrange
            var eventSeatsService = new EventSeatService(_adoDbContext);

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () => await eventSeatsService.GetByIdAsync(-1));
        }
    }
}
