﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using TicketManagement.DataAccess.DbContexts;
using TicketManagement.DataAccess.Domain.Models;
using TicketManagement.DataAccess.Repositories.EfRepositories;

namespace TicketManagement.IntegrationTests.DataAccess.Repositories
{
    [TestFixture]
    internal class SeatTests : TestDatabaseLoader
    {
        private readonly List<Seat> _seats = new ();

        public EfDbContext DbContext { get; set; }

        [OneTimeSetUp]
        public async Task InitSeatsAsync()
        {
            DbContext = new EfDbContext(connectionString: DefaultConnectionString);
            var repository = new EfRepository<Seat>(DbContext);
            var countAllSeats = repository.GetAllAsQueryable().OrderBy(x => x.Id).Last().Id;

            for (var i = 1; i <= countAllSeats; i++)
            {
                _seats.Add(await repository.GetByIdAsync(i));
            }
        }

        [Test]
        public void GetAllAsQueryable_WhenSeatsExist_ShouldReturnSeatList()
        {
            // Arrange
            var expected = _seats;

            // Act
            var actual = new EfRepository<Seat>(DbContext).GetAllAsQueryable();

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public async Task CreateAsync_WhenAddSeat_ShouldReturnSeatWithNewSeat()
        {
            // Arrange
            var repository = new EfRepository<Seat>(DbContext);
            var seat = new Seat { Row = 2, AreaId = 2, Number = 2 };
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
        public void CreateAsync_WhenSeatEmpty_ShouldThrowArgumentNullException()
        {
            // Arrange
            var repository = new EfRepository<Seat>(DbContext);

            // Act & Assert
            Assert.ThrowsAsync<ArgumentNullException>(async () => await repository.CreateAsync(null));
        }

        [Test]
        public async Task DeleteAsync_WhenExistSeat_ShouldReturnSeatListWithoutDeletedSeatAsync()
        {
            // Arrange
            var dbContext = new EfDbContext(connectionString: DefaultConnectionString);
            var repository = new EfRepository<Seat>(dbContext);

            // Act
            var allSeats = repository.GetAllAsQueryable().OrderBy(x => x.Id).ToList();
            var lastSeat = allSeats.LastOrDefault();
            await repository.DeleteAsync(lastSeat);

            var actual = repository.GetAllAsQueryable();
            int countSeatWithoutLast = allSeats.ToList().Count - 1;

            // Assert
            actual.Should().BeEquivalentTo(allSeats.Take(countSeatWithoutLast));
        }

        [Test]
        public void DeleteAsync_WhenIdEqualZeroSeat_ShouldThrowInvalidOperationException()
        {
            // Arrange
            var seat = new Seat { Id = 0 };
            var repository = new EfRepository<Seat>(DbContext);

            // Assert
            Assert.ThrowsAsync<InvalidOperationException>(async () => await repository.DeleteAsync(seat));
        }

        [Test]
        public void DeleteAsync_WhenNullSeat_ShouldThrowArgumentNullException()
        {
            // Arrange
            var repository = new EfRepository<Seat>(DbContext);

            // Assert
            Assert.ThrowsAsync<ArgumentNullException>(async () => await repository.DeleteAsync(null));
        }

        [Test]
        public async Task UpdateAsync_WhenExistSeat_ShouldUpdateLastSeatAsync()
        {
            // Arrange
            var dbContext = new EfDbContext(connectionString: DefaultConnectionString);
            var repository = new EfRepository<Seat>(dbContext);
            var expected = new Seat { Number = 2, AreaId = 2, Row = 2 };

            // Act
            var lastSeat = repository.GetAllAsQueryable().OrderBy(x => x.Id).Last();
            var idLastSeat = lastSeat.Id;
            expected.Id = idLastSeat;

            await repository.UpdateAsync(expected);
            var actual = repository.GetAllAsQueryable().OrderBy(x => x.Id).Last();

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void UpdateAsync_WhenNullSeat_ShouldThrowArgumentNullException()
        {
            // Arrange
            var repository = new EfRepository<Seat>(DbContext);

            // Act & Assert
            Assert.ThrowsAsync<ArgumentNullException>(async () => await repository.UpdateAsync(null));
        }

        [Test]
        public void UpdateAsync_WhenIdEqualZeroSeat_ShouldThrowDbUpdateConcurrencyException()
        {
            // Arrange
            var seat = new Seat { Id = 0 };
            var repository = new EfRepository<Seat>(DbContext);

            // Act & Assert
            Assert.ThrowsAsync<DbUpdateConcurrencyException>(async () => await repository.UpdateAsync(seat));
        }

        [Test]
        public async Task GetByIdAsync_WhenExistSeat_ShouldReturnSeatAsync()
        {
            // Arrange
            var repository = new EfRepository<Seat>(DbContext);

            // Act
            var lastSeat = repository.GetAllAsQueryable().OrderBy(x => x.Id).LastOrDefault();
            var expectedSeat = new Seat { Id = lastSeat.Id, Row = lastSeat.Row, AreaId = lastSeat.AreaId, Number = lastSeat.Number };

            var actualId = expectedSeat.Id;
            var actual = await repository.GetByIdAsync(actualId);

            // Assert
            actual.Should().BeEquivalentTo(expectedSeat);
        }

        [Test]
        public async Task GetByIdAsync_WhenNonExistSeat_ShouldReturnNullAsync()
        {
            // Arrange
            var repository = new EfRepository<Seat>(DbContext);

            // Act
            var lastSeat = repository.GetAllAsQueryable().OrderBy(x => x.Id).Last();
            var nonExistId = lastSeat.Id + 10;
            var actual = await repository.GetByIdAsync(nonExistId);

            // Assert
            actual.Should().BeEquivalentTo((Seat) null);
        }

        [Test]
        public void UpdateAsync_WhenIdLessThenZero_ShouldThrowDbUpdateConcurrencyException()
        {
            // Arrange
            var seat = new Seat { Id = -1 };
            var repository = new EfRepository<Seat>(DbContext);

            // Act & Assert
            Assert.ThrowsAsync<DbUpdateConcurrencyException>(async () => await repository.UpdateAsync(seat));
        }

        [Test]
        public void DeleteAsync_WhenIdLessThenZero_ShouldThrowDbUpdateConcurrencyException()
        {
            // Arrange
            var seat = new Seat { Id = -1 };
            var repository = new EfRepository<Seat>(DbContext);

            // Act & Assert
            Assert.ThrowsAsync<DbUpdateConcurrencyException>(async () => await repository.DeleteAsync(seat));
        }
    }
}
