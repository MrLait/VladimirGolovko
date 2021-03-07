using System;
using System.Linq;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using TicketManagement.BusinessLogic.Services;
using TicketManagement.DataAccess.Domain.Models;
using TicketManagement.Dto;

namespace TicketManagement.UnitTests.BusinessLogic.Services
{
    [TestFixture]
    public class EventAreaServiceTests : MockEntites
    {
        [Test]
        public void GivenUpdatePrice_WhenEventAreaExist_ShouldReturnListWithUpdatedEventArea()
        {
            // Arrange
            var eventAreaLast = EventAreas.Last();
            EventArea expected = new EventArea
            {
                Id = eventAreaLast.Id,
                EventId = eventAreaLast.EventId,
                CoordX = eventAreaLast.CoordX,
                CoordY = eventAreaLast.CoordY,
                Description = eventAreaLast.Description,
                Price = eventAreaLast.Price + 100,
            };

            var areaService = new EventAreaService(Mock.Object);

            // Act
            Action<EventArea> updateLastAction = venues => EventAreas.RemoveAt(eventAreaLast.Id - 1);
            updateLastAction += v => EventAreas.Insert(v.Id - 1, v);
            Mock.Setup(x => x.EventAreas.GetByID(eventAreaLast.Id)).Returns(eventAreaLast);
            Mock.Setup(x => x.EventAreas.Update(It.IsAny<EventArea>())).Callback(updateLastAction);

            areaService.UpdatePrice(new EventAreaDto
            {
                Id = eventAreaLast.Id,
                EventId = expected.EventId,
                CoordX = expected.CoordX,
                CoordY = expected.CoordY,
                Description = expected.Description,
                Price = expected.Price,
            });

            // Assert
            expected.Should().BeEquivalentTo(EventAreas[eventAreaLast.Id - 1]);
        }

        [Test]
        public void GivenUpdatePrice_WhenEventAreaEmpty_ShouldReturnArgumentException()
        {
            // Arrange
            var eventAreaService = new EventAreaService(Mock.Object);

            // Act
            TestDelegate testAction = () => eventAreaService.UpdatePrice(null);

            // Assert
            Assert.Throws<ArgumentException>(testAction);
        }

        [Test]
        public void GivenUpdatePrice_WhenIdEqualZero_ShouldReturnArgumentException()
        {
            // Arrange
            var eventAreaService = new EventAreaService(Mock.Object);

            // Act
            TestDelegate testAction = () => eventAreaService.UpdatePrice(new EventAreaDto { Id = 0 });

            // Assert
            Assert.Throws<ArgumentException>(testAction);
        }

        [Test]
        public void GivenUpdatePrice_WhenIdEqualLeesThanZero_ShouldReturnArgumentException()
        {
            // Arrange
            var eventAreaService = new EventAreaService(Mock.Object);

            // Act
            TestDelegate testAction = () => eventAreaService.UpdatePrice(new EventAreaDto { Id = -1 });

            // Assert
            Assert.Throws<ArgumentException>(testAction);
        }

        [Test]
        public void GivenUpdatePrice_WhenPriceLeesThanZero_ShouldReturnArgumentException()
        {
            // Arrange
            var eventAreaService = new EventAreaService(Mock.Object);

            // Act
            TestDelegate testAction = () => eventAreaService.UpdatePrice(new EventAreaDto { Id = 1, Price = -1 });

            // Assert
            Assert.Throws<ArgumentException>(testAction);
        }
    }
}
