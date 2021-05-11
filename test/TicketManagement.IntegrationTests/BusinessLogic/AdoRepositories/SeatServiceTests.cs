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

namespace TicketManagement.IntegrationTests.BusinessLogic.AdoRepositories
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
        public async Task CreateAsync_WhenSeatExist_ShouldReturnCreatedSeat()
        {
            // Arrange
            var expected = new Seat { Id = _seatRepository.GetAllAsQueryable().Last().Id + 1, AreaId = 12, Number = 3, Row = 5 };
            var seatService = new SeatService(_adoDbContext);

            // Act
            await seatService.CreateAsync(new SeatDto { AreaId = 12, Number = 3, Row = 5 });
            var actual = _seatRepository.GetAllAsQueryable().Last();

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void CreateAsync_WhenSeatEmpty_ShouldThrowValidationException()
        {
            // Arrange
            var seatService = new SeatService(_adoDbContext);

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () => await seatService.CreateAsync(null));
        }

        [Test]
        public void CreateAsync_WhenSeatAlreadyExist_ShouldThrowValidationException()
        {
            // Arrange
            var seatFirst = _seatRepository.GetAllAsQueryable().First();
            var seatDto = new SeatDto { Id = seatFirst.Id, Row = seatFirst.Row, Number = seatFirst.Number, AreaId = seatFirst.AreaId };
            var seatService = new SeatService(_adoDbContext);

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () => await seatService.CreateAsync(seatDto));
        }

        [Test]
        public async Task DeleteAsync_WhenSeatExist_ShouldDeleteLastSeat()
        {
            // Arrange
            var expected = _seatRepository.GetAllAsQueryable().Last();
            var seatService = new SeatService(_adoDbContext);
            var seatLast = _seatRepository.GetAllAsQueryable().Last();

            // Act
            await seatService.DeleteAsync(new SeatDto { Id = seatLast.Id, AreaId = seatLast.AreaId, Number = seatLast.Number, Row = seatLast.Row });
            var actual = _seatRepository.GetAllAsQueryable().Last();

            // Assert
            actual.Should().NotBeEquivalentTo(expected);
        }

        [Test]
        public void DeleteAsync_WhenSeatEmpty_ShouldThrowValidationException()
        {
            // Arrange
            var seatService = new SeatService(_adoDbContext);

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () => await seatService.DeleteAsync(null));
        }

        [Test]
        public void DeleteAsync_WhenIdEqualZero_ShouldThrowValidationException()
        {
            // Arrange
            var seatService = new SeatService(_adoDbContext);

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () => await seatService.DeleteAsync(new SeatDto { Id = 0 }));
        }

        [Test]
        public void DeleteAsync_WhenIdEqualLeesThanZero_ShouldThrowValidationException()
        {
            // Arrange
            var seatService = new SeatService(_adoDbContext);

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () => await seatService.DeleteAsync(new SeatDto { Id = -1 }));
        }

        [Test]
        public async Task UpdateAsync_WhenSeatExist_ShouldUpdateLastSeat()
        {
            // Arrange
            var seatLast = _seatRepository.GetAllAsQueryable().Last();
            var expected = new Seat { Id = seatLast.Id, Row = seatLast.Row + 1, Number = seatLast.Number + 1, AreaId = seatLast.AreaId };
            var seatService = new SeatService(_adoDbContext);

            // Act
            await seatService.UpdateAsync(new SeatDto { Id = seatLast.Id, AreaId = expected.AreaId, Number = expected.Number, Row = expected.Row });
            var actual = _seatRepository.GetAllAsQueryable().Last();

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void UpdateAsync_WhenSeatEmpty_ShouldThrowValidationException()
        {
            // Arrange
            var seatService = new SeatService(_adoDbContext);

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () => await seatService.UpdateAsync(null));
        }

        [Test]
        public void UpdateAsync_WhenIdEqualZero_ShouldThrowValidationException()
        {
            // Arrange
            var seatService = new SeatService(_adoDbContext);

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () => await seatService.UpdateAsync(new SeatDto { Id = 0 }));
        }

        [Test]
        public void UpdateAsync_WhenIdEqualLeesThanZero_ShouldThrowValidationException()
        {
            // Arrange
            var seatService = new SeatService(_adoDbContext);

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () => await seatService.UpdateAsync(new SeatDto { Id = -1 }));
        }

        [Test]
        public void UpdateAsync_WhenSeatAlreadyExist_ShouldThrowValidationException()
        {
            // Arrange
            var seatFirst = _seatRepository.GetAllAsQueryable().First();
            var seatDto = new SeatDto { Id = seatFirst.Id, Row = seatFirst.Row, Number = seatFirst.Number, AreaId = seatFirst.AreaId };
            var seatService = new SeatService(_adoDbContext);

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () => await seatService.UpdateAsync(seatDto));
        }

        [Test]
        public void GetAllAsync_WhenSeatsExist_ShouldReturnSeats()
        {
            // Arrange
            var expected = _seatRepository.GetAllAsQueryable();
            var seatService = new SeatService(_adoDbContext);

            // Act
            var actual = seatService.GetAll();

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public async Task GetByIdAsync_WhenSeatExist_ShouldReturnLastSeat()
        {
            // Arrange
            var expected = _seatRepository.GetAllAsQueryable().Last();
            var expectedId = expected.Id;
            var seatService = new SeatService(_adoDbContext);

            // Act
            var actual = await seatService.GetByIDAsync(expectedId);

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void GetByIDAsync_WhenIdEqualZero_ShouldThrowValidationException()
        {
            // Arrange
            var seatService = new SeatService(_adoDbContext);

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () => await seatService.GetByIDAsync(0));
        }

        [Test]
        public void GetByIDAsync_WhenIdEqualLeesThanZero_ShouldThrowValidationException()
        {
            // Arrange
            var seatService = new SeatService(_adoDbContext);

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () => await seatService.GetByIDAsync(-1));
        }
    }
}
