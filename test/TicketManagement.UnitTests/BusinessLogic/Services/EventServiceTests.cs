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
        public void Create_WhenEventExist_ShouldReturnCreatedEvent()
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
        public void Create_WhenEventCreatedInThePast_ShouldReturnValidationException()
        {
            // Arrange
            var firstEvent = Events.First();
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
            var eventService = new EventService(Mock.Object);
            Mock.Setup(x => x.Events.Create(It.IsAny<Event>())).Callback<Event>(v => Events.Add(v));

            // Act & Assert
            Assert.Throws<ValidationException>(() => eventService.Create(eventDto));
        }

        [Test]
        public void Create_WhenTheSameVenueInTheSameTime_ShouldReturnValidationException()
        {
            // Arrange
            var firstEvent = Events.First();
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
            var eventService = new EventService(Mock.Object);
            Mock.Setup(x => x.Events.Create(It.IsAny<Event>())).Callback<Event>(v => Events.Add(v));

            // Act & Assert
            Assert.Throws<ValidationException>(() => eventService.Create(eventDto));
        }

        [Test]
        public void Create_WhenNoOneAreaNotContainSeats_ShouldReturnValidationException()
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
        public void Delete_WhenEventExist_ShouldReturnListWithDeletedEvent()
        {
            // Arrange
            var expected = Events.Last();
            Mock.Setup(x => x.Events.Delete(It.IsAny<Event>())).Callback<Event>(v => Events.RemoveAt(v.Id - 1));
            Mock.Setup(x => x.Events.GetByID(expected.Id)).Returns(expected);
            var eventService = new EventService(Mock.Object);
            var layoutLast = Events.Last();
            var eventDto = new EventDto
            {
                Id = layoutLast.Id,
                DateTime = layoutLast.DateTime,
                Description = layoutLast.Description,
                LayoutId = layoutLast.LayoutId,
                Name = layoutLast.Name,
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
        public void Update_WhenEventExist_ShouldReturnListWithUpdatedEvent()
        {
            // Arrange
            var layoutLast = Events.Last();
            var expected = new Event
            {
                Id = layoutLast.Id,
                DateTime = layoutLast.DateTime,
                Description = "Updated Description",
                LayoutId = layoutLast.LayoutId,
                Name = "Updated name",
            };
            var eventService = new EventService(Mock.Object);
            var eventDto = new EventDto
            {
                Id = layoutLast.Id,
                DateTime = expected.DateTime,
                Description = expected.Description,
                LayoutId = expected.LayoutId,
                Name = expected.Name,
                Price = 100,
                State = 0,
            };
            Mock.Setup(x => x.Events.GetByID(layoutLast.Id)).Returns(layoutLast);

            // Act
            Action<Event> updateLastAction = venues => Events.RemoveAt(layoutLast.Id - 1);
            updateLastAction += v => Events.Insert(v.Id - 1, v);
            Mock.Setup(x => x.Events.Update(It.IsAny<Event>())).Callback(updateLastAction);

            eventService.Update(eventDto);
            var actual = Events[layoutLast.Id - 1];

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
        public void Update_WhenLayoutChanged_ShouldReturnListWithUpdatedEvent()
        {
            // Arrange
            var layoutLast = Events.Last();
            var layoutIdChanged = layoutLast.Id + 1;
            var expected = new Event
            {
                Id = layoutLast.Id,
                DateTime = layoutLast.DateTime,
                Description = "Updated Description",
                LayoutId = layoutIdChanged,
                Name = "Updated name",
            };
            var eventService = new EventService(Mock.Object);
            var eventDto = new EventDto
            {
                Id = layoutLast.Id,
                DateTime = expected.DateTime,
                Description = expected.Description,
                LayoutId = expected.LayoutId,
                Name = expected.Name,
                Price = 100,
                State = 0,
            };
            Mock.Setup(x => x.Events.GetByID(layoutLast.Id)).Returns(layoutLast);

            // Act
            Action<Event> updateLastAction = venues => Events.RemoveAt(layoutLast.Id - 1);
            updateLastAction += v => Events.Insert(v.Id - 1, v);
            Mock.Setup(x => x.Events.Update(It.IsAny<Event>())).Callback(updateLastAction);

            eventService.Update(eventDto);
            var actual = Events[layoutLast.Id - 1];

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void Update_WhenEventUpdatedInThePast_ShouldReturnValidationException()
        {
            // Arrange
            var layoutLast = Events.Last();
            var eventService = new EventService(Mock.Object);
            var eventDto = new EventDto
            {
                Id = layoutLast.Id,
                DateTime = new DateTime(2000, 1, 1),
                Description = layoutLast.Description,
                LayoutId = layoutLast.LayoutId,
                Name = layoutLast.Name,
                Price = 100,
                State = 0,
            };
            Mock.Setup(x => x.Events.GetByID(layoutLast.Id)).Returns(layoutLast);

            // Act & Assert
            Assert.Throws<ValidationException>(() => eventService.Update(eventDto));
        }

        [Test]
        public void Update_WhenTheSameVenueInTheSameTime_ShouldReturnValidationException()
        {
            // Arrange
            var firstEvent = Events.First();
            var layoutIdChanged = 2;
            var eventDto = new EventDto
            {
                Id = firstEvent.Id,
                DateTime = Events[2].DateTime,
                Description = firstEvent.Description,
                LayoutId = layoutIdChanged,
                Name = firstEvent.Name,
                Price = 100,
                State = 0,
            };
            var eventService = new EventService(Mock.Object);
            Mock.Setup(x => x.Events.GetByID(firstEvent.Id)).Returns(firstEvent);
            Mock.Setup(x => x.Events.Create(It.IsAny<Event>())).Callback<Event>(v => Events.Add(v));

            // Act & Assert
            Assert.Throws<ValidationException>(() => eventService.Update(eventDto));
        }

        [Test]
        public void Update_WhenNoOneAreaNotContainSeats_ShouldReturnValidationException()
        {
            // Arrange
            var lastEvent = Events.Last();
            var layoutWithoutSeatsArea = Layouts.Last().Id;
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
            Mock.Setup(x => x.Events.GetByID(lastEvent.Id)).Returns(lastEvent);

            // Act & Assert
            Assert.Throws<ValidationException>(() => eventService.Update(eventDto));
        }
    }
}
