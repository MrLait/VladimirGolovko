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
    internal class VenueServiceTests : TestDatabaseLoader
    {
        private AdoUsingParametersRepository<Venue> _venueRepository;
        private AdoDbContext _adoDbContext;

        [OneTimeSetUp]
        public void InitRepositories()
        {
            _venueRepository = new AdoUsingParametersRepository<Venue>(DefaultConnectionString);
            _adoDbContext = new AdoDbContext(DefaultConnectionString);
        }

        [Test]
        public async Task CreateAsync_WhenVenueExist_ShouldReturnCreatedVenue()
        {
            // Arrange
            var expected = new Venue { Id = _venueRepository.GetAllAsQueryable().Last().Id + 1, Address = "Added Address", Description = "Added Description", Phone = "+375293094300" };
            var venueService = new VenueService(_adoDbContext);

            // Act
            await venueService.CreateAsync(new VenueDto { Address = "Added Address", Description = "Added Description", Phone = "+375293094300" });
            var actual = _venueRepository.GetAllAsQueryable().Last();

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void CreateAsync_WhenVenueEmpty_ShouldThrowValidationException()
        {
            // Arrange
            var venueService = new VenueService(_adoDbContext);

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () => await venueService.CreateAsync(null));
        }

        [Test]
        public void CreateAsync_WhenDescriptionExist_ShouldThrowValidationException()
        {
            // Arrange
            var venueDto = new VenueDto { Id = 1, Description = "Luzhniki Stadium", Address = "st. Luzhniki, 24, Moscow, Russia, 119048", Phone = "+7 495 780-08-08" };
            var venueService = new VenueService(_adoDbContext);

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () => await venueService.CreateAsync(venueDto));
        }

        [Test]
        public async Task DeleteAsync_WhenVenueExist_ShouldDeleteLastVenue()
        {
            // Arrange
            var expected = _venueRepository.GetAllAsQueryable().Last();
            var venueService = new VenueService(_adoDbContext);
            var venueLast = _venueRepository.GetAllAsQueryable().Last();

            // Act
            await venueService.DeleteAsync(new VenueDto { Id = venueLast.Id, Description = venueLast.Description, Address = venueLast.Address, Phone = venueLast.Phone });
            var actual = _venueRepository.GetAllAsQueryable().Last();

            // Assert
            actual.Should().NotBeEquivalentTo(expected);
        }

        [Test]
        public void DeleteAsync_WhenVenueEmpty_ShouldThrowValidationException()
        {
            // Arrange
            var venueService = new VenueService(_adoDbContext);

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () => await venueService.DeleteAsync(null));
        }

        [Test]
        public void DeleteAsync_WhenIdEqualZero_ShouldThrowValidationException()
        {
            // Arrange
            var venueService = new VenueService(_adoDbContext);

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () => await venueService.DeleteAsync(new VenueDto { Id = 0 }));
        }

        [Test]
        public void DeleteAsync_WhenIdEqualLeesThanZero_ShouldThrowValidationException()
        {
            // Arrange
            var venueService = new VenueService(_adoDbContext);

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () => await venueService.DeleteAsync(new VenueDto { Id = -1 }));
        }

        [Test]
        public async Task UpdateAsync_WhenVenueExist_ShouldUpdateLastVenue()
        {
            // Arrange
            var venueLast = _venueRepository.GetAllAsQueryable().Last();
            var expected = new Venue { Id = venueLast.Id, Address = "Added Address", Description = "Added Description", Phone = "+375293094300" };
            var venueService = new VenueService(_adoDbContext);

            // Act
            await venueService.UpdateAsync(new VenueDto { Id = venueLast.Id, Description = expected.Description, Address = expected.Address, Phone = expected.Phone });
            var actual = _venueRepository.GetAllAsQueryable().Last();

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void UpdateAsync_WhenVenueEmpty_ShouldThrowValidationException()
        {
            // Arrange
            var venueService = new VenueService(_adoDbContext);

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () => await venueService.UpdateAsync(null));
        }

        [Test]
        public void UpdateAsync_WhenIdEqualZero_ShouldThrowValidationException()
        {
            // Arrange
            var venueService = new VenueService(_adoDbContext);

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () => await venueService.UpdateAsync(new VenueDto { Id = 0 }));
        }

        [Test]
        public void UpdateAsync_WhenIdEqualLeesThanZero_ShouldThrowValidationException()
        {
            // Arrange
            var venueService = new VenueService(_adoDbContext);

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () => await venueService.UpdateAsync(new VenueDto { Id = -1 }));
        }

        [Test]
        public void GetAllAsync_WhenVenuesExist_ShouldReturnVenues()
        {
            // Arrange
            var expected = _venueRepository.GetAllAsQueryable();
            var venueService = new VenueService(_adoDbContext);

            // Act
            var actual = venueService.GetAll();

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public async Task GetByIdAsync_WhenVenueExist_ShouldReturnLastVenue()
        {
            // Arrange
            var expected = _venueRepository.GetAllAsQueryable().Last();
            var expectedId = expected.Id;
            var venueService = new VenueService(_adoDbContext);

            // Act
            var actual = await venueService.GetByIDAsync(expectedId);

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void GetByIDAsync_WhenIdEqualZero_ShouldThrowValidationException()
        {
            // Arrange
            var venueService = new VenueService(_adoDbContext);

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () => await venueService.GetByIDAsync(0));
        }

        [Test]
        public void GetByIDAsync_WhenIdEqualLeesThanZero_ShouldThrowValidationException()
        {
            // Arrange
            var venueService = new VenueService(_adoDbContext);

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () => await venueService.GetByIDAsync(-1));
        }
    }
}
