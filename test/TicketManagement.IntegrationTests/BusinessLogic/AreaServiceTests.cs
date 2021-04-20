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

namespace TicketManagement.IntegrationTests.BusinessLogic
{
    [TestFixture]
    internal class AreaServiceTests : TestDatabaseLoader
    {
        private AdoUsingParametersRepository<Area> _areaRepository;
        private AdoDbContext _adoDbContext;

        [OneTimeSetUp]
        public void InitRepositories()
        {
            _areaRepository = new AdoUsingParametersRepository<Area>(MainConnectionString);
            _adoDbContext = new AdoDbContext(MainConnectionString);
        }

        [Test]
        public async Task CreateAsync_WhenAreaExist_ShouldReturnCreatedArea()
        {
            // Arrange
            var expected = new Area { Id = (await _areaRepository.GetAllAsync()).Last().Id + 1, LayoutId = 2, Description = "Created", CoordX = 1, CoordY = 2 };
            var areaService = new AreaService(_adoDbContext);

            // Act
            await areaService.CreateAsync(new AreaDto { LayoutId = 2, Description = "Created", CoordY = 2, CoordX = 1 });
            var actual = (await _areaRepository.GetAllAsync()).Last();

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void CreateAsync_WhenAreaEmpty_ShouldThrowValidationException()
        {
            // Arrange
            var areaService = new AreaService(_adoDbContext);

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () => await areaService.CreateAsync(null));
        }

        [Test]
        public async Task CreateAsync_WhenAreaAlreadyExist_ShouldThrowValidationException()
        {
            // Arrange
            var firstArea = (await _areaRepository.GetAllAsync()).First();
            var areaDto = new AreaDto { Id = firstArea.Id, CoordX = firstArea.CoordX, CoordY = firstArea.CoordY, Description = firstArea.Description, LayoutId = firstArea.LayoutId };
            var areaService = new AreaService(_adoDbContext);

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () => await areaService.CreateAsync(areaDto));
        }

        [Test]
        public async Task DeleteAsync_WhenAreaExist_ShouldDeleteLastArea()
        {
            // Arrange
            var expected = (await _areaRepository.GetAllAsync()).Last();
            var areaService = new AreaService(_adoDbContext);

            // Act
            await areaService.DeleteAsync(new AreaDto { Id = expected.Id, LayoutId = expected.LayoutId, Description = expected.Description, CoordY = expected.CoordY, CoordX = expected.CoordX });
            var actual = (await _areaRepository.GetAllAsync()).Last();

            // Assert
            actual.Should().NotBeEquivalentTo(expected);
        }

        [Test]
        public void DeleteAsync_WhenAreaEmpty_ShouldThrowValidationException()
        {
            // Arrange
            var areaService = new AreaService(_adoDbContext);

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () => await areaService.DeleteAsync(null));
        }

        [Test]
        public void DeleteAsync_WhenIdEqualZero_ShouldThrowValidationException()
        {
            // Arrange
            var areaService = new AreaService(_adoDbContext);

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () => await areaService.DeleteAsync(new AreaDto { Id = 0 }));
        }

        [Test]
        public void DeleteAsync_WhenIdEqualLeesThanZero_ShouldThrowValidationException()
        {
            // Arrange
            var areaService = new AreaService(_adoDbContext);

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () => await areaService.DeleteAsync(new AreaDto { Id = -1 }));
        }

        [Test]
        public async Task UpdateAsync_WhenAreaExist_ShouldUpdateLastArea()
        {
            // Arrange
            var areaLast = (await _areaRepository.GetAllAsync()).Last();
            var expected = new Area { Id = areaLast.Id, Description = "Updated Description", CoordX = areaLast.CoordX + 1, CoordY = areaLast.CoordY + 1, LayoutId = areaLast.LayoutId };
            var areaService = new AreaService(_adoDbContext);

            // Act
            await areaService.UpdateAsync(new AreaDto { Id = areaLast.Id, LayoutId = expected.LayoutId, CoordY = expected.CoordY, CoordX = expected.CoordX, Description = expected.Description });
            var actual = (await _areaRepository.GetAllAsync()).Last();

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void UpdateAsync_WhenAreaEmpty_ShouldThrowValidationException()
        {
            // Arrange
            var areaService = new AreaService(_adoDbContext);

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () => await areaService.UpdateAsync(null));
        }

        [Test]
        public void UpdateAsync_WhenIdEqualZero_ShouldThrowValidationException()
        {
            // Arrange
            var areaService = new AreaService(_adoDbContext);

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () => await areaService.UpdateAsync(new AreaDto { Id = 0 }));
        }

        [Test]
        public void UpdateAsync_WhenIdEqualLeesThanZero_ShouldThrowValidationException()
        {
            // Arrange
            var areaService = new AreaService(_adoDbContext);

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () => await areaService.UpdateAsync(new AreaDto { Id = -1 }));
        }

        [Test]
        public async Task UpdateAsync_WhenAreaWithThisDescriptionAlreadyExist_ShouldThrowValidationException()
        {
            // Arrange
            var firstArea = (await _areaRepository.GetAllAsync()).First();
            var areaDto = new AreaDto { Id = firstArea.Id, Description = firstArea.Description, CoordX = firstArea.CoordX, CoordY = firstArea.CoordY, LayoutId = firstArea.LayoutId };
            var areaService = new AreaService(_adoDbContext);

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () => await areaService.UpdateAsync(areaDto));
        }

        [Test]
        public async Task GetAllAsync_WhenAreasExist_ShouldReturnAreas()
        {
            // Arrange
            var expected = await _areaRepository.GetAllAsync();
            var areaService = new AreaService(_adoDbContext);

            // Act
            var actual = await areaService.GetAllAsync();

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public async Task GetByIdAsync_WhenAreaExist_ShouldReturnLastArea()
        {
            // Arrange
            var expected = (await _areaRepository.GetAllAsync()).Last();
            var expectedId = expected.Id;
            var areaService = new AreaService(_adoDbContext);

            // Act
            var actual = await areaService.GetByIDAsync(expectedId);

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void GetByIDAsync_WhenIdEqualZero_ShouldThrowValidationException()
        {
            // Arrange
            var areaService = new AreaService(_adoDbContext);

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () => await areaService.GetByIDAsync(0));
        }

        [Test]
        public void GetByIDAsync_WhenIdEqualLeesThanZero_ShouldThrowValidationException()
        {
            // Arrange
            var areaService = new AreaService(_adoDbContext);

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () => await areaService.GetByIDAsync(-1));
        }
    }
}
