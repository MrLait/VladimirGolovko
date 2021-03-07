using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using TicketManagement.DataAccess.Domain.Models;
using TicketManagement.DataAccess.Repositories.AdoRepositories;

namespace TicketManagement.IntegrationTests.DataAccess.Repositories.AdoRepositories
{
    [TestFixture]
    internal class EventAreaTests : AdoRepositoryTests
    {
        private readonly List<EventArea> _eventAreas = new List<EventArea>();

        [OneTimeSetUp]
        public void InitEventAreas()
        {
            var eventAreaRepository = new AdoUsingParametersRepository<EventArea>(MainConnectionString);
            var coutAllEventAreas = eventAreaRepository.GetAll().Last().Id;

            for (int i = 1; i <= coutAllEventAreas; i++)
            {
                _eventAreas.Add(eventAreaRepository.GetByID(i));
            }
        }

        [Test]
        public void GivenGetAll_WhenEventAreasExist_ShouldReturnEventAreaList()
        {
            // Arrange
            var expected = _eventAreas;

            // Act
            var actual = new AdoUsingParametersRepository<EventArea>(MainConnectionString).GetAll();

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void GivenGetAll_WhenEventAreasIncorrectConnectionSting_ShouldReturnArgumentException()
        {
            // Arrange
            var actual = new AdoUsingParametersRepository<EventArea>("Server=myServerAddress;Database=myDataBase;User Id=myUsername;Password=myPassword;");

            // Act
            TestDelegate testAction = () => actual.GetAll();

            // Assert
            Assert.Throws<ArgumentException>(testAction);
        }

        [Test]
        public void GivenCreate_WhenAddEventArea_ShouldReturnEventAreaWithNewEventArea()
        {
            // Arrange
            var repository = new AdoUsingParametersRepository<EventArea>(MainConnectionString);
            EventArea eventArea = new EventArea { Id = repository.GetAll().ToList().Count + 1, CoordX = 2, CoordY = 2, Description = "Creaded", EventId = 2, Price = 10 };
            List<EventArea> expected = new List<EventArea>(_eventAreas);

            // Act
            expected.Add(eventArea);
            repository.Create(eventArea);
            var actual = repository.GetAll();

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void GivenCreate_WhenEventAreaEmpty_ShouldReturnArgumentException()
        {
            // Arrange
            var repository = new AdoUsingParametersRepository<EventArea>(MainConnectionString);

            // Act
            TestDelegate testAction = () => repository.Create(null);

            // Assert
            Assert.Throws<ArgumentException>(testAction);
        }

        [Test]
        public void GivenDelete_WhenExistEventArea_ShouldReturnEventAreaListWithoutDeletedEventArea()
        {
            // Arrange
            var repository = new AdoUsingParametersRepository<EventArea>(MainConnectionString);

            // Act
            var allEventAreas = repository.GetAll();
            var lastEventArea = allEventAreas.Last();
            repository.Delete(lastEventArea);

            var actual = repository.GetAll();
            int countEventAreaWithoutLast = allEventAreas.ToList().Count - 1;

            // Assert
            actual.Should().BeEquivalentTo(allEventAreas.Take(countEventAreaWithoutLast));
        }

        [Test]
        public void GivenDelete_WhenIdEqualZeroEventArea_ShouldReturnArgumentException()
        {
            // Arrange
            EventArea eventArea = new EventArea { Id = 0 };
            var repository = new AdoUsingParametersRepository<EventArea>(MainConnectionString);

            // Act
            TestDelegate testAction = () => repository.Delete(eventArea);

            // Assert
            Assert.Throws<ArgumentException>(testAction);
        }

        [Test]
        public void GivenDelete_WhenNullEventArea_ShouldReturnArgumentException()
        {
            // Arrange
            var repository = new AdoUsingParametersRepository<EventArea>(MainConnectionString);

            // Act
            TestDelegate testAction = () => repository.Delete(null);

            // Assert
            Assert.Throws<ArgumentException>(testAction);
        }

        [Test]
        public void GivenDelete_WhenIncorrectConnectionStringEventArea_ShouldReturnArgumentException()
        {
            // Arrange
            EventArea eventArea = new EventArea { Id = 3 };
            var repository = new AdoUsingParametersRepository<EventArea>("Server=myServerAddress;Database=myDataBase;User Id=myUsername;Password=myPassword;");

            // Act
            TestDelegate testAction = () => repository.Delete(eventArea);

            // Assert
            Assert.Throws<ArgumentException>(testAction);
        }

        [Test]
        public void GivenUpdate_WhenExistEventArea_ShouldReturnListWithUpdateEventArea()
        {
            // Arrange
            var repository = new AdoUsingParametersRepository<EventArea>(MainConnectionString);
            var expected = new EventArea { Price = 2, EventId = 2, Description = "Updated", CoordY = 2, CoordX = 2 };

            // Act
            var lastEventArea = repository.GetAll().Last();
            var idLastEventArea = lastEventArea.Id;
            expected.Id = idLastEventArea;

            repository.Update(expected);
            var actual = repository.GetAll().Last();

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void GivenUpdate_WhenNullEventArea_ShouldReturnArgumentException()
        {
            // Arrange
            var repository = new AdoUsingParametersRepository<EventArea>(MainConnectionString);

            // Act
            TestDelegate testAction = () => repository.Update(null);

            // Assert
            Assert.Throws<ArgumentException>(testAction);
        }

        [Test]
        public void GivenUpdate_WhenIdEqualZeroEventArea_ShouldReturnArgumentException()
        {
            // Arrange
            EventArea eventArea = new EventArea { Id = 0 };
            var repository = new AdoUsingParametersRepository<EventArea>(MainConnectionString);

            // Act
            TestDelegate testAction = () => repository.Update(eventArea);

            // Assert
            Assert.Throws<ArgumentException>(testAction);
        }

        [Test]
        public void GivenGetById_WhenExistEventArea_ShouldReturnEventArea()
        {
            // Arrange
            var repository = new AdoUsingParametersRepository<EventArea>(MainConnectionString);

            // Act
            var lastEventArea = repository.GetAll().Last();
            EventArea expectedEventArea = new EventArea
            {
                Id = lastEventArea.Id,
                CoordX = lastEventArea.CoordX,
                CoordY = lastEventArea.CoordY,
                Description = lastEventArea.Description,
                EventId = lastEventArea.EventId,
                Price = lastEventArea.Price,
            };

            int actualId = expectedEventArea.Id;
            var actual = repository.GetByID(actualId);

            // Assert
            actual.Should().BeEquivalentTo(expectedEventArea);
        }

        [Test]
        public void GivenGetById_WhenNonExistEventArea_ShouldReturnNull()
        {
            // Arrange
            EventArea expected = null;
            var repository = new AdoUsingParametersRepository<EventArea>(MainConnectionString);

            // Act
            var lastEventArea = repository.GetAll().Last();
            int nonExistId = lastEventArea.Id + 1;
            var actual = repository.GetByID(nonExistId);

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void GivenGetById_WhenIdEqualZeroEventArea_ShouldReturnArgumentException()
        {
            // Arrange
            EventArea eventArea = new EventArea { Id = 0 };
            var repository = new AdoUsingParametersRepository<EventArea>(MainConnectionString);

            // Act
            TestDelegate testAction = () => repository.GetByID(eventArea.Id);

            // Assert
            Assert.Throws<ArgumentException>(testAction);
        }
    }
}
