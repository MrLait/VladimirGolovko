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
            var actual = Seats.Last();

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void Create_WhenSeatEmpty_ShouldThrowValidationException()
        {
            // Arrange
            var seatService = new SeatService(Mock.Object);

            // Act & Assert
            Assert.Throws<ValidationException>(() => seatService.Create(null));
        }

        [Test]
        public void Create_WhenSeatAlreadyExist_ShouldThrowValidationException()
        {
            // Arrange
            var seatFirst = Seats.First();
            var seatDto = new SeatDto { Id = seatFirst.Id, Row = seatFirst.Row, Number = seatFirst.Number, AreaId = seatFirst.AreaId };
            var seatService = new SeatService(Mock.Object);
            Mock.Setup(x => x.Seats.Create(It.IsAny<Seat>())).Callback<Seat>(v => Seats.Add(v));

            // Act & Assert
            Assert.Throws<ValidationException>(() => seatService.Create(seatDto));
        }

        [Test]
        public void Delete_WhenSeatExist_ShouldDeleteLastSeat()
        {
            // Arrange
            var expected = Seats.Last();
            Mock.Setup(x => x.Seats.Delete(It.IsAny<Seat>())).Callback<Seat>(v => Seats.RemoveAt(v.Id - 1));
            var seatService = new SeatService(Mock.Object);
            var seatLast = Seats.Last();

            // Act
            seatService.Delete(new SeatDto { Id = seatLast.Id, AreaId = seatLast.AreaId, Number = seatLast.Number, Row = seatLast.Row });
            var actual = Seats.Last();

            // Assert
            actual.Should().NotBeEquivalentTo(expected);
        }

        [Test]
        public void Delete_WhenSeatEmpty_ShouldThrowValidationException()
        {
            // Arrange
            var seatService = new SeatService(Mock.Object);

            // Act & Assert
            Assert.Throws<ValidationException>(() => seatService.Delete(null));
        }

        [Test]
        public void Delete_WhenIdEqualZero_ShouldThrowValidationException()
        {
            // Arrange
            var seatService = new SeatService(Mock.Object);

            // Act & Assert
            Assert.Throws<ValidationException>(() => seatService.Delete(new SeatDto { Id = 0 }));
        }

        [Test]
        public void Delete_WhenIdEqualLeesThanZero_ShouldThrowValidationException()
        {
            // Arrange
            var seatService = new SeatService(Mock.Object);

            // Act & Assert
            Assert.Throws<ValidationException>(() => seatService.Delete(new SeatDto { Id = -1 }));
        }

        [Test]
        public void Update_WhenSeatExist_ShouldUpdateLastSeat()
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
            var actual = Seats[seatLast.Id - 1];

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void Update_WhenSeatEmpty_ShouldThrowValidationException()
        {
            // Arrange
            var seatService = new SeatService(Mock.Object);

            // Act & Assert
            Assert.Throws<ValidationException>(() => seatService.Update(null));
        }

        [Test]
        public void Update_WhenIdEqualZero_ShouldThrowValidationException()
        {
            // Arrange
            var seatService = new SeatService(Mock.Object);

            // Act & Assert
            Assert.Throws<ValidationException>(() => seatService.Update(new SeatDto { Id = 0 }));
        }

        [Test]
        public void Update_WhenIdEqualLeesThanZero_ShouldThrowValidationException()
        {
            // Arrange
            var seatService = new SeatService(Mock.Object);

            // Act & Assert
            Assert.Throws<ValidationException>(() => seatService.Update(new SeatDto { Id = -1 }));
        }

        [Test]
        public void Update_WhenSeatAlreadyExist_ShouldThrowValidationException()
        {
            // Arrange
            var seatFirst = Seats.First();
            var seatDto = new SeatDto { Id = seatFirst.Id, Row = seatFirst.Row, Number = seatFirst.Number, AreaId = seatFirst.AreaId };
            var seatService = new SeatService(Mock.Object);
            Mock.Setup(x => x.Seats.Update(It.IsAny<Seat>())).Callback<Seat>(v => Seats.Add(v));

            // Act & Assert
            Assert.Throws<ValidationException>(() => seatService.Update(seatDto));
        }
    }
}
