using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using TicketManagement.DataAccess.Domain.Models;
using TicketManagement.Dto;
using TicketManagement.Services.EventFlow.API.Infrastructure.Exceptions;
using TicketManagement.Services.EventFlow.API.Infrastructure.Services;

namespace TicketManagement.UnitTests.BusinessLogic.Services
{
    /// <summary>
    /// Seat service tests.
    /// </summary>
    [TestFixture]
    public class SeatServiceTests : MockEntites
    {
        [Test]
        public async Task CreateAsync_WhenSeatExist_ShouldReturnCreatedSeat()
        {
            // Arrange
            var expected = new Seat { AreaId = 12, Number = 3, Row = 5 };
            Mock.Setup(x => x.Seats.CreateAsync(It.IsAny<Seat>())).Callback<Seat>(v => Seats.Add(v));
            var seatService = new SeatService(Mock.Object);

            // Act
            await seatService.CreateAsync(new SeatDto { AreaId = 12, Number = 3, Row = 5 });
            var actual = Seats.Last();

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void CreateAsync_WhenSeatEmpty_ShouldThrowValidationException()
        {
            // Arrange
            var seatService = new SeatService(Mock.Object);

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () => await seatService.CreateAsync(null));
        }

        [Test]
        public void CreateAsync_WhenSeatAlreadyExist_ShouldThrowValidationException()
        {
            // Arrange
            var seatFirst = Seats.First();
            var seatDto = new SeatDto { Id = seatFirst.Id, Row = seatFirst.Row, Number = seatFirst.Number, AreaId = seatFirst.AreaId };
            var seatService = new SeatService(Mock.Object);
            Mock.Setup(x => x.Seats.CreateAsync(It.IsAny<Seat>())).Callback<Seat>(v => Seats.Add(v));

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () => await seatService.CreateAsync(seatDto));
        }

        [Test]
        public async Task DeleteAsync_WhenSeatExist_ShouldDeleteLastSeat()
        {
            // Arrange
            var expected = Seats.Last();
            Mock.Setup(x => x.Seats.DeleteAsync(It.IsAny<Seat>())).Callback<Seat>(v => Seats.RemoveAt(v.Id - 1));
            var seatService = new SeatService(Mock.Object);
            var seatLast = Seats.Last();

            // Act
            await seatService.DeleteAsync(new SeatDto { Id = seatLast.Id, AreaId = seatLast.AreaId, Number = seatLast.Number, Row = seatLast.Row });
            var actual = Seats.Last();

            // Assert
            actual.Should().NotBeEquivalentTo(expected);
        }

        [Test]
        public void DeleteAsync_WhenSeatEmpty_ShouldThrowValidationException()
        {
            // Arrange
            var seatService = new SeatService(Mock.Object);

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () => await seatService.DeleteAsync(null));
        }

        [Test]
        public void DeleteAsync_WhenIdEqualZero_ShouldThrowValidationException()
        {
            // Arrange
            var seatService = new SeatService(Mock.Object);

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () => await seatService.DeleteAsync(new SeatDto { Id = 0 }));
        }

        [Test]
        public void DeleteAsync_WhenIdEqualLeesThanZero_ShouldThrowValidationException()
        {
            // Arrange
            var seatService = new SeatService(Mock.Object);

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () => await seatService.DeleteAsync(new SeatDto { Id = -1 }));
        }

        [Test]
        public async Task UpdateAsync_WhenSeatExist_ShouldUpdateLastSeat()
        {
            // Arrange
            var seatLast = Seats.Last();
            var expected = new Seat { Id = seatLast.Id, Row = seatLast.Row + 1, Number = seatLast.Number + 1, AreaId = seatLast.AreaId };
            var seatService = new SeatService(Mock.Object);

            // Act
            Action<Seat> updateLastAction = venues => Seats.RemoveAt(seatLast.Id - 1);
            updateLastAction += v => Seats.Insert(v.Id - 1, v);
            Mock.Setup(x => x.Seats.UpdateAsync(It.IsAny<Seat>())).Callback(updateLastAction);

            await seatService.UpdateAsync(new SeatDto { Id = seatLast.Id, AreaId = expected.AreaId, Number = expected.Number, Row = expected.Row });
            var actual = Seats[seatLast.Id - 1];

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void UpdateAsync_WhenSeatEmpty_ShouldThrowValidationException()
        {
            // Arrange
            var seatService = new SeatService(Mock.Object);

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () => await seatService.UpdateAsync(null));
        }

        [Test]
        public void UpdateAsync_WhenIdEqualZero_ShouldThrowValidationException()
        {
            // Arrange
            var seatService = new SeatService(Mock.Object);

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () => await seatService.UpdateAsync(new SeatDto { Id = 0 }));
        }

        [Test]
        public void UpdateAsync_WhenIdEqualLeesThanZero_ShouldThrowValidationException()
        {
            // Arrange
            var seatService = new SeatService(Mock.Object);

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () => await seatService.UpdateAsync(new SeatDto { Id = -1 }));
        }

        [Test]
        public void UpdateAsync_WhenSeatAlreadyExist_ShouldThrowValidationException()
        {
            // Arrange
            var seatFirst = Seats.First();
            var seatDto = new SeatDto { Id = seatFirst.Id, Row = seatFirst.Row, Number = seatFirst.Number, AreaId = seatFirst.AreaId };
            var seatService = new SeatService(Mock.Object);
            Mock.Setup(x => x.Seats.UpdateAsync(It.IsAny<Seat>())).Callback<Seat>(v => Seats.Add(v));

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () => await seatService.UpdateAsync(seatDto));
        }

        [Test]
        public void GetAllAsQueryable_WhenSeatsExist_ShouldReturnSeats()
        {
            // Arrange
            var expected = Seats;
            Mock.Setup(x => x.Seats.GetAllAsQueryable()).Returns(Seats.AsQueryable());
            var seatService = new SeatService(Mock.Object);

            // Act
            var actual = seatService.GetAll();

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public async Task GetByIdAsync_WhenSeatExist_ShouldReturnLastSeat()
        {
            // Arrange
            var expected = Seats.Last();
            var expectedId = expected.Id - 1;
            Mock.Setup(x => x.Seats.GetByIDAsync(expectedId)).ReturnsAsync(Seats.Last());
            var seatService = new SeatService(Mock.Object);

            // Act
            var actual = await seatService.GetByIDAsync(expectedId);

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void GetByIDAsync_WhenIdEqualZero_ShouldThrowValidationException()
        {
            // Arrange
            var seatService = new SeatService(Mock.Object);

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () => await seatService.GetByIDAsync(0));
        }

        [Test]
        public void GetByIDAsync_WhenIdEqualLeesThanZero_ShouldThrowValidationException()
        {
            // Arrange
            var seatService = new SeatService(Mock.Object);

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () => await seatService.GetByIDAsync(-1));
        }
    }
}
