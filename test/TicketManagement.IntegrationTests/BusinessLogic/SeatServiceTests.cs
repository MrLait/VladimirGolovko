using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using TicketManagement.DataAccess.DbContexts;
using TicketManagement.DataAccess.Domain.Models;
using TicketManagement.DataAccess.Repositories.EfRepositories;
using TicketManagement.Dto;
using TicketManagement.Services.EventFlow.API.Infrastructure.Exceptions;
using TicketManagement.Services.EventFlow.API.Infrastructure.Services;

namespace TicketManagement.IntegrationTests.BusinessLogic
{
    [TestFixture]
    internal class SeatServiceTests : TestDatabaseLoader
    {
        private EfRepository<Seat> _seatRepository;

        private EfDbContext DbContext { get; set; }

        [OneTimeSetUp]
        public void InitRepositories()
        {
            DbContext = new EfDbContext(DefaultConnectionString);
            _seatRepository = new EfRepository<Seat>(DbContext);
        }

        [Test]
        public async Task CreateAsync_WhenSeatExist_ShouldReturnCreatedSeat()
        {
            // Arrange
            var expected = new Seat { Id = _seatRepository.GetAllAsQueryable().OrderBy(x => x.Id).Last().Id + 1, AreaId = 12, Number = 3, Row = 5 };
            var seatService = new SeatService(DbContext);

            // Act
            await seatService.CreateAsync(new SeatDto { AreaId = 12, Number = 3, Row = 5 });
            var actual = _seatRepository.GetAllAsQueryable().OrderBy(x => x.Id).Last();

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void CreateAsync_WhenSeatEmpty_ShouldThrowValidationException()
        {
            // Arrange
            var seatService = new SeatService(DbContext);

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () => await seatService.CreateAsync(null));
        }

        [Test]
        public void CreateAsync_WhenSeatAlreadyExist_ShouldThrowValidationException()
        {
            // Arrange
            var seatFirst = _seatRepository.GetAllAsQueryable().First();
            var seatDto = new SeatDto { Id = seatFirst.Id, Row = seatFirst.Row, Number = seatFirst.Number, AreaId = seatFirst.AreaId };
            var seatService = new SeatService(DbContext);

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () => await seatService.CreateAsync(seatDto));
        }

        [Test]
        public async Task DeleteAsync_WhenSeatExist_ShouldDeleteLastSeat()
        {
            // Arrange
            var expected = _seatRepository.GetAllAsQueryable().OrderBy(x => x.Id).Last();
            var seatService = new SeatService(DbContext);
            var seatLast = _seatRepository.GetAllAsQueryable().OrderBy(x => x.Id).Last();

            // Act
            await seatService.DeleteAsync(new SeatDto { Id = seatLast.Id, AreaId = seatLast.AreaId, Number = seatLast.Number, Row = seatLast.Row });
            var actual = _seatRepository.GetAllAsQueryable().OrderBy(x => x.Id).Last();

            // Assert
            actual.Should().NotBeEquivalentTo(expected);
        }

        [Test]
        public void DeleteAsync_WhenSeatEmpty_ShouldThrowValidationException()
        {
            // Arrange
            var seatService = new SeatService(DbContext);

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () => await seatService.DeleteAsync(null));
        }

        [Test]
        public void DeleteAsync_WhenIdEqualZero_ShouldThrowValidationException()
        {
            // Arrange
            var seatService = new SeatService(DbContext);

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () => await seatService.DeleteAsync(new SeatDto { Id = 0 }));
        }

        [Test]
        public void DeleteAsync_WhenIdEqualLeesThanZero_ShouldThrowValidationException()
        {
            // Arrange
            var seatService = new SeatService(DbContext);

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () => await seatService.DeleteAsync(new SeatDto { Id = -1 }));
        }

        [Test]
        public async Task UpdateAsync_WhenSeatExist_ShouldUpdateLastSeat()
        {
            // Arrange
            var dbContext = new EfDbContext(connectionString: DefaultConnectionString);
            var seatLast = _seatRepository.GetAllAsQueryable().OrderBy(x => x.Id).Last();
            var expected = new Seat { Id = seatLast.Id, Row = seatLast.Row + 1, Number = seatLast.Number + 1, AreaId = seatLast.AreaId };
            var seatService = new SeatService(dbContext);

            // Act
            await seatService.UpdateAsync(new SeatDto { Id = seatLast.Id, AreaId = expected.AreaId, Number = expected.Number, Row = expected.Row });
            var actual = _seatRepository.GetAllAsQueryable().OrderBy(x => x.Id).Last();

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void UpdateAsync_WhenSeatEmpty_ShouldThrowValidationException()
        {
            // Arrange
            var seatService = new SeatService(DbContext);

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () => await seatService.UpdateAsync(null));
        }

        [Test]
        public void UpdateAsync_WhenIdEqualZero_ShouldThrowValidationException()
        {
            // Arrange
            var seatService = new SeatService(DbContext);

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () => await seatService.UpdateAsync(new SeatDto { Id = 0 }));
        }

        [Test]
        public void UpdateAsync_WhenIdEqualLeesThanZero_ShouldThrowValidationException()
        {
            // Arrange
            var seatService = new SeatService(DbContext);

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () => await seatService.UpdateAsync(new SeatDto { Id = -1 }));
        }

        [Test]
        public void UpdateAsync_WhenSeatAlreadyExist_ShouldThrowValidationException()
        {
            // Arrange
            var seatFirst = _seatRepository.GetAllAsQueryable().First();
            var seatDto = new SeatDto { Id = seatFirst.Id, Row = seatFirst.Row, Number = seatFirst.Number, AreaId = seatFirst.AreaId };
            var seatService = new SeatService(DbContext);

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () => await seatService.UpdateAsync(seatDto));
        }

        [Test]
        public void GetAllAsync_WhenSeatsExist_ShouldReturnSeats()
        {
            // Arrange
            var expected = _seatRepository.GetAllAsQueryable();
            var seatService = new SeatService(DbContext);

            // Act
            var actual = seatService.GetAll();

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public async Task GetByIdAsync_WhenSeatExist_ShouldReturnLastSeat()
        {
            // Arrange
            var expected = _seatRepository.GetAllAsQueryable().OrderBy(x => x.Id).Last();
            var expectedId = expected.Id;
            var seatService = new SeatService(DbContext);

            // Act
            var actual = await seatService.GetByIdAsync(expectedId);

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void GetByIDAsync_WhenIdEqualZero_ShouldThrowValidationException()
        {
            // Arrange
            var seatService = new SeatService(DbContext);

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () => await seatService.GetByIdAsync(0));
        }

        [Test]
        public void GetByIDAsync_WhenIdEqualLeesThanZero_ShouldThrowValidationException()
        {
            // Arrange
            var seatService = new SeatService(DbContext);

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () => await seatService.GetByIdAsync(-1));
        }
    }
}
