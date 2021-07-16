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
    internal class VenueServiceTests : TestDatabaseLoader
    {
        private EfRepository<Venue> _venueRepository;

        public EfDbContext DbContext { get; set; }

        [OneTimeSetUp]
        public void InitRepositories()
        {
            DbContext = new EfDbContext(DefaultConnectionString);
            _venueRepository = new EfRepository<Venue>(DbContext);
        }

        [Test]
        public async Task CreateAsync_WhenVenueExist_ShouldReturnCreatedVenue()
        {
            // Arrange
            var expected = new Venue
            {
                Id = _venueRepository.GetAllAsQueryable().OrderBy(x => x.Id).Last().Id + 1,
                Address = "Added Address",
                Description = "Added Description",
                Phone = "+375293094300",
            };
            var venueService = new VenueService(DbContext);

            // Act
            await venueService.CreateAsync(new VenueDto { Address = "Added Address", Description = "Added Description", Phone = "+375293094300" });
            var actual = _venueRepository.GetAllAsQueryable().OrderBy(x=> x.Id).Last();

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void CreateAsync_WhenVenueEmpty_ShouldThrowValidationException()
        {
            // Arrange
            var venueService = new VenueService(DbContext);

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () => await venueService.CreateAsync(null));
        }

        [Test]
        public void CreateAsync_WhenDescriptionExist_ShouldThrowValidationException()
        {
            // Arrange
            var venueDto = new VenueDto { Id = 1, Description = "Luzhniki Stadium", Address = "st. Luzhniki, 24, Moscow, Russia, 119048", Phone = "+7 495 780-08-08" };
            var venueService = new VenueService(DbContext);

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () => await venueService.CreateAsync(venueDto));
        }

        [Test]
        public async Task DeleteAsync_WhenVenueExist_ShouldDeleteLastVenue()
        {
            // Arrange
            var expected = _venueRepository.GetAllAsQueryable().OrderBy(x=> x.Id).Last();
            var venueService = new VenueService(DbContext);
            var venueLast = _venueRepository.GetAllAsQueryable().OrderBy(x=> x.Id).Last();

            // Act
            await venueService.DeleteAsync(new VenueDto { Id = venueLast.Id, Description = venueLast.Description, Address = venueLast.Address, Phone = venueLast.Phone });
            var actual = _venueRepository.GetAllAsQueryable().OrderBy(x=> x.Id).Last();

            // Assert
            actual.Should().NotBeEquivalentTo(expected);
        }

        [Test]
        public void DeleteAsync_WhenVenueEmpty_ShouldThrowValidationException()
        {
            // Arrange
            var venueService = new VenueService(DbContext);

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () => await venueService.DeleteAsync(null));
        }

        [Test]
        public void DeleteAsync_WhenIdEqualZero_ShouldThrowValidationException()
        {
            // Arrange
            var venueService = new VenueService(DbContext);

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () => await venueService.DeleteAsync(new VenueDto { Id = 0 }));
        }

        [Test]
        public void DeleteAsync_WhenIdEqualLeesThanZero_ShouldThrowValidationException()
        {
            // Arrange
            var venueService = new VenueService(DbContext);

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () => await venueService.DeleteAsync(new VenueDto { Id = -1 }));
        }

        [Test]
        public async Task UpdateAsync_WhenVenueExist_ShouldUpdateLastVenue()
        {
            // Arrange
            var dbContext = new EfDbContext(connectionString: DefaultConnectionString);
            var venueLast = _venueRepository.GetAllAsQueryable().OrderBy(x=> x.Id).Last();
            var expected = new Venue { Id = venueLast.Id, Address = "Added Address", Description = "Added Description", Phone = "+375293094300" };
            var venueService = new VenueService(dbContext);

            // Act
            await venueService.UpdateAsync(new VenueDto { Id = venueLast.Id, Description = expected.Description, Address = expected.Address, Phone = expected.Phone });
            var actual = _venueRepository.GetAllAsQueryable().OrderBy(x=> x.Id).Last();

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void UpdateAsync_WhenVenueEmpty_ShouldThrowValidationException()
        {
            // Arrange
            var venueService = new VenueService(DbContext);

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () => await venueService.UpdateAsync(null));
        }

        [Test]
        public void UpdateAsync_WhenIdEqualZero_ShouldThrowValidationException()
        {
            // Arrange
            var venueService = new VenueService(DbContext);

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () => await venueService.UpdateAsync(new VenueDto { Id = 0 }));
        }

        [Test]
        public void UpdateAsync_WhenIdEqualLeesThanZero_ShouldThrowValidationException()
        {
            // Arrange
            var venueService = new VenueService(DbContext);

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () => await venueService.UpdateAsync(new VenueDto { Id = -1 }));
        }

        [Test]
        public void GetAllAsync_WhenVenuesExist_ShouldReturnVenues()
        {
            // Arrange
            var expected = _venueRepository.GetAllAsQueryable();
            var venueService = new VenueService(DbContext);

            // Act
            var actual = venueService.GetAll();

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public async Task GetByIdAsync_WhenVenueExist_ShouldReturnLastVenue()
        {
            // Arrange
            var expected = _venueRepository.GetAllAsQueryable().OrderBy(x=> x.Id).Last();
            var expectedId = expected.Id;
            var venueService = new VenueService(DbContext);

            // Act
            var actual = await venueService.GetByIdAsync(expectedId);

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void GetByIDAsync_WhenIdEqualZero_ShouldThrowValidationException()
        {
            // Arrange
            var venueService = new VenueService(DbContext);

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () => await venueService.GetByIdAsync(0));
        }

        [Test]
        public void GetByIDAsync_WhenIdEqualLeesThanZero_ShouldThrowValidationException()
        {
            // Arrange
            var venueService = new VenueService(DbContext);

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () => await venueService.GetByIdAsync(-1));
        }
    }
}
