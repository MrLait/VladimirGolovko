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
    /// <summary>
    /// Event area service tests.
    /// </summary>
    [TestFixture]
    public class EventAreaServiceTests : MockEntites
    {
        [Test]
        public void UpdatePrice_WhenEventAreaExist_ShouldReturnListWithUpdatedEventArea()
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
        public void UpdatePrice_WhenEventAreaEmpty_ShouldThrowArgumentException()
        {
            // Arrange
            var eventAreaService = new EventAreaService(Mock.Object);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => eventAreaService.UpdatePrice(null));
        }

        [Test]
        public void UpdatePrice_WhenIdEqualZero_ShouldThrowArgumentException()
        {
            // Arrange
            var eventAreaService = new EventAreaService(Mock.Object);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => eventAreaService.UpdatePrice(new EventAreaDto { Id = 0 }));
        }

        [Test]
        public void UpdatePrice_WhenIdEqualLeesThanZero_ShouldThrowArgumentException()
        {
            // Arrange
            var eventAreaService = new EventAreaService(Mock.Object);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => eventAreaService.UpdatePrice(new EventAreaDto { Id = -1 }));
        }

        [Test]
        public void UpdatePrice_WhenPriceLeesThanZero_ShouldThrowArgumentException()
        {
            // Arrange
            var eventAreaService = new EventAreaService(Mock.Object);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => eventAreaService.UpdatePrice(new EventAreaDto { Id = 1, Price = -1 }));
        }
    }
}
