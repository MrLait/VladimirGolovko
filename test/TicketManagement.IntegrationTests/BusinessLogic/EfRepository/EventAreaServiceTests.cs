﻿using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using TicketManagement.BusinessLogic.Infrastructure;
using TicketManagement.BusinessLogic.Services;
using TicketManagement.DataAccess.Ado;
using TicketManagement.DataAccess.Domain.Models;
using TicketManagement.DataAccess.Repositories.AdoRepositories;
using TicketManagement.DataAccess.Repositories.EfRepositories;
using TicketManagement.Dto;

namespace TicketManagement.IntegrationTests.BusinessLogic.EfRepository
{
    [TestFixture]
    internal class EventAreaServiceTests : TestDatabaseLoader
    {
        private EfRepository<EventArea> _eventAreaRepository;

        public EfDbContext DbContext { get; set; }

        [OneTimeSetUp]
        public void InitRepositories()
        {
            DbContext = new EfDbContext(MainConnectionString);
            _eventAreaRepository = new EfRepository<EventArea>(DbContext);
        }

        [Test]
        public async Task UpdatePriceAsync_WhenEventAreaExist_ShouldUpdatePriceInLastEventArea()
        {
            // Arrange
            var eventAreaLast = _eventAreaRepository.GetAllAsQueryable().OrderBy(x => x.Id).Last();
            EventArea expected = new EventArea
            {
                Id = eventAreaLast.Id,
                EventId = eventAreaLast.EventId,
                CoordX = eventAreaLast.CoordX,
                CoordY = eventAreaLast.CoordY,
                Description = eventAreaLast.Description,
                Price = eventAreaLast.Price + 100,
            };

            var eventAreaService = new EventAreaService(DbContext);

            // Act
            await eventAreaService.UpdatePriceAsync(new EventAreaDto
            {
                Id = eventAreaLast.Id,
                EventId = expected.EventId,
                CoordX = expected.CoordX,
                CoordY = expected.CoordY,
                Description = expected.Description,
                Price = expected.Price,
            });
            var actual = _eventAreaRepository.GetAllAsQueryable().OrderBy(x => x.Id).Last();

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void UpdatePriceAsync_WhenEventAreaEmpty_ShouldThrowValidationException()
        {
            // Arrange
            var eventAreaService = new EventAreaService(DbContext);

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () => await eventAreaService.UpdatePriceAsync(new EventAreaDto()));
        }

        [Test]
        public void UpdatePriceAsync_WhenIdEqualZero_ShouldThrowValidationException()
        {
            // Arrange
            var eventAreaService = new EventAreaService(DbContext);

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () => await eventAreaService.UpdatePriceAsync(new EventAreaDto { Id = 0 }));
        }

        [Test]
        public void UpdatePriceAsync_WhenIdEqualLeesThanZero_ShouldThrowValidationException()
        {
            // Arrange
            var eventAreaService = new EventAreaService(DbContext);

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () => await eventAreaService.UpdatePriceAsync(new EventAreaDto { Id = -1 }));
        }

        [Test]
        public void UpdatePriceAsync_WhenPriceLeesThanZero_ShouldThrowValidationException()
        {
            // Arrange
            var eventAreaService = new EventAreaService(DbContext);

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () => await eventAreaService.UpdatePriceAsync(new EventAreaDto { Id = 1, Price = -1 }));
        }

        [Test]
        public void GetAll_WhenEventAreasExist_ShouldReturnAreas()
        {
            // Arrange
            var expected = _eventAreaRepository.GetAllAsQueryable();
            var eventAreaService = new EventAreaService(DbContext);

            // Act
            var actual = eventAreaService.GetAll();

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public async Task GetByIdAsync_WhenEventAreaExist_ShouldReturnLastEventArea()
        {
            // Arrange
            var expected = _eventAreaRepository.GetAllAsQueryable().OrderBy(x => x.Id).Last();
            var expectedId = expected.Id;
            var eventAreaService = new EventAreaService(DbContext);

            // Act
            var actual = await eventAreaService.GetByIDAsync(expectedId);

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void GetByIDAsync_WhenIdEqualZero_ShouldThrowValidationException()
        {
            // Arrange
            var eventAreaService = new EventAreaService(DbContext);

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () => await eventAreaService.GetByIDAsync(0));
        }

        [Test]
        public void GetByIDAsync_WhenIdEqualLeesThanZero_ShouldThrowValidationException()
        {
            // Arrange
            var eventAreaService = new AreaService(DbContext);

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () => await eventAreaService.GetByIDAsync(-1));
        }
    }
}