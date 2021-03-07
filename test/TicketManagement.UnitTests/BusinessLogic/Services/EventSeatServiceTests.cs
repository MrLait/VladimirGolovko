using System;
using System.Linq;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using TicketManagement.BusinessLogic.Services;
using TicketManagement.DataAccess.Domain.Models;
using TicketManagement.Dto;
using TicketManagement.UnitTests.DataAccess;

namespace TicketManagement.UnitTests.BusinessLogic.Services
{
    [TestFixture]
    public class EventSeatServiceTests : MockEntites
    {
        [Test]
        public void GivenUpdateState_WhenEventSeatExist_ShouldReturnListWithUpdatedState()
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

            var areaService = new EventSeatService(Mock.Object);

            // Act
            Action<EventSeat> updateLastAction = venues => EventSeats.RemoveAt(eventSeatLast.Id - 1);
            updateLastAction += v => EventSeats.Insert(v.Id - 1, v);
            Mock.Setup(x => x.EventSeats.GetByID(eventSeatLast.Id)).Returns(eventSeatLast);
            Mock.Setup(x => x.EventSeats.Update(It.IsAny<EventSeat>())).Callback(updateLastAction);

            areaService.UpdateState(new EventSeatDto
            {
                Id = eventSeatLast.Id,
                EventAreaId = expected.EventAreaId,
                Number = expected.Number,
                Row = expected.Row,
                State = expected.State,
            });

            // Assert
            expected.Should().BeEquivalentTo(EventSeats[eventSeatLast.Id - 1]);
        }

        [Test]
        public void GivenUpdateState_WhenEventSeatEmpty_ShouldReturnArgumentException()
        {
            // Arrange
            var eventSeatService = new EventSeatService(Mock.Object);

            // Act
            TestDelegate testAction = () => eventSeatService.UpdateState(null);

            // Assert
            Assert.Throws<ArgumentException>(testAction);
        }

        [Test]
        public void GivenUpdateState_WhenIdEqualZero_ShouldReturnArgumentException()
        {
            // Arrange
            var eventSeatService = new EventSeatService(Mock.Object);

            // Act
            TestDelegate testAction = () => eventSeatService.UpdateState(new EventSeatDto { Id = 0 });

            // Assert
            Assert.Throws<ArgumentException>(testAction);
        }

        [Test]
        public void GivenUpdateState_WhenIdEqualLeesThanZero_ShouldReturnArgumentException()
        {
            // Arrange
            var eventSeatService = new EventSeatService(Mock.Object);

            // Act
            TestDelegate testAction = () => eventSeatService.UpdateState(new EventSeatDto { Id = -1 });

            // Assert
            Assert.Throws<ArgumentException>(testAction);
        }

        [Test]
        public void GivenUpdateState_WhenStateLeesThanZero_ShouldReturnArgumentException()
        {
            // Arrange
            var eventSeatService = new EventSeatService(Mock.Object);

            // Act
            TestDelegate testAction = () => eventSeatService.UpdateState(new EventSeatDto { Id = 1, State = -1 });

            // Assert
            Assert.Throws<ArgumentException>(testAction);
        }
    }
}
