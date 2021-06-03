using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using TicketManagement.DataAccess.Domain.Models;
using TicketManagement.DataAccess.Repositories.AdoRepositories;

namespace TicketManagement.IntegrationTests.DataAccess.Repositories.AdoRepositories
{
    [TestFixture]
    internal class EventAreaTests : TestDatabaseLoader
    {
        private readonly List<EventArea> _eventAreas = new List<EventArea>();

        [OneTimeSetUp]
        public async Task InitEventAreasAsync()
        {
            var eventAreaRepository = new AdoUsingParametersRepository<EventArea>(DefaultConnectionString);
            var countAllEventAreas = eventAreaRepository.GetAllAsQueryable().Last().Id;

            for (int i = 1; i <= countAllEventAreas; i++)
            {
                _eventAreas.Add(await eventAreaRepository.GetByIDAsync(i));
            }
        }

        [Test]
        public void GetAllAsQueryable_WhenEventAreasExist_ShouldReturnEventAreaList()
        {
            // Arrange
            var expected = _eventAreas;

            // Act
            var actual = new AdoUsingParametersRepository<EventArea>(DefaultConnectionString).GetAllAsQueryable();

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void GetAllAsQueryable_WhenEventAreasIncorrectConnectionSting_ShouldThrowArgumentException()
        {
            // Arrange
            var actual = new AdoUsingParametersRepository<EventArea>("Server=myServerAddress;Database=myDataBase;User Id=myUsername;Password=myPassword;");

            // Act & Assert
            Assert.Throws<ArgumentException>(() => actual.GetAllAsQueryable());
        }

        [Test]
        public async Task CreateAsync_WhenAddEventArea_ShouldReturnEventAreaWithNewEventArea()
        {
            // Arrange
            var repository = new AdoUsingParametersRepository<EventArea>(DefaultConnectionString);
            EventArea eventArea = new EventArea { Id = repository.GetAllAsQueryable().ToList().Count + 1, CoordX = 2, CoordY = 2, Description = "Creaded", EventId = 2, Price = 10 };
            List<EventArea> expected = new List<EventArea>(_eventAreas)
            {
                eventArea,
            };

            // Act
            await repository.CreateAsync(eventArea);
            var actual = repository.GetAllAsQueryable();

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void CreateAsync_WhenEventAreaEmpty_ShouldThrowArgumentException()
        {
            // Arrange
            var repository = new AdoUsingParametersRepository<EventArea>(DefaultConnectionString);

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await repository.CreateAsync(null));
        }

        [Test]
        public async Task DeleteAsync_WhenExistEventArea_ShouldReturnEventAreaListWithoutDeletedEventArea()
        {
            // Arrange
            var repository = new AdoUsingParametersRepository<EventArea>(DefaultConnectionString);

            // Act
            var allEventAreas = repository.GetAllAsQueryable();
            var lastEventArea = allEventAreas.Last();
            await repository.DeleteAsync(lastEventArea);

            var actual = repository.GetAllAsQueryable();
            int countEventAreaWithoutLast = allEventAreas.ToList().Count - 1;

            // Assert
            actual.Should().BeEquivalentTo(allEventAreas.Take(countEventAreaWithoutLast));
        }

        [Test]
        public void DeleteAsync_WhenIdEqualZeroEventArea_ShouldThrowArgumentException()
        {
            // Arrange
            EventArea eventArea = new EventArea { Id = 0 };
            var repository = new AdoUsingParametersRepository<EventArea>(DefaultConnectionString);

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await repository.DeleteAsync(eventArea));
        }

        [Test]
        public void DeleteAsync_WhenNullEventArea_ShouldThrowArgumentException()
        {
            // Arrange
            var repository = new AdoUsingParametersRepository<EventArea>(DefaultConnectionString);

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await repository.DeleteAsync(null));
        }

        [Test]
        public void DeleteAsync_WhenIncorrectConnectionStringEventArea_ShouldThrowArgumentException()
        {
            // Arrange
            EventArea eventArea = new EventArea { Id = 3 };
            var repository = new AdoUsingParametersRepository<EventArea>("Server=myServerAddress;Database=myDataBase;User Id=myUsername;Password=myPassword;");

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await repository.DeleteAsync(eventArea));
        }

        [Test]
        public async Task UpdateAsync_WhenExistEventArea_ShouldUpdateLastEventArea()
        {
            // Arrange
            var repository = new AdoUsingParametersRepository<EventArea>(DefaultConnectionString);
            var expected = new EventArea { Price = 2, EventId = 2, Description = "Updated", CoordY = 2, CoordX = 2 };

            // Act
            var lastEventArea = repository.GetAllAsQueryable().Last();
            var idLastEventArea = lastEventArea.Id;
            expected.Id = idLastEventArea;

            await repository.UpdateAsync(expected);
            var actual = repository.GetAllAsQueryable().Last();

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void UpdateAsync_WhenNullEventArea_ShouldThrowArgumentException()
        {
            // Arrange
            var repository = new AdoUsingParametersRepository<EventArea>(DefaultConnectionString);

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await repository.UpdateAsync(null));
        }

        [Test]
        public void UpdateAsync_WhenIdEqualZeroEventArea_ShouldThrowArgumentException()
        {
            // Arrange
            EventArea eventArea = new EventArea { Id = 0 };
            var repository = new AdoUsingParametersRepository<EventArea>(DefaultConnectionString);

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await repository.UpdateAsync(eventArea));
        }

        [Test]
        public async Task GetByIdAsync_WhenExistEventArea_ShouldReturnEventAreaAsync()
        {
            // Arrange
            var repository = new AdoUsingParametersRepository<EventArea>(DefaultConnectionString);

            // Act
            var lastEventArea = repository.GetAllAsQueryable().Last();
            EventArea expectedEventArea = new EventArea
            {
                Id = lastEventArea.Id,
                CoordX = lastEventArea.CoordX,
                CoordY = lastEventArea.CoordY,
                Description = lastEventArea.Description,
                EventId = lastEventArea.EventId,
                Price = lastEventArea.Price,
            };

            int actualId = expectedEventArea.Id;
            var actual = await repository.GetByIDAsync(actualId);

            // Assert
            actual.Should().BeEquivalentTo(expectedEventArea);
        }

        [Test]
        public async Task GetByIdAsync_WhenNonExistEventArea_ShouldReturnNull()
        {
            // Arrange
            EventArea expected = null;
            var repository = new AdoUsingParametersRepository<EventArea>(DefaultConnectionString);

            // Act
            var lastEventArea = repository.GetAllAsQueryable().Last();
            int nonExistId = lastEventArea.Id + 1;
            var actual = await repository.GetByIDAsync(nonExistId);

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void GetByIdAsync_WhenIdEqualZeroEventArea_ShouldThrowArgumentException()
        {
            // Arrange
            EventArea eventArea = new EventArea { Id = 0 };
            var repository = new AdoUsingParametersRepository<EventArea>(DefaultConnectionString);

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await repository.GetByIDAsync(eventArea.Id));
        }

        [Test]
        public void GetByIdAsync_WhenIdLessThenZero_ShouldThrowArgumentException()
        {
            // Arrange
            EventArea eventArea = new EventArea { Id = -1 };
            var repository = new AdoUsingParametersRepository<EventArea>(DefaultConnectionString);

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await repository.GetByIDAsync(eventArea.Id));
        }

        [Test]
        public void UpdateAsync_WhenIdLessThenZero_ShouldThrowArgumentException()
        {
            // Arrange
            EventArea eventArea = new EventArea { Id = -1 };
            var repository = new AdoUsingParametersRepository<EventArea>(DefaultConnectionString);

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await repository.UpdateAsync(eventArea));
        }

        [Test]
        public void DeleteAsync_WhenIdLessThenZero_ShouldThrowArgumentException()
        {
            // Arrange
            EventArea eventArea = new EventArea { Id = -1 };
            var repository = new AdoUsingParametersRepository<EventArea>(DefaultConnectionString);

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await repository.DeleteAsync(eventArea));
        }
    }
}
