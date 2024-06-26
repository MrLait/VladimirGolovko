﻿using System;
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
    /// Event area service tests.
    /// </summary>
    [TestFixture]
    public class EventAreaServiceTests : MockEntities
    {
        [Test]
        public async Task UpdatePriceAsync_WhenEventAreaExist_ShouldUpdatePriceInLastEventArea()
        {
            // Arrange
            var eventAreaLast = EventAreas.Last();
            var expected = new EventArea
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
            Action<EventArea> updateLastAction = _ => EventAreas.RemoveAt(eventAreaLast.Id - 1);
            updateLastAction += v => EventAreas.Insert(v.Id - 1, v);
            Mock.Setup(x => x.EventAreas.GetByIdAsync(eventAreaLast.Id)).ReturnsAsync(eventAreaLast);
            Mock.Setup(x => x.EventAreas.UpdateAsync(It.IsAny<EventArea>())).Callback(updateLastAction);

            await eventAreaService.UpdatePriceAsync(new EventAreaDto
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
        public void UpdatePriceAsync_WhenEventAreaEmpty_ShouldThrowValidationException()
        {
            // Arrange
            var eventAreaService = new EventAreaService(Mock.Object);

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () =>
            {
                await eventAreaService.UpdatePriceAsync(new EventAreaDto());
            });
        }

        [Test]
        public void UpdatePriceAsync_WhenIdEqualZero_ShouldThrowValidationException()
        {
            // Arrange
            var eventAreaService = new EventAreaService(Mock.Object);

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () => await eventAreaService.UpdatePriceAsync(new EventAreaDto { Id = 0 }));
        }

        [Test]
        public void UpdatePriceAsync_WhenIdEqualLeesThanZero_ShouldThrowValidationException()
        {
            // Arrange
            var eventAreaService = new EventAreaService(Mock.Object);

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () => await eventAreaService.UpdatePriceAsync(new EventAreaDto { Id = -1 }));
        }

        [Test]
        public void UpdatePriceAsync_WhenPriceLeesThanZero_ShouldThrowValidationException()
        {
            // Arrange
            var eventAreaService = new EventAreaService(Mock.Object);

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () => await eventAreaService.UpdatePriceAsync(new EventAreaDto { Id = 1, Price = -1 }));
        }

        [Test]
        public void GetAllAsQueryable_WhenEventAreasExist_ShouldReturnAreas()
        {
            // Arrange
            var expected = EventAreas;
            Mock.Setup(x => x.EventAreas.GetAllAsQueryable()).Returns(EventAreas.AsQueryable());
            var eventAreaService = new EventAreaService(Mock.Object);

            // Act
            var actual = eventAreaService.GetAll();

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public async Task GetByIdAsync_WhenEventAreaExist_ShouldReturnLastEventArea()
        {
            // Arrange
            var expected = EventAreas.Last();
            var expectedId = expected.Id - 1;
            Mock.Setup(x => x.EventAreas.GetByIdAsync(expectedId)).ReturnsAsync(EventAreas.Last());
            var eventAreaService = new EventAreaService(Mock.Object);

            // Act
            var actual = await eventAreaService.GetByIdAsync(expectedId);

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void GetByIDAsync_WhenIdEqualZero_ShouldThrowValidationException()
        {
            // Arrange
            var eventAreaService = new EventAreaService(Mock.Object);

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () => await eventAreaService.GetByIdAsync(0));
        }

        [Test]
        public void GetByIDAsync_WhenIdEqualLeesThanZero_ShouldThrowValidationException()
        {
            // Arrange
            var eventAreaService = new AreaService(Mock.Object);

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () => await eventAreaService.GetByIdAsync(-1));
        }
    }
}
