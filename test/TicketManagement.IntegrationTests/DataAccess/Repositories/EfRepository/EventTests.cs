using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using TicketManagement.DataAccess.Ado;
using TicketManagement.DataAccess.Domain.Models;
using TicketManagement.DataAccess.Repositories.EfRepositories;

namespace TicketManagement.IntegrationTests.DataAccess.Repositories.EfRepository
{
    [TestFixture]
    internal class EventTests : TestDatabaseLoader
    {
        private readonly List<Event> _events = new List<Event>();

        public EfDbContext DbContext { get; set; }

        [OneTimeSetUp]
        public async Task InitEventsAsync()
        {
            DbContext = new EfDbContext(connectionString: MainConnectionString);
            var repository = new EfRepository<Event>(DbContext);
            var countAllEvents = repository.GetAllAsQueryable().OrderBy(x => x.Id).Last().Id;

            for (int i = 1; i <= countAllEvents; i++)
            {
                _events.Add(await repository.GetByIDAsync(i));
            }
        }

        [Test]
        public void GetAllAsQueryable_WhenEventsExist_ShouldReturnEventList()
        {
            // Arrange
            var expected = _events;

            // Act
            var actual = new EfRepository<Event>(DbContext).GetAllAsQueryable();

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public async Task CreateAsync_WhenAddEvent_ShouldReturnEventWithNewEvent()
        {
            // Arrange
            var repository = new EfRepository<Event>(DbContext);
            Event eventModel = new Event
            {
                LayoutId = 2,
                StartDateTime = DateTime.Today,
                Description = "Created event",
                ImageUrl = "Test event",
                Name = "Test event",
                EndDateTime = DateTime.Today.AddDays(1),
            };
            List<Event> expected = new List<Event>(_events)
            {
                eventModel,
            };

            // Act
            await repository.CreateAsync(eventModel);
            var actual = repository.GetAllAsQueryable();

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void CreateAsync_WhenEventEmpty_ShouldThrowArgumentNullException()
        {
            // Arrange
            var repository = new EfRepository<Event>(DbContext);

            // Act & Assert
            Assert.ThrowsAsync<ArgumentNullException>(async () => await repository.CreateAsync(null));
        }

        ////[Test]
        ////public async Task DeleteAsync_WhenExistEvent_ShouldReturnEventListWithoutDeletedEventAsync()
        ////{
        ////    // Arrange
        ////    var repository = new EfRepository<Event>(DbContext);

        ////    // Act
        ////    var allEvents = repository.GetAllAsQueryable().ToList();
        ////    var lastEvent = allEvents.LastOrDefault();
        ////    await repository.DeleteAsync(lastEvent);

        ////    var actual = repository.GetAllAsQueryable();
        ////    int countEventWithoutLast = allEvents.ToList().Count - 1;

        ////    // Assert
        ////    actual.Should().BeEquivalentTo(allEvents.Take(countEventWithoutLast));
        ////}

        [Test]
        public void DeleteAsync_WhenIdEqualZeroEvent_ShouldThrowInvalidOperationException()
        {
            // Arrange
            var event_ = new Event { Id = 0 };
            var repository = new EfRepository<Event>(DbContext);

            // Assert
            Assert.ThrowsAsync<InvalidOperationException>(async () => await repository.DeleteAsync(event_));
        }

        [Test]
        public void DeleteAsync_WhenNullEvent_ShouldThrowArgumentNullException()
        {
            // Arrange
            var repository = new EfRepository<Event>(DbContext);

            // Assert
            Assert.ThrowsAsync<ArgumentNullException>(async () => await repository.DeleteAsync(null));
        }

        ////[Test]
        ////public async Task UpdateAsync_WhenExistEvent_ShouldUpdateLastEventAsync()
        ////{
        ////    // Arrange
        ////    var repository = new EfRepository<Event>(DbContext);
        ////    var expected = new Event
        ////    {
        ////        LayoutId = 2,
        ////        Name = "Updated ivent",
        ////        Description = "Description updated event",
        ////        StartDateTime = DateTime.Today.AddDays(1),
        ////        EndDateTime = DateTime.Today.AddDays(2),
        ////        ImageUrl = "asd",
        ////    };

        ////    // Act
        ////    var lastEvent = repository.GetAllAsQueryable().OrderBy(x => x.Id).Last();
        ////    var idLastEvent = lastEvent.Id;
        ////    expected.Id = idLastEvent;

        ////    await repository.UpdateAsync(expected);
        ////    var actual = repository.GetAllAsQueryable().Last();

        ////    // Assert
        ////    actual.Should().BeEquivalentTo(expected);
        ////}

        [Test]
        public void UpdateAsync_WhenNullEvent_ShouldThrowArgumentNullException()
        {
            // Arrange
            var repository = new EfRepository<Event>(DbContext);

            // Act & Assert
            Assert.ThrowsAsync<ArgumentNullException>(async () => await repository.UpdateAsync(null));
        }

        [Test]
        public void UpdateAsync_WhenIdEqualZeroEvent_ShouldThrowDbUpdateConcurrencyException()
        {
            // Arrange
            var event_ = new Event { Id = 0 };
            var repository = new EfRepository<Event>(DbContext);

            // Act & Assert
            Assert.ThrowsAsync<DbUpdateConcurrencyException>(async () => await repository.UpdateAsync(event_));
        }

        [Test]
        public async Task GetByIdAsync_WhenExistEvent_ShouldReturnEventAsync()
        {
            // Arrange
            var repository = new EfRepository<Event>(DbContext);

            // Act
            var lastEvent = repository.GetAllAsQueryable().OrderBy(x => x.Id).LastOrDefault();
            Event expectedEvent = new Event
            {
                Id = lastEvent.Id,
                StartDateTime = lastEvent.StartDateTime,
                Description = lastEvent.Description,
                Name = lastEvent.Name,
                LayoutId = lastEvent.LayoutId,
                EndDateTime = lastEvent.EndDateTime,
                ImageUrl = lastEvent.ImageUrl,
            };

            int actualId = expectedEvent.Id;
            var actual = await repository.GetByIDAsync(actualId);

            // Assert
            actual.Should().BeEquivalentTo(expectedEvent);
        }

        [Test]
        public async Task GetByIdAsync_WhenNonExistEvent_ShouldReturnNullAsync()
        {
            // Arrange
            Event expected = null;
            var repository = new EfRepository<Event>(DbContext);

            // Act
            var lastEvent = repository.GetAllAsQueryable().OrderBy(x => x.Id).Last();
            int nonExistId = lastEvent.Id + 10;
            var actual = await repository.GetByIDAsync(nonExistId);

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void UpdateAsync_WhenIdLessThenZero_ShouldThrowDbUpdateConcurrencyException()
        {
            // Arrange
            var event_ = new Event { Id = -1 };
            var repository = new EfRepository<Event>(DbContext);

            // Act & Assert
            Assert.ThrowsAsync<DbUpdateConcurrencyException>(async () => await repository.UpdateAsync(event_));
        }

        [Test]
        public void DeleteAsync_WhenIdLessThenZero_ShouldThrowDbUpdateConcurrencyException()
        {
            // Arrange
            var event_ = new Event { Id = -1 };
            var repository = new EfRepository<Event>(DbContext);

            // Act & Assert
            Assert.ThrowsAsync<DbUpdateConcurrencyException>(async () => await repository.DeleteAsync(event_));
        }
    }
}
