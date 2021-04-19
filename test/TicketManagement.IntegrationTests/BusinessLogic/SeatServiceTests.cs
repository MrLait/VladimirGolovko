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
    internal class SeatServiceTests : TestDatabaseLoader
    {
        private AdoUsingParametersRepository<Seat> _seatRepository;
        private AdoDbContext _adoDbContext;

        [OneTimeSetUp]
        public void InitRepositories()
        {
            _seatRepository = new AdoUsingParametersRepository<Seat>(MainConnectionString);
            _adoDbContext = new AdoDbContext(MainConnectionString);
        }

        [Test]
        public void Create_WhenSeatExist_ShouldReturnCreatedSeat()
        {
            // Arrange
            var expected = new Seat { Id = _seatRepository.GetAll().Last().Id + 1, AreaId = 12, Number = 3, Row = 5 };
            var seatService = new SeatService(_adoDbContext);

            // Act
            seatService.Create(new SeatDto { AreaId = 12, Number = 3, Row = 5 });
            var actual = _seatRepository.GetAll().Last();

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void Create_WhenSeatEmpty_ShouldThrowValidationException()
        {
            // Arrange
            var seatService = new SeatService(_adoDbContext);

            // Act & Assert
            Assert.Throws<ValidationException>(() => seatService.Create(null));
        }

        [Test]
        public void Create_WhenSeatAlreadyExist_ShouldThrowValidationException()
        {
            // Arrange
            var seatFirst = _seatRepository.GetAll().First();
            var seatDto = new SeatDto { Id = seatFirst.Id, Row = seatFirst.Row, Number = seatFirst.Number, AreaId = seatFirst.AreaId };
            var seatService = new SeatService(_adoDbContext);

            // Act & Assert
            Assert.Throws<ValidationException>(() => seatService.Create(seatDto));
        }

        [Test]
        public void Delete_WhenSeatExist_ShouldDeleteLastSeat()
        {
            // Arrange
            var expected = _seatRepository.GetAll().Last();
            var seatService = new SeatService(_adoDbContext);
            var seatLast = _seatRepository.GetAll().Last();

            // Act
            seatService.Delete(new SeatDto { Id = seatLast.Id, AreaId = seatLast.AreaId, Number = seatLast.Number, Row = seatLast.Row });
            var actual = _seatRepository.GetAll().Last();

            // Assert
            actual.Should().NotBeEquivalentTo(expected);
        }

        [Test]
        public void Delete_WhenSeatEmpty_ShouldThrowValidationException()
        {
            // Arrange
            var seatService = new SeatService(_adoDbContext);

            // Act & Assert
            Assert.Throws<ValidationException>(() => seatService.Delete(null));
        }

        [Test]
        public void Delete_WhenIdEqualZero_ShouldThrowValidationException()
        {
            // Arrange
            var seatService = new SeatService(_adoDbContext);

            // Act & Assert
            Assert.Throws<ValidationException>(() => seatService.Delete(new SeatDto { Id = 0 }));
        }

        [Test]
        public void Delete_WhenIdEqualLeesThanZero_ShouldThrowValidationException()
        {
            // Arrange
            var seatService = new SeatService(_adoDbContext);

            // Act & Assert
            Assert.Throws<ValidationException>(() => seatService.Delete(new SeatDto { Id = -1 }));
        }

        [Test]
        public void Update_WhenSeatExist_ShouldUpdateLastSeat()
        {
            // Arrange
            var seatLast = _seatRepository.GetAll().Last();
            var expected = new Seat { Id = seatLast.Id, Row = seatLast.Row + 1, Number = seatLast.Number + 1, AreaId = seatLast.AreaId };
            var seatService = new SeatService(_adoDbContext);

            // Act
            seatService.Update(new SeatDto { Id = seatLast.Id, AreaId = expected.AreaId, Number = expected.Number, Row = expected.Row });
            var actual = _seatRepository.GetAll().Last();

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void Update_WhenSeatEmpty_ShouldThrowValidationException()
        {
            // Arrange
            var seatService = new SeatService(_adoDbContext);

            // Act & Assert
            Assert.Throws<ValidationException>(() => seatService.Update(null));
        }

        [Test]
        public void Update_WhenIdEqualZero_ShouldThrowValidationException()
        {
            // Arrange
            var seatService = new SeatService(_adoDbContext);

            // Act & Assert
            Assert.Throws<ValidationException>(() => seatService.Update(new SeatDto { Id = 0 }));
        }

        [Test]
        public void Update_WhenIdEqualLeesThanZero_ShouldThrowValidationException()
        {
            // Arrange
            var seatService = new SeatService(_adoDbContext);

            // Act & Assert
            Assert.Throws<ValidationException>(() => seatService.Update(new SeatDto { Id = -1 }));
        }

        [Test]
        public void Update_WhenSeatAlreadyExist_ShouldThrowValidationException()
        {
            // Arrange
            var seatFirst = _seatRepository.GetAll().First();
            var seatDto = new SeatDto { Id = seatFirst.Id, Row = seatFirst.Row, Number = seatFirst.Number, AreaId = seatFirst.AreaId };
            var seatService = new SeatService(_adoDbContext);

            // Act & Assert
            Assert.Throws<ValidationException>(() => seatService.Update(seatDto));
        }

        [Test]
        public void GetAll_WhenSeatsExist_ShouldReturnSeats()
        {
            // Arrange
            var expected = _seatRepository.GetAll();
            var seatService = new SeatService(_adoDbContext);

            // Act
            var actual = seatService.GetAll();

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void GetById_WhenSeatExist_ShouldReturnLastSeat()
        {
            // Arrange
            var expected = _seatRepository.GetAll().Last();
            var expectedId = expected.Id;
            var seatService = new SeatService(_adoDbContext);

            // Act
            var actual = seatService.GetByID(expectedId);

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void GetByID_WhenIdEqualZero_ShouldThrowValidationException()
        {
            // Arrange
            var seatService = new SeatService(_adoDbContext);

            // Act & Assert
            Assert.Throws<ValidationException>(() => seatService.GetByID(0));
        }

        [Test]
        public void GetByID_WhenIdEqualLeesThanZero_ShouldThrowValidationException()
        {
            // Arrange
            var seatService = new SeatService(_adoDbContext);

            // Act & Assert
            Assert.Throws<ValidationException>(() => seatService.GetByID(-1));
        }
    }
}
