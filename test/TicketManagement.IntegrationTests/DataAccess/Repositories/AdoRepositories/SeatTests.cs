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
    internal class SeatTests : TestDatabaseLoader
    {
        private readonly List<Seat> _seats = new List<Seat>();

        [OneTimeSetUp]
        public async Task InitSeatsAsync()
        {
            var seatRepository = new AdoUsingParametersRepository<Seat>(MainConnectionString);
            var countAllSeats = seatRepository.GetAllAsQueryable().Last().Id;

            for (int i = 1; i <= countAllSeats; i++)
            {
                _seats.Add(await seatRepository.GetByIDAsync(i));
            }
        }

        [Test]
        public void GetAllAsQueryable_WhenSeatsExist_ShouldReturnSeatList()
        {
            // Arrange
            var expected = _seats;

            // Act
            var actual = new AdoUsingParametersRepository<Seat>(MainConnectionString).GetAllAsQueryable();

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
            var repository = new AdoUsingParametersRepository<Seat>(MainConnectionString);
            Seat seat = new Seat { Id = repository.GetAllAsQueryable().ToList().Count + 1, Row = 2, AreaId = 2, Number = 2 };
            List<Seat> expected = new List<Seat>(_seats)
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
            var repository = new AdoUsingParametersRepository<Seat>(MainConnectionString);

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await repository.CreateAsync(null));
        }

        [Test]
        public async Task DeleteAsync_WhenExistSeat_ShouldReturnSeatListWithoutDeletedSeat()
        {
            // Arrange
            var repository = new AdoUsingParametersRepository<Seat>(MainConnectionString);

            // Act
            var allSeats = repository.GetAllAsQueryable();
            var lastSeat = allSeats.Last();
            await repository.DeleteAsync(lastSeat);

            var actual = repository.GetAllAsQueryable();
            int countSeatWithoutLast = allSeats.ToList().Count - 1;

            // Assert
            actual.Should().BeEquivalentTo(allSeats.Take(countSeatWithoutLast));
        }

        [Test]
        public void DeleteAsync_WhenIdEqualZeroSeat_ShouldThrowArgumentException()
        {
            // Arrange
            Seat seat = new Seat { Id = 0 };
            var repository = new AdoUsingParametersRepository<Seat>(MainConnectionString);

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await repository.DeleteAsync(seat));
        }

        [Test]
        public void DeleteAsync_WhenNullSeat_ShouldThrowArgumentException()
        {
            // Arrange
            var repository = new AdoUsingParametersRepository<Seat>(MainConnectionString);

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await repository.DeleteAsync(null));
        }

        [Test]
        public void DeleteAsync_WhenIncorrectConnectionStringSeat_ShouldThrowArgumentException()
        {
            // Arrange
            Seat seat = new Seat { Id = 3 };
            var repository = new AdoUsingParametersRepository<Seat>("Server=myServerAddress;Database=myDataBase;User Id=myUsername;Password=myPassword;");

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await repository.DeleteAsync(seat));
        }

        [Test]
        public async Task UpdateAsync_WhenExistSeat_ShouldUpdateLastSeat()
        {
            // Arrange
            var repository = new AdoUsingParametersRepository<Seat>(MainConnectionString);
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
            var repository = new AdoUsingParametersRepository<Seat>(MainConnectionString);

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await repository.UpdateAsync(null));
        }

        [Test]
        public void UpdateAsync_WhenIdEqualZeroSeat_ShouldThrowArgumentException()
        {
            // Arrange
            Seat seat = new Seat { Id = 0 };
            var repository = new AdoUsingParametersRepository<Seat>(MainConnectionString);

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await repository.UpdateAsync(seat));
        }

        [Test]
        public async Task GetByIdAsync_WhenExistSeat_ShouldReturnSeat()
        {
            // Arrange
            var repository = new AdoUsingParametersRepository<Seat>(MainConnectionString);

            // Act
            var lastSeat = repository.GetAllAsQueryable().Last();
            Seat expectedSeat = new Seat { Id = lastSeat.Id, Row = lastSeat.Row, AreaId = lastSeat.AreaId, Number = lastSeat.Number };

            int actualId = expectedSeat.Id;
            var actual = await repository.GetByIDAsync(actualId);

            // Assert
            actual.Should().BeEquivalentTo(expectedSeat);
        }

        [Test]
        public async Task GetById_WhenNonExistSeat_ShouldReturnNull()
        {
            // Arrange
            Seat expected = null;
            var repository = new AdoUsingParametersRepository<Seat>(MainConnectionString);

            // Act
            var lastSeat = repository.GetAllAsQueryable().Last();
            int nonExistId = lastSeat.Id + 1;
            var actual = await repository.GetByIDAsync(nonExistId);

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void GetByIdAsync_WhenIdEqualZeroSeat_ShouldThrowArgumentException()
        {
            // Arrange
            Seat seat = new Seat { Id = 0 };
            var repository = new AdoUsingParametersRepository<Seat>(MainConnectionString);

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await repository.GetByIDAsync(seat.Id));
        }

        [Test]
        public void GetByIdAsync_WhenIdLessThenZero_ShouldThrowArgumentException()
        {
            // Arrange
            Seat seat = new Seat { Id = -1 };
            var repository = new AdoUsingParametersRepository<Seat>(MainConnectionString);

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await repository.GetByIDAsync(seat.Id));
        }

        [Test]
        public void UpdateAsync_WhenIdLessThenZero_ShouldThrowArgumentException()
        {
            // Arrange
            Seat seat = new Seat { Id = -1 };
            var repository = new AdoUsingParametersRepository<Seat>(MainConnectionString);

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await repository.UpdateAsync(seat));
        }

        [Test]
        public void DeleteAsync_WhenIdLessThenZero_ShouldThrowArgumentException()
        {
            // Arrange
            Seat seat = new Seat { Id = -1 };
            var repository = new AdoUsingParametersRepository<Seat>(MainConnectionString);

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await repository.DeleteAsync(seat));
        }
    }
}
