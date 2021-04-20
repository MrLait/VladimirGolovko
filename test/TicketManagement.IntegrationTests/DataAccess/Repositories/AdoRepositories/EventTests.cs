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
    internal class EventTests : TestDatabaseLoader
    {
        private readonly List<Event> _eventModels = new List<Event>();

        [OneTimeSetUp]
        public async Task InitEventsAsync()
        {
            var eventModelRepository = new AdoUsingStoredProcedureRepository<Event>(MainConnectionString);
            var countAllEvents = (await eventModelRepository.GetAllAsync()).Last().Id;

            for (int i = 1; i <= countAllEvents; i++)
            {
                _eventModels.Add(await eventModelRepository.GetByIDAsync(i));
            }
        }

        [Test]
        public async Task GetAllAsync_WhenEventsExist_ShouldReturnEventList()
        {
            // Arrange
            var expected = _eventModels;

            // Act
            var actual = await new AdoUsingStoredProcedureRepository<Event>(MainConnectionString).GetAllAsync();

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void GetAllAsync_WhenEventsIncorrectConnectionSting_ShouldThrowArgumentException()
        {
            // Arrange
            var actual = new AdoUsingStoredProcedureRepository<Event>("Server=myServerAddress;Database=myDataBase;User Id=myUsername;Password=myPassword;");

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await actual.GetAllAsync());
        }

        [Test]
        public async Task CreateAsync_WhenAddEvent_ShouldReturnEventWithNewEvent()
        {
            // Arrange
            var repository = new AdoUsingStoredProcedureRepository<Event>(MainConnectionString);
            Event eventModel = new Event { Id = (await repository.GetAllAsync()).ToList().Count + 1, LayoutId = 2, DateTime = DateTime.Today, Description = "Created event", Name = "Test event" };
            List<Event> expected = new List<Event>(_eventModels)
            {
                eventModel,
            };

            // Act
            await repository.CreateAsync(eventModel);
            var actual = await repository.GetAllAsync();

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void CreateAsync_WhenEventEmpty_ShouldThrowArgumentException()
        {
            // Arrange
            var repository = new AdoUsingStoredProcedureRepository<Event>(MainConnectionString);

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await repository.CreateAsync(null));
        }

        [Test]
        public async Task DeleteAsync_WhenExistEvent_ShouldReturnEventListWithoutDeletedEvent()
        {
            // Arrange
            var repository = new AdoUsingStoredProcedureRepository<Event>(MainConnectionString);

            // Act
            var allEvents = await repository.GetAllAsync();
            var lastEvent = allEvents.Last();
            await repository.DeleteAsync(lastEvent);

            var actual = await repository.GetAllAsync();
            int countEventWithoutLast = allEvents.ToList().Count - 1;

            // Assert
            actual.Should().BeEquivalentTo(allEvents.Take(countEventWithoutLast));
        }

        [Test]
        public void DeleteAsync_WhenIdEqualZeroEvent_ShouldThrowArgumentException()
        {
            // Arrange
            Event eventModel = new Event { Id = 0 };
            var repository = new AdoUsingStoredProcedureRepository<Event>(MainConnectionString);

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await repository.DeleteAsync(eventModel));
        }

        [Test]
        public void DeleteAsync_WhenNullEvent_ShouldThrowArgumentException()
        {
            // Arrange
            var repository = new AdoUsingStoredProcedureRepository<Event>(MainConnectionString);

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await repository.DeleteAsync(null));
        }

        [Test]
        public void DeleteAsync_WhenIncorrectConnectionStringEvent_ShouldThrowArgumentException()
        {
            // Arrange
            Event eventModel = new Event { Id = 3 };
            var repository = new AdoUsingStoredProcedureRepository<Event>("Server=myServerAddress;Database=myDataBase;User Id=myUsername;Password=myPassword;");

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await repository.DeleteAsync(eventModel));
        }

        [Test]
        public async Task UpdateAsync_WhenExistEvent_ShouldUpdateLastEvent()
        {
            // Arrange
            var repository = new AdoUsingStoredProcedureRepository<Event>(MainConnectionString);
            var expected = new Event { LayoutId = 2, Name = "Updated ivent", Description = "Description updated event", DateTime = DateTime.Today };

            // Act
            var lastEvent = (await repository.GetAllAsync()).Last();
            var idLastEvent = lastEvent.Id;
            expected.Id = idLastEvent;

            await repository.UpdateAsync(expected);
            var actual = (await repository.GetAllAsync()).Last();

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void UpdateAsync_WhenNullEvent_ShouldThrowArgumentException()
        {
            // Arrange
            var repository = new AdoUsingStoredProcedureRepository<Event>(MainConnectionString);

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await repository.UpdateAsync(null));
        }

        [Test]
        public void UpdateAsync_WhenIdEqualZeroEvent_ShouldThrowArgumentException()
        {
            // Arrange
            Event eventModel = new Event { Id = 0 };
            var repository = new AdoUsingStoredProcedureRepository<Event>(MainConnectionString);

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await repository.UpdateAsync(eventModel));
        }

        [Test]
        public async Task GetByIdAsync_WhenExistEvent_ShouldReturnEvent()
        {
            // Arrange
            var repository = new AdoUsingStoredProcedureRepository<Event>(MainConnectionString);

            // Act
            var lastEvent = (await repository.GetAllAsync()).Last();
            Event expectedEvent = new Event { Id = lastEvent.Id, DateTime = lastEvent.DateTime, Description = lastEvent.Description, Name = lastEvent.Name, LayoutId = lastEvent.LayoutId };

            int actualId = expectedEvent.Id;
            var actual = await repository.GetByIDAsync(actualId);

            // Assert
            actual.Should().BeEquivalentTo(expectedEvent);
        }

        [Test]
        public async Task GetByIdAsync_WhenNonExistEvent_ShouldReturnNull()
        {
            // Arrange
            Event expected = null;
            var repository = new AdoUsingStoredProcedureRepository<Event>(MainConnectionString);

            // Act
            var lastEvent = (await repository.GetAllAsync()).Last();
            int nonExistId = lastEvent.Id + 1;
            var actual = await repository.GetByIDAsync(nonExistId);

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void GetByIdAsync_WhenIdEqualZeroEvent_ShouldThrowArgumentException()
        {
            // Arrange
            Event eventModel = new Event { Id = 0 };
            var repository = new AdoUsingStoredProcedureRepository<Event>(MainConnectionString);

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await repository.GetByIDAsync(eventModel.Id));
        }

        [Test]
        public void GetByIdAsync_WhenIdLessThenZero_ShouldThrowArgumentException()
        {
            // Arrange
            Event eventModel = new Event { Id = -1 };
            var repository = new AdoUsingParametersRepository<Event>(MainConnectionString);

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await repository.GetByIDAsync(eventModel.Id));
        }

        [Test]
        public void UpdateAsync_WhenIdLessThenZero_ShouldThrowArgumentException()
        {
            // Arrange
            Event eventModel = new Event { Id = -1 };
            var repository = new AdoUsingParametersRepository<Event>(MainConnectionString);

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await repository.UpdateAsync(eventModel));
        }

        [Test]
        public void DeleteAsync_WhenIdLessThenZero_ShouldThrowArgumentException()
        {
            // Arrange
            Event eventModel = new Event { Id = -1 };
            var repository = new AdoUsingParametersRepository<Event>(MainConnectionString);

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await repository.DeleteAsync(eventModel));
        }
    }
}
