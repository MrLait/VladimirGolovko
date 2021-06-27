using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using TicketManagement.DataAccess.Domain.Models;
using TicketManagement.Dto;
using TicketManagement.Services.EventFlow.API.Infrastructure.Exceptions;
using TicketManagement.Services.EventFlow.API.Infrastructure.Services;

namespace TicketManagement.UnitTests.BusinessLogic.Services
{
    /// <summary>
    /// Area service tests.
    /// </summary>
    [TestFixture]
    public class AreaServiceTests : MockEntities
    {
        [Test]
        public async Task CreateAsync_WhenAreaExist_ShouldReturnCreatedArea()
        {
            // Arrange
            var expected = new Area { LayoutId = 2, Description = "Created", CoordX = 1, CoordY = 2 };
            Mock.Setup(x => x.Areas.CreateAsync(It.IsAny<Area>())).Callback<Area>(v => Areas.Add(v));
            var areaService = new AreaService(Mock.Object);

            // Act
            await areaService.CreateAsync(new AreaDto { LayoutId = 2, Description = "Created", CoordY = 2, CoordX = 1 });
            var actual = Areas.Last();

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void CreateAsync_WhenAreaEmpty_ShouldThrowValidationException()
        {
            // Arrange
            var areaService = new AreaService(Mock.Object);

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () => await areaService.CreateAsync(null));
        }

        [Test]
        public void CreateAsync_WhenAreaAlreadyExist_ShouldThrowValidationException()
        {
            // Arrange
            var firstArea = Areas.First();
            var areaDto = new AreaDto { Id = firstArea.Id, CoordX = firstArea.CoordX, CoordY = firstArea.CoordY, Description = firstArea.Description, LayoutId = firstArea.LayoutId };
            var areaService = new AreaService(Mock.Object);
            Mock.Setup(x => x.Areas.CreateAsync(It.IsAny<Area>())).Callback<Area>(v => Areas.Add(v));

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () => await areaService.CreateAsync(areaDto));
        }

        [Test]
        public async Task DeleteAsync_WhenAreaExist_ShouldDeleteLastArea()
        {
            // Arrange
            var expected = Areas.Last();
            Mock.Setup(x => x.Areas.DeleteAsync(It.IsAny<Area>())).Callback<Area>(v => Areas.RemoveAt(v.Id - 1));
            var areaService = new AreaService(Mock.Object);

            // Act
            await areaService.DeleteAsync(new AreaDto { Id = expected.Id, LayoutId = expected.LayoutId, Description = expected.Description, CoordY = expected.CoordY, CoordX = expected.CoordX });
            var actual = Areas.Last();

            // Assert
            actual.Should().NotBeEquivalentTo(expected);
        }

        [Test]
        public void DeleteAsync_WhenAreaEmpty_ShouldThrowValidationException()
        {
            // Arrange
            var areaService = new AreaService(Mock.Object);

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () => await areaService.DeleteAsync(null));
        }

        [Test]
        public void DeleteAsync_WhenIdEqualZero_ShouldThrowValidationException()
        {
            // Arrange
            var areaService = new AreaService(Mock.Object);

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () => await areaService.DeleteAsync(new AreaDto { Id = 0 }));
        }

        [Test]
        public void DeleteAsync_WhenIdEqualLeesThanZero_ShouldThrowValidationException()
        {
            // Arrange
            var areaService = new AreaService(Mock.Object);

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () => await areaService.DeleteAsync(new AreaDto { Id = -1 }));
        }

        [Test]
        public async Task UpdateAsync_WhenAreaExist_ShouldUpdateLastArea()
        {
            // Arrange
            var areaLast = Areas.Last();
            var expected = new Area { Id = areaLast.Id, Description = "Updated Description", CoordX = areaLast.CoordX + 1, CoordY = areaLast.CoordY + 1, LayoutId = areaLast.LayoutId };
            var areaService = new AreaService(Mock.Object);

            // Act
            Action<Area> updateLastAction = _ => Areas.RemoveAt(areaLast.Id - 1);
            updateLastAction += v => Areas.Insert(v.Id - 1, v);
            Mock.Setup(x => x.Areas.UpdateAsync(It.IsAny<Area>())).Callback(updateLastAction);

            await areaService.UpdateAsync(new AreaDto { Id = areaLast.Id, LayoutId = expected.LayoutId, CoordY = expected.CoordY, CoordX = expected.CoordX, Description = expected.Description });
            var actual = Areas[areaLast.Id - 1];

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void UpdateAsync_WhenAreaEmpty_ShouldThrowValidationException()
        {
            // Arrange
            var areaService = new AreaService(Mock.Object);

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () => await areaService.UpdateAsync(null));
        }

        [Test]
        public void UpdateAsync_WhenIdEqualZero_ShouldThrowValidationException()
        {
            // Arrange
            var areaService = new AreaService(Mock.Object);

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () => await areaService.UpdateAsync(new AreaDto { Id = 0 }));
        }

        [Test]
        public void UpdateAsync_WhenIdEqualLeesThanZero_ShouldThrowValidationException()
        {
            // Arrange
            var areaService = new AreaService(Mock.Object);

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () => await areaService.UpdateAsync(new AreaDto { Id = -1 }));
        }

        [Test]
        public void UpdateAsync_WhenAreaWithThisDescriptionAlreadyExist_ShouldThrowValidationException()
        {
            // Arrange
            var firstArea = Areas.First();
            var areaDto = new AreaDto { Id = firstArea.Id, Description = firstArea.Description, CoordX = firstArea.CoordX, CoordY = firstArea.CoordY, LayoutId = firstArea.LayoutId };
            var areaService = new AreaService(Mock.Object);
            Mock.Setup(x => x.Areas.UpdateAsync(It.IsAny<Area>())).Callback<Area>(v => Areas.Add(v));

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () => await areaService.UpdateAsync(areaDto));
        }

        [Test]
        public void GetAllAsync_WhenAreasExist_ShouldReturnAreas()
        {
            // Arrange
            var expected = Areas;
            Mock.Setup(x => x.Areas.GetAllAsQueryable()).Returns(Areas.AsQueryable());
            var areaService = new AreaService(Mock.Object);

            // Act
            var actual = areaService.GetAll();

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public async Task GetByIdAsync_WhenAreaExist_ShouldReturnLastArea()
        {
            // Arrange
            var expected = Areas.Last();
            var expectedId = expected.Id - 1;
            Mock.Setup(x => x.Areas.GetByIdAsync(expectedId)).ReturnsAsync(Areas.Last());
            var areaService = new AreaService(Mock.Object);

            // Act
            var actual = await areaService.GetByIdAsync(expectedId);

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void GetByIDAsync_WhenIdEqualZero_ShouldThrowValidationException()
        {
            // Arrange
            var areaService = new AreaService(Mock.Object);

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () => await areaService.GetByIdAsync(0));
        }

        [Test]
        public void GetByIDAsync_WhenIdEqualLeesThanZero_ShouldThrowValidationException()
        {
            // Arrange
            var areaService = new AreaService(Mock.Object);

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () => await areaService.GetByIdAsync(-1));
        }
    }
}
