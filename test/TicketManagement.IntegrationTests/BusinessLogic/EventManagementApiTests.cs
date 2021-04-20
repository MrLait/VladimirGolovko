using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using TicketManagement.BusinessLogic.Infrastructure;
using TicketManagement.BusinessLogic.Services;
using TicketManagement.DataAccess.Ado;
using TicketManagement.DataAccess.Domain.Models;
using TicketManagement.DataAccess.Repositories.AdoRepositories;
using TicketManagement.Dto;

namespace TicketManagement.IntegrationTests.BusinessLogic
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
            _layoutRepository = new AdoUsingParametersRepository<Layout>(MainConnectionString);
            _eventRepository = new AdoUsingStoredProcedureRepository<Event>(MainConnectionString);
            _adoDbContext = new AdoDbContext(MainConnectionString);
        }

        [Test]
        public async Task CreateAsync_WhenEventExist_ShouldCreateEvent()
        {
            // Arrange
            var firstLayoutId = (await _layoutRepository.GetAllAsync()).First().Id;
            var expected = new Event
            {
                Id = (await _eventRepository.GetAllAsync()).Last().Id + 1,
                Name = "Created",
                LayoutId = firstLayoutId,
                Description = "Created",
                DateTime = new DateTime(3000, 1, 1),
            };
            var eventService = new EventService(_adoDbContext);

            // Act
            await eventService.CreateAsync(new EventDto { Name = "Created", LayoutId = firstLayoutId, Description = "Created", DateTime = new DateTime(3000, 1, 1) });
            var actual = (await _eventRepository.GetAllAsync()).Last();

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public async Task Create_WhenEventExist_ShouldCreateEventAreas()
        {
            // Arrange
            var firstLayoutId = (await _layoutRepository.GetAllAsync()).First().Id;
            var eventService = new EventService(_adoDbContext);
            var areaService = new AreaService(_adoDbContext);
            var eventAreasService = new EventAreaService(_adoDbContext);
            int lastEventAreaId = (await eventAreasService.GetAllAsync()).Last().Id;
            var expectedEventAreasDto = new List<EventAreaDto>();

            // Act
            await eventService.CreateAsync(new EventDto { Name = "Created", LayoutId = firstLayoutId, Description = "Created", DateTime = new DateTime(3000, 1, 1) });
            var eventId = (await eventService.GetAllAsync()).Last().Id;
            var allAreasInLayout = (await areaService.GetAllAsync()).Where(x => x.LayoutId == firstLayoutId).ToList();

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

            var actualEventAreasDto = (await eventAreasService.GetAllAsync()).Where(x => x.Id > lastEventAreaId);

            // Assert
            actualEventAreasDto.Should().BeEquivalentTo(expectedEventAreasDto);
        }

        [Test]
        public async Task CreateAsync_WhenEventExist_ShouldCreateEventSeats()
        {
            // Arrange
            var firstLayoutId = (await _layoutRepository.GetAllAsync()).First().Id;
            var eventService = new EventService(_adoDbContext);
            var areaService = new AreaService(_adoDbContext);
            var seatsService = new SeatService(_adoDbContext);
            var eventSeatService = new EventSeatService(_adoDbContext);
            var expectedEventSeatsDto = new List<EventSeatDto>();
            var allSeatsForAllAreas = new List<SeatDto>();
            var lastEventAreaId = (await new EventAreaService(_adoDbContext).GetAllAsync()).Last().Id;
            var lastEventSeatId = (await eventSeatService.GetAllAsync()).Last().Id;

            // Act
            await eventService.CreateAsync(new EventDto { Name = "Created", LayoutId = firstLayoutId, Description = "Created", DateTime = new DateTime(3000, 1, 1) });
            var allAreasInLayout = (await areaService.GetAllAsync()).Where(x => x.LayoutId == firstLayoutId).ToList();
            foreach (var item in allAreasInLayout)
            {
                allSeatsForAllAreas.AddRange((await seatsService.GetAllAsync()).Where(x => x.AreaId == item.Id));
            }

            int currSateId = allSeatsForAllAreas.FirstOrDefault().AreaId;
            bool isChanged = true;

            for (int i = 0; i < allSeatsForAllAreas.Count; i++)
            {
                if (isChanged)
                {
                    lastEventAreaId++;
                    isChanged = false;
                }

                expectedEventSeatsDto.Add(new EventSeatDto
                {
                    Id = lastEventSeatId + i + 1,
                    EventAreaId = lastEventAreaId,
                    Number = allSeatsForAllAreas[i].Number,
                    Row = allSeatsForAllAreas[i].Row,
                });

                if (currSateId != allSeatsForAllAreas[i].AreaId)
                {
                    isChanged = true;
                    currSateId = allSeatsForAllAreas[i].AreaId;
                }
            }

            var actualEventSeatsDto = (await eventSeatService.GetAllAsync()).Where(x => x.Id > lastEventSeatId);

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
        public async Task CreateAsync_WhenEventCreatedInThePast_ShouldThrowValidationException()
        {
            // Arrange
            var firstEvent = (await _eventRepository.GetAllAsync()).First();
            var eventDto = new EventDto
            {
                Id = firstEvent.Id,
                DateTime = new DateTime(2000, 1, 1),
                Description = firstEvent.Description,
                LayoutId = firstEvent.LayoutId,
                Name = firstEvent.Name,
                Price = 100,
                State = 0,
            };
            var eventService = new EventService(_adoDbContext);

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () => await eventService.CreateAsync(eventDto));
        }

        [Test]
        public async Task CreateAsync_WhenTheSameVenueInTheSameTime_ShouldThrowValidationException()
        {
            // Arrange
            var firstEvent = (await _eventRepository.GetAllAsync()).First();
            var eventDto = new EventDto
            {
                Id = firstEvent.Id,
                DateTime = firstEvent.DateTime,
                Description = firstEvent.Description,
                LayoutId = firstEvent.LayoutId,
                Name = firstEvent.Name,
                Price = 100,
                State = 0,
            };
            var eventService = new EventService(_adoDbContext);

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () => await eventService.CreateAsync(eventDto));
        }

        [Test]
        public async Task CreateAsync_WhenNoOneAreaNotContainSeats_ShouldThrowValidationException()
        {
            // Arrange
            var layoutWithoutSeatsArea = (await _layoutRepository.GetAllAsync()).Last().Id;
            var lastEvent = (await _eventRepository.GetAllAsync()).Last();
            var eventDto = new EventDto
            {
                Id = lastEvent.Id,
                DateTime = new DateTime(2990, 1, 1),
                Description = lastEvent.Description,
                LayoutId = layoutWithoutSeatsArea,
                Name = lastEvent.Name,
                Price = 100,
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
            var expected = (await _eventRepository.GetAllAsync()).Last();
            var eventService = new EventService(_adoDbContext);
            var eventDto = new EventDto
            {
                Id = expected.Id,
                DateTime = expected.DateTime,
                Description = expected.Description,
                LayoutId = expected.LayoutId,
                Name = expected.Name,
                Price = 100,
                State = 0,
            };

            // Act
            await eventService.DeleteAsync(eventDto);
            var actual = (await _eventRepository.GetAllAsync()).Last();

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
        public void DeleteAsync_WhenEventWithIdNotExist_ShouldThrowValidationException()
        {
            // Arrange
            var eventService = new EventService(_adoDbContext);

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () => await eventService.DeleteAsync(new EventDto { Id = (await _eventRepository.GetAllAsync()).Last().Id + 1 }));
        }

        [Test]
        public async Task UpdateAsync_WhenEventExist_ShouldUpdateEvent()
        {
            // Arrange
            var eventLast = (await _eventRepository.GetAllAsync()).Last();
            var expected = new Event
            {
                Id = eventLast.Id,
                DateTime = eventLast.DateTime.AddYears(1000),
                Description = "Updated Description",
                LayoutId = eventLast.LayoutId,
                Name = "Updated name",
            };
            var eventService = new EventService(_adoDbContext);
            var eventDto = new EventDto
            {
                Id = expected.Id,
                DateTime = expected.DateTime,
                Description = expected.Description,
                LayoutId = expected.LayoutId,
                Name = expected.Name,
                Price = 100,
                State = 0,
            };

            // Act
            await eventService.UpdateAsync(eventDto);
            var actual = (await _eventRepository.GetAllAsync()).Last();

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
            Assert.ThrowsAsync<ValidationException>(async () => await eventService.UpdateAsync(new EventDto { Id = (await _eventRepository.GetAllAsync()).Last().Id + 1 }));
        }

        [Test]
        public async Task UpdateAsync_WhenLayoutChanged_ShouldUpdateLastEvent()
        {
            // Arrange
            var eventLast = (await _eventRepository.GetAllAsync()).Last();
            var layoutIdChanged = eventLast.Id + 1;
            var expected = new Event
            {
                Id = eventLast.Id,
                DateTime = eventLast.DateTime.AddYears(1000),
                Description = "Updated Description",
                LayoutId = layoutIdChanged,
                Name = "Updated name",
            };
            var eventService = new EventService(_adoDbContext);
            var eventDto = new EventDto
            {
                Id = expected.Id,
                DateTime = expected.DateTime,
                Description = expected.Description,
                LayoutId = expected.LayoutId,
                Name = expected.Name,
                Price = 100,
                State = 0,
            };

            // Act
            await eventService.UpdateAsync(eventDto);
            var actual = (await _eventRepository.GetAllAsync()).Last();

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public async Task UpdateAsync_WhenEventUpdatedInThePast_ShouldThrowValidationException()
        {
            // Arrange
            var eventLast = (await _eventRepository.GetAllAsync()).Last();
            var eventService = new EventService(_adoDbContext);
            var eventDto = new EventDto
            {
                Id = eventLast.Id,
                DateTime = new DateTime(2000, 1, 1),
                Description = eventLast.Description,
                LayoutId = eventLast.LayoutId,
                Name = eventLast.Name,
                Price = 100,
                State = 0,
            };

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () => await eventService.UpdateAsync(eventDto));
        }

        [Test]
        public async Task UpdateAsync_WhenTheSameVenueInTheSameTime_ShouldThrowValidationException()
        {
            // Arrange
            var firstEvent = (await _eventRepository.GetAllAsync()).First();
            var layoutIdChanged = 2;
            var eventDto = new EventDto
            {
                Id = firstEvent.Id,
                DateTime = (await _eventRepository.GetAllAsync()).ToList()[2].DateTime,
                Description = firstEvent.Description,
                LayoutId = layoutIdChanged,
                Name = firstEvent.Name,
                Price = 100,
                State = 0,
            };
            var eventService = new EventService(_adoDbContext);

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () => await eventService.UpdateAsync(eventDto));
        }

        [Test]
        public async Task UpdateAsync_WhenNoOneAreaNotContainSeats_ShouldThrowValidationException()
        {
            // Arrange
            var lastEvent = (await _eventRepository.GetAllAsync()).Last();
            var layoutWithoutSeatsArea = (await _layoutRepository.GetAllAsync()).Last().Id;
            var eventDto = new EventDto
            {
                Id = lastEvent.Id,
                DateTime = new DateTime(2990, 1, 1),
                Description = lastEvent.Description,
                LayoutId = layoutWithoutSeatsArea,
                Name = lastEvent.Name,
                Price = 100,
                State = 0,
            };
            var eventService = new EventService(_adoDbContext);

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () => await eventService.UpdateAsync(eventDto));
        }
    }
}
