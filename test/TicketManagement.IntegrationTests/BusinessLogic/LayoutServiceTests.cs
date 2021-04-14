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
    internal class LayoutServiceTests : TestDatabaseLoader
    {
        private AdoUsingParametersRepository<Layout> _layoutRepository;
        private AdoDbContext _adoDbContext;

        [OneTimeSetUp]
        public void InitRepositories()
        {
            _layoutRepository = new AdoUsingParametersRepository<Layout>(MainConnectionString);
            _adoDbContext = new AdoDbContext(MainConnectionString);
        }

        [Test]
        public void Create_WhenLayoutExist_ShouldReturnCreatedLayout()
        {
            // Arrange
            var expected = new Layout { Id = _layoutRepository.GetAll().Last().Id + 1, VenueId = 2, Description = "Created Description" };
            var layoutService = new LayoutService(_adoDbContext);

            // Act
            layoutService.Create(new LayoutDto { VenueId = 2, Description = "Created Description" });
            var actual = _layoutRepository.GetAll().Last();

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void Create_WhenLayoutEmpty_ShouldThrowValidationException()
        {
            // Arrange
            var layoutService = new LayoutService(_adoDbContext);

            // Act & Assert
            Assert.Throws<ValidationException>(() => layoutService.Create(null));
        }

        [Test]
        public void Create_WhenLayoutAlreadyExist_ShouldThrowValidationException()
        {
            // Arrange
            var layoutFirst = _layoutRepository.GetAll().First();
            var layoutDto = new LayoutDto { Id = layoutFirst.Id, Description = layoutFirst.Description, VenueId = layoutFirst.VenueId };
            var layoutService = new LayoutService(_adoDbContext);

            // Act & Assert
            Assert.Throws<ValidationException>(() => layoutService.Create(layoutDto));
        }

        [Test]
        public void Delete_WhenLayoutExist_ShouldDeleteLastLayout()
        {
            // Arrange
            var expected = _layoutRepository.GetAll().Last();
            var layoutService = new LayoutService(_adoDbContext);

            // Act
            layoutService.Delete(new LayoutDto { Id = expected.Id, VenueId = expected.VenueId, Description = expected.Description });
            var actual = _layoutRepository.GetAll().Last();

            // Assert
            actual.Should().NotBeEquivalentTo(expected);
        }

        [Test]
        public void Delete_WhenLayoutEmpty_ShouldThrowValidationException()
        {
            // Arrange
            var layoutService = new LayoutService(_adoDbContext);

            // Act & Assert
            Assert.Throws<ValidationException>(() => layoutService.Delete(null));
        }

        [Test]
        public void Delete_WhenIdEqualZero_ShouldThrowValidationException()
        {
            // Arrange
            var layoutService = new LayoutService(_adoDbContext);

            // Act & Assert
            Assert.Throws<ValidationException>(() => layoutService.Delete(new LayoutDto { Id = 0 }));
        }

        [Test]
        public void Delete_WhenIdEqualLeesThanZero_ShouldThrowValidationException()
        {
            // Arrange
            var layoutService = new LayoutService(_adoDbContext);

            // Act & Assert
            Assert.Throws<ValidationException>(() => layoutService.Delete(new LayoutDto { Id = -1 }));
        }

        [Test]
        public void Update_WhenLayoutExist_ShouldUpdateLastLayout()
        {
            // Arrange
            var layoutLast = _layoutRepository.GetAll().Last();
            var expected = new Layout { Id = layoutLast.Id, Description = "Updated Description", VenueId = layoutLast.VenueId };
            var layoutService = new LayoutService(_adoDbContext);

            // Act
            layoutService.Update(new LayoutDto { Id = layoutLast.Id, VenueId = expected.VenueId, Description = expected.Description });
            var actual = _layoutRepository.GetAll().Last();

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void Update_WhenLayoutEmpty_ShouldThrowValidationException()
        {
            // Arrange
            var layoutService = new LayoutService(_adoDbContext);

            // Act & Assert
            Assert.Throws<ValidationException>(() => layoutService.Update(null));
        }

        [Test]
        public void Update_WhenIdEqualZero_ShouldThrowValidationException()
        {
            // Arrange
            var layoutService = new LayoutService(_adoDbContext);

            // Act & Assert
            Assert.Throws<ValidationException>(() => layoutService.Update(new LayoutDto { Id = 0 }));
        }

        [Test]
        public void Update_WhenIdEqualLeesThanZero_ShouldThrowValidationException()
        {
            // Arrange
            var layoutService = new LayoutService(_adoDbContext);

            // Act & Assert
            Assert.Throws<ValidationException>(() => layoutService.Update(new LayoutDto { Id = -1 }));
        }

        [Test]
        public void Update_WhenLayoutWithThisDescriptionAlreadyExist_ShouldThrowValidationException()
        {
            // Arrange
            var layoutFirst = _layoutRepository.GetAll().First();
            var layoutDto = new LayoutDto { Id = layoutFirst.Id, Description = layoutFirst.Description, VenueId = layoutFirst.VenueId };
            var layoutService = new LayoutService(_adoDbContext);

            // Act & Assert
            Assert.Throws<ValidationException>(() => layoutService.Update(layoutDto));
        }

        [Test]
        public void GetAll_WhenLayoutsExist_ShouldReturnLayouts()
        {
            // Arrange
            var expected = _layoutRepository.GetAll();
            var layoutService = new LayoutService(_adoDbContext);

            // Act
            var actual = layoutService.GetAll();

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void GetById_WhenLayoutExist_ShouldReturnLastLayout()
        {
            // Arrange
            var expected = _layoutRepository.GetAll().Last();
            var expectedId = expected.Id;
            var layoutService = new LayoutService(_adoDbContext);

            // Act
            var actual = layoutService.GetByID(expectedId);

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void GetByID_WhenIdEqualZero_ShouldThrowValidationException()
        {
            // Arrange
            var layoutService = new LayoutService(_adoDbContext);

            // Act & Assert
            Assert.Throws<ValidationException>(() => layoutService.GetByID(0));
        }

        [Test]
        public void GetByID_WhenIdEqualLeesThanZero_ShouldThrowValidationException()
        {
            // Arrange
            var layoutService = new LayoutService(_adoDbContext);

            // Act & Assert
            Assert.Throws<ValidationException>(() => layoutService.GetByID(-1));
        }
    }
}
