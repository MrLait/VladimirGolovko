using System;
using System.Collections.Generic;
using System.Linq;
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
        public void Create_WhenEventExist_ShouldCreateEvent()
        {
            // Arrange
            var firstLayoutId = _layoutRepository.GetAll().First().Id;
            var expected = new Event { Id = _eventRepository.GetAll().Last().Id + 1,  Name = "Created", LayoutId = firstLayoutId, Description = "Created", DateTime = new DateTime(3000, 1, 1) };
            var eventService = new EventService(_adoDbContext);

            // Act
            eventService.Create(new EventDto { Name = "Created", LayoutId = firstLayoutId, Description = "Created", DateTime = new DateTime(3000, 1, 1) });
            var actual = _eventRepository.GetAll().Last();

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void Create_WhenEventExist_ShouldCreateEventAreas()
        {
            // Arrange
            var firstLayoutId = _layoutRepository.GetAll().First().Id;
            var eventService = new EventService(_adoDbContext);
            var areaService = new AreaService(_adoDbContext);
            var eventAreasService = new EventAreaService(_adoDbContext);
            int lastEventAreaId = eventAreasService.GetAll().Last().Id;
            var expectedEventAreasDto = new List<EventAreaDto>();

            // Act
            eventService.Create(new EventDto { Name = "Created", LayoutId = firstLayoutId, Description = "Created", DateTime = new DateTime(3000, 1, 1) });
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
        public void Create_WhenEventExist_ShouldCreateEventSeats()
        {
            // Arrange
            var firstLayoutId = _layoutRepository.GetAll().First().Id;
            var eventService = new EventService(_adoDbContext);
            var areaService = new AreaService(_adoDbContext);
            var seatsService = new SeatService(_adoDbContext);
            var eventSeatService = new EventSeatService(_adoDbContext);
            var expectedEventSeatsDto = new List<EventSeatDto>();
            var allSeatsForAllAreas = new List<SeatDto>();
            var lastEventAreaId = new EventAreaService(_adoDbContext).GetAll().Last().Id;
            var lastEventSeatId = eventSeatService.GetAll().Last().Id;

            // Act
            eventService.Create(new EventDto { Name = "Created", LayoutId = firstLayoutId, Description = "Created", DateTime = new DateTime(3000, 1, 1) });
            var allAreasInLayout = areaService.GetAll().Where(x => x.LayoutId == firstLayoutId).ToList();
            foreach (var item in allAreasInLayout)
            {
                allSeatsForAllAreas.AddRange(seatsService.GetAll().Where(x => x.AreaId == item.Id));
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

            var actualEventSeatsDto = eventSeatService.GetAll().Where(x => x.Id > lastEventSeatId);

            // Assert
            actualEventSeatsDto.Should().BeEquivalentTo(expectedEventSeatsDto);
        }

        [Test]
        public void Create_WhenEventEmpty_ShouldThrowValidationException()
        {
            // Arrange
            var eventService = new EventService(_adoDbContext);

            // Act & Assert
            Assert.Throws<ValidationException>(() => eventService.Create(null));
        }

        [Test]
        public void Create_WhenEventCreatedInThePast_ShouldThrowValidationException()
        {
            // Arrange
            var firstEvent = _eventRepository.GetAll().First();
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
            Assert.Throws<ValidationException>(() => eventService.Create(eventDto));
        }

        [Test]
        public void Create_WhenTheSameVenueInTheSameTime_ShouldThrowValidationException()
        {
            // Arrange
            var firstEvent = _eventRepository.GetAll().First();
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
            Assert.Throws<ValidationException>(() => eventService.Create(eventDto));
        }

        [Test]
        public void Create_WhenNoOneAreaNotContainSeats_ShouldThrowValidationException()
        {
            // Arrange
            var layoutWithoutSeatsArea = _layoutRepository.GetAll().Last().Id;
            var lastEvent = _eventRepository.GetAll().Last();
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
            Assert.Throws<ValidationException>(() => eventService.Create(eventDto));
        }

        [Test]
        public void Delete_WhenEventExist_ShouldDeleteLastEvent()
        {
            // Arrange
            var expected = _eventRepository.GetAll().Last();
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
            eventService.Delete(eventDto);
            var actual = _eventRepository.GetAll().Last();

            // Assert
            actual.Should().NotBeEquivalentTo(expected);
        }

        [Test]
        public void Delete_WhenEventEmpty_ShouldThrowValidationException()
        {
            // Arrange
            var eventService = new EventService(_adoDbContext);

            // Act & Assert
            Assert.Throws<ValidationException>(() => eventService.Delete(null));
        }

        [Test]
        public void Delete_WhenIdEqualZero_ShouldThrowValidationException()
        {
            // Arrange
            var eventService = new EventService(_adoDbContext);

            // Act & Assert
            Assert.Throws<ValidationException>(() => eventService.Delete(new EventDto { Id = 0 }));
        }

        [Test]
        public void Delete_WhenIdEqualLeesThanZero_ShouldThrowValidationException()
        {
            // Arrange
            var eventService = new EventService(_adoDbContext);

            // Act & Assert
            Assert.Throws<ValidationException>(() => eventService.Delete(new EventDto { Id = -1 }));
        }

        [Test]
        public void Delete_WhenEventWithIdNotExist_ShouldThrowValidationException()
        {
            // Arrange
            var eventService = new EventService(_adoDbContext);

            // Act & Assert
            Assert.Throws<ValidationException>(() => eventService.Delete(new EventDto { Id = _eventRepository.GetAll().Last().Id + 1 }));
        }

        [Test]
        public void Update_WhenEventExist_ShouldUpdateEvent()
        {
            // Arrange
            var eventLast = _eventRepository.GetAll().Last();
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
            eventService.Update(eventDto);
            var actual = _eventRepository.GetAll().Last();

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void Update_WhenEventEmpty_ShouldThrowValidationException()
        {
            // Arrange
            var eventService = new EventService(_adoDbContext);

            // Act & Assert
            Assert.Throws<ValidationException>(() => eventService.Update(null));
        }

        [Test]
        public void Update_WhenIdEqualZero_ShouldThrowValidationException()
        {
            // Arrange
            var eventService = new EventService(_adoDbContext);

            // Act & Assert
            Assert.Throws<ValidationException>(() => eventService.Update(new EventDto { Id = 0 }));
        }

        [Test]
        public void Update_WhenIdEqualLeesThanZero_ShouldThrowValidationException()
        {
            // Arrange
            var eventService = new EventService(_adoDbContext);

            // Act & Assert
            Assert.Throws<ValidationException>(() => eventService.Update(new EventDto { Id = -1 }));
        }

        [Test]
        public void Update_WhenIdIsNotExist_ShouldThrowValidationException()
        {
            // Arrange
            var eventService = new EventService(_adoDbContext);

            // Act & Assert
            Assert.Throws<ValidationException>(() => eventService.Update(new EventDto { Id = _eventRepository.GetAll().Last().Id + 1 }));
        }

        [Test]
        public void Update_WhenLayoutChanged_ShouldUpdateLastEvent()
        {
            // Arrange
            var eventLast = _eventRepository.GetAll().Last();
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
            eventService.Update(eventDto);
            var actual = _eventRepository.GetAll().Last();

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void Update_WhenEventUpdatedInThePast_ShouldThrowValidationException()
        {
            // Arrange
            var eventLast = _eventRepository.GetAll().Last();
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
            Assert.Throws<ValidationException>(() => eventService.Update(eventDto));
        }

        [Test]
        public void Update_WhenTheSameVenueInTheSameTime_ShouldThrowValidationException()
        {
            // Arrange
            var firstEvent = _eventRepository.GetAll().First();
            var layoutIdChanged = 2;
            var eventDto = new EventDto
            {
                Id = firstEvent.Id,
                DateTime = _eventRepository.GetAll().ToList()[2].DateTime,
                Description = firstEvent.Description,
                LayoutId = layoutIdChanged,
                Name = firstEvent.Name,
                Price = 100,
                State = 0,
            };
            var eventService = new EventService(_adoDbContext);

            // Act & Assert
            Assert.Throws<ValidationException>(() => eventService.Update(eventDto));
        }

        [Test]
        public void Update_WhenNoOneAreaNotContainSeats_ShouldThrowValidationException()
        {
            // Arrange
            var lastEvent = _eventRepository.GetAll().Last();
            var layoutWithoutSeatsArea = _layoutRepository.GetAll().Last().Id;
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
            Assert.Throws<ValidationException>(() => eventService.Update(eventDto));
        }
    }
}
