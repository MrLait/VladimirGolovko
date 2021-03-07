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
    [TestFixture]
    public class SeatServiceTests : MockEntites
    {
        [Test]
        public void GivenCreate_WhenSeatExist_ShouldReturnCreatedSeat()
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
        public void GivenCreate_WhenSeatEmpty_ShouldReturnArgumentException()
        {
            // Arrange
            var seatService = new SeatService(Mock.Object);

            // Act
            TestDelegate testAction = () => seatService.Create(null);

            // Assert
            Assert.Throws<ArgumentException>(testAction);
        }

        [Test]
        public void GivenCreate_WhenSeatAlreadyExist_ShouldReturnValidationException()
        {
            // Arrange
            var firstSeat = Seats.First();
            var seatDto = new SeatDto { Id = firstSeat.Id, Row = firstSeat.Row, Number = firstSeat.Number, AreaId = firstSeat.AreaId };
            var seatService = new SeatService(Mock.Object);
            Mock.Setup(x => x.Seats.Create(It.IsAny<Seat>())).Callback<Seat>(v => Seats.Add(v));

            // Act
            TestDelegate testAction = () => seatService.Create(seatDto);

            // Assert
            Assert.Throws<ValidationException>(testAction);
        }

        [Test]
        public void GivenDelete_WhenSeatExist_ShouldReturnListWithDeletedSeat()
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
        public void GivenDelete_WhenSeatEmpty_ShouldReturnArgumentException()
        {
            // Arrange
            var seatService = new SeatService(Mock.Object);

            // Act
            TestDelegate testAction = () => seatService.Delete(null);

            // Assert
            Assert.Throws<ArgumentException>(testAction);
        }

        [Test]
        public void GivenDelete_WhenIdEqualZero_ShouldReturnArgumentException()
        {
            // Arrange
            var seatService = new SeatService(Mock.Object);

            // Act
            TestDelegate testAction = () => seatService.Delete(new SeatDto { Id = 0 });

            // Assert
            Assert.Throws<ArgumentException>(testAction);
        }

        [Test]
        public void GivenDelete_WhenIdEqualLeesThanZero_ShouldReturnArgumentException()
        {
            // Arrange
            var seatService = new SeatService(Mock.Object);

            // Act
            TestDelegate testAction = () => seatService.Delete(new SeatDto { Id = -1 });

            // Assert
            Assert.Throws<ArgumentException>(testAction);
        }

        [Test]
        public void GivenUpdate_WhenSeatExist_ShouldReturnListWithUpdatedSeat()
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
        public void GivenUpdate_WhenSeatEmpty_ShouldReturnArgumentException()
        {
            // Arrange
            var seatService = new SeatService(Mock.Object);

            // Act
            TestDelegate testAction = () => seatService.Update(null);

            // Assert
            Assert.Throws<ArgumentException>(testAction);
        }

        [Test]
        public void GivenUpdate_WhenIdEqualZero_ShouldReturnArgumentException()
        {
            // Arrange
            var seatService = new SeatService(Mock.Object);

            // Act
            TestDelegate testAction = () => seatService.Update(new SeatDto { Id = 0 });

            // Assert
            Assert.Throws<ArgumentException>(testAction);
        }

        [Test]
        public void GivenUpdate_WhenIdEqualLeesThanZero_ShouldReturnArgumentException()
        {
            // Arrange
            var seatService = new SeatService(Mock.Object);

            // Act
            TestDelegate testAction = () => seatService.Update(new SeatDto { Id = -1 });

            // Assert
            Assert.Throws<ArgumentException>(testAction);
        }

        [Test]
        public void GivenUpdate_WhenSeatAlreadyExist_ShouldReturnValidationException()
        {
            // Arrange
            var firstSeat = Seats.First();
            var seatDto = new SeatDto { Id = firstSeat.Id, Row = firstSeat.Row, Number = firstSeat.Number, AreaId = firstSeat.AreaId };
            var seatService = new SeatService(Mock.Object);
            Mock.Setup(x => x.Seats.Update(It.IsAny<Seat>())).Callback<Seat>(v => Seats.Add(v));

            // Act
            TestDelegate testAction = () => seatService.Update(seatDto);

            // Assert
            Assert.Throws<ValidationException>(testAction);
        }
    }
}
