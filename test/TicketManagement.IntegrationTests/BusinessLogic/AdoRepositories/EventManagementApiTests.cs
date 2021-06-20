using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using TicketManagement.DataAccess.DbContexts;
using TicketManagement.DataAccess.Domain.Models;
using TicketManagement.DataAccess.Repositories.AdoRepositories;
using TicketManagement.Dto;
using TicketManagement.Services.EventFlow.API.Infrastructure.Exceptions;
using TicketManagement.Services.EventFlow.API.Infrastructure.Services;

namespace TicketManagement.IntegrationTests.BusinessLogic.AdoRepositories
{
    [TestFixture]
    internal class EventManagementApiTests : TestDatabaseLoader
    {
        private AdoUsingParametersRepository<Layout> _layoutRepository;
        private AdoUsingStoredProcedureRepository<Event> _eventRepository;
        private AdoDbContext _adoDbContext;

        [OneTimeSetUp]
        public void InitRepositories()
        {
            _layoutRepository = new AdoUsingParametersRepository<Layout>(DefaultConnectionString);
            _eventRepository = new AdoUsingStoredProcedureRepository<Event>(DefaultConnectionString);
            _adoDbContext = new AdoDbContext(DefaultConnectionString);
        }

        [Test]
        public async Task CreateAsync_WhenEventExist_ShouldCreateEvent()
        {
            // Arrange
            var firstLayoutId = _layoutRepository.GetAllAsQueryable().First().Id;
            var expected = new Event
            {
                Id = _eventRepository.GetAllAsQueryable().Last().Id + 1,
                Name = "Created",
                LayoutId = firstLayoutId,
                Description = "Created",
                StartDateTime = new DateTime(3000, 1, 1),
                EndDateTime = new DateTime(4000, 1, 1),
                ImageUrl = "asd",
            };
            var eventService = new EventService(_adoDbContext);

            // Act
            await eventService.CreateAsync(
                new EventDto
                {
                    Name = "Created",
                    LayoutId = firstLayoutId,
                    Description = "Created",
                    StartDateTime = new DateTime(3000, 1, 1),
                    EndDateTime = new DateTime(4000, 1, 1),
                    ImageUrl = "asd",
                });
            var actual = _eventRepository.GetAllAsQueryable().Last();

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public async Task Create_WhenEventExist_ShouldCreateEventAreas()
        {
            // Arrange
            var firstLayoutId = _layoutRepository.GetAllAsQueryable().First().Id;
            var eventService = new EventService(_adoDbContext);
            var areaService = new AreaService(_adoDbContext);
            var eventAreasService = new EventAreaService(_adoDbContext);
            int lastEventAreaId = eventAreasService.GetAll().Last().Id;
            var expectedEventAreasDto = new List<EventAreaDto>();

            // Act
            await eventService.CreateAsync(new EventDto
            {
                Name = "Created",
                LayoutId = firstLayoutId,
                Description = "Created",
                StartDateTime = new DateTime(3000, 1, 1),
                EndDateTime = new DateTime(4000, 1, 1),
                ImageUrl = "asd",
            });
            var eventId = eventService.GetAll().Last().Id;
            var allAreasInLayout = areaService.GetAll().Where(x => x.LayoutId == firstLayoutId).ToList();

            for (int i = 0; i < allAreasInLayout.Count; i++)
            {
                expectedEventAreasDto.Add(new EventAreaDto
                {
                    Id = lastEventAreaId + i + 1,
                    Description = allAreasInLayout[i].Description,
                    EventId = eventId,
                    CoordX = allAreasInLayout[i].CoordX,
                    CoordY = allAreasInLayout[i].CoordY,
                });
            }

            var actualEventAreasDto = eventAreasService.GetAll().Where(x => x.Id > lastEventAreaId);

            // Assert
            actualEventAreasDto.Should().BeEquivalentTo(expectedEventAreasDto);
        }

        [Test]
        public async Task CreateAsync_WhenEventExist_ShouldCreateEventSeats()
        {
            // Arrange
            var firstLayoutId = _layoutRepository.GetAllAsQueryable().First().Id;
            var eventService = new EventService(_adoDbContext);
            var areaService = new AreaService(_adoDbContext);
            var seatsService = new SeatService(_adoDbContext);
            var eventSeatService = new EventSeatService(_adoDbContext);
            var expectedEventSeatsDto = new List<EventSeatDto>();
            var allSeatsForAllAreas = new List<SeatDto>();
            var lastEventSeatId = eventSeatService.GetAll().Last().Id;

            // Act
            await eventService.CreateAsync(
                new EventDto
                {
                    Name = "Created",
                    LayoutId = firstLayoutId,
                    Description = "Created",
                    StartDateTime = new DateTime(3000, 1, 1),
                    EndDateTime = new DateTime(4000, 1, 1),
                    ImageUrl = "asd",
                });
            var allAreasInLayout = areaService.GetAll().Where(x => x.LayoutId == firstLayoutId).ToList();
            var lastEventAreaId = new EventAreaService(_adoDbContext).GetAll().Last().Id - allAreasInLayout.Count;

            foreach (var item in allAreasInLayout)
            {
                allSeatsForAllAreas.AddRange(seatsService.GetAll().Where(x => x.AreaId == item.Id));
            }

            int currSateId = allSeatsForAllAreas.FirstOrDefault()?.AreaId ?? 0;

            for (int i = 0; i < allSeatsForAllAreas.Count; i++)
            {
                if (currSateId != allSeatsForAllAreas[i].AreaId)
                {
                    currSateId = allSeatsForAllAreas[i].AreaId;
                    lastEventAreaId++;
                }

                expectedEventSeatsDto.Add(new EventSeatDto
                {
                    Id = lastEventSeatId + i + 1,
                    EventAreaId = lastEventAreaId + 1,
                    Number = allSeatsForAllAreas[i].Number,
                    Row = allSeatsForAllAreas[i].Row,
                });
            }

            var actualEventSeatsDto = eventSeatService.GetAll().Where(x => x.Id > lastEventSeatId);

            // Assert
            actualEventSeatsDto.Should().BeEquivalentTo(expectedEventSeatsDto);
        }

        [Test]
        public void CreateAsync_WhenEventEmpty_ShouldThrowValidationException()
        {
            // Arrange
            var eventService = new EventService(_adoDbContext);

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () => await eventService.CreateAsync(null));
        }

        [Test]
        public void CreateAsync_WhenEventCreatedInThePast_ShouldThrowValidationException()
        {
            // Arrange
            var firstEvent = _eventRepository.GetAllAsQueryable().First();
            var eventDto = new EventDto
            {
                Id = firstEvent.Id,
                StartDateTime = new DateTime(2000, 1, 1),
                Description = firstEvent.Description,
                LayoutId = firstEvent.LayoutId,
                Name = firstEvent.Name,
                PriceFrom = 100,
                State = 0,
            };
            var eventService = new EventService(_adoDbContext);

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () => await eventService.CreateAsync(eventDto));
        }

        [Test]
        public void CreateAsync_WhenTheSameVenueInTheSameTime_ShouldThrowValidationException()
        {
            // Arrange
            var firstEvent = _eventRepository.GetAllAsQueryable().First();
            var eventDto = new EventDto
            {
                Id = firstEvent.Id,
                StartDateTime = firstEvent.StartDateTime,
                Description = firstEvent.Description,
                LayoutId = firstEvent.LayoutId,
                Name = firstEvent.Name,
                PriceFrom = 100,
                State = 0,
            };
            var eventService = new EventService(_adoDbContext);

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () => await eventService.CreateAsync(eventDto));
        }

        [Test]
        public void CreateAsync_WhenNoOneAreaNotContainSeats_ShouldThrowValidationException()
        {
            // Arrange
            var layoutWithoutSeatsArea = _layoutRepository.GetAllAsQueryable().Last().Id;
            var lastEvent = _eventRepository.GetAllAsQueryable().Last();
            var eventDto = new EventDto
            {
                Id = lastEvent.Id,
                StartDateTime = new DateTime(2990, 1, 1),
                Description = lastEvent.Description,
                LayoutId = layoutWithoutSeatsArea,
                Name = lastEvent.Name,
                PriceFrom = 100,
                State = 0,
            };
            var eventService = new EventService(_adoDbContext);

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () => await eventService.CreateAsync(eventDto));
        }

        [Test]
        public async Task DeleteAsync_WhenEventExist_ShouldDeleteLastEvent()
        {
            // Arrange
            var expected = _eventRepository.GetAllAsQueryable().Last();
            var eventService = new EventService(_adoDbContext, new EventSeatService(_adoDbContext), new EventAreaService(_adoDbContext));
            var eventDto = new EventDto
            {
                Id = expected.Id,
                StartDateTime = expected.StartDateTime,
                EndDateTime = expected.EndDateTime,
                Description = expected.Description,
                LayoutId = expected.LayoutId,
                Name = expected.Name,
                PriceFrom = 100,
                State = 0,
            };

            // Act
            await eventService.DeleteAsync(eventDto);
            var actual = _eventRepository.GetAllAsQueryable().Last();

            // Assert
            actual.Should().NotBeEquivalentTo(expected);
        }

        [Test]
        public void DeleteAsync_WhenEventEmpty_ShouldThrowValidationException()
        {
            // Arrange
            var eventService = new EventService(_adoDbContext);

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () => await eventService.DeleteAsync(null));
        }

        [Test]
        public void DeleteAsync_WhenIdEqualZero_ShouldThrowValidationException()
        {
            // Arrange
            var eventService = new EventService(_adoDbContext);

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () => await eventService.DeleteAsync(new EventDto { Id = 0 }));
        }

        [Test]
        public void DeleteAsync_WhenIdEqualLeesThanZero_ShouldThrowValidationException()
        {
            // Arrange
            var eventService = new EventService(_adoDbContext);

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () => await eventService.DeleteAsync(new EventDto { Id = -1 }));
        }

        [Test]
        public async Task UpdateAsync_WhenEventExist_ShouldUpdateEvent()
        {
            // Arrange
            var eventLast = _eventRepository.GetAllAsQueryable().Last();
            var expected = new Event
            {
                Id = eventLast.Id,
                StartDateTime = eventLast.StartDateTime.AddYears(1000),
                EndDateTime = eventLast.EndDateTime.AddYears(1001),
                Description = "Updated Description",
                LayoutId = eventLast.LayoutId,
                Name = "Updated name",
                ImageUrl = "asd",
            };
            var eventService = new EventService(_adoDbContext);
            var eventDto = new EventDto
            {
                Id = expected.Id,
                StartDateTime = expected.StartDateTime,
                EndDateTime = expected.EndDateTime,
                Description = expected.Description,
                LayoutId = expected.LayoutId,
                Name = expected.Name,
                PriceFrom = 100,
                State = 0,
                ImageUrl = expected.ImageUrl,
            };

            // Act
            await eventService.UpdateAsync(eventDto);
            var actual = _eventRepository.GetAllAsQueryable().Last();

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void UpdateAsync_WhenEventEmpty_ShouldThrowValidationException()
        {
            // Arrange
            var eventService = new EventService(_adoDbContext);

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () => await eventService.UpdateAsync(null));
        }

        [Test]
        public void UpdateAsync_WhenIdEqualZero_ShouldThrowValidationException()
        {
            // Arrange
            var eventService = new EventService(_adoDbContext);

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () => await eventService.UpdateAsync(new EventDto { Id = 0 }));
        }

        [Test]
        public void UpdateAsync_WhenIdEqualLeesThanZero_ShouldThrowValidationException()
        {
            // Arrange
            var eventService = new EventService(_adoDbContext);

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () => await eventService.UpdateAsync(new EventDto { Id = -1 }));
        }

        [Test]
        public void UpdateAsync_WhenIdIsNotExist_ShouldThrowValidationException()
        {
            // Arrange
            var eventService = new EventService(_adoDbContext);

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () => await eventService.UpdateAsync(new EventDto { Id = _eventRepository.GetAllAsQueryable().Last().Id + 1 }));
        }

        [Test]
        public async Task UpdateAsync_WhenLayoutChanged_ShouldUpdateLastEvent()
        {
            // Arrange
            var eventLast = _eventRepository.GetAllAsQueryable().Last();
            var layoutIdChanged = eventLast.Id + 1;
            var expected = new Event
            {
                Id = eventLast.Id,
                StartDateTime = eventLast.StartDateTime.AddYears(1000),
                EndDateTime = eventLast.EndDateTime.AddYears(1001),
                Description = "Updated Description",
                LayoutId = layoutIdChanged,
                Name = "Updated name",
                ImageUrl = eventLast.ImageUrl,
            };
            var eventService = new EventService(_adoDbContext);
            var eventDto = new EventDto
            {
                Id = expected.Id,
                StartDateTime = expected.StartDateTime,
                EndDateTime = expected.EndDateTime,
                Description = expected.Description,
                LayoutId = expected.LayoutId,
                Name = expected.Name,
                PriceFrom = 100,
                State = 0,
                ImageUrl = expected.ImageUrl,
            };

            // Act
            await eventService.UpdateAsync(eventDto);
            var actual = _eventRepository.GetAllAsQueryable().Last();

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void UpdateAsync_WhenEventUpdatedInThePast_ShouldThrowValidationException()
        {
            // Arrange
            var eventLast = _eventRepository.GetAllAsQueryable().Last();
            var eventService = new EventService(_adoDbContext);
            var eventDto = new EventDto
            {
                Id = eventLast.Id,
                StartDateTime = new DateTime(2000, 1, 1),
                Description = eventLast.Description,
                LayoutId = eventLast.LayoutId,
                Name = eventLast.Name,
                PriceFrom = 100,
                State = 0,
            };

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () => await eventService.UpdateAsync(eventDto));
        }

        [Test]
        public void UpdateAsync_WhenTheSameVenueInTheSameTime_ShouldThrowValidationException()
        {
            // Arrange
            var firstEvent = _eventRepository.GetAllAsQueryable().First();
            var layoutIdChanged = 2;
            var eventDto = new EventDto
            {
                Id = firstEvent.Id,
                StartDateTime = _eventRepository.GetAllAsQueryable().ToList()[2].StartDateTime,
                Description = firstEvent.Description,
                LayoutId = layoutIdChanged,
                Name = firstEvent.Name,
                PriceFrom = 100,
                State = 0,
            };
            var eventService = new EventService(_adoDbContext);

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () => await eventService.UpdateAsync(eventDto));
        }

        [Test]
        public void UpdateAsync_WhenNoOneAreaNotContainSeats_ShouldThrowValidationException()
        {
            // Arrange
            var lastEvent = _eventRepository.GetAllAsQueryable().Last();
            var layoutWithoutSeatsArea = _layoutRepository.GetAllAsQueryable().Last().Id;
            var eventDto = new EventDto
            {
                Id = lastEvent.Id,
                StartDateTime = new DateTime(2990, 1, 1),
                Description = lastEvent.Description,
                LayoutId = layoutWithoutSeatsArea,
                Name = lastEvent.Name,
                PriceFrom = 100,
                State = 0,
            };
            var eventService = new EventService(_adoDbContext);

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () => await eventService.UpdateAsync(eventDto));
        }
    }
}
