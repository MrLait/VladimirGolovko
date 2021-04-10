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
    internal class EventSeatTests : AdoRepositoryTests
    {
        private readonly List<EventSeat> _eventSeats = new List<EventSeat>();

        [OneTimeSetUp]
        public void InitEventSeats()
        {
            var eventSeatsRepository = new AdoUsingParametersRepository<EventSeat>(MainConnectionString);
            var countAllEventSeats = eventSeatsRepository.GetAll().Last().Id;

            for (var i = 1; i <= countAllEventSeats; i++)
            {
                _eventSeats.Add(eventSeatsRepository.GetByID(i));
            }
        }

        [Test]
        public void GetAll_WhenEventSeatsExist_ShouldReturnEventSeatList()
        {
            // Arrange
            var expected = _eventSeats;

            // Act
            var actual = new AdoUsingParametersRepository<EventSeat>(MainConnectionString).GetAll();

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void GetAll_WhenEventSeatsIncorrectConnectionSting_ShouldThrowArgumentException()
        {
            // Arrange
            var actual = new AdoUsingParametersRepository<EventSeat>("Server=myServerAddress;Database=myDataBase;User Id=myUsername;Password=myPassword;");

            // Act & Assert
            Assert.Throws<ArgumentException>(() => actual.GetAll());
        }

        [Test]
        public void Create_WhenAddEventSeat_ShouldReturnEventSeatWithNewEventSeat()
        {
            // Arrange
            var repository = new AdoUsingParametersRepository<EventSeat>(MainConnectionString);
            EventSeat eventSeat = new EventSeat { Id = repository.GetAll().ToList().Count + 1, EventAreaId = 2, Number = 2, Row = 2, State = 2 };
            List<EventSeat> expected = new List<EventSeat>(_eventSeats)
            {
                eventSeat,
            };

            // Act
            repository.Create(eventSeat);
            var actual = repository.GetAll();

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void Create_WhenEventSeatEmpty_ShouldThrowArgumentException()
        {
            // Arrange
            var repository = new AdoUsingParametersRepository<EventSeat>(MainConnectionString);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => repository.Create(null));
        }

        [Test]
        public void Delete_WhenExistEventSeat_ShouldReturnEventSeatListWithoutDeletedEventSeat()
        {
            // Arrange
            var repository = new AdoUsingParametersRepository<EventSeat>(MainConnectionString);

            // Act
            var allEventSeats = repository.GetAll();
            var lastEventSeat = allEventSeats.Last();
            repository.Delete(lastEventSeat);

            var actual = repository.GetAll();
            int countEventSeatWithoutLast = allEventSeats.ToList().Count - 1;

            // Assert
            actual.Should().BeEquivalentTo(allEventSeats.Take(countEventSeatWithoutLast));
        }

        [Test]
        public void Delete_WhenIdEqualZeroEventSeat_ShouldThrowArgumentException()
        {
            // Arrange
            EventSeat eventSeat = new EventSeat { Id = 0 };
            var repository = new AdoUsingParametersRepository<EventSeat>(MainConnectionString);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => repository.Delete(eventSeat));
        }

        [Test]
        public void Delete_WhenNullEventSeat_ShouldThrowArgumentException()
        {
            // Arrange
            var repository = new AdoUsingParametersRepository<EventSeat>(MainConnectionString);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => repository.Delete(null));
        }

        [Test]
        public void Delete_WhenIncorrectConnectionStringEventSeat_ShouldThrowArgumentException()
        {
            // Arrange
            EventSeat eventSeat = new EventSeat { Id = 3 };
            var repository = new AdoUsingParametersRepository<EventSeat>("Server=myServerAddress;Database=myDataBase;User Id=myUsername;Password=myPassword;");

            // Act & Assert
            Assert.Throws<ArgumentException>(() => repository.Delete(eventSeat));
        }

        [Test]
        public void Update_WhenExistEventSeat_ShouldReturnListWithUpdateEventSeat()
        {
            // Arrange
            var repository = new AdoUsingParametersRepository<EventSeat>(MainConnectionString);
            var expected = new EventSeat { EventAreaId = 2, State = 1, Row = 2, Number = 2 };

            // Act
            var lastEventSeat = repository.GetAll().Last();
            var idLastEventSeat = lastEventSeat.Id;
            expected.Id = idLastEventSeat;

            repository.Update(expected);
            var actual = repository.GetAll().Last();

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void Update_WhenNullEventSeat_ShouldThrowArgumentException()
        {
            // Arrange
            var repository = new AdoUsingParametersRepository<EventSeat>(MainConnectionString);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => repository.Update(null));
        }

        [Test]
        public void Update_WhenIdEqualZeroEventSeat_ShouldThrowArgumentException()
        {
            // Arrange
            EventSeat eventSeat = new EventSeat { Id = 0 };
            var repository = new AdoUsingParametersRepository<EventSeat>(MainConnectionString);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => repository.Update(eventSeat));
        }

        [Test]
        public void GetById_WhenExistEventSeat_ShouldReturnEventSeat()
        {
            // Arrange
            var repository = new AdoUsingParametersRepository<EventSeat>(MainConnectionString);

            // Act
            var lastEventSeat = repository.GetAll().Last();
            EventSeat expectedEventSeat = new EventSeat
            {
                Id = lastEventSeat.Id,
                Number = lastEventSeat.Number,
                Row = lastEventSeat.Row,
                State = lastEventSeat.State,
                EventAreaId = lastEventSeat.EventAreaId,
            };

            int actualId = expectedEventSeat.Id;
            var actual = repository.GetByID(actualId);

            // Assert
            actual.Should().BeEquivalentTo(expectedEventSeat);
        }

        [Test]
        public void GetById_WhenNonExistEventSeat_ShouldReturnNull()
        {
            // Arrange
            EventSeat expected = null;
            var repository = new AdoUsingParametersRepository<EventSeat>(MainConnectionString);

            // Act
            var lastEventSeat = repository.GetAll().Last();
            int nonExistId = lastEventSeat.Id + 1;
            var actual = repository.GetByID(nonExistId);

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void GetById_WhenIdEqualZeroEventSeat_ShouldThrowArgumentException()
        {
            // Arrange
            EventSeat eventSeat = new EventSeat { Id = 0 };
            var repository = new AdoUsingParametersRepository<EventSeat>(MainConnectionString);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => repository.GetByID(eventSeat.Id));
        }

        [Test]
        public void GetById_WhenIdLessThenZero_ShouldThrowArgumentException()
        {
            // Arrange
            EventSeat eventSeat = new EventSeat { Id = -1 };
            var repository = new AdoUsingParametersRepository<EventSeat>(MainConnectionString);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => repository.GetByID(eventSeat.Id));
        }

        [Test]
        public void Update_WhenIdLessThenZero_ShouldThrowArgumentException()
        {
            // Arrange
            EventSeat eventSeat = new EventSeat { Id = -1 };
            var repository = new AdoUsingParametersRepository<EventSeat>(MainConnectionString);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => repository.Update(eventSeat));
        }

        [Test]
        public void Delete_WhenIdLessThenZero_ShouldThrowArgumentException()
        {
            // Arrange
            EventSeat eventSeat = new EventSeat { Id = -1 };
            var repository = new AdoUsingParametersRepository<EventSeat>(MainConnectionString);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => repository.Delete(eventSeat));
        }
    }
}
