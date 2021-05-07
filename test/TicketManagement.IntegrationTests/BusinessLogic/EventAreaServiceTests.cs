using System.Linq;
using System.Threading.Tasks;
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
    internal class EventAreaServiceTests : TestDatabaseLoader
    {
        private AdoUsingParametersRepository<EventArea> _eventAreaRepository;
        private AdoDbContext _adoDbContext;

        [OneTimeSetUp]
        public void InitRepositories()
        {
            _eventAreaRepository = new AdoUsingParametersRepository<EventArea>(MainConnectionString);
            _adoDbContext = new AdoDbContext(MainConnectionString);
        }

        [Test]
        public async Task UpdatePriceAsync_WhenEventAreaExist_ShouldUpdatePriceInLastEventArea()
        {
            // Arrange
            var eventAreaLast = (await _eventAreaRepository.GetAllAsync()).Last();
            EventArea expected = new EventArea
            {
                Id = eventAreaLast.Id,
                EventId = eventAreaLast.EventId,
                CoordX = eventAreaLast.CoordX,
                CoordY = eventAreaLast.CoordY,
                Description = eventAreaLast.Description,
                Price = eventAreaLast.Price + 100,
            };

            var eventAreaService = new EventAreaService(_adoDbContext);

            // Act
            await eventAreaService.UpdatePriceAsync(new EventAreaDto
            {
                Id = eventAreaLast.Id,
                EventId = expected.EventId,
                CoordX = expected.CoordX,
                CoordY = expected.CoordY,
                Description = expected.Description,
                Price = expected.Price,
            });
            var actual = (await _eventAreaRepository.GetAllAsync()).Last();

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void UpdatePriceAsync_WhenEventAreaEmpty_ShouldThrowValidationException()
        {
            // Arrange
            var eventAreaService = new EventAreaService(_adoDbContext);

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () => await eventAreaService.UpdatePriceAsync(null));
        }

        [Test]
        public void UpdatePriceAsync_WhenIdEqualZero_ShouldThrowValidationException()
        {
            // Arrange
            var eventAreaService = new EventAreaService(_adoDbContext);

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () => await eventAreaService.UpdatePriceAsync(new EventAreaDto { Id = 0 }));
        }

        [Test]
        public void UpdatePriceAsync_WhenIdEqualLeesThanZero_ShouldThrowValidationException()
        {
            // Arrange
            var eventAreaService = new EventAreaService(_adoDbContext);

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () => await eventAreaService.UpdatePriceAsync(new EventAreaDto { Id = -1 }));
        }

        [Test]
        public void UpdatePriceAsync_WhenPriceLeesThanZero_ShouldThrowValidationException()
        {
            // Arrange
            var eventAreaService = new EventAreaService(_adoDbContext);

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () => await eventAreaService.UpdatePriceAsync(new EventAreaDto { Id = 1, Price = -1 }));
        }

        [Test]
        public async Task GetAllAsync_WhenEventAreasExist_ShouldReturnAreas()
        {
            // Arrange
            var expected = await _eventAreaRepository.GetAllAsync();
            var eventAreaService = new EventAreaService(_adoDbContext);

            // Act
            var actual = await eventAreaService.GetAll();

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public async Task GetByIdAsync_WhenEventAreaExist_ShouldReturnLastEventArea()
        {
            // Arrange
            var expected = (await _eventAreaRepository.GetAllAsync()).Last();
            var expectedId = expected.Id;
            var eventAreaService = new EventAreaService(_adoDbContext);

            // Act
            var actual = await eventAreaService.GetByIDAsync(expectedId);

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void GetByIDAsync_WhenIdEqualZero_ShouldThrowValidationException()
        {
            // Arrange
            var eventAreaService = new EventAreaService(_adoDbContext);

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () => await eventAreaService.GetByIDAsync(0));
        }

        [Test]
        public void GetByIDAsync_WhenIdEqualLeesThanZero_ShouldThrowValidationException()
        {
            // Arrange
            var eventAreaService = new AreaService(_adoDbContext);

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () => await eventAreaService.GetByIDAsync(-1));
        }
    }
}
