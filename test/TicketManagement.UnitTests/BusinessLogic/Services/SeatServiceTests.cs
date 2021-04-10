using System;
using System.Linq;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using TicketManagement.BusinessLogic.Infrastructure;
using TicketManagement.BusinessLogic.Services;
using TicketManagement.DataAccess.Domain.Models;
using TicketManagement.Dto;

namespace TicketManagement.UnitTests.BusinessLogic.Services
{
    /// <summary>
    /// Seat service tests.
    /// </summary>
    [TestFixture]
    public class SeatServiceTests : MockEntites
    {
        [Test]
        public void Create_WhenSeatExist_ShouldReturnCreatedSeat()
        {
            // Arrange
            var expected = new Seat { AreaId = 12, Number = 3, Row = 5 };
            Mock.Setup(x => x.Seats.Create(It.IsAny<Seat>())).Callback<Seat>(v => Seats.Add(v));
            var seatService = new SeatService(Mock.Object);

            // Act
            seatService.Create(new SeatDto { AreaId = 12, Number = 3, Row = 5 });

            // Assert
            expected.Should().BeEquivalentTo(Seats.Last());
        }

        [Test]
        public void Create_WhenSeatEmpty_ShouldThrowArgumentException()
        {
            // Arrange
            var seatService = new SeatService(Mock.Object);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => seatService.Create(null));
        }

        [Test]
        public void Create_WhenSeatAlreadyExist_ShouldReturnValidationException()
        {
            // Arrange
            var firstSeat = Seats.First();
            var seatDto = new SeatDto { Id = firstSeat.Id, Row = firstSeat.Row, Number = firstSeat.Number, AreaId = firstSeat.AreaId };
            var seatService = new SeatService(Mock.Object);
            Mock.Setup(x => x.Seats.Create(It.IsAny<Seat>())).Callback<Seat>(v => Seats.Add(v));

            // Act & Assert
            Assert.Throws<ValidationException>(() => seatService.Create(seatDto));
        }

        [Test]
        public void Delete_WhenSeatExist_ShouldReturnListWithDeletedSeat()
        {
            // Arrange
            var expected = Seats.Last();
            Mock.Setup(x => x.Seats.Delete(It.IsAny<Seat>())).Callback<Seat>(v => Seats.RemoveAt(v.Id - 1));
            var seatService = new SeatService(Mock.Object);
            var seatLast = Seats.Last();

            // Act
            seatService.Delete(new SeatDto { Id = seatLast.Id, AreaId = seatLast.AreaId, Number = seatLast.Number, Row = seatLast.Row });

            // Assert
            expected.Should().NotBeEquivalentTo(Seats.Last());
        }

        [Test]
        public void Delete_WhenSeatEmpty_ShouldThrowArgumentException()
        {
            // Arrange
            var seatService = new SeatService(Mock.Object);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => seatService.Delete(null));
        }

        [Test]
        public void Delete_WhenIdEqualZero_ShouldThrowArgumentException()
        {
            // Arrange
            var seatService = new SeatService(Mock.Object);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => seatService.Delete(new SeatDto { Id = 0 }));
        }

        [Test]
        public void Delete_WhenIdEqualLeesThanZero_ShouldThrowArgumentException()
        {
            // Arrange
            var seatService = new SeatService(Mock.Object);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => seatService.Delete(new SeatDto { Id = -1 }));
        }

        [Test]
        public void Update_WhenSeatExist_ShouldReturnListWithUpdatedSeat()
        {
            // Arrange
            var seatLast = Seats.Last();
            var expected = new Seat { Id = seatLast.Id, Row = seatLast.Row + 1, Number = seatLast.Number + 1, AreaId = seatLast.AreaId };
            var seatService = new SeatService(Mock.Object);

            // Act
            Action<Seat> updateLastAction = venues => Seats.RemoveAt(seatLast.Id - 1);
            updateLastAction += v => Seats.Insert(v.Id - 1, v);
            Mock.Setup(x => x.Seats.Update(It.IsAny<Seat>())).Callback(updateLastAction);

            seatService.Update(new SeatDto { Id = seatLast.Id, AreaId = expected.AreaId, Number = expected.Number, Row = expected.Row });

            // Assert
            expected.Should().BeEquivalentTo(Seats[seatLast.Id - 1]);
        }

        [Test]
        public void Update_WhenSeatEmpty_ShouldThrowArgumentException()
        {
            // Arrange
            var seatService = new SeatService(Mock.Object);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => seatService.Update(null));
        }

        [Test]
        public void Update_WhenIdEqualZero_ShouldThrowArgumentException()
        {
            // Arrange
            var seatService = new SeatService(Mock.Object);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => seatService.Update(new SeatDto { Id = 0 }));
        }

        [Test]
        public void Update_WhenIdEqualLeesThanZero_ShouldThrowArgumentException()
        {
            // Arrange
            var seatService = new SeatService(Mock.Object);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => seatService.Update(new SeatDto { Id = -1 }));
        }

        [Test]
        public void Update_WhenSeatAlreadyExist_ShouldReturnValidationException()
        {
            // Arrange
            var firstSeat = Seats.First();
            var seatDto = new SeatDto { Id = firstSeat.Id, Row = firstSeat.Row, Number = firstSeat.Number, AreaId = firstSeat.AreaId };
            var seatService = new SeatService(Mock.Object);
            Mock.Setup(x => x.Seats.Update(It.IsAny<Seat>())).Callback<Seat>(v => Seats.Add(v));

            // Act & Assert
            Assert.Throws<ValidationException>(() => seatService.Update(seatDto));
        }
    }
}
