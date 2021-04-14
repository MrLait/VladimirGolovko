﻿using System;
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
    /// Event area service tests.
    /// </summary>
    [TestFixture]
    public class EventAreaServiceTests : MockEntites
    {
        [Test]
        public void UpdatePrice_WhenEventAreaExist_ShouldUpdatePriceInLastEventArea()
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

            var eventAreaService = new EventAreaService(Mock.Object);

            // Act
            Action<EventArea> updateLastAction = venues => EventAreas.RemoveAt(eventAreaLast.Id - 1);
            updateLastAction += v => EventAreas.Insert(v.Id - 1, v);
            Mock.Setup(x => x.EventAreas.GetByID(eventAreaLast.Id)).Returns(eventAreaLast);
            Mock.Setup(x => x.EventAreas.Update(It.IsAny<EventArea>())).Callback(updateLastAction);

            eventAreaService.UpdatePrice(new EventAreaDto
            {
                Id = eventAreaLast.Id,
                EventId = expected.EventId,
                CoordX = expected.CoordX,
                CoordY = expected.CoordY,
                Description = expected.Description,
                Price = expected.Price,
            });
            var actual = EventAreas[eventAreaLast.Id - 1];

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void UpdatePrice_WhenEventAreaEmpty_ShouldThrowValidationException()
        {
            // Arrange
            var eventAreaService = new EventAreaService(Mock.Object);

            // Act & Assert
            Assert.Throws<ValidationException>(() => eventAreaService.UpdatePrice(null));
        }

        [Test]
        public void UpdatePrice_WhenIdEqualZero_ShouldThrowValidationException()
        {
            // Arrange
            var eventAreaService = new EventAreaService(Mock.Object);

            // Act & Assert
            Assert.Throws<ValidationException>(() => eventAreaService.UpdatePrice(new EventAreaDto { Id = 0 }));
        }

        [Test]
        public void UpdatePrice_WhenIdEqualLeesThanZero_ShouldThrowValidationException()
        {
            // Arrange
            var eventAreaService = new EventAreaService(Mock.Object);

            // Act & Assert
            Assert.Throws<ValidationException>(() => eventAreaService.UpdatePrice(new EventAreaDto { Id = -1 }));
        }

        [Test]
        public void UpdatePrice_WhenPriceLeesThanZero_ShouldThrowValidationException()
        {
            // Arrange
            var eventAreaService = new EventAreaService(Mock.Object);

            // Act & Assert
            Assert.Throws<ValidationException>(() => eventAreaService.UpdatePrice(new EventAreaDto { Id = 1, Price = -1 }));
        }

        [Test]
        public void GetAll_WhenEventAreasExist_ShouldReturnAreas()
        {
            // Arrange
            var expected = EventAreas;
            Mock.Setup(x => x.EventAreas.GetAll()).Returns(EventAreas);
            var eventAreaService = new EventAreaService(Mock.Object);

            // Act
            var actual = eventAreaService.GetAll();

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void GetById_WhenEventAreaExist_ShouldReturnLastEventArea()
        {
            // Arrange
            var expected = EventAreas.Last();
            var expectedId = expected.Id - 1;
            Mock.Setup(x => x.EventAreas.GetByID(expectedId)).Returns(EventAreas.Last());
            var eventAreaService = new EventAreaService(Mock.Object);

            // Act
            var actual = eventAreaService.GetByID(expectedId);

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void GetByID_WhenIdEqualZero_ShouldThrowValidationException()
        {
            // Arrange
            var eventAreaService = new EventAreaService(Mock.Object);

            // Act & Assert
            Assert.Throws<ValidationException>(() => eventAreaService.GetByID(0));
        }

        [Test]
        public void GetByID_WhenIdEqualLeesThanZero_ShouldThrowValidationException()
        {
            // Arrange
            var eventAreaService = new AreaService(Mock.Object);

            // Act & Assert
            Assert.Throws<ValidationException>(() => eventAreaService.GetByID(-1));
        }
    }
}
