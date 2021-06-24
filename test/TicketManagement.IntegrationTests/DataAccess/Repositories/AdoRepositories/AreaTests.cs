using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using TicketManagement.DataAccess.Domain.Models;
using TicketManagement.DataAccess.Repositories.AdoRepositories;

namespace TicketManagement.IntegrationTests.DataAccess.Repositories.AdoRepositories
{
    [TestFixture]
    internal class AreaTests : TestDatabaseLoader
    {
        private readonly List<Area> _areas = new ();

        [OneTimeSetUp]
        public async Task InitAreasAsync()
        {
            var areaRepository = new AdoUsingParametersRepository<Area>(DefaultConnectionString);
            var countAllAreas = areaRepository.GetAllAsQueryable().Last().Id;

            for (var i = 1; i <= countAllAreas; i++)
            {
                _areas.Add(await areaRepository.GetByIdAsync(i));
            }
        }

        [Test]
        public void GetAllAsQueryable_WhenAreasExist_ShouldReturnAreaList()
        {
            // Arrange
            var expected = _areas;

            // Act
            var actual = new AdoUsingParametersRepository<Area>(DefaultConnectionString).GetAllAsQueryable();

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void GetAllAsQueryable_WhenAreasIncorrectConnectionSting_ShouldThrowArgumentException()
        {
            // Arrange
            var actual = new AdoUsingParametersRepository<Area>("Server=myServerAddress;Database=myDataBase;User Id=myUsername;Password=myPassword;");

            // Assert
            Assert.Throws<ArgumentException>(() => actual.GetAllAsQueryable());
        }

        [Test]
        public async Task CreateAsync_WhenAddArea_ShouldReturnAreaWithNewArea()
        {
            // Arrange
            var repository = new AdoUsingParametersRepository<Area>(DefaultConnectionString);
            var area = new Area { Id = repository.GetAllAsQueryable().ToList().Count + 1, LayoutId = 2, Description = "Created Area", CoordX = 1, CoordY = 1 };
            var expected = new List<Area>(_areas)
            {
                area,
            };

            // Act
            await repository.CreateAsync(area);
            var actual = repository.GetAllAsQueryable();

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void CreateAsync_WhenAreaEmpty_ShouldThrowArgumentException()
        {
            // Arrange
            var repository = new AdoUsingParametersRepository<Area>(DefaultConnectionString);

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await repository.CreateAsync(null));
        }

        [Test]
        public async Task DeleteAsync_WhenExistArea_ShouldReturnAreaListWithoutDeletedAreaAsync()
        {
            // Arrange
            var repository = new AdoUsingParametersRepository<Area>(DefaultConnectionString);

            // Act
            var allAreas = repository.GetAllAsQueryable();
            var lastArea = allAreas.Last();
            await repository.DeleteAsync(lastArea);

            var actual = repository.GetAllAsQueryable();
            var countAreaWithoutLast = allAreas.ToList().Count - 1;

            // Assert
            actual.Should().BeEquivalentTo(allAreas.Take(countAreaWithoutLast));
        }

        [Test]
        public void DeleteAsync_WhenIdEqualZeroArea_ShouldThrowArgumentException()
        {
            // Arrange
            var area = new Area { Id = 0 };
            var repository = new AdoUsingParametersRepository<Area>(DefaultConnectionString);

            // Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await repository.DeleteAsync(area));
        }

        [Test]
        public void DeleteAsync_WhenNullArea_ShouldThrowArgumentException()
        {
            // Arrange
            var repository = new AdoUsingParametersRepository<Area>(DefaultConnectionString);

            // Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await repository.DeleteAsync(null));
        }

        [Test]
        public void DeleteAsync_WhenIncorrectConnectionStringArea_ShouldThrowArgumentException()
        {
            // Arrange
            var area = new Area { Id = 3 };
            var repository = new AdoUsingParametersRepository<Area>("Server=myServerAddress;Database=myDataBase;User Id=myUsername;Password=myPassword;");

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await repository.DeleteAsync(area));
        }

        [Test]
        public async Task UpdateAsync_WhenExistArea_ShouldUpdateLastAreaAsync()
        {
            // Arrange
            var repository = new AdoUsingParametersRepository<Area>(DefaultConnectionString);
            var expected = new Area { LayoutId = 2, Description = "Updated Area", CoordY = 3, CoordX = 4 };

            // Act
            var lastArea = repository.GetAllAsQueryable().Last();
            var idLastArea = lastArea.Id;
            expected.Id = idLastArea;

            await repository.UpdateAsync(expected);
            var actual = repository.GetAllAsQueryable().Last();

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void UpdateAsync_WhenNullArea_ShouldThrowArgumentException()
        {
            // Arrange
            var repository = new AdoUsingParametersRepository<Area>(DefaultConnectionString);

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await repository.UpdateAsync(null));
        }

        [Test]
        public void UpdateAsync_WhenIdEqualZeroArea_ShouldThrowArgumentException()
        {
            // Arrange
            var area = new Area { Id = 0 };
            var repository = new AdoUsingParametersRepository<Area>(DefaultConnectionString);

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await repository.UpdateAsync(area));
        }

        [Test]
        public async Task GetByIdAsync_WhenExistArea_ShouldReturnAreaAsync()
        {
            // Arrange
            var repository = new AdoUsingParametersRepository<Area>(DefaultConnectionString);

            // Act
            var lastArea = repository.GetAllAsQueryable().Last();
            var expectedArea = new Area { Id = lastArea.Id, LayoutId = lastArea.LayoutId, CoordX = lastArea.CoordX, CoordY = lastArea.CoordY, Description = lastArea.Description };

            var actualId = expectedArea.Id;
            var actual = await repository.GetByIdAsync(actualId);

            // Assert
            actual.Should().BeEquivalentTo(expectedArea);
        }

        [Test]
        public async Task GetByIdAsync_WhenNonExistArea_ShouldReturnNullAsync()
        {
            // Arrange
            var repository = new AdoUsingParametersRepository<Area>(DefaultConnectionString);

            // Act
            var lastArea = repository.GetAllAsQueryable().Last();
            var nonExistId = lastArea.Id + 1;
            var actual = await repository.GetByIdAsync(nonExistId);

            // Assert
            actual.Should().BeEquivalentTo((Area) null);
        }

        [Test]
        public void GetByIdAsync_WhenIdEqualZeroArea_ShouldThrowArgumentException()
        {
            // Arrange
            var area = new Area { Id = 0 };
            var repository = new AdoUsingParametersRepository<Area>(DefaultConnectionString);

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await repository.GetByIdAsync(area.Id));
        }

        [Test]
        public void GetByIdAsync_WhenIdLessThenZero_ShouldThrowArgumentException()
        {
            // Arrange
            var area = new Area { Id = -1 };
            var repository = new AdoUsingParametersRepository<Area>(DefaultConnectionString);

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await repository.GetByIdAsync(area.Id));
        }

        [Test]
        public void UpdateAsync_WhenIdLessThenZero_ShouldThrowArgumentException()
        {
            // Arrange
            var area = new Area { Id = -1 };
            var repository = new AdoUsingParametersRepository<Area>(DefaultConnectionString);

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await repository.UpdateAsync(area));
        }

        [Test]
        public void DeleteAsync_WhenIdLessThenZero_ShouldThrowArgumentException()
        {
            // Arrange
            var area = new Area { Id = -1 };
            var repository = new AdoUsingParametersRepository<Area>(DefaultConnectionString);

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await repository.DeleteAsync(area));
        }
    }
}
