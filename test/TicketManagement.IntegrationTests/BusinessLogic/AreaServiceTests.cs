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
        public void Create_WhenAreaExist_ShouldReturnCreatedArea()
        {
            // Arrange
            var expected = new Area { Id = _areaRepository.GetAll().Last().Id + 1, LayoutId = 2, Description = "Created", CoordX = 1, CoordY = 2 };
            var areaService = new AreaService(_adoDbContext);

            // Act
            areaService.Create(new AreaDto { LayoutId = 2, Description = "Created", CoordY = 2, CoordX = 1 });
            var actual = _areaRepository.GetAll().Last();

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void Create_WhenAreaEmpty_ShouldThrowValidationException()
        {
            // Arrange
            var areaService = new AreaService(_adoDbContext);

            // Act & Assert
            Assert.Throws<ValidationException>(() => areaService.Create(null));
        }

        [Test]
        public void Create_WhenAreaAlreadyExist_ShouldThrowValidationException()
        {
            // Arrange
            var firstArea = _areaRepository.GetAll().First();
            var areaDto = new AreaDto { Id = firstArea.Id, CoordX = firstArea.CoordX, CoordY = firstArea.CoordY, Description = firstArea.Description, LayoutId = firstArea.LayoutId };
            var areaService = new AreaService(_adoDbContext);

            // Act & Assert
            Assert.Throws<ValidationException>(() => areaService.Create(areaDto));
        }

        [Test]
        public void Delete_WhenAreaExist_ShouldDeleteLastArea()
        {
            // Arrange
            var expected = _areaRepository.GetAll().Last();
            var areaService = new AreaService(_adoDbContext);

            // Act
            areaService.Delete(new AreaDto { Id = expected.Id, LayoutId = expected.LayoutId, Description = expected.Description, CoordY = expected.CoordY, CoordX = expected.CoordX });
            var actual = _areaRepository.GetAll().Last();

            // Assert
            actual.Should().NotBeEquivalentTo(expected);
        }

        [Test]
        public void Delete_WhenAreaEmpty_ShouldThrowValidationException()
        {
            // Arrange
            var areaService = new AreaService(_adoDbContext);

            // Act & Assert
            Assert.Throws<ValidationException>(() => areaService.Delete(null));
        }

        [Test]
        public void Delete_WhenIdEqualZero_ShouldThrowValidationException()
        {
            // Arrange
            var areaService = new AreaService(_adoDbContext);

            // Act & Assert
            Assert.Throws<ValidationException>(() => areaService.Delete(new AreaDto { Id = 0 }));
        }

        [Test]
        public void Delete_WhenIdEqualLeesThanZero_ShouldThrowValidationException()
        {
            // Arrange
            var areaService = new AreaService(_adoDbContext);

            // Act & Assert
            Assert.Throws<ValidationException>(() => areaService.Delete(new AreaDto { Id = -1 }));
        }

        [Test]
        public void Update_WhenAreaExist_ShouldUpdateLastArea()
        {
            // Arrange
            var areaLast = _areaRepository.GetAll().Last();
            var expected = new Area { Id = areaLast.Id, Description = "Updated Description", CoordX = areaLast.CoordX + 1, CoordY = areaLast.CoordY + 1, LayoutId = areaLast.LayoutId };
            var areaService = new AreaService(_adoDbContext);

            // Act
            areaService.Update(new AreaDto { Id = areaLast.Id, LayoutId = expected.LayoutId, CoordY = expected.CoordY, CoordX = expected.CoordX, Description = expected.Description });
            var actual = _areaRepository.GetAll().Last();

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void Update_WhenAreaEmpty_ShouldThrowValidationException()
        {
            // Arrange
            var areaService = new AreaService(_adoDbContext);

            // Act & Assert
            Assert.Throws<ValidationException>(() => areaService.Update(null));
        }

        [Test]
        public void Update_WhenIdEqualZero_ShouldThrowValidationException()
        {
            // Arrange
            var areaService = new AreaService(_adoDbContext);

            // Act & Assert
            Assert.Throws<ValidationException>(() => areaService.Update(new AreaDto { Id = 0 }));
        }

        [Test]
        public void Update_WhenIdEqualLeesThanZero_ShouldThrowValidationException()
        {
            // Arrange
            var areaService = new AreaService(_adoDbContext);

            // Act & Assert
            Assert.Throws<ValidationException>(() => areaService.Update(new AreaDto { Id = -1 }));
        }

        [Test]
        public void Update_WhenAreaWithThisDescriptionAlreadyExist_ShouldThrowValidationException()
        {
            // Arrange
            var firstArea = _areaRepository.GetAll().First();
            var areaDto = new AreaDto { Id = firstArea.Id, Description = firstArea.Description, CoordX = firstArea.CoordX, CoordY = firstArea.CoordY, LayoutId = firstArea.LayoutId };
            var areaService = new AreaService(_adoDbContext);

            // Act & Assert
            Assert.Throws<ValidationException>(() => areaService.Update(areaDto));
        }

        [Test]
        public void GetAll_WhenAreasExist_ShouldReturnAreas()
        {
            // Arrange
            var expected = _areaRepository.GetAll();
            var areaService = new AreaService(_adoDbContext);

            // Act
            var actual = areaService.GetAll();

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void GetById_WhenAreaExist_ShouldReturnLastArea()
        {
            // Arrange
            var expected = _areaRepository.GetAll().Last();
            var expectedId = expected.Id;
            var areaService = new AreaService(_adoDbContext);

            // Act
            var actual = areaService.GetByID(expectedId);

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void GetByID_WhenIdEqualZero_ShouldThrowValidationException()
        {
            // Arrange
            var areaService = new AreaService(_adoDbContext);

            // Act & Assert
            Assert.Throws<ValidationException>(() => areaService.GetByID(0));
        }

        [Test]
        public void GetByID_WhenIdEqualLeesThanZero_ShouldThrowValidationException()
        {
            // Arrange
            var areaService = new AreaService(_adoDbContext);

            // Act & Assert
            Assert.Throws<ValidationException>(() => areaService.GetByID(-1));
        }
    }
}
