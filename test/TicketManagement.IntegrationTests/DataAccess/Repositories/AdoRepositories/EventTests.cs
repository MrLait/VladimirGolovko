namespace TicketManagement.IntegrationTests.DataAccess.Repositories.AdoRepositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using FluentAssertions;
    using NUnit.Framework;
    using TicketManagement.DataAccess.Domain.Models;
    using TicketManagement.DataAccess.Repositories.AdoRepositories;

    [TestFixture]
    internal class EventTests : AdoRepositoryTests
    {
        private readonly List<Event> _eventModels = new List<Event>();

        [OneTimeSetUp]
        public void InitEvents()
        {
            var eventModelRepository = new AdoUsingStoredProcedureRepository<Event>(MainConnectionString);
            var coutAllEvents = eventModelRepository.GetAll().Last().Id;

            for (int i = 1; i <= coutAllEvents; i++)
            {
                _eventModels.Add(eventModelRepository.GetByID(i));
            }
        }

        [Test]
        public void GivenGetAll_WhenEventsExist_ShouldReturnEventList()
        {
            // Arrange
            var expected = _eventModels;

            // Act
            var actual = new AdoUsingStoredProcedureRepository<Event>(MainConnectionString).GetAll();

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void GivenGetAll_WhenEventsIncorrectConnectionSting_ShouldReturnArgumentException()
        {
            // Arrange
            var actual = new AdoUsingStoredProcedureRepository<Event>("Server=myServerAddress;Database=myDataBase;User Id=myUsername;Password=myPassword;");

            // Act
            Assert.Throws<ArgumentException>(() => actual.GetAll());
        }

        [Test]
        public void GivenCreate_WhenAddEvent_ShouldReturnEventWithNewEvent()
        {
            // Arrange
            var repository = new AdoUsingStoredProcedureRepository<Event>(MainConnectionString);
            Event eventModel = new Event { Id = repository.GetAll().ToList().Count + 1, LayoutId = 2, DateTime = DateTime.Today, Description = "Created event", Name = "Test event" };
            List<Event> expected = new List<Event>(_eventModels)
            {
                eventModel,
            };

            // Act
            repository.Create(eventModel);
            var actual = repository.GetAll();

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void GivenCreate_WhenEventEmpty_ShouldReturnArgumentException()
        {
            // Arrange
            var repository = new AdoUsingStoredProcedureRepository<Event>(MainConnectionString);

            // Act
            Assert.Throws<ArgumentException>(() => repository.Create(null));
        }

        [Test]
        public void GivenDelete_WhenExistEvent_ShouldReturnEventListWithoutDeletedEvent()
        {
            // Arrange
            var repository = new AdoUsingStoredProcedureRepository<Event>(MainConnectionString);

            // Act
            var allEvents = repository.GetAll();
            var lastEvent = allEvents.Last();
            repository.Delete(lastEvent);

            var actual = repository.GetAll();
            int countEventWithoutLast = allEvents.ToList().Count - 1;

            // Assert
            actual.Should().BeEquivalentTo(allEvents.Take(countEventWithoutLast));
        }

        [Test]
        public void GivenDelete_WhenIdEqualZeroEvent_ShouldReturnArgumentException()
        {
            // Arrange
            Event eventModel = new Event { Id = 0 };
            var repository = new AdoUsingStoredProcedureRepository<Event>(MainConnectionString);

            // Act
            Assert.Throws<ArgumentException>(() => repository.Delete(eventModel));
        }

        [Test]
        public void GivenDelete_WhenNullEvent_ShouldReturnArgumentException()
        {
            // Arrange
            var repository = new AdoUsingStoredProcedureRepository<Event>(MainConnectionString);

            // Act
            Assert.Throws<ArgumentException>(() => repository.Delete(null));
        }

        [Test]
        public void GivenDelete_WhenIncorrectConnectionStringEvent_ShouldReturnArgumentException()
        {
            // Arrange
            Event eventModel = new Event { Id = 3 };
            var repository = new AdoUsingStoredProcedureRepository<Event>("Server=myServerAddress;Database=myDataBase;User Id=myUsername;Password=myPassword;");

            // Act
            Assert.Throws<ArgumentException>(() => repository.Delete(eventModel));
        }

        [Test]
        public void GivenUpdate_WhenExistEvent_ShouldReturnListWithUpdateEvent()
        {
            // Arrange
            var repository = new AdoUsingStoredProcedureRepository<Event>(MainConnectionString);
            var expected = new Event { LayoutId = 2, Name = "Updated ivent", Description = "Description updated event", DateTime = DateTime.Today };

            // Act
            var lastEvent = repository.GetAll().Last();
            var idLastEvent = lastEvent.Id;
            expected.Id = idLastEvent;

            repository.Update(expected);
            var actual = repository.GetAll().Last();

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void GivenUpdate_WhenNullEvent_ShouldReturnArgumentException()
        {
            // Arrange
            var repository = new AdoUsingStoredProcedureRepository<Event>(MainConnectionString);

            // Act
            Assert.Throws<ArgumentException>(() => repository.Update(null));
        }

        [Test]
        public void GivenUpdate_WhenIdEqualZeroEvent_ShouldReturnArgumentException()
        {
            // Arrange
            Event eventModel = new Event { Id = 0 };
            var repository = new AdoUsingStoredProcedureRepository<Event>(MainConnectionString);

            // Act
            Assert.Throws<ArgumentException>(() => repository.Update(eventModel));
        }

        [Test]
        public void GivenGetById_WhenExistEvent_ShouldReturnEvent()
        {
            // Arrange
            var repository = new AdoUsingStoredProcedureRepository<Event>(MainConnectionString);

            // Act
            var lastEvent = repository.GetAll().Last();
            Event expectedEvent = new Event { Id = lastEvent.Id, DateTime = lastEvent.DateTime, Description = lastEvent.Description, Name = lastEvent.Name, LayoutId = lastEvent.LayoutId };

            int actualId = expectedEvent.Id;
            var actual = repository.GetByID(actualId);

            // Assert
            actual.Should().BeEquivalentTo(expectedEvent);
        }

        [Test]
        public void GivenGetById_WhenNonExistEvent_ShouldReturnNull()
        {
            // Arrange
            Event expected = null;
            var repository = new AdoUsingStoredProcedureRepository<Event>(MainConnectionString);

            // Act
            var lastEvent = repository.GetAll().Last();
            int nonExistId = lastEvent.Id + 1;
            var actual = repository.GetByID(nonExistId);

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void GivenGetById_WhenIdEqualZeroEvent_ShouldReturnArgumentException()
        {
            // Arrange
            Event eventModel = new Event { Id = 0 };
            var repository = new AdoUsingStoredProcedureRepository<Event>(MainConnectionString);

            // Act
            Assert.Throws<ArgumentException>(() => repository.GetByID(eventModel.Id));
        }

        [Test]
        public void GivenGetById_WhenIdLessThenZero_ShouldReturnArgumentException()
        {
            // Arrange
            Event eventModel = new Event { Id = -1 };
            var repository = new AdoUsingParametersRepository<Event>(MainConnectionString);

            // Act
            Assert.Throws<ArgumentException>(() => repository.GetByID(eventModel.Id));
        }

        [Test]
        public void GivenUpdate_WhenIdLessThenZero_ShouldReturnArgumentException()
        {
            // Arrange
            Event eventModel = new Event { Id = -1 };
            var repository = new AdoUsingParametersRepository<Event>(MainConnectionString);

            // Act
            Assert.Throws<ArgumentException>(() => repository.Update(eventModel));
        }

        [Test]
        public void GivenDelete_WhenIdLessThenZero_ShouldReturnArgumentException()
        {
            // Arrange
            Event eventModel = new Event { Id = -1 };
            var repository = new AdoUsingParametersRepository<Event>(MainConnectionString);

            // Act
            Assert.Throws<ArgumentException>(() => repository.Delete(eventModel));
        }
    }
}
