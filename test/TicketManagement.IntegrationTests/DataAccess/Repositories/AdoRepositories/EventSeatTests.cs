﻿using System;
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
    internal class EventSeatTests : TestDatabaseLoader
    {
        private readonly List<EventSeat> _eventSeats = new List<EventSeat>();

        [OneTimeSetUp]
        public async Task InitEventSeatsAsync()
        {
            var eventSeatsRepository = new AdoUsingParametersRepository<EventSeat>(MainConnectionString);
            var countAllEventSeats = (await eventSeatsRepository.GetAllAsync()).Last().Id;

            for (var i = 1; i <= countAllEventSeats; i++)
            {
                _eventSeats.Add(await eventSeatsRepository.GetByIDAsync(i));
            }
        }

        [Test]
        public async Task GetAllAsync_WhenEventSeatsExist_ShouldReturnEventSeatList()
        {
            // Arrange
            var expected = _eventSeats;

            // Act
            var actual = await new AdoUsingParametersRepository<EventSeat>(MainConnectionString).GetAllAsync();

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void GetAllAsync_WhenEventSeatsIncorrectConnectionSting_ShouldThrowArgumentException()
        {
            // Arrange
            var actual = new AdoUsingParametersRepository<EventSeat>("Server=myServerAddress;Database=myDataBase;User Id=myUsername;Password=myPassword;");

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await actual.GetAllAsync());
        }

        [Test]
        public async Task CreateAsync_WhenAddEventSeat_ShouldReturnEventSeatWithNewEventSeat()
        {
            // Arrange
            var repository = new AdoUsingParametersRepository<EventSeat>(MainConnectionString);
            EventSeat eventSeat = new EventSeat { Id = (await repository.GetAllAsync()).ToList().Count + 1, EventAreaId = 2, Number = 2, Row = 2, State = 2 };
            List<EventSeat> expected = new List<EventSeat>(_eventSeats)
            {
                eventSeat,
            };

            // Act
            await repository.CreateAsync(eventSeat);
            var actual = await repository.GetAllAsync();

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void CreateAsync_WhenEventSeatEmpty_ShouldThrowArgumentException()
        {
            // Arrange
            var repository = new AdoUsingParametersRepository<EventSeat>(MainConnectionString);

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await repository.CreateAsync(null));
        }

        [Test]
        public async Task DeleteAsync_WhenExistEventSeat_ShouldReturnEventSeatListWithoutDeletedEventSeat()
        {
            // Arrange
            var repository = new AdoUsingParametersRepository<EventSeat>(MainConnectionString);

            // Act
            var allEventSeats = await repository.GetAllAsync();
            var lastEventSeat = allEventSeats.Last();
            await repository.DeleteAsync(lastEventSeat);

            var actual = await repository.GetAllAsync();
            int countEventSeatWithoutLast = allEventSeats.ToList().Count - 1;

            // Assert
            actual.Should().BeEquivalentTo(allEventSeats.Take(countEventSeatWithoutLast));
        }

        [Test]
        public void DeleteAsync_WhenIdEqualZeroEventSeat_ShouldThrowArgumentException()
        {
            // Arrange
            EventSeat eventSeat = new EventSeat { Id = 0 };
            var repository = new AdoUsingParametersRepository<EventSeat>(MainConnectionString);

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await repository.DeleteAsync(eventSeat));
        }

        [Test]
        public void DeleteAsync_WhenNullEventSeat_ShouldThrowArgumentException()
        {
            // Arrange
            var repository = new AdoUsingParametersRepository<EventSeat>(MainConnectionString);

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await repository.DeleteAsync(null));
        }

        [Test]
        public void DeleteAsync_WhenIncorrectConnectionStringEventSeat_ShouldThrowArgumentException()
        {
            // Arrange
            EventSeat eventSeat = new EventSeat { Id = 3 };
            var repository = new AdoUsingParametersRepository<EventSeat>("Server=myServerAddress;Database=myDataBase;User Id=myUsername;Password=myPassword;");

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await repository.DeleteAsync(eventSeat));
        }

        [Test]
        public async Task UpdateAsync_WhenExistEventSeat_ShouldUpdateLastEventSeat()
        {
            // Arrange
            var repository = new AdoUsingParametersRepository<EventSeat>(MainConnectionString);
            var expected = new EventSeat { EventAreaId = 2, State = 1, Row = 2, Number = 2 };

            // Act
            var lastEventSeat = (await repository.GetAllAsync()).Last();
            var idLastEventSeat = lastEventSeat.Id;
            expected.Id = idLastEventSeat;

            await repository.UpdateAsync(expected);
            var actual = (await repository.GetAllAsync()).Last();

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void UpdateAsync_WhenNullEventSeat_ShouldThrowArgumentException()
        {
            // Arrange
            var repository = new AdoUsingParametersRepository<EventSeat>(MainConnectionString);

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await repository.UpdateAsync(null));
        }

        [Test]
        public void UpdateAsync_WhenIdEqualZeroEventSeat_ShouldThrowArgumentException()
        {
            // Arrange
            EventSeat eventSeat = new EventSeat { Id = 0 };
            var repository = new AdoUsingParametersRepository<EventSeat>(MainConnectionString);

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await repository.UpdateAsync(eventSeat));
        }

        [Test]
        public async Task GetByIdAsync_WhenExistEventSeat_ShouldReturnEventSeat()
        {
            // Arrange
            var repository = new AdoUsingParametersRepository<EventSeat>(MainConnectionString);

            // Act
            var lastEventSeat = (await repository.GetAllAsync()).Last();
            EventSeat expectedEventSeat = new EventSeat
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
        public async Task GetByIdAsync_WhenNonExistEventSeat_ShouldReturnNull()
        {
            // Arrange
            EventSeat expected = null;
            var repository = new AdoUsingParametersRepository<EventSeat>(MainConnectionString);

            // Act
            var lastEventSeat = (await repository.GetAllAsync()).Last();
            int nonExistId = lastEventSeat.Id + 1;
            var actual = await repository.GetByIDAsync(nonExistId);

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void GetByIdAsync_WhenIdEqualZeroEventSeat_ShouldThrowArgumentException()
        {
            // Arrange
            EventSeat eventSeat = new EventSeat { Id = 0 };
            var repository = new AdoUsingParametersRepository<EventSeat>(MainConnectionString);

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await repository.GetByIDAsync(eventSeat.Id));
        }

        [Test]
        public void GetByIdAsync_WhenIdLessThenZero_ShouldThrowArgumentException()
        {
            // Arrange
            EventSeat eventSeat = new EventSeat { Id = -1 };
            var repository = new AdoUsingParametersRepository<EventSeat>(MainConnectionString);

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await repository.GetByIDAsync(eventSeat.Id));
        }

        [Test]
        public void UpdateAsync_WhenIdLessThenZero_ShouldThrowArgumentException()
        {
            // Arrange
            EventSeat eventSeat = new EventSeat { Id = -1 };
            var repository = new AdoUsingParametersRepository<EventSeat>(MainConnectionString);

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await repository.UpdateAsync(eventSeat));
        }

        [Test]
        public void DeleteAsync_WhenIdLessThenZero_ShouldThrowArgumentException()
        {
            // Arrange
            EventSeat eventSeat = new EventSeat { Id = -1 };
            var repository = new AdoUsingParametersRepository<EventSeat>(MainConnectionString);

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await repository.DeleteAsync(eventSeat));
        }
    }
}
