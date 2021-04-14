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
    /// Event seat server tests.
    /// </summary>
    [TestFixture]
    public class EventSeatServiceTests : MockEntites
    {
        [Test]
        public void UpdateState_WhenEventSeatExist_ShouldUpdateStateInLastEventSeat()
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
            Mock.Setup(x => x.EventSeats.GetByID(eventSeatLast.Id)).Returns(eventSeatLast);
            Mock.Setup(x => x.EventSeats.Update(It.IsAny<EventSeat>())).Callback(updateLastAction);

            eventSeatsService.UpdateState(new EventSeatDto
            {
                Id = eventSeatLast.Id,
                EventAreaId = expected.EventAreaId,
                Number = expected.Number,
                Row = expected.Row,
                State = expected.State,
            });
            var actual = EventSeats[eventSeatLast.Id - 1];

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void UpdateState_WhenEventSeatEmpty_ShouldThrowValidationException()
        {
            // Arrange
            var eventSeatService = new EventSeatService(Mock.Object);

            // Act & Assert
            Assert.Throws<ValidationException>(() => eventSeatService.UpdateState(null));
        }

        [Test]
        public void UpdateState_WhenIdEqualZero_ShouldThrowValidationException()
        {
            // Arrange
            var eventSeatService = new EventSeatService(Mock.Object);

            // Act & Assert
            Assert.Throws<ValidationException>(() => eventSeatService.UpdateState(new EventSeatDto { Id = 0 }));
        }

        [Test]
        public void UpdateState_WhenIdEqualLeesThanZero_ShouldThrowValidationException()
        {
            // Arrange
            var eventSeatService = new EventSeatService(Mock.Object);

            // Act & Assert
            Assert.Throws<ValidationException>(() => eventSeatService.UpdateState(new EventSeatDto { Id = -1 }));
        }

        [Test]
        public void UpdateState_WhenStateLeesThanZero_ShouldThrowValidationException()
        {
            // Arrange
            var eventSeatService = new EventSeatService(Mock.Object);

            // Act & Assert
            Assert.Throws<ValidationException>(() => eventSeatService.UpdateState(new EventSeatDto { Id = 1, State = -1 }));
        }

        [Test]
        public void GetAll_WhenEventSeatsExist_ShouldReturnEventSeats()
        {
            // Arrange
            var expected = EventSeats;
            Mock.Setup(x => x.EventSeats.GetAll()).Returns(EventSeats);
            var eventSeatsService = new EventSeatService(Mock.Object);

            // Act
            var actual = eventSeatsService.GetAll();

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void GetById_WhenEventSeatExist_ShouldReturnLastEventSeat()
        {
            // Arrange
            var expected = EventSeats.Last();
            var expectedId = expected.Id - 1;
            Mock.Setup(x => x.EventSeats.GetByID(expectedId)).Returns(EventSeats.Last());
            var eventSeatsService = new EventSeatService(Mock.Object);

            // Act
            var actual = eventSeatsService.GetByID(expectedId);

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void GetByID_WhenIdEqualZero_ShouldThrowValidationException()
        {
            // Arrange
            var eventSeatsService = new EventSeatService(Mock.Object);

            // Act & Assert
            Assert.Throws<ValidationException>(() => eventSeatsService.GetByID(0));
        }

        [Test]
        public void GetByID_WhenIdEqualLeesThanZero_ShouldThrowValidationException()
        {
            // Arrange
            var eventSeatsService = new EventSeatService(Mock.Object);

            // Act & Assert
            Assert.Throws<ValidationException>(() => eventSeatsService.GetByID(-1));
        }
    }
}
