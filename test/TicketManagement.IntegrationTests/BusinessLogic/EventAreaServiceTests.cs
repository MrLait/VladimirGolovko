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
        public void UpdatePrice_WhenEventAreaExist_ShouldUpdatePriceInLastEventArea()
        {
            // Arrange
            var eventAreaLast = _eventAreaRepository.GetAll().Last();
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
            eventAreaService.UpdatePrice(new EventAreaDto
            {
                Id = eventAreaLast.Id,
                EventId = expected.EventId,
                CoordX = expected.CoordX,
                CoordY = expected.CoordY,
                Description = expected.Description,
                Price = expected.Price,
            });
            var actual = _eventAreaRepository.GetAll().Last();

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void UpdatePrice_WhenEventAreaEmpty_ShouldThrowValidationException()
        {
            // Arrange
            var eventAreaService = new EventAreaService(_adoDbContext);

            // Act & Assert
            Assert.Throws<ValidationException>(() => eventAreaService.UpdatePrice(null));
        }

        [Test]
        public void UpdatePrice_WhenIdEqualZero_ShouldThrowValidationException()
        {
            // Arrange
            var eventAreaService = new EventAreaService(_adoDbContext);

            // Act & Assert
            Assert.Throws<ValidationException>(() => eventAreaService.UpdatePrice(new EventAreaDto { Id = 0 }));
        }

        [Test]
        public void UpdatePrice_WhenIdEqualLeesThanZero_ShouldThrowValidationException()
        {
            // Arrange
            var eventAreaService = new EventAreaService(_adoDbContext);

            // Act & Assert
            Assert.Throws<ValidationException>(() => eventAreaService.UpdatePrice(new EventAreaDto { Id = -1 }));
        }

        [Test]
        public void UpdatePrice_WhenPriceLeesThanZero_ShouldThrowValidationException()
        {
            // Arrange
            var eventAreaService = new EventAreaService(_adoDbContext);

            // Act & Assert
            Assert.Throws<ValidationException>(() => eventAreaService.UpdatePrice(new EventAreaDto { Id = 1, Price = -1 }));
        }

        [Test]
        public void GetAll_WhenEventAreasExist_ShouldReturnAreas()
        {
            // Arrange
            var expected = _eventAreaRepository.GetAll();
            var eventAreaService = new EventAreaService(_adoDbContext);

            // Act
            var actual = eventAreaService.GetAll();

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void GetById_WhenEventAreaExist_ShouldReturnLastEventArea()
        {
            // Arrange
            var expected = _eventAreaRepository.GetAll().Last();
            var expectedId = expected.Id;
            var eventAreaService = new EventAreaService(_adoDbContext);

            // Act
            var actual = eventAreaService.GetByID(expectedId);

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void GetByID_WhenIdEqualZero_ShouldThrowValidationException()
        {
            // Arrange
            var eventAreaService = new EventAreaService(_adoDbContext);

            // Act & Assert
            Assert.Throws<ValidationException>(() => eventAreaService.GetByID(0));
        }

        [Test]
        public void GetByID_WhenIdEqualLeesThanZero_ShouldThrowValidationException()
        {
            // Arrange
            var eventAreaService = new AreaService(_adoDbContext);

            // Act & Assert
            Assert.Throws<ValidationException>(() => eventAreaService.GetByID(-1));
        }
    }
}
