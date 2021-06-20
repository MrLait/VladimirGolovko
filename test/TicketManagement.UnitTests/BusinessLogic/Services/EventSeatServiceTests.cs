using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using TicketManagement.DataAccess.Domain.Models;
using TicketManagement.DataAccess.Enums;
using TicketManagement.Dto;
using TicketManagement.Services.EventFlow.API.Infrastructure.Exceptions;
using TicketManagement.Services.EventFlow.API.Infrastructure.Services;

namespace TicketManagement.UnitTests.BusinessLogic.Services
{
    /// <summary>
    /// Event seat server tests.
    /// </summary>
    [TestFixture]
    public class EventSeatServiceTests : MockEntites
    {
        [Test]
        public async Task UpdateStateAsync_WhenEventSeatExist_ShouldUpdateStateInLastEventSeat()
        {
            // Arrange
            var eventSeatLast = EventSeats.Last();
            EventSeat expected = new EventSeat
            {
                Id = eventSeatLast.Id,
                EventAreaId = eventSeatLast.EventAreaId,
                Number = eventSeatLast.Number,
                Row = eventSeatLast.Row,
                State = eventSeatLast.State + 1,
            };

            var eventSeatsService = new EventSeatService(Mock.Object);

            // Act
            Action<EventSeat> updateLastAction = venues => EventSeats.RemoveAt(eventSeatLast.Id - 1);
            updateLastAction += v => EventSeats.Insert(v.Id - 1, v);
            Mock.Setup(x => x.EventSeats.GetByIDAsync(eventSeatLast.Id)).ReturnsAsync(eventSeatLast);
            Mock.Setup(x => x.EventSeats.UpdateAsync(It.IsAny<EventSeat>())).Callback(updateLastAction);

            await eventSeatsService.UpdateStateAsync(new EventSeatDto
            {
                Id = eventSeatLast.Id,
                EventAreaId = expected.EventAreaId,
                Number = expected.Number,
                Row = expected.Row,
                State = (States)expected.State,
            });
            var actual = EventSeats[eventSeatLast.Id - 1];

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void UpdateStateAsync_WhenEventSeatEmpty_ShouldThrowValidationException()
        {
            // Arrange
            var eventSeatService = new EventSeatService(Mock.Object);

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () => await eventSeatService.UpdateStateAsync(null));
        }

        [Test]
        public void UpdateStateAsync_WhenIdEqualZero_ShouldThrowValidationException()
        {
            // Arrange
            var eventSeatService = new EventSeatService(Mock.Object);

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () => await eventSeatService.UpdateStateAsync(new EventSeatDto { Id = 0 }));
        }

        [Test]
        public void UpdateStateAsync_WhenIdEqualLeesThanZero_ShouldThrowValidationException()
        {
            // Arrange
            var eventSeatService = new EventSeatService(Mock.Object);

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () => await eventSeatService.UpdateStateAsync(new EventSeatDto { Id = -1 }));
        }

        [Test]
        public void GetAllAsQueryable_WhenEventSeatsExist_ShouldReturnEventSeats()
        {
            // Arrange
            var expected = EventSeats;
            Mock.Setup(x => x.EventSeats.GetAllAsQueryable()).Returns(EventSeats.AsQueryable());
            var eventSeatsService = new EventSeatService(Mock.Object);

            // Act
            var actual = eventSeatsService.GetAll();

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public async Task GetByIdAsync_WhenEventSeatExist_ShouldReturnLastEventSeat()
        {
            // Arrange
            var expected = EventSeats.Last();
            var expectedId = expected.Id - 1;
            Mock.Setup(x => x.EventSeats.GetByIDAsync(expectedId)).ReturnsAsync(EventSeats.Last());
            var eventSeatsService = new EventSeatService(Mock.Object);

            // Act
            var actual = await eventSeatsService.GetByIDAsync(expectedId);

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void GetByIDAsync_WhenIdEqualZero_ShouldThrowValidationException()
        {
            // Arrange
            var eventSeatsService = new EventSeatService(Mock.Object);

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () => await eventSeatsService.GetByIDAsync(0));
        }

        [Test]
        public void GetByIDAsync_WhenIdEqualLeesThanZero_ShouldThrowValidationException()
        {
            // Arrange
            var eventSeatsService = new EventSeatService(Mock.Object);

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () => await eventSeatsService.GetByIDAsync(-1));
        }
    }
}
