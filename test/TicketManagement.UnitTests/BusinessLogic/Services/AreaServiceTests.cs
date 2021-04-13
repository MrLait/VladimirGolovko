using System;
using System.Linq;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using TicketManagement.BusinessLogic.Infrastructure;
using TicketManagement.BusinessLogic.Services;
using TicketManagement.DataAccess.Domain.Models;
using TicketManagement.Dto;

namespace TicketManagement.UnitTests.BusinessLogic.Services
{
    /// <summary>
    /// Area service tests.
    /// </summary>
    [TestFixture]
    public class AreaServiceTests : MockEntites
    {
        [Test]
        public void Create_WhenAreaExist_ShouldReturnCreatedArea()
        {
            // Arrange
            var expected = new Area { LayoutId = 2, Description = "Created", CoordX = 1, CoordY = 2 };
            Mock.Setup(x => x.Areas.Create(It.IsAny<Area>())).Callback<Area>(v => Areas.Add(v));
            var areaService = new AreaService(Mock.Object);

            // Act
            areaService.Create(new AreaDto { LayoutId = 2, Description = "Created", CoordY = 2, CoordX = 1 });
            var actual = Areas.Last();

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void Create_WhenAreaEmpty_ShouldThrowValidationException()
        {
            // Arrange
            var areaService = new AreaService(Mock.Object);

            // Act & Assert
            Assert.Throws<ValidationException>(() => areaService.Create(null));
        }

        [Test]
        public void Create_WhenAreaAlreadyExist_ShouldReturnValidationException()
        {
            // Arrange
            var firstArea = Areas.First();
            var areaDto = new AreaDto { Id = firstArea.Id, CoordX = firstArea.CoordX, CoordY = firstArea.CoordY, Description = firstArea.Description, LayoutId = firstArea.LayoutId };
            var areaService = new AreaService(Mock.Object);
            Mock.Setup(x => x.Areas.Create(It.IsAny<Area>())).Callback<Area>(v => Areas.Add(v));

            // Act & Assert
            Assert.Throws<ValidationException>(() => areaService.Create(areaDto));
        }

        [Test]
        public void Delete_WhenAreaExist_ShouldReturnListWithDeletedArea()
        {
            // Arrange
            var expected = Areas.Last();
            Mock.Setup(x => x.Areas.Delete(It.IsAny<Area>())).Callback<Area>(v => Areas.RemoveAt(v.Id - 1));
            var areaService = new AreaService(Mock.Object);

            // Act
            areaService.Delete(new AreaDto { Id = expected.Id, LayoutId = expected.LayoutId, Description = expected.Description, CoordY = expected.CoordY, CoordX = expected.CoordX });
            var actual = Areas.Last();

            // Assert
            actual.Should().NotBeEquivalentTo(expected);
        }

        [Test]
        public void Delete_WhenAreaEmpty_ShouldThrowValidationException()
        {
            // Arrange
            var areaService = new AreaService(Mock.Object);

            // Act & Assert
            Assert.Throws<ValidationException>(() => areaService.Delete(null));
        }

        [Test]
        public void Delete_WhenIdEqualZero_ShouldThrowValidationException()
        {
            // Arrange
            var areaService = new AreaService(Mock.Object);

            // Act & Assert
            Assert.Throws<ValidationException>(() => areaService.Delete(new AreaDto { Id = 0 }));
        }

        [Test]
        public void Delete_WhenIdEqualLeesThanZero_ShouldThrowValidationException()
        {
            // Arrange
            var areaService = new AreaService(Mock.Object);

            // Act & Assert
            Assert.Throws<ValidationException>(() => areaService.Delete(new AreaDto { Id = -1 }));
        }

        [Test]
        public void Update_WhenAreaExist_ShouldReturnListWithUpdatedArea()
        {
            // Arrange
            var areaLast = Areas.Last();
            var expected = new Area { Id = areaLast.Id, Description = "Updated Description", CoordX = areaLast.CoordX + 1, CoordY = areaLast.CoordY + 1, LayoutId = areaLast.LayoutId };
            var areaService = new AreaService(Mock.Object);

            // Act
            Action<Area> updateLastAction = venues => Areas.RemoveAt(areaLast.Id - 1);
            updateLastAction += v => Areas.Insert(v.Id - 1, v);
            Mock.Setup(x => x.Areas.Update(It.IsAny<Area>())).Callback(updateLastAction);

            areaService.Update(new AreaDto { Id = areaLast.Id, LayoutId = expected.LayoutId, CoordY = expected.CoordY, CoordX = expected.CoordX, Description = expected.Description });
            var actual = Areas[areaLast.Id - 1];

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void Update_WhenAreaEmpty_ShouldThrowValidationException()
        {
            // Arrange
            var areaService = new AreaService(Mock.Object);

            // Act & Assert
            Assert.Throws<ValidationException>(() => areaService.Update(null));
        }

        [Test]
        public void Update_WhenIdEqualZero_ShouldThrowValidationException()
        {
            // Arrange
            var areaService = new AreaService(Mock.Object);

            // Act & Assert
            Assert.Throws<ValidationException>(() => areaService.Update(new AreaDto { Id = 0 }));
        }

        [Test]
        public void Update_WhenIdEqualLeesThanZero_ShouldThrowValidationException()
        {
            // Arrange
            var areaService = new AreaService(Mock.Object);

            // Act & Assert
            Assert.Throws<ValidationException>(() => areaService.Update(new AreaDto { Id = -1 }));
        }

        [Test]
        public void Update_WhenAreaWithThisDescriptionAlreadyExist_ShouldReturnValidationException()
        {
            // Arrange
            var firstArea = Areas.First();
            var areaDto = new AreaDto { Id = firstArea.Id, Description = firstArea.Description, CoordX = firstArea.CoordX, CoordY = firstArea.CoordY, LayoutId = firstArea.LayoutId };
            var areaService = new AreaService(Mock.Object);
            Mock.Setup(x => x.Areas.Update(It.IsAny<Area>())).Callback<Area>(v => Areas.Add(v));

            // Act & Assert
            Assert.Throws<ValidationException>(() => areaService.Update(areaDto));
        }
    }
}
