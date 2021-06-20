using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using TicketManagement.DataAccess.DbContexts;
using TicketManagement.DataAccess.Domain.Models;
using TicketManagement.DataAccess.Repositories.EfRepositories;

namespace TicketManagement.IntegrationTests.DataAccess.Repositories.EfRepository
{
    [TestFixture]
    internal class AreaTests : TestDatabaseLoader
    {
        private readonly List<Area> _areas = new List<Area>();

        public EfDbContext DbContext { get; set; }

        [OneTimeSetUp]
        public async Task InitAreasAsync()
        {
            DbContext = new EfDbContext(connectionString: DefaultConnectionString);
            var repository = new EfRepository<Area>(DbContext);
            var countAllAreas = repository.GetAllAsQueryable().OrderBy(x => x.Id).Last().Id;

            for (int i = 1; i <= countAllAreas; i++)
            {
                _areas.Add(await repository.GetByIDAsync(i));
            }
        }

        [Test]
        public void GetAllAsQueryable_WhenAreasExist_ShouldReturnAreaList()
        {
            // Arrange
            var expected = _areas;

            // Act
            var actual = new EfRepository<Area>(DbContext).GetAllAsQueryable();

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public async Task CreateAsync_WhenAddArea_ShouldReturnAreaWithNewArea()
        {
            // Arrange
            var repository = new EfRepository<Area>(DbContext);
            var area = new Area { LayoutId = 2, Description = "Created Area", CoordX = 1, CoordY = 1 };
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
        public void CreateAsync_WhenAreaEmpty_ShouldThrowArgumentNullException()
        {
            // Arrange
            var repository = new EfRepository<Area>(DbContext);

            // Act & Assert
            Assert.ThrowsAsync<ArgumentNullException>(async () => await repository.CreateAsync(null));
        }

        [Test]
        public async Task DeleteAsync_WhenExistArea_ShouldReturnAreaListWithoutDeletedAreaAsync()
        {
            // Arrange
            var dbContext = new EfDbContext(connectionString: DefaultConnectionString);
            var repository = new EfRepository<Area>(dbContext);

            // Act
            var allAreas = repository.GetAllAsQueryable().ToList();
            var lastArea = allAreas.LastOrDefault();
            await repository.DeleteAsync(lastArea);

            var actual = repository.GetAllAsQueryable();
            int countAreaWithoutLast = allAreas.ToList().Count - 1;

            // Assert
            actual.Should().BeEquivalentTo(allAreas.Take(countAreaWithoutLast));
        }

        [Test]
        public void DeleteAsync_WhenIdEqualZeroArea_ShouldThrowInvalidOperationException()
        {
            // Arrange
            var area = new Area { Id = 0 };
            var repository = new EfRepository<Area>(DbContext);

            // Assert
            Assert.ThrowsAsync<InvalidOperationException>(async () => await repository.DeleteAsync(area));
        }

        [Test]
        public void DeleteAsync_WhenNullArea_ShouldThrowArgumentNullException()
        {
            // Arrange
            var repository = new EfRepository<Area>(DbContext);

            // Assert
            Assert.ThrowsAsync<ArgumentNullException>(async () => await repository.DeleteAsync(null));
        }

        [Test]
        public async Task UpdateAsync_WhenExistArea_ShouldUpdateLastAreaAsync()
        {
            // Arrange
            var dbContext = new EfDbContext(connectionString: DefaultConnectionString);
            var repository = new EfRepository<Area>(dbContext);
            var expected = new Area { LayoutId = 2, Description = "Updated Area", CoordY = 3, CoordX = 4 };

            // Act
            var lastArea = repository.GetAllAsQueryable().OrderBy(x => x.Id).Last();
            var idLastArea = lastArea.Id;
            expected.Id = idLastArea;

            await repository.UpdateAsync(expected);
            var actual = repository.GetAllAsQueryable().OrderBy(x => x.Id).Last();

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void UpdateAsync_WhenNullArea_ShouldThrowArgumentNullException()
        {
            // Arrange
            var repository = new EfRepository<Area>(DbContext);

            // Act & Assert
            Assert.ThrowsAsync<ArgumentNullException>(async () => await repository.UpdateAsync(null));
        }

        [Test]
        public void UpdateAsync_WhenIdEqualZeroArea_ShouldThrowDbUpdateConcurrencyException()
        {
            // Arrange
            var area = new Area { Id = 0 };
            var repository = new EfRepository<Area>(DbContext);

            // Act & Assert
            Assert.ThrowsAsync<DbUpdateConcurrencyException>(async () => await repository.UpdateAsync(area));
        }

        [Test]
        public async Task GetByIdAsync_WhenExistArea_ShouldReturnAreaAsync()
        {
            // Arrange
            var repository = new EfRepository<Area>(DbContext);

            // Act
            var lastArea = repository.GetAllAsQueryable().OrderBy(x => x.Id).LastOrDefault();
            var expectedArea = new Area { Id = lastArea.Id, LayoutId = lastArea.LayoutId, CoordX = lastArea.CoordX, CoordY = lastArea.CoordY, Description = lastArea.Description };

            int actualId = expectedArea.Id;
            var actual = await repository.GetByIDAsync(actualId);

            // Assert
            actual.Should().BeEquivalentTo(expectedArea);
        }

        [Test]
        public async Task GetByIdAsync_WhenNonExistArea_ShouldReturnNullAsync()
        {
            // Arrange
            Area expected = null;
            var repository = new EfRepository<Area>(DbContext);

            // Act
            var lastArea = repository.GetAllAsQueryable().OrderBy(x => x.Id).Last();
            int nonExistId = lastArea.Id + 10;
            var actual = await repository.GetByIDAsync(nonExistId);

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void UpdateAsync_WhenIdLessThenZero_ShouldThrowDbUpdateConcurrencyException()
        {
            // Arrange
            var area = new Area { Id = -1 };
            var repository = new EfRepository<Area>(DbContext);

            // Act & Assert
            Assert.ThrowsAsync<DbUpdateConcurrencyException>(async () => await repository.UpdateAsync(area));
        }

        [Test]
        public void DeleteAsync_WhenIdLessThenZero_ShouldThrowDbUpdateConcurrencyException()
        {
            // Arrange
            var area = new Area { Id = -1 };
            var repository = new EfRepository<Area>(DbContext);

            // Act & Assert
            Assert.ThrowsAsync<DbUpdateConcurrencyException>(async () => await repository.DeleteAsync(area));
        }
    }
}
