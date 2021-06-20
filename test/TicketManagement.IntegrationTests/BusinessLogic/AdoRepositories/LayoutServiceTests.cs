using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using TicketManagement.DataAccess.DbContexts;
using TicketManagement.DataAccess.Domain.Models;
using TicketManagement.DataAccess.Repositories.AdoRepositories;
using TicketManagement.Dto;
using TicketManagement.Services.EventFlow.API.Infrastructure.Exceptions;
using TicketManagement.Services.EventFlow.API.Infrastructure.Services;

namespace TicketManagement.IntegrationTests.BusinessLogic.AdoRepositories
{
    [TestFixture]
    internal class LayoutServiceTests : TestDatabaseLoader
    {
        private AdoUsingParametersRepository<Layout> _layoutRepository;
        private AdoDbContext _adoDbContext;

        [OneTimeSetUp]
        public void InitRepositories()
        {
            _layoutRepository = new AdoUsingParametersRepository<Layout>(DefaultConnectionString);
            _adoDbContext = new AdoDbContext(DefaultConnectionString);
        }

        [Test]
        public async Task CreateAsync_WhenLayoutExist_ShouldReturnCreatedLayout()
        {
            // Arrange
            var expected = new Layout { Id = _layoutRepository.GetAllAsQueryable().Last().Id + 1, VenueId = 2, Description = "Created Description" };
            var layoutService = new LayoutService(_adoDbContext);

            // Act
            await layoutService.CreateAsync(new LayoutDto { VenueId = 2, Description = "Created Description" });
            var actual = _layoutRepository.GetAllAsQueryable().Last();

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void CreateAsync_WhenLayoutEmpty_ShouldThrowValidationException()
        {
            // Arrange
            var layoutService = new LayoutService(_adoDbContext);

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () => await layoutService.CreateAsync(null));
        }

        [Test]
        public void CreateAsync_WhenLayoutAlreadyExist_ShouldThrowValidationException()
        {
            // Arrange
            var layoutFirst = _layoutRepository.GetAllAsQueryable().First();
            var layoutDto = new LayoutDto { Id = layoutFirst.Id, Description = layoutFirst.Description, VenueId = layoutFirst.VenueId };
            var layoutService = new LayoutService(_adoDbContext);

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () => await layoutService.CreateAsync(layoutDto));
        }

        [Test]
        public async Task DeleteAsync_WhenLayoutExist_ShouldDeleteLastLayout()
        {
            // Arrange
            var expected = _layoutRepository.GetAllAsQueryable().Last();
            var layoutService = new LayoutService(_adoDbContext);

            // Act
            await layoutService.DeleteAsync(new LayoutDto { Id = expected.Id, VenueId = expected.VenueId, Description = expected.Description });
            var actual = _layoutRepository.GetAllAsQueryable().Last();

            // Assert
            actual.Should().NotBeEquivalentTo(expected);
        }

        [Test]
        public void DeleteAsync_WhenLayoutEmpty_ShouldThrowValidationException()
        {
            // Arrange
            var layoutService = new LayoutService(_adoDbContext);

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () => await layoutService.DeleteAsync(null));
        }

        [Test]
        public void DeleteAsync_WhenIdEqualZero_ShouldThrowValidationException()
        {
            // Arrange
            var layoutService = new LayoutService(_adoDbContext);

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () => await layoutService.DeleteAsync(new LayoutDto { Id = 0 }));
        }

        [Test]
        public void DeleteAsync_WhenIdEqualLeesThanZero_ShouldThrowValidationException()
        {
            // Arrange
            var layoutService = new LayoutService(_adoDbContext);

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () => await layoutService.DeleteAsync(new LayoutDto { Id = -1 }));
        }

        [Test]
        public async Task UpdateAsync_WhenLayoutExist_ShouldUpdateLastLayout()
        {
            // Arrange
            var layoutLast = _layoutRepository.GetAllAsQueryable().Last();
            var expected = new Layout { Id = layoutLast.Id, Description = "Updated Description", VenueId = layoutLast.VenueId };
            var layoutService = new LayoutService(_adoDbContext);

            // Act
            await layoutService.UpdateAsync(new LayoutDto { Id = layoutLast.Id, VenueId = expected.VenueId, Description = expected.Description });
            var actual = _layoutRepository.GetAllAsQueryable().Last();

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void UpdateAsync_WhenLayoutEmpty_ShouldThrowValidationException()
        {
            // Arrange
            var layoutService = new LayoutService(_adoDbContext);

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () => await layoutService.UpdateAsync(null));
        }

        [Test]
        public void UpdateAsync_WhenIdEqualZero_ShouldThrowValidationException()
        {
            // Arrange
            var layoutService = new LayoutService(_adoDbContext);

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () => await layoutService.UpdateAsync(new LayoutDto { Id = 0 }));
        }

        [Test]
        public void UpdateAsync_WhenIdEqualLeesThanZero_ShouldThrowValidationException()
        {
            // Arrange
            var layoutService = new LayoutService(_adoDbContext);

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () => await layoutService.UpdateAsync(new LayoutDto { Id = -1 }));
        }

        [Test]
        public void UpdateAsync_WhenLayoutWithThisDescriptionAlreadyExist_ShouldThrowValidationException()
        {
            // Arrange
            var layoutFirst = _layoutRepository.GetAllAsQueryable().First();
            var layoutDto = new LayoutDto { Id = layoutFirst.Id, Description = layoutFirst.Description, VenueId = layoutFirst.VenueId };
            var layoutService = new LayoutService(_adoDbContext);

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () => await layoutService.UpdateAsync(layoutDto));
        }

        [Test]
        public void GetAllAsync_WhenLayoutsExist_ShouldReturnLayouts()
        {
            // Arrange
            var expected = _layoutRepository.GetAllAsQueryable();
            var layoutService = new LayoutService(_adoDbContext);

            // Act
            var actual = layoutService.GetAll();

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public async Task GetByIdAsync_WhenLayoutExist_ShouldReturnLastLayout()
        {
            // Arrange
            var expected = _layoutRepository.GetAllAsQueryable().Last();
            var expectedId = expected.Id;
            var layoutService = new LayoutService(_adoDbContext);

            // Act
            var actual = await layoutService.GetByIDAsync(expectedId);

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void GetByIDAsync_WhenIdEqualZero_ShouldThrowValidationException()
        {
            // Arrange
            var layoutService = new LayoutService(_adoDbContext);

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () => await layoutService.GetByIDAsync(0));
        }

        [Test]
        public void GetByIDAsync_WhenIdEqualLeesThanZero_ShouldThrowValidationException()
        {
            // Arrange
            var layoutService = new LayoutService(_adoDbContext);

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () => await layoutService.GetByIDAsync(-1));
        }
    }
}
