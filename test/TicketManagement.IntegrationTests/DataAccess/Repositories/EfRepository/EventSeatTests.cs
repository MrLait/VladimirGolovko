using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using TicketManagement.DataAccess.Ado;
using TicketManagement.DataAccess.Domain.Models;
using TicketManagement.DataAccess.Enums;
using TicketManagement.DataAccess.Repositories.EfRepositories;

namespace TicketManagement.IntegrationTests.DataAccess.Repositories.EfRepository
{
    [TestFixture]
    internal class EventSeatTests : TestDatabaseLoader
    {
        private readonly List<EventSeat> _eventSeats = new List<EventSeat>();

        public EfDbContext DbContext { get; set; }

        [OneTimeSetUp]
        public async Task InitEventSeatsAsync()
        {
            DbContext = new EfDbContext(connectionString: MainConnectionString);
            var repository = new EfRepository<EventSeat>(DbContext);
            var countAllEventSeats = repository.GetAllAsQueryable().OrderBy(x => x.Id).Last().Id;

            for (int i = 1; i <= countAllEventSeats; i++)
            {
                _eventSeats.Add(await repository.GetByIDAsync(i));
            }
        }

        [Test]
        public void GetAllAsQueryable_WhenEventSeatsExist_ShouldReturnEventSeatList()
        {
            // Arrange
            var expected = _eventSeats;

            // Act
            var actual = new EfRepository<EventSeat>(DbContext).GetAllAsQueryable();

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public async Task CreateAsync_WhenAddEventSeat_ShouldReturnEventSeatWithNewEventSeat()
        {
            // Arrange
            var repository = new EfRepository<EventSeat>(DbContext);
            EventSeat eventSeat = new EventSeat { EventAreaId = 2, Number = 2, Row = 2, State = (int)States.Booked };
            var expected = new List<EventSeat>(_eventSeats)
            {
                eventSeat,
            };

            // Act
            await repository.CreateAsync(eventSeat);
            var actual = repository.GetAllAsQueryable();

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void CreateAsync_WhenEventSeatEmpty_ShouldThrowArgumentNullException()
        {
            // Arrange
            var repository = new EfRepository<EventSeat>(DbContext);

            // Act & Assert
            Assert.ThrowsAsync<ArgumentNullException>(async () => await repository.CreateAsync(null));
        }

        ////[Test]
        ////public async Task DeleteAsync_WhenExistEventSeat_ShouldReturnEventSeatListWithoutDeletedEventSeatAsync()
        ////{
        ////    // Arrange
        ////    var repository = new EfRepository<EventSeat>(DbContext);

        ////    // Act
        ////    var allEventSeats = repository.GetAllAsQueryable().ToList();
        ////    var lastEventSeat = allEventSeats.LastOrDefault();
        ////    await repository.DeleteAsync(lastEventSeat);

        ////    var actual = repository.GetAllAsQueryable();
        ////    int countEventSeatWithoutLast = allEventSeats.ToList().Count - 1;

        ////    // Assert
        ////    actual.Should().BeEquivalentTo(allEventSeats.Take(countEventSeatWithoutLast));
        ////}

        [Test]
        public void DeleteAsync_WhenIdEqualZeroEventSeat_ShouldThrowInvalidOperationException()
        {
            // Arrange
            var eventSeat = new EventSeat { Id = 0 };
            var repository = new EfRepository<EventSeat>(DbContext);

            // Assert
            Assert.ThrowsAsync<InvalidOperationException>(async () => await repository.DeleteAsync(eventSeat));
        }

        [Test]
        public void DeleteAsync_WhenNullEventSeat_ShouldThrowArgumentNullException()
        {
            // Arrange
            var repository = new EfRepository<EventSeat>(DbContext);

            // Assert
            Assert.ThrowsAsync<ArgumentNullException>(async () => await repository.DeleteAsync(null));
        }

        [Test]
        public async Task UpdateAsync_WhenExistEventSeat_ShouldUpdateLastEventSeatAsync()
        {
            // Arrange
            var dbContext = new EfDbContext(connectionString: MainConnectionString);
            var repository = new EfRepository<EventSeat>(dbContext);
            var expected = new EventSeat { EventAreaId = 2, State = (int)States.Purchased, Row = 2, Number = 2 };

            // Act
            var lastEventSeat = repository.GetAllAsQueryable().OrderBy(x => x.Id).Last();
            var idLastEventSeat = lastEventSeat.Id;
            expected.Id = idLastEventSeat;

            await repository.UpdateAsync(expected);
            var actual = repository.GetAllAsQueryable().OrderBy(x => x.Id).Last();

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void UpdateAsync_WhenNullEventSeat_ShouldThrowArgumentNullException()
        {
            // Arrange
            var repository = new EfRepository<EventSeat>(DbContext);

            // Act & Assert
            Assert.ThrowsAsync<ArgumentNullException>(async () => await repository.UpdateAsync(null));
        }

        [Test]
        public void UpdateAsync_WhenIdEqualZeroEventSeat_ShouldThrowDbUpdateConcurrencyException()
        {
            // Arrange
            var eventSeat = new EventSeat { Id = 0 };
            var repository = new EfRepository<EventSeat>(DbContext);

            // Act & Assert
            Assert.ThrowsAsync<DbUpdateConcurrencyException>(async () => await repository.UpdateAsync(eventSeat));
        }

        [Test]
        public async Task GetByIdAsync_WhenExistEventSeat_ShouldReturnEventSeatAsync()
        {
            // Arrange
            var repository = new EfRepository<EventSeat>(DbContext);

            // Act
            var lastEventSeat = repository.GetAllAsQueryable().OrderBy(x => x.Id).LastOrDefault();
            var expectedEventSeat = new EventSeat
            {
                Id = lastEventSeat.Id,
                Number = lastEventSeat.Number,
                Row = lastEventSeat.Row,
                State = lastEventSeat.State,
                EventAreaId = lastEventSeat.EventAreaId,
            };

            int actualId = expectedEventSeat.Id;
            var actual = await repository.GetByIDAsync(actualId);

            // Assert
            actual.Should().BeEquivalentTo(expectedEventSeat);
        }

        [Test]
        public async Task GetByIdAsync_WhenNonExistEventSeat_ShouldReturnNullAsync()
        {
            // Arrange
            EventSeat expected = null;
            var repository = new EfRepository<EventSeat>(DbContext);

            // Act
            var lastEventSeat = repository.GetAllAsQueryable().OrderBy(x => x.Id).Last();
            int nonExistId = lastEventSeat.Id + 10;
            var actual = await repository.GetByIDAsync(nonExistId);

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void UpdateAsync_WhenIdLessThenZero_ShouldThrowDbUpdateConcurrencyException()
        {
            // Arrange
            var eventSeat = new EventSeat { Id = -1 };
            var repository = new EfRepository<EventSeat>(DbContext);

            // Act & Assert
            Assert.ThrowsAsync<DbUpdateConcurrencyException>(async () => await repository.UpdateAsync(eventSeat));
        }

        [Test]
        public void DeleteAsync_WhenIdLessThenZero_ShouldThrowDbUpdateConcurrencyException()
        {
            // Arrange
            var eventSeat = new EventSeat { Id = -1 };
            var repository = new EfRepository<EventSeat>(DbContext);

            // Act & Assert
            Assert.ThrowsAsync<DbUpdateConcurrencyException>(async () => await repository.DeleteAsync(eventSeat));
        }
    }
}
