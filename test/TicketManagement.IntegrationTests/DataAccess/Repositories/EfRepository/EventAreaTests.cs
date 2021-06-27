using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using TicketManagement.DataAccess.DbContexts;
using TicketManagement.DataAccess.Domain.Models;
using TicketManagement.DataAccess.Repositories.EfRepositories;

namespace TicketManagement.IntegrationTests.DataAccess.Repositories.EfRepository
{
    [TestFixture]
    internal class EventAreaTests : TestDatabaseLoader
    {
        private readonly List<EventArea> _eventAreas = new ();

        public EfDbContext DbContext { get; set; }

        [OneTimeSetUp]
        public async Task InitEventAreasAsync()
        {
            DbContext = new EfDbContext(connectionString: DefaultConnectionString);
            var repository = new EfRepository<EventArea>(DbContext);
            var countAllEventAreas = repository.GetAllAsQueryable().OrderBy(x => x.Id).Last().Id;

            for (int i = 1; i <= countAllEventAreas; i++)
            {
                _eventAreas.Add(await repository.GetByIdAsync(i));
            }
        }

        [Test]
        public void GetAllAsQueryable_WhenEventAreasExist_ShouldReturnEventAreaList()
        {
            // Arrange
            var expected = _eventAreas;

            // Act
            var actual = new EfRepository<EventArea>(DbContext).GetAllAsQueryable();

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public async Task CreateAsync_WhenAddEventArea_ShouldReturnEventAreaWithNewEventArea()
        {
            // Arrange
            var repository = new EfRepository<EventArea>(DbContext);
            var eventArea = new EventArea { CoordX = 2, CoordY = 2, Description = "Creaded", EventId = 2, Price = 10 };
            var expected = new List<EventArea>(_eventAreas)
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
        public void CreateAsync_WhenEventAreaEmpty_ShouldThrowArgumentNullException()
        {
            // Arrange
            var repository = new EfRepository<EventArea>(DbContext);

            // Act & Assert
            Assert.ThrowsAsync<ArgumentNullException>(async () => await repository.CreateAsync(null));
        }

        [Test]
        public async Task DeleteAsync_WhenExistEventArea_ShouldReturnEventAreaListWithoutDeletedEventAreaAsync()
        {
            // Arrange
            var dbContext = new EfDbContext(connectionString: DefaultConnectionString);
            var repository = new EfRepository<EventArea>(dbContext);

            // Act
            var allEventAreas = repository.GetAllAsQueryable().OrderBy(x => x.Id).ToList();
            var lastEventArea = allEventAreas.LastOrDefault();
            await repository.DeleteAsync(lastEventArea);

            var actual = repository.GetAllAsQueryable();
            var countEventAreaWithoutLast = allEventAreas.ToList().Count - 1;

            // Assert
            actual.Should().BeEquivalentTo(allEventAreas.Take(countEventAreaWithoutLast));
        }

        [Test]
        public void DeleteAsync_WhenIdEqualZeroEventArea_ShouldThrowInvalidOperationException()
        {
            // Arrange
            var eventArea = new EventArea { Id = 0 };
            var repository = new EfRepository<EventArea>(DbContext);

            // Assert
            Assert.ThrowsAsync<InvalidOperationException>(async () => await repository.DeleteAsync(eventArea));
        }

        [Test]
        public void DeleteAsync_WhenNullEventArea_ShouldThrowArgumentNullException()
        {
            // Arrange
            var repository = new EfRepository<EventArea>(DbContext);

            // Assert
            Assert.ThrowsAsync<ArgumentNullException>(async () => await repository.DeleteAsync(null));
        }

        [Test]
        public async Task UpdateAsync_WhenExistEventArea_ShouldUpdateLastEventAreaAsync()
        {
            // Arrange
            var dbContext = new EfDbContext(connectionString: DefaultConnectionString);
            var repository = new EfRepository<EventArea>(dbContext);
            var expected = new EventArea { Price = 2, EventId = 2, Description = "Updated", CoordY = 2, CoordX = 2 };

            // Act
            var lastEventArea = repository.GetAllAsQueryable().OrderBy(x => x.Id).Last();
            var idLastEventArea = lastEventArea.Id;
            expected.Id = idLastEventArea;

            await repository.UpdateAsync(expected);
            var actual = repository.GetAllAsQueryable().OrderBy(x => x.Id).Last();

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void UpdateAsync_WhenNullEventArea_ShouldThrowArgumentNullException()
        {
            // Arrange
            var repository = new EfRepository<EventArea>(DbContext);

            // Act & Assert
            Assert.ThrowsAsync<ArgumentNullException>(async () => await repository.UpdateAsync(null));
        }

        [Test]
        public void UpdateAsync_WhenIdEqualZeroEventArea_ShouldThrowDbUpdateConcurrencyException()
        {
            // Arrange
            var eventArea = new EventArea { Id = 0 };
            var repository = new EfRepository<EventArea>(DbContext);

            // Act & Assert
            Assert.ThrowsAsync<DbUpdateConcurrencyException>(async () => await repository.UpdateAsync(eventArea));
        }

        [Test]
        public async Task GetByIdAsync_WhenExistEventArea_ShouldReturnEventAreaAsync()
        {
            // Arrange
            var repository = new EfRepository<EventArea>(DbContext);

            // Act
            var lastEventArea = repository.GetAllAsQueryable().OrderBy(x => x.Id).LastOrDefault();
            var expectedEventArea = new EventArea
            {
                Id = lastEventArea.Id,
                CoordX = lastEventArea.CoordX,
                CoordY = lastEventArea.CoordY,
                Description = lastEventArea.Description,
                EventId = lastEventArea.EventId,
                Price = lastEventArea.Price,
            };

            var actualId = expectedEventArea.Id;
            var actual = await repository.GetByIdAsync(actualId);

            // Assert
            actual.Should().BeEquivalentTo(expectedEventArea);
        }

        [Test]
        public async Task GetByIdAsync_WhenNonExistEventArea_ShouldReturnNullAsync()
        {
            // Arrange
            var repository = new EfRepository<EventArea>(DbContext);

            // Act
            var nonExistId = repository.GetAllAsQueryable().OrderBy(x => x.Id).Last().Id + 10;
            var actual = await repository.GetByIdAsync(nonExistId);

            // Assert
            actual.Should().BeEquivalentTo((EventArea) null);
        }

        [Test]
        public void UpdateAsync_WhenIdLessThenZero_ShouldThrowDbUpdateConcurrencyException()
        {
            // Arrange
            var eventArea = new EventArea { Id = -1 };
            var repository = new EfRepository<EventArea>(DbContext);

            // Act & Assert
            Assert.ThrowsAsync<DbUpdateConcurrencyException>(async () => await repository.UpdateAsync(eventArea));
        }

        [Test]
        public void DeleteAsync_WhenIdLessThenZero_ShouldThrowDbUpdateConcurrencyException()
        {
            // Arrange
            var eventArea = new EventArea { Id = -1 };
            var repository = new EfRepository<EventArea>(DbContext);

            // Act & Assert
            Assert.ThrowsAsync<DbUpdateConcurrencyException>(async () => await repository.DeleteAsync(eventArea));
        }
    }
}
