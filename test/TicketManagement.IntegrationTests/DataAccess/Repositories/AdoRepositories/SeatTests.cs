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
    internal class SeatTests : TestDatabaseLoader
    {
        private readonly List<Seat> _seats = new ();

        [OneTimeSetUp]
        public async Task InitSeatsAsync()
        {
            var seatRepository = new AdoUsingParametersRepository<Seat>(DefaultConnectionString);
            var countAllSeats = seatRepository.GetAllAsQueryable().Last().Id;

            for (var i = 1; i <= countAllSeats; i++)
            {
                _seats.Add(await seatRepository.GetByIdAsync(i));
            }
        }

        [Test]
        public void GetAllAsQueryable_WhenSeatsExist_ShouldReturnSeatList()
        {
            // Arrange
            var expected = _seats;

            // Act
            var actual = new AdoUsingParametersRepository<Seat>(DefaultConnectionString).GetAllAsQueryable();

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void GetAllAsQueryable_WhenSeatsIncorrectConnectionSting_ShouldThrowArgumentException()
        {
            // Arrange
            var actual = new AdoUsingParametersRepository<Seat>("Server=myServerAddress;Database=myDataBase;User Id=myUsername;Password=myPassword;");

            // Act & Assert
            Assert.Throws<ArgumentException>(() => actual.GetAllAsQueryable());
        }

        [Test]
        public async Task CreateAsync_WhenAddSeat_ShouldReturnSeatWithNewSeat()
        {
            // Arrange
            var repository = new AdoUsingParametersRepository<Seat>(DefaultConnectionString);
            var seat = new Seat { Id = repository.GetAllAsQueryable().ToList().Count + 1, Row = 2, AreaId = 2, Number = 2 };
            var expected = new List<Seat>(_seats)
            {
                seat,
            };

            // Act
            await repository.CreateAsync(seat);
            var actual = repository.GetAllAsQueryable();

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void CreateAsync_WhenSeatEmpty_ShouldThrowArgumentException()
        {
            // Arrange
            var repository = new AdoUsingParametersRepository<Seat>(DefaultConnectionString);

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await repository.CreateAsync(null));
        }

        [Test]
        public async Task DeleteAsync_WhenExistSeat_ShouldReturnSeatListWithoutDeletedSeat()
        {
            // Arrange
            var repository = new AdoUsingParametersRepository<Seat>(DefaultConnectionString);

            // Act
            var allSeats = repository.GetAllAsQueryable();
            var lastSeat = allSeats.Last();
            await repository.DeleteAsync(lastSeat);

            var actual = repository.GetAllAsQueryable();
            var countSeatWithoutLast = allSeats.ToList().Count - 1;

            // Assert
            actual.Should().BeEquivalentTo(allSeats.Take(countSeatWithoutLast));
        }

        [Test]
        public void DeleteAsync_WhenIdEqualZeroSeat_ShouldThrowArgumentException()
        {
            // Arrange
            var seat = new Seat { Id = 0 };
            var repository = new AdoUsingParametersRepository<Seat>(DefaultConnectionString);

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await repository.DeleteAsync(seat));
        }

        [Test]
        public void DeleteAsync_WhenNullSeat_ShouldThrowArgumentException()
        {
            // Arrange
            var repository = new AdoUsingParametersRepository<Seat>(DefaultConnectionString);

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await repository.DeleteAsync(null));
        }

        [Test]
        public void DeleteAsync_WhenIncorrectConnectionStringSeat_ShouldThrowArgumentException()
        {
            // Arrange
            var seat = new Seat { Id = 3 };
            var repository = new AdoUsingParametersRepository<Seat>("Server=myServerAddress;Database=myDataBase;User Id=myUsername;Password=myPassword;");

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await repository.DeleteAsync(seat));
        }

        [Test]
        public async Task UpdateAsync_WhenExistSeat_ShouldUpdateLastSeat()
        {
            // Arrange
            var repository = new AdoUsingParametersRepository<Seat>(DefaultConnectionString);
            var expected = new Seat { Number = 2, AreaId = 2, Row = 2 };

            // Act
            var lastSeat = repository.GetAllAsQueryable().Last();
            var idLastSeat = lastSeat.Id;
            expected.Id = idLastSeat;

            await repository.UpdateAsync(expected);
            var actual = repository.GetAllAsQueryable().Last();

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void UpdateAsync_WhenNullSeat_ShouldThrowArgumentException()
        {
            // Arrange
            var repository = new AdoUsingParametersRepository<Seat>(DefaultConnectionString);

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await repository.UpdateAsync(null));
        }

        [Test]
        public void UpdateAsync_WhenIdEqualZeroSeat_ShouldThrowArgumentException()
        {
            // Arrange
            var seat = new Seat { Id = 0 };
            var repository = new AdoUsingParametersRepository<Seat>(DefaultConnectionString);

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await repository.UpdateAsync(seat));
        }

        [Test]
        public async Task GetByIdAsync_WhenExistSeat_ShouldReturnSeat()
        {
            // Arrange
            var repository = new AdoUsingParametersRepository<Seat>(DefaultConnectionString);

            // Act
            var lastSeat = repository.GetAllAsQueryable().Last();
            var expectedSeat = new Seat { Id = lastSeat.Id, Row = lastSeat.Row, AreaId = lastSeat.AreaId, Number = lastSeat.Number };

            var actualId = expectedSeat.Id;
            var actual = await repository.GetByIdAsync(actualId);

            // Assert
            actual.Should().BeEquivalentTo(expectedSeat);
        }

        [Test]
        public async Task GetById_WhenNonExistSeat_ShouldReturnNull()
        {
            // Arrange
            var repository = new AdoUsingParametersRepository<Seat>(DefaultConnectionString);

            // Act
            var lastSeat = repository.GetAllAsQueryable().Last();
            var nonExistId = lastSeat.Id + 1;
            var actual = await repository.GetByIdAsync(nonExistId);

            // Assert
            actual.Should().BeEquivalentTo((Seat) null);
        }

        [Test]
        public void GetByIdAsync_WhenIdEqualZeroSeat_ShouldThrowArgumentException()
        {
            // Arrange
            var seat = new Seat { Id = 0 };
            var repository = new AdoUsingParametersRepository<Seat>(DefaultConnectionString);

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await repository.GetByIdAsync(seat.Id));
        }

        [Test]
        public void GetByIdAsync_WhenIdLessThenZero_ShouldThrowArgumentException()
        {
            // Arrange
            var seat = new Seat { Id = -1 };
            var repository = new AdoUsingParametersRepository<Seat>(DefaultConnectionString);

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await repository.GetByIdAsync(seat.Id));
        }

        [Test]
        public void UpdateAsync_WhenIdLessThenZero_ShouldThrowArgumentException()
        {
            // Arrange
            var seat = new Seat { Id = -1 };
            var repository = new AdoUsingParametersRepository<Seat>(DefaultConnectionString);

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await repository.UpdateAsync(seat));
        }

        [Test]
        public void DeleteAsync_WhenIdLessThenZero_ShouldThrowArgumentException()
        {
            // Arrange
            var seat = new Seat { Id = -1 };
            var repository = new AdoUsingParametersRepository<Seat>(DefaultConnectionString);

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await repository.DeleteAsync(seat));
        }
    }
}
