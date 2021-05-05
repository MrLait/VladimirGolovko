////using System;
////using System.Linq;
////using System.Threading.Tasks;
////using FluentAssertions;
////using Moq;
////using NUnit.Framework;
////using TicketManagement.BusinessLogic.Infrastructure;
////using TicketManagement.BusinessLogic.Services;
////using TicketManagement.DataAccess.Domain.Models;
////using TicketManagement.Dto;

////namespace TicketManagement.UnitTests.BusinessLogic.Services
////{
////    /// <summary>
////    /// Event service tests.
////    /// </summary>
////    [TestFixture]
////    public class EventServiceTests : MockEntites
////    {
////        [Test]
////        public async Task CreateAsync_WhenEventExist_ShouldCreateEvent()
////        {
////            // Arrange
////            var firstLayoutId = Layouts.First().Id;
////            var expected = new Event { Name = "Created", LayoutId = firstLayoutId, Description = "Created", StartDateTime = new DateTime(3000, 1, 1) };
////            Mock.Setup(x => x.Events.CreateAsync(It.IsAny<Event>())).Callback<Event>(v => Events.Add(v));
////            var eventService = new EventService(Mock.Object);

////            // Act
////            await eventService.CreateAsync(new EventDto { Name = "Created", LayoutId = firstLayoutId, Description = "Created", StartDateTime = new DateTime(3000, 1, 1) });
////            var actual = Events.Last();

////            // Assert
////            actual.Should().BeEquivalentTo(expected);
////        }

////        [Test]
////        public void CreateAsync_WhenEventEmpty_ShouldThrowValidationException()
////        {
////            // Arrange
////            var eventService = new EventService(Mock.Object);

////            // Act & Assert
////            Assert.ThrowsAsync<ValidationException>(async () => await eventService.CreateAsync(null));
////        }

////        [Test]
////        public void CreateAsync_WhenEventCreatedInThePast_ShouldThrowValidationException()
////        {
////            // Arrange
////            var eventFirst = Events.First();
////            var eventDto = new EventDto
////            {
////                Id = eventFirst.Id,
////                StartDateTime = new DateTime(2000, 1, 1),
////                Description = eventFirst.Description,
////                LayoutId = eventFirst.LayoutId,
////                Name = eventFirst.Name,
////                Price = 100,
////                State = 0,
////            };
////            var eventService = new EventService(Mock.Object);
////            Mock.Setup(x => x.Events.CreateAsync(It.IsAny<Event>())).Callback<Event>(v => Events.Add(v));

////            // Act & Assert
////            Assert.ThrowsAsync<ValidationException>(async () => await eventService.CreateAsync(eventDto));
////        }

////        [Test]
////        public void CreateAsync_WhenTheSameVenueInTheSameTime_ShouldThrowValidationException()
////        {
////            // Arrange
////            var eventFirst = Events.First();
////            var eventDto = new EventDto
////            {
////                Id = eventFirst.Id,
////                StartDateTime = eventFirst.StartDateTime,
////                Description = eventFirst.Description,
////                LayoutId = eventFirst.LayoutId,
////                Name = eventFirst.Name,
////                Price = 100,
////                State = 0,
////            };
////            var eventService = new EventService(Mock.Object);
////            Mock.Setup(x => x.Events.CreateAsync(It.IsAny<Event>())).Callback<Event>(v => Events.Add(v));

////            // Act & Assert
////            Assert.ThrowsAsync<ValidationException>(async () => await eventService.CreateAsync(eventDto));
////        }

////        [Test]
////        public void CreateAsync_WhenNoOneAreaNotContainSeats_ShouldThrowValidationException()
////        {
////            // Arrange
////            var layoutWithoutSeatsArea = Layouts.Last().Id;
////            var lastEvent = Events.Last();
////            var eventDto = new EventDto
////            {
////                Id = lastEvent.Id,
////                StartDateTime = new DateTime(2990, 1, 1),
////                Description = lastEvent.Description,
////                LayoutId = layoutWithoutSeatsArea,
////                Name = lastEvent.Name,
////                Price = 100,
////                State = 0,
////            };
////            var eventService = new EventService(Mock.Object);
////            Mock.Setup(x => x.Events.CreateAsync(It.IsAny<Event>())).Callback<Event>(v => Events.Add(v));

////            // Act & Assert
////            Assert.ThrowsAsync<ValidationException>(async () => await eventService.CreateAsync(eventDto));
////        }

////        [Test]
////        public async Task DeleteAsync_WhenEventExist_ShouldUpdateEvent()
////        {
////            // Arrange
////            var expected = Events.Last();
////            Mock.Setup(x => x.Events.DeleteAsync(It.IsAny<Event>())).Callback<Event>(v => Events.RemoveAt(v.Id - 1));
////            Mock.Setup(x => x.Events.GetByIDAsync(expected.Id)).ReturnsAsync(expected);
////            var eventService = new EventService(Mock.Object);
////            var eventDto = new EventDto
////            {
////                Id = expected.Id,
////                StartDateTime = expected.StartDateTime,
////                Description = expected.Description,
////                LayoutId = expected.LayoutId,
////                Name = expected.Name,
////                Price = 100,
////                State = 0,
////            };

////            // Act
////            await eventService.DeleteAsync(eventDto);
////            var actual = Events.Last();

////            // Assert
////            actual.Should().NotBeEquivalentTo(expected);
////        }

////        [Test]
////        public void DeleteAsync_WhenEventEmpty_ShouldThrowValidationException()
////        {
////            // Arrange
////            var eventService = new EventService(Mock.Object);

////            // Act & Assert
////            Assert.ThrowsAsync<ValidationException>(async () => await eventService.DeleteAsync(null));
////        }

////        [Test]
////        public void DeleteAsync_WhenIdEqualZero_ShouldThrowValidationException()
////        {
////            // Arrange
////            var eventService = new EventService(Mock.Object);

////            // Act & Assert
////            Assert.ThrowsAsync<ValidationException>(async () => await eventService.DeleteAsync(new EventDto { Id = 0 }));
////        }

////        [Test]
////        public void DeleteAsync_WhenIdEqualLeesThanZero_ShouldThrowValidationException()
////        {
////            // Arrange
////            var eventService = new EventService(Mock.Object);

////            // Act & Assert
////            Assert.ThrowsAsync<ValidationException>(async () => await eventService.DeleteAsync(new EventDto { Id = -1 }));
////        }

////        [Test]
////        public void DeleteAsync_WhenEventWithIdNotExist_ShouldThrowValidationException()
////        {
////            // Arrange
////            var eventService = new EventService(Mock.Object);

////            // Act & Assert
////            Assert.ThrowsAsync<ValidationException>(async () => await eventService.DeleteAsync(new EventDto { Id = Events.Last().Id + 1 }));
////        }

////        [Test]
////        public async Task UpdateAsync_WhenEventExist_ShouldUpdateLastEvent()
////        {
////            // Arrange
////            var eventLast = Events.Last();
////            var expected = new Event
////            {
////                Id = eventLast.Id,
////                StartDateTime = eventLast.StartDateTime,
////                Description = "Updated Description",
////                LayoutId = eventLast.LayoutId,
////                Name = "Updated name",
////            };
////            var eventService = new EventService(Mock.Object);
////            var eventDto = new EventDto
////            {
////                Id = expected.Id,
////                StartDateTime = expected.StartDateTime,
////                Description = expected.Description,
////                LayoutId = expected.LayoutId,
////                Name = expected.Name,
////                Price = 100,
////                State = 0,
////            };
////            Mock.Setup(x => x.Events.GetByIDAsync(eventLast.Id)).ReturnsAsync(eventLast);

////            // Act
////            Action<Event> updateLastAction = venues => Events.RemoveAt(eventLast.Id - 1);
////            updateLastAction += v => Events.Insert(v.Id - 1, v);
////            Mock.Setup(x => x.Events.UpdateAsync(It.IsAny<Event>())).Callback(updateLastAction);

////            await eventService.UpdateAsync(eventDto);
////            var actual = Events[eventLast.Id - 1];

////            // Assert
////            actual.Should().BeEquivalentTo(expected);
////        }

////        [Test]
////        public void UpdateAsync_WhenEventEmpty_ShouldThrowValidationException()
////        {
////            // Arrange
////            var eventService = new EventService(Mock.Object);

////            // Act & Assert
////            Assert.ThrowsAsync<ValidationException>(async () => await eventService.UpdateAsync(null));
////        }

////        [Test]
////        public void UpdateAsync_WhenIdEqualZero_ShouldThrowValidationException()
////        {
////            // Arrange
////            var eventService = new EventService(Mock.Object);

////            // Act & Assert
////            Assert.ThrowsAsync<ValidationException>(async () => await eventService.UpdateAsync(new EventDto { Id = 0 }));
////        }

////        [Test]
////        public void UpdateAsync_WhenIdEqualLeesThanZero_ShouldThrowValidationException()
////        {
////            // Arrange
////            var eventService = new EventService(Mock.Object);

////            // Act & Assert
////            Assert.ThrowsAsync<ValidationException>(async () => await eventService.UpdateAsync(new EventDto { Id = -1 }));
////        }

////        [Test]
////        public void UpdateAsync_WhenIdIsNotExist_ShouldThrowValidationException()
////        {
////            // Arrange
////            var eventService = new EventService(Mock.Object);

////            // Act & Assert
////            Assert.ThrowsAsync<ValidationException>(async () => await eventService.UpdateAsync(new EventDto { Id = Events.Last().Id + 1 }));
////        }

////        [Test]
////        public async Task UpdateAsync_WhenLayoutChanged_ShouldUpdateLastEvent()
////        {
////            // Arrange
////            var eventLast = Events.Last();
////            var layoutIdChanged = eventLast.Id + 1;
////            var expected = new Event
////            {
////                Id = eventLast.Id,
////                StartDateTime = eventLast.StartDateTime,
////                Description = "Updated Description",
////                LayoutId = layoutIdChanged,
////                Name = "Updated name",
////            };
////            var eventService = new EventService(Mock.Object);
////            var eventDto = new EventDto
////            {
////                Id = eventLast.Id,
////                StartDateTime = expected.StartDateTime,
////                Description = expected.Description,
////                LayoutId = expected.LayoutId,
////                Name = expected.Name,
////                Price = 100,
////                State = 0,
////            };
////            Mock.Setup(x => x.Events.GetByIDAsync(eventLast.Id)).ReturnsAsync(eventLast);

////            // Act
////            Action<Event> updateLastAction = events => Events.RemoveAt(eventLast.Id - 1);
////            updateLastAction += v => Events.Insert(v.Id - 1, v);
////            Mock.Setup(x => x.Events.UpdateAsync(It.IsAny<Event>())).Callback(updateLastAction);

////            await eventService.UpdateAsync(eventDto);
////            var actual = Events[eventLast.Id - 1];

////            // Assert
////            actual.Should().BeEquivalentTo(expected);
////        }

////        [Test]
////        public void UpdateAsync_WhenEventUpdatedInThePast_ShouldThrowValidationException()
////        {
////            // Arrange
////            var eventLast = Events.Last();
////            var eventService = new EventService(Mock.Object);
////            var eventDto = new EventDto
////            {
////                Id = eventLast.Id,
////                StartDateTime = new DateTime(2000, 1, 1),
////                Description = eventLast.Description,
////                LayoutId = eventLast.LayoutId,
////                Name = eventLast.Name,
////                Price = 100,
////                State = 0,
////            };
////            Mock.Setup(x => x.Events.GetByIDAsync(eventLast.Id)).ReturnsAsync(eventLast);

////            // Act & Assert
////            Assert.ThrowsAsync<ValidationException>(async () => await eventService.UpdateAsync(eventDto));
////        }

////        [Test]
////        public void UpdateAsync_WhenTheSameVenueInTheSameTime_ShouldThrowValidationException()
////        {
////            // Arrange
////            var eventFirst = Events.First();
////            var layoutIdChanged = 2;
////            var eventDto = new EventDto
////            {
////                Id = eventFirst.Id,
////                StartDateTime = Events[2].StartDateTime,
////                Description = eventFirst.Description,
////                LayoutId = layoutIdChanged,
////                Name = eventFirst.Name,
////                Price = 100,
////                State = 0,
////            };
////            var eventService = new EventService(Mock.Object);
////            Mock.Setup(x => x.Events.GetByIDAsync(eventFirst.Id)).ReturnsAsync(eventFirst);
////            Mock.Setup(x => x.Events.CreateAsync(It.IsAny<Event>())).Callback<Event>(v => Events.Add(v));

////            // Act & Assert
////            Assert.ThrowsAsync<ValidationException>(async () => await eventService.UpdateAsync(eventDto));
////        }

////        [Test]
////        public void UpdateAsync_WhenNoOneAreaNotContainSeats_ShouldThrowValidationException()
////        {
////            // Arrange
////            var eventLast = Events.Last();
////            var layoutWithoutSeatsArea = Layouts.Last().Id;
////            var eventDto = new EventDto
////            {
////                Id = eventLast.Id,
////                StartDateTime = new DateTime(2990, 1, 1),
////                Description = eventLast.Description,
////                LayoutId = layoutWithoutSeatsArea,
////                Name = eventLast.Name,
////                Price = 100,
////                State = 0,
////            };
////            var eventService = new EventService(Mock.Object);
////            Mock.Setup(x => x.Events.CreateAsync(It.IsAny<Event>())).Callback<Event>(v => Events.Add(v));
////            Mock.Setup(x => x.Events.GetByIDAsync(eventLast.Id)).ReturnsAsync(eventLast);

////            // Act & Assert
////            Assert.ThrowsAsync<ValidationException>(async () => await eventService.UpdateAsync(eventDto));
////        }

////        [Test]
////        public async Task GetAllAsync_WhenEventsExist_ShouldReturnEvents()
////        {
////            // Arrange
////            var expected = Events;
////            Mock.Setup(x => x.Events.GetAllAsync()).ReturnsAsync(Events.AsQueryable());
////            var eventService = new EventService(Mock.Object);

////            // Act
////            var actual = await eventService.GetAllAsync();

////            // Assert
////            actual.Should().BeEquivalentTo(expected);
////        }

////        [Test]
////        public async Task GetByIdAsync_WhenEventExist_ShouldReturnLastEvent()
////        {
////            // Arrange
////            var expected = Events.Last();
////            var expectedId = expected.Id - 1;
////            Mock.Setup(x => x.Events.GetByIDAsync(expectedId)).ReturnsAsync(Events.Last());
////            var eventService = new EventService(Mock.Object);

////            // Act
////            var actual = await eventService.GetByIDAsync(expectedId);

////            // Assert
////            actual.Should().BeEquivalentTo(expected);
////        }

////        [Test]
////        public void GetByIDAsync_WhenIdEqualZero_ShouldThrowValidationException()
////        {
////            // Arrange
////            var eventService = new EventService(Mock.Object);

////            // Act & Assert
////            Assert.ThrowsAsync<ValidationException>(async () => await eventService.GetByIDAsync(0));
////        }

////        [Test]
////        public void GetByIDAsync_WhenIdEqualLeesThanZero_ShouldThrowValidationException()
////        {
////            // Arrange
////            var eventService = new EventService(Mock.Object);

////            // Act & Assert
////            Assert.ThrowsAsync<ValidationException>(async () => await eventService.GetByIDAsync(-1));
////        }
////    }
////}
