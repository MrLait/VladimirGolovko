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
            var countAllEventAreas = eventAreaRepository.GetAll().Last().Id;

            for (int i = 1; i <= countAllEventAreas; i++)
            {
                _eventAreas.Add(eventAreaRepository.GetByID(i));
            }
        }

        [Test]
        public void GetAll_WhenEventAreasExist_ShouldReturnEventAreaList()
        {
            // Arrange
            var expected = _eventAreas;

            // Act
            var actual = new AdoUsingParametersRepository<EventArea>(MainConnectionString).GetAll();

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void GetAll_WhenEventAreasIncorrectConnectionSting_ShouldThrowArgumentException()
        {
            // Arrange
            var actual = new AdoUsingParametersRepository<EventArea>("Server=myServerAddress;Database=myDataBase;User Id=myUsername;Password=myPassword;");

            // Act & Assert
            Assert.Throws<ArgumentException>(() => actual.GetAll());
        }

        [Test]
        public void Create_WhenAddEventArea_ShouldReturnEventAreaWithNewEventArea()
        {
            // Arrange
            var repository = new AdoUsingParametersRepository<EventArea>(MainConnectionString);
            EventArea eventArea = new EventArea { Id = repository.GetAll().ToList().Count + 1, CoordX = 2, CoordY = 2, Description = "Creaded", EventId = 2, Price = 10 };
            List<EventArea> expected = new List<EventArea>(_eventAreas)
            {
                eventArea,
            };

            // Act
            repository.Create(eventArea);
            var actual = repository.GetAll();

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void Create_WhenEventAreaEmpty_ShouldThrowArgumentException()
        {
            // Arrange
            var repository = new AdoUsingParametersRepository<EventArea>(MainConnectionString);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => repository.Create(null));
        }

        [Test]
        public void Delete_WhenExistEventArea_ShouldReturnEventAreaListWithoutDeletedEventArea()
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
        public void Delete_WhenIdEqualZeroEventArea_ShouldThrowArgumentException()
        {
            // Arrange
            EventArea eventArea = new EventArea { Id = 0 };
            var repository = new AdoUsingParametersRepository<EventArea>(MainConnectionString);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => repository.Delete(eventArea));
        }

        [Test]
        public void Delete_WhenNullEventArea_ShouldThrowArgumentException()
        {
            // Arrange
            var repository = new AdoUsingParametersRepository<EventArea>(MainConnectionString);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => repository.Delete(null));
        }

        [Test]
        public void Delete_WhenIncorrectConnectionStringEventArea_ShouldThrowArgumentException()
        {
            // Arrange
            EventArea eventArea = new EventArea { Id = 3 };
            var repository = new AdoUsingParametersRepository<EventArea>("Server=myServerAddress;Database=myDataBase;User Id=myUsername;Password=myPassword;");

            // Act & Assert
            Assert.Throws<ArgumentException>(() => repository.Delete(eventArea));
        }

        [Test]
        public void Update_WhenExistEventArea_ShouldReturnListWithUpdateEventArea()
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
        public void Update_WhenNullEventArea_ShouldThrowArgumentException()
        {
            // Arrange
            var repository = new AdoUsingParametersRepository<EventArea>(MainConnectionString);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => repository.Update(null));
        }

        [Test]
        public void Update_WhenIdEqualZeroEventArea_ShouldThrowArgumentException()
        {
            // Arrange
            EventArea eventArea = new EventArea { Id = 0 };
            var repository = new AdoUsingParametersRepository<EventArea>(MainConnectionString);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => repository.Update(eventArea));
        }

        [Test]
        public void GetById_WhenExistEventArea_ShouldReturnEventArea()
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
        public void GetById_WhenNonExistEventArea_ShouldReturnNull()
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
        public void GetById_WhenIdEqualZeroEventArea_ShouldThrowArgumentException()
        {
            // Arrange
            EventArea eventArea = new EventArea { Id = 0 };
            var repository = new AdoUsingParametersRepository<EventArea>(MainConnectionString);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => repository.GetByID(eventArea.Id));
        }

        [Test]
        public void GetById_WhenIdLessThenZero_ShouldThrowArgumentException()
        {
            // Arrange
            EventArea eventArea = new EventArea { Id = -1 };
            var repository = new AdoUsingParametersRepository<EventArea>(MainConnectionString);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => repository.GetByID(eventArea.Id));
        }

        [Test]
        public void Update_WhenIdLessThenZero_ShouldThrowArgumentException()
        {
            // Arrange
            EventArea eventArea = new EventArea { Id = -1 };
            var repository = new AdoUsingParametersRepository<EventArea>(MainConnectionString);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => repository.Update(eventArea));
        }

        [Test]
        public void Delete_WhenIdLessThenZero_ShouldThrowArgumentException()
        {
            // Arrange
            EventArea eventArea = new EventArea { Id = -1 };
            var repository = new AdoUsingParametersRepository<EventArea>(MainConnectionString);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => repository.Delete(eventArea));
        }
    }
}
