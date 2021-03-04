using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using TicketManagement.DataAccess.Domain.Models;
using TicketManagement.DataAccess.Repositories.AdoRepositories;

namespace TicketManagement.IntegrationTests.DataAccess.Repositories.AdoRepositories
{
    [TestFixture]
    internal class AreaTests : AdoRepositoryTests
    {
        private readonly List<Area> _areas = new List<Area>();

        [OneTimeSetUp]
        public void InitAreas()
        {
            var areaRepository = new AdoUsingParametersRepository<Area>(MainConnectionString);
            var coutAllAreas = areaRepository.GetAll().Last().Id;

            for (int i = 1; i <= coutAllAreas; i++)
            {
                _areas.Add(areaRepository.GetByID(i));
            }
        }

        [Test]
        public void GivenGetAll_WhenAreasExist_ShouldReturnAreaList()
        {
            // Arrange
            var expected = _areas;

            // Act
            var actual = new AdoUsingParametersRepository<Area>(MainConnectionString).GetAll();

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void GivenGetAll_WhenAreasIncorrectConnectionSting_ShouldReturnArgumentException()
        {
            // Arrange
            var actual = new AdoUsingParametersRepository<Area>("Server=myServerAddress;Database=myDataBase;User Id=myUsername;Password=myPassword;");

            // Act
            TestDelegate testAction = () => actual.GetAll();

            // Assert
            Assert.Throws<ArgumentException>(testAction);
        }

        [Test]
        public void GivenCreate_WhenAddArea_ShouldReturnAreaWithNewArea()
        {
            // Arrange
            var repository = new AdoUsingParametersRepository<Area>(MainConnectionString);
            Area area = new Area { Id = repository.GetAll().ToList().Count + 1, LayoutId = 2, Description = "Created Area", CoordX = 1, CoordY = 1 };
            List<Area> expected = new List<Area>(_areas);

            // Act
            expected.Add(area);
            repository.Create(area);
            var actual = repository.GetAll();

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void GivenCreate_WhenAreaEmpty_ShouldReturnArgumentException()
        {
            // Arrange
            var repository = new AdoUsingParametersRepository<Area>(MainConnectionString);

            // Act
            TestDelegate testAction = () => repository.Create(null);

            // Assert
            Assert.Throws<ArgumentException>(testAction);
        }

        [Test]
        public void GivenDelete_WhenExistArea_ShouldReturnAreaListWithoutDeletedArea()
        {
            // Arrange
            var repository = new AdoUsingParametersRepository<Area>(MainConnectionString);

            // Act
            var allAreas = repository.GetAll();
            var lastArea = allAreas.Last();
            repository.Delete(lastArea);

            var actual = repository.GetAll();
            int countAreaWithoutLast = allAreas.ToList().Count - 1;

            // Assert
            actual.Should().BeEquivalentTo(allAreas.Take(countAreaWithoutLast));
        }

        [Test]
        public void GivenDelete_WhenIdEqualZeroArea_ShouldReturnArgumentException()
        {
            // Arrange
            Area area = new Area { Id = 0 };
            var repository = new AdoUsingParametersRepository<Area>(MainConnectionString);

            // Act
            TestDelegate testAction = () => repository.Delete(area);

            // Assert
            Assert.Throws<ArgumentException>(testAction);
        }

        [Test]
        public void GivenDelete_WhenNullArea_ShouldReturnArgumentException()
        {
            // Arrange
            var repository = new AdoUsingParametersRepository<Area>(MainConnectionString);

            // Act
            TestDelegate testAction = () => repository.Delete(null);

            // Assert
            Assert.Throws<ArgumentException>(testAction);
        }

        [Test]
        public void GivenDelete_WhenIncorrectConnectionStringArea_ShouldReturnArgumentException()
        {
            // Arrange
            Area area = new Area { Id = 3 };
            var repository = new AdoUsingParametersRepository<Area>("Server=myServerAddress;Database=myDataBase;User Id=myUsername;Password=myPassword;");

            // Act
            TestDelegate testAction = () => repository.Delete(area);

            // Assert
            Assert.Throws<ArgumentException>(testAction);
        }

        [Test]
        public void GivenUpdate_WhenExistArea_ShouldReturnListWithUpdateArea()
        {
            // Arrange
            var repository = new AdoUsingParametersRepository<Area>(MainConnectionString);
            var expected = new Area { LayoutId = 2, Description = "Updated Area", CoordY = 3, CoordX = 4 };

            // Act
            var lastArea = repository.GetAll().Last();
            var idLastArea = lastArea.Id;
            expected.Id = idLastArea;

            repository.Update(expected);
            var actual = repository.GetAll().Last();

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void GivenUpdate_WhenNullArea_ShouldReturnArgumentException()
        {
            // Arrange
            var repository = new AdoUsingParametersRepository<Area>(MainConnectionString);

            // Act
            TestDelegate testAction = () => repository.Update(null);

            // Assert
            Assert.Throws<ArgumentException>(testAction);
        }

        [Test]
        public void GivenUpdate_WhenIdEqualZeroArea_ShouldReturnArgumentException()
        {
            // Arrange
            Area area = new Area { Id = 0 };
            var repository = new AdoUsingParametersRepository<Area>(MainConnectionString);

            // Act
            TestDelegate testAction = () => repository.Update(area);

            // Assert
            Assert.Throws<ArgumentException>(testAction);
        }

        [Test]
        public void GivenGetById_WhenExistArea_ShouldReturnArea()
        {
            // Arrange
            var repository = new AdoUsingParametersRepository<Area>(MainConnectionString);

            // Act
            var lastArea = repository.GetAll().Last();
            Area expectedArea = new Area { Id = lastArea.Id, LayoutId = lastArea.LayoutId, CoordX = lastArea.CoordX, CoordY = lastArea.CoordY, Description = lastArea.Description };

            int actualId = expectedArea.Id;
            var actual = repository.GetByID(actualId);

            // Assert
            actual.Should().BeEquivalentTo(expectedArea);
        }

        [Test]
        public void GivenGetById_WhenNonExistArea_ShouldReturnNull()
        {
            // Arrange
            Area expected = null;
            var repository = new AdoUsingParametersRepository<Area>(MainConnectionString);

            // Act
            var lastArea = repository.GetAll().Last();
            int nonExistId = lastArea.Id + 1;
            var actual = repository.GetByID(nonExistId);

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void GivenGetById_WhenIdEqualZeroArea_ShouldReturnArgumentException()
        {
            // Arrange
            Area area = new Area { Id = 0 };
            var repository = new AdoUsingParametersRepository<Area>(MainConnectionString);

            // Act
            TestDelegate testAction = () => repository.GetByID(area.Id);

            // Assert
            Assert.Throws<ArgumentException>(testAction);
        }
    }
}
