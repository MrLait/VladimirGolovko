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
    internal class VenueServiceTests : TestDatabaseLoader
    {
        private AdoUsingParametersRepository<Venue> _venueRepository;
        private AdoDbContext _adoDbContext;

        [OneTimeSetUp]
        public void InitRepositories()
        {
            _venueRepository = new AdoUsingParametersRepository<Venue>(MainConnectionString);
            _adoDbContext = new AdoDbContext(MainConnectionString);
        }

        [Test]
        public void Create_WhenVenueExist_ShouldReturnCreatedVenue()
        {
            // Arrange
            var expected = new Venue { Id = _venueRepository.GetAll().Last().Id + 1, Address = "Added Address", Description = "Added Description", Phone = "+375293094300" };
            var venueService = new VenueService(_adoDbContext);

            // Act
            venueService.Create(new VenueDto { Address = "Added Address", Description = "Added Description", Phone = "+375293094300" });
            var actual = _venueRepository.GetAll().Last();

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void Create_WhenVenueEmpty_ShouldThrowValidationException()
        {
            // Arrange
            var venueService = new VenueService(_adoDbContext);

            // Act & Assert
            Assert.Throws<ValidationException>(() => venueService.Create(null));
        }

        [Test]
        public void Create_WhenDescriptionExist_ShouldThrowValidationException()
        {
            // Arrange
            var venueDto = new VenueDto { Id = 1, Description = "Luzhniki Stadium", Address = "st. Luzhniki, 24, Moscow, Russia, 119048", Phone = "+7 495 780-08-08" };
            var venueService = new VenueService(_adoDbContext);

            // Act & Assert
            Assert.Throws<ValidationException>(() => venueService.Create(venueDto));
        }

        [Test]
        public void Delete_WhenVenueExist_ShouldDeleteLastVenue()
        {
            // Arrange
            var expected = _venueRepository.GetAll().Last();
            var venueService = new VenueService(_adoDbContext);
            var venueLast = _venueRepository.GetAll().Last();

            // Act
            venueService.Delete(new VenueDto { Id = venueLast.Id, Description = venueLast.Description, Address = venueLast.Address, Phone = venueLast.Phone });
            var actual = _venueRepository.GetAll().Last();

            // Assert
            actual.Should().NotBeEquivalentTo(expected);
        }

        [Test]
        public void Delete_WhenVenueEmpty_ShouldThrowValidationException()
        {
            // Arrange
            var venueService = new VenueService(_adoDbContext);

            // Act & Assert
            Assert.Throws<ValidationException>(() => venueService.Delete(null));
        }

        [Test]
        public void Delete_WhenIdEqualZero_ShouldThrowValidationException()
        {
            // Arrange
            var venueService = new VenueService(_adoDbContext);

            // Act & Assert
            Assert.Throws<ValidationException>(() => venueService.Delete(new VenueDto { Id = 0 }));
        }

        [Test]
        public void Delete_WhenIdEqualLeesThanZero_ShouldThrowValidationException()
        {
            // Arrange
            var venueService = new VenueService(_adoDbContext);

            // Act & Assert
            Assert.Throws<ValidationException>(() => venueService.Delete(new VenueDto { Id = -1 }));
        }

        [Test]
        public void Update_WhenVenueExist_ShouldUpdateLastVenue()
        {
            // Arrange
            var venueLast = _venueRepository.GetAll().Last();
            var expected = new Venue { Id = venueLast.Id, Address = "Added Address", Description = "Added Description", Phone = "+375293094300" };
            var venueService = new VenueService(_adoDbContext);

            // Act
            venueService.Update(new VenueDto { Id = venueLast.Id, Description = expected.Description, Address = expected.Address, Phone = expected.Phone });
            var actual = _venueRepository.GetAll().Last();

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void Update_WhenVenueEmpty_ShouldThrowValidationException()
        {
            // Arrange
            var venueService = new VenueService(_adoDbContext);

            // Act & Assert
            Assert.Throws<ValidationException>(() => venueService.Update(null));
        }

        [Test]
        public void Update_WhenIdEqualZero_ShouldThrowValidationException()
        {
            // Arrange
            var venueService = new VenueService(_adoDbContext);

            // Act & Assert
            Assert.Throws<ValidationException>(() => venueService.Update(new VenueDto { Id = 0 }));
        }

        [Test]
        public void Update_WhenIdEqualLeesThanZero_ShouldThrowValidationException()
        {
            // Arrange
            var venueService = new VenueService(_adoDbContext);

            // Act & Assert
            Assert.Throws<ValidationException>(() => venueService.Update(new VenueDto { Id = -1 }));
        }

        [Test]
        public void GetAll_WhenVenuesExist_ShouldReturnVenues()
        {
            // Arrange
            var expected = _venueRepository.GetAll();
            var venueService = new VenueService(_adoDbContext);

            // Act
            var actual = venueService.GetAll();

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void GetById_WhenVenueExist_ShouldReturnLastVenue()
        {
            // Arrange
            var expected = _venueRepository.GetAll().Last();
            var expectedId = expected.Id;
            var venueService = new VenueService(_adoDbContext);

            // Act
            var actual = venueService.GetByID(expectedId);

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void GetByID_WhenIdEqualZero_ShouldThrowValidationException()
        {
            // Arrange
            var venueService = new VenueService(_adoDbContext);

            // Act & Assert
            Assert.Throws<ValidationException>(() => venueService.GetByID(0));
        }

        [Test]
        public void GetByID_WhenIdEqualLeesThanZero_ShouldThrowValidationException()
        {
            // Arrange
            var venueService = new VenueService(_adoDbContext);

            // Act & Assert
            Assert.Throws<ValidationException>(() => venueService.GetByID(-1));
        }
    }
}
