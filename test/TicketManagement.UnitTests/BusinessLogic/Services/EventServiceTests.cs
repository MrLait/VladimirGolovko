using System;
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
    /// Event service tests.
    /// </summary>
    [TestFixture]
    public class EventServiceTests : MockEntites
    {
        [Test]
        public void Create_WhenEventExist_ShouldCreateEvent()
        {
            // Arrange
            var firstLayoutId = Layouts.First().Id;
            var expected = new Event { Name = "Created", LayoutId = firstLayoutId, Description = "Created", DateTime = new DateTime(3000, 1, 1) };
            Mock.Setup(x => x.Events.Create(It.IsAny<Event>())).Callback<Event>(v => Events.Add(v));
            var eventService = new EventService(Mock.Object);

            // Act
            eventService.Create(new EventDto { Name = "Created", LayoutId = firstLayoutId, Description = "Created", DateTime = new DateTime(3000, 1, 1) });
            var actual = Events.Last();

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void Create_WhenEventEmpty_ShouldThrowValidationException()
        {
            // Arrange
            var eventService = new EventService(Mock.Object);

            // Act & Assert
            Assert.Throws<ValidationException>(() => eventService.Create(null));
        }

        [Test]
        public void Create_WhenEventCreatedInThePast_ShouldThrowValidationException()
        {
            // Arrange
            var eventFirst = Events.First();
            var eventDto = new EventDto
            {
                Id = eventFirst.Id,
                DateTime = new DateTime(2000, 1, 1),
                Description = eventFirst.Description,
                LayoutId = eventFirst.LayoutId,
                Name = eventFirst.Name,
                Price = 100,
                State = 0,
            };
            var eventService = new EventService(Mock.Object);
            Mock.Setup(x => x.Events.Create(It.IsAny<Event>())).Callback<Event>(v => Events.Add(v));

            // Act & Assert
            Assert.Throws<ValidationException>(() => eventService.Create(eventDto));
        }

        [Test]
        public void Create_WhenTheSameVenueInTheSameTime_ShouldThrowValidationException()
        {
            // Arrange
            var eventFirst = Events.First();
            var eventDto = new EventDto
            {
                Id = eventFirst.Id,
                DateTime = eventFirst.DateTime,
                Description = eventFirst.Description,
                LayoutId = eventFirst.LayoutId,
                Name = eventFirst.Name,
                Price = 100,
                State = 0,
            };
            var eventService = new EventService(Mock.Object);
            Mock.Setup(x => x.Events.Create(It.IsAny<Event>())).Callback<Event>(v => Events.Add(v));

            // Act & Assert
            Assert.Throws<ValidationException>(() => eventService.Create(eventDto));
        }

        [Test]
        public void Create_WhenNoOneAreaNotContainSeats_ShouldThrowValidationException()
        {
            // Arrange
            var layoutWithoutSeatsArea = Layouts.Last().Id;
            var lastEvent = Events.Last();
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
            var eventService = new EventService(Mock.Object);
            Mock.Setup(x => x.Events.Create(It.IsAny<Event>())).Callback<Event>(v => Events.Add(v));

            // Act & Assert
            Assert.Throws<ValidationException>(() => eventService.Create(eventDto));
        }

        [Test]
        public void Delete_WhenEventExist_ShouldUpdateEvent()
        {
            // Arrange
            var expected = Events.Last();
            Mock.Setup(x => x.Events.Delete(It.IsAny<Event>())).Callback<Event>(v => Events.RemoveAt(v.Id - 1));
            Mock.Setup(x => x.Events.GetByID(expected.Id)).Returns(expected);
            var eventService = new EventService(Mock.Object);
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
            var actual = Events.Last();

            // Assert
            actual.Should().NotBeEquivalentTo(expected);
        }

        [Test]
        public void Delete_WhenEventEmpty_ShouldThrowValidationException()
        {
            // Arrange
            var eventService = new EventService(Mock.Object);

            // Act & Assert
            Assert.Throws<ValidationException>(() => eventService.Delete(null));
        }

        [Test]
        public void Delete_WhenIdEqualZero_ShouldThrowValidationException()
        {
            // Arrange
            var eventService = new EventService(Mock.Object);

            // Act & Assert
            Assert.Throws<ValidationException>(() => eventService.Delete(new EventDto { Id = 0 }));
        }

        [Test]
        public void Delete_WhenIdEqualLeesThanZero_ShouldThrowValidationException()
        {
            // Arrange
            var eventService = new EventService(Mock.Object);

            // Act & Assert
            Assert.Throws<ValidationException>(() => eventService.Delete(new EventDto { Id = -1 }));
        }

        [Test]
        public void Delete_WhenEventWithIdNotExist_ShouldThrowValidationException()
        {
            // Arrange
            var eventService = new EventService(Mock.Object);

            // Act & Assert
            Assert.Throws<ValidationException>(() => eventService.Delete(new EventDto { Id = Events.Last().Id + 1 }));
        }

        [Test]
        public void Update_WhenEventExist_ShouldUpdateLastEvent()
        {
            // Arrange
            var eventLast = Events.Last();
            var expected = new Event
            {
                Id = eventLast.Id,
                DateTime = eventLast.DateTime,
                Description = "Updated Description",
                LayoutId = eventLast.LayoutId,
                Name = "Updated name",
            };
            var eventService = new EventService(Mock.Object);
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
            Mock.Setup(x => x.Events.GetByID(eventLast.Id)).Returns(eventLast);

            // Act
            Action<Event> updateLastAction = venues => Events.RemoveAt(eventLast.Id - 1);
            updateLastAction += v => Events.Insert(v.Id - 1, v);
            Mock.Setup(x => x.Events.Update(It.IsAny<Event>())).Callback(updateLastAction);

            eventService.Update(eventDto);
            var actual = Events[eventLast.Id - 1];

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void Update_WhenEventEmpty_ShouldThrowValidationException()
        {
            // Arrange
            var eventService = new EventService(Mock.Object);

            // Act & Assert
            Assert.Throws<ValidationException>(() => eventService.Update(null));
        }

        [Test]
        public void Update_WhenIdEqualZero_ShouldThrowValidationException()
        {
            // Arrange
            var eventService = new EventService(Mock.Object);

            // Act & Assert
            Assert.Throws<ValidationException>(() => eventService.Update(new EventDto { Id = 0 }));
        }

        [Test]
        public void Update_WhenIdEqualLeesThanZero_ShouldThrowValidationException()
        {
            // Arrange
            var eventService = new EventService(Mock.Object);

            // Act & Assert
            Assert.Throws<ValidationException>(() => eventService.Update(new EventDto { Id = -1 }));
        }

        [Test]
        public void Update_WhenIdIsNotExist_ShouldThrowValidationException()
        {
            // Arrange
            var eventService = new EventService(Mock.Object);

            // Act & Assert
            Assert.Throws<ValidationException>(() => eventService.Update(new EventDto { Id = Events.Last().Id + 1 }));
        }

        [Test]
        public void Update_WhenLayoutChanged_ShouldUpdateLastEvent()
        {
            // Arrange
            var eventLast = Events.Last();
            var layoutIdChanged = eventLast.Id + 1;
            var expected = new Event
            {
                Id = eventLast.Id,
                DateTime = eventLast.DateTime,
                Description = "Updated Description",
                LayoutId = layoutIdChanged,
                Name = "Updated name",
            };
            var eventService = new EventService(Mock.Object);
            var eventDto = new EventDto
            {
                Id = eventLast.Id,
                DateTime = expected.DateTime,
                Description = expected.Description,
                LayoutId = expected.LayoutId,
                Name = expected.Name,
                Price = 100,
                State = 0,
            };
            Mock.Setup(x => x.Events.GetByID(eventLast.Id)).Returns(eventLast);

            // Act
            Action<Event> updateLastAction = events => Events.RemoveAt(eventLast.Id - 1);
            updateLastAction += v => Events.Insert(v.Id - 1, v);
            Mock.Setup(x => x.Events.Update(It.IsAny<Event>())).Callback(updateLastAction);

            eventService.Update(eventDto);
            var actual = Events[eventLast.Id - 1];

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void Update_WhenEventUpdatedInThePast_ShouldThrowValidationException()
        {
            // Arrange
            var eventLast = Events.Last();
            var eventService = new EventService(Mock.Object);
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
            Mock.Setup(x => x.Events.GetByID(eventLast.Id)).Returns(eventLast);

            // Act & Assert
            Assert.Throws<ValidationException>(() => eventService.Update(eventDto));
        }

        [Test]
        public void Update_WhenTheSameVenueInTheSameTime_ShouldThrowValidationException()
        {
            // Arrange
            var eventFirst = Events.First();
            var layoutIdChanged = 2;
            var eventDto = new EventDto
            {
                Id = eventFirst.Id,
                DateTime = Events[2].DateTime,
                Description = eventFirst.Description,
                LayoutId = layoutIdChanged,
                Name = eventFirst.Name,
                Price = 100,
                State = 0,
            };
            var eventService = new EventService(Mock.Object);
            Mock.Setup(x => x.Events.GetByID(eventFirst.Id)).Returns(eventFirst);
            Mock.Setup(x => x.Events.Create(It.IsAny<Event>())).Callback<Event>(v => Events.Add(v));

            // Act & Assert
            Assert.Throws<ValidationException>(() => eventService.Update(eventDto));
        }

        [Test]
        public void Update_WhenNoOneAreaNotContainSeats_ShouldThrowValidationException()
        {
            // Arrange
            var eventLast = Events.Last();
            var layoutWithoutSeatsArea = Layouts.Last().Id;
            var eventDto = new EventDto
            {
                Id = eventLast.Id,
                DateTime = new DateTime(2990, 1, 1),
                Description = eventLast.Description,
                LayoutId = layoutWithoutSeatsArea,
                Name = eventLast.Name,
                Price = 100,
                State = 0,
            };
            var eventService = new EventService(Mock.Object);
            Mock.Setup(x => x.Events.Create(It.IsAny<Event>())).Callback<Event>(v => Events.Add(v));
            Mock.Setup(x => x.Events.GetByID(eventLast.Id)).Returns(eventLast);

            // Act & Assert
            Assert.Throws<ValidationException>(() => eventService.Update(eventDto));
        }
    }
}
