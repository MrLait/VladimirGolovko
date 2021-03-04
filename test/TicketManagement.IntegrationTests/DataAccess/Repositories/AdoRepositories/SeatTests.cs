using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using TicketManagement.DataAccess.Domain.Models;
using TicketManagement.DataAccess.Repositories.AdoRepositories;

namespace TicketManagement.IntegrationTests.DataAccess.Repositories.AdoRepositories
{
    [TestFixture]
    internal class SeatTests : AdoRepositoryTests
    {
        private readonly List<Seat> _seats = new List<Seat>();

        [OneTimeSetUp]
        public void InitSeats()
        {
            var seatRepository = new AdoUsingParametersRepository<Seat>(MainConnectionString);
            var coutAllSeats = seatRepository.GetAll().Last().Id;

            for (int i = 1; i <= coutAllSeats; i++)
            {
                _seats.Add(seatRepository.GetByID(i));
            }
        }

        [Test]
        public void GivenGetAll_WhenSeatsExist_ShouldReturnSeatList()
        {
            // Arrange
            var expected = _seats;

            // Act
            var actual = new AdoUsingParametersRepository<Seat>(MainConnectionString).GetAll();

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void GivenGetAll_WhenSeatsIncorrectConnectionSting_ShouldReturnArgumentException()
        {
            // Arrange
            var actual = new AdoUsingParametersRepository<Seat>("Server=myServerAddress;Database=myDataBase;User Id=myUsername;Password=myPassword;");

            // Act
            TestDelegate testAction = () => actual.GetAll();

            // Assert
            Assert.Throws<ArgumentException>(testAction);
        }

        [Test]
        public void GivenCreate_WhenAddSeat_ShouldReturnSeatWithNewSeat()
        {
            // Arrange
            var repository = new AdoUsingParametersRepository<Seat>(MainConnectionString);
            Seat seat = new Seat { Id = repository.GetAll().ToList().Count + 1, AreaId = 11, Number = 5, Row = 5 };
            List<Seat> expected = new List<Seat>(_seats);

            // Act
            expected.Add(seat);
            repository.Create(seat);
            var actual = repository.GetAll();

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void GivenCreate_WhenSeatEmpty_ShouldReturnArgumentException()
        {
            // Arrange
            var repository = new AdoUsingParametersRepository<Seat>(MainConnectionString);

            // Act
            TestDelegate testAction = () => repository.Create(null);

            // Assert
            Assert.Throws<ArgumentException>(testAction);
        }

        [Test]
        public void GivenDelete_WhenExistSeat_ShouldReturnSeatListWithoutDeletedSeat()
        {
            // Arrange
            var repository = new AdoUsingParametersRepository<Seat>(MainConnectionString);

            // Act
            var allSeats = repository.GetAll();
            var lastSeat = allSeats.Last();
            repository.Delete(lastSeat);

            var actual = repository.GetAll();
            int countSeatWithoutLast = allSeats.ToList().Count - 1;

            // Assert
            actual.Should().BeEquivalentTo(allSeats.Take(countSeatWithoutLast));
        }

        [Test]
        public void GivenDelete_WhenIdEqualZeroSeat_ShouldReturnArgumentException()
        {
            // Arrange
            Seat seat = new Seat { Id = 0 };
            var repository = new AdoUsingParametersRepository<Seat>(MainConnectionString);

            // Act
            TestDelegate testAction = () => repository.Delete(seat);

            // Assert
            Assert.Throws<ArgumentException>(testAction);
        }

        [Test]
        public void GivenDelete_WhenNullSeat_ShouldReturnArgumentException()
        {
            // Arrange
            var repository = new AdoUsingParametersRepository<Seat>(MainConnectionString);

            // Act
            TestDelegate testAction = () => repository.Delete(null);

            // Assert
            Assert.Throws<ArgumentException>(testAction);
        }

        [Test]
        public void GivenDelete_WhenIncorrectConnectionStringSeat_ShouldReturnArgumentException()
        {
            // Arrange
            Seat seat = new Seat { Id = 3 };
            var repository = new AdoUsingParametersRepository<Seat>("Server=myServerAddress;Database=myDataBase;User Id=myUsername;Password=myPassword;");

            // Act
            TestDelegate testAction = () => repository.Delete(seat);

            // Assert
            Assert.Throws<ArgumentException>(testAction);
        }

        [Test]
        public void GivenUpdate_WhenExistSeat_ShouldReturnListWithUpdateSeat()
        {
            // Arrange
            var repository = new AdoUsingParametersRepository<Seat>(MainConnectionString);
            var expected = new Seat { AreaId = 11, Number = 15, Row = 15 };

            // Act
            var lastSeat = repository.GetAll().Last();
            var idLastSeat = lastSeat.Id;
            expected.Id = idLastSeat;

            repository.Update(expected);
            var actual = repository.GetAll().Last();

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void GivenUpdate_WhenNullSeat_ShouldReturnArgumentException()
        {
            // Arrange
            var repository = new AdoUsingParametersRepository<Seat>(MainConnectionString);

            // Act
            TestDelegate testAction = () => repository.Update(null);

            // Assert
            Assert.Throws<ArgumentException>(testAction);
        }

        [Test]
        public void GivenUpdate_WhenIdEqualZeroSeat_ShouldReturnArgumentException()
        {
            // Arrange
            Seat seat = new Seat { Id = 0 };
            var repository = new AdoUsingParametersRepository<Seat>(MainConnectionString);

            // Act
            TestDelegate testAction = () => repository.Update(seat);

            // Assert
            Assert.Throws<ArgumentException>(testAction);
        }

        [Test]
        public void GivenGetById_WhenExistSeat_ShouldReturnSeat()
        {
            // Arrange
            var repository = new AdoUsingParametersRepository<Seat>(MainConnectionString);

            // Act
            var lastSeat = repository.GetAll().Last();
            Seat expectedSeat = new Seat { Id = lastSeat.Id, AreaId = lastSeat.AreaId, Number = lastSeat.Number, Row = lastSeat.Row };

            int actualId = expectedSeat.Id;
            var actual = repository.GetByID(actualId);

            // Assert
            actual.Should().BeEquivalentTo(expectedSeat);
        }

        [Test]
        public void GivenGetById_WhenNonExistSeat_ShouldReturnNull()
        {
            // Arrange
            Seat expected = null;
            var repository = new AdoUsingParametersRepository<Seat>(MainConnectionString);

            // Act
            var lastSeat = repository.GetAll().Last();
            int nonExistId = lastSeat.Id + 1;
            var actual = repository.GetByID(nonExistId);

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void GivenGetById_WhenIdEqualZeroSeat_ShouldReturnArgumentException()
        {
            // Arrange
            Seat seat = new Seat { Id = 0 };
            var repository = new AdoUsingParametersRepository<Seat>(MainConnectionString);

            // Act
            TestDelegate testAction = () => repository.GetByID(seat.Id);

            // Assert
            Assert.Throws<ArgumentException>(testAction);
        }
    }
}
