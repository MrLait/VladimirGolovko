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
    internal class EventTests : TestDatabaseLoader
    {
        private readonly List<Event> _events = new List<Event>();

        public EfDbContext DbContext { get; set; }

        [OneTimeSetUp]
        public async Task InitEventsAsync()
        {
            DbContext = new EfDbContext(DefaultConnectionString);
            var repository = new EfRepositoryUsingStoredProcedure<Event>(DbContext);
            var countAllEvents = repository.GetAllAsQueryable().AsEnumerable().Last().Id;

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
            var actual = new EfRepositoryUsingStoredProcedure<Event>(DbContext).GetAllAsQueryable();

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public async Task CreateAsync_WhenAddEvent_ShouldReturnEventWithNewEvent()
        {
            // Arrange
            var dbContext = new EfDbContext(DefaultConnectionString);
            var repository = new EfRepositoryUsingStoredProcedure<Event>(dbContext);
            var lastItem = repository.GetAllAsQueryable().AsEnumerable().Last();
            Event eventModel = new Event
            {
                LayoutId = 2,
                StartDateTime = DateTime.Today.AddDays(2),
                Description = "Created event",
                ImageUrl = "Test event",
                Name = "Test event",
                EndDateTime = DateTime.Today.AddDays(3),
            };

            var expected = eventModel;
            expected.Id = lastItem.Id + 1;

            // Act
            await repository.CreateAsync(eventModel);
            var actual = repository.GetAllAsQueryable().AsEnumerable().Last();

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void CreateAsync_WhenEventEmpty_ShouldThrowNullReferenceException()
        {
            // Arrange
            var repository = new EfRepositoryUsingStoredProcedure<Event>(DbContext);

            // Act & Assert
            Assert.ThrowsAsync<NullReferenceException>(async () => await repository.CreateAsync(null));
        }

        [Test]
        public async Task DeleteAsync_WhenExistEvent_ShouldReturnEventListWithoutDeletedEventAsync()
        {
            // Arrange
            var dbContext = new EfDbContext(connectionString: DefaultConnectionString);
            var repository = new EfRepositoryUsingStoredProcedure<Event>(dbContext);

            // Act
            var allEvents = repository.GetAllAsQueryable().ToList();
            var lastEvent = allEvents.LastOrDefault();
            await repository.DeleteAsync(lastEvent);

            var actual = repository.GetAllAsQueryable();
            int countEventWithoutLast = allEvents.ToList().Count - 1;

            // Assert
            actual.Should().BeEquivalentTo(allEvents.Take(countEventWithoutLast));
        }

        [Test]
        public void DeleteAsync_WhenNullEvent_ShouldThrowNullReferenceException()
        {
            // Arrange
            var repository = new EfRepositoryUsingStoredProcedure<Event>(DbContext);

            // Assert
            Assert.ThrowsAsync<NullReferenceException>(async () => await repository.DeleteAsync(null));
        }

        [Test]
        public async Task UpdateAsync_WhenExistEvent_ShouldUpdateLastEventAsync()
        {
            // Arrange
            var dbContext = new EfDbContext(connectionString: DefaultConnectionString);
            var repository = new EfRepositoryUsingStoredProcedure<Event>(dbContext);
            var expected = new Event
            {
                LayoutId = 2,
                Name = "Updated ivent",
                Description = "Description updated event",
                StartDateTime = DateTime.Today.AddDays(1),
                EndDateTime = DateTime.Today.AddDays(2),
                ImageUrl = "asd",
            };

            // Act
            var lastEvent = repository.GetAllAsQueryable().AsEnumerable().Last();
            var idLastEvent = lastEvent.Id;
            expected.Id = idLastEvent;

            await repository.UpdateAsync(expected);
            var actual = repository.GetAllAsQueryable().AsNoTracking().AsEnumerable().Last();

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void UpdateAsync_WhenNullEvent_ShouldThrowNullReferenceException()
        {
            // Arrange
            var repository = new EfRepositoryUsingStoredProcedure<Event>(DbContext);

            // Act & Assert
            Assert.ThrowsAsync<NullReferenceException>(async () => await repository.UpdateAsync(null));
        }

        [Test]
        public async Task GetByIdAsync_WhenExistEvent_ShouldReturnEventAsync()
        {
            // Arrange
            var repository = new EfRepositoryUsingStoredProcedure<Event>(DbContext);

            // Act
            var lastEvent = repository.GetAllAsQueryable().AsEnumerable().LastOrDefault();
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
            var repository = new EfRepositoryUsingStoredProcedure<Event>(DbContext);

            // Act
            var lastEvent = repository.GetAllAsQueryable().AsEnumerable().Last();
            int nonExistId = lastEvent.Id + 10;
            var actual = await repository.GetByIDAsync(nonExistId);

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }
    }
}
