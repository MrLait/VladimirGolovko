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
    internal class LayoutTests : TestDatabaseLoader
    {
        private readonly List<Layout> _layouts = new List<Layout>();

        public EfDbContext DbContext { get; set; }

        [OneTimeSetUp]
        public async Task InitLayoutsAsync()
        {
            DbContext = new EfDbContext(connectionString: DefaultConnectionString);
            var repository = new EfRepository<Layout>(DbContext);
            var countAllLayouts = repository.GetAllAsQueryable().OrderBy(x => x.Id).Last().Id;

            for (int i = 1; i <= countAllLayouts; i++)
            {
                _layouts.Add(await repository.GetByIDAsync(i));
            }
        }

        [Test]
        public void GetAllAsQueryable_WhenLayoutsExist_ShouldReturnLayoutList()
        {
            // Arrange
            var expected = _layouts;

            // Act
            var actual = new EfRepository<Layout>(DbContext).GetAllAsQueryable();

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public async Task CreateAsync_WhenAddLayout_ShouldReturnLayoutWithNewLayout()
        {
            // Arrange
            var repository = new EfRepository<Layout>(DbContext);
            Layout layout = new Layout { VenueId = 2, Description = "Created layout" };
            var expected = new List<Layout>(_layouts)
            {
                layout,
            };

            // Act
            await repository.CreateAsync(layout);
            var actual = repository.GetAllAsQueryable();

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void CreateAsync_WhenLayoutEmpty_ShouldThrowArgumentNullException()
        {
            // Arrange
            var repository = new EfRepository<Layout>(DbContext);

            // Act & Assert
            Assert.ThrowsAsync<ArgumentNullException>(async () => await repository.CreateAsync(null));
        }

        [Test]
        public async Task DeleteAsync_WhenExistLayout_ShouldReturnLayoutListWithoutDeletedLayoutAsync()
        {
            // Arrange
            var dbContext = new EfDbContext(connectionString: DefaultConnectionString);
            var repository = new EfRepository<Layout>(dbContext);

            // Act
            var allLayouts = repository.GetAllAsQueryable().ToList();
            var lastLayout = allLayouts.LastOrDefault();
            await repository.DeleteAsync(lastLayout);

            var actual = repository.GetAllAsQueryable();
            int countLayoutWithoutLast = allLayouts.ToList().Count - 1;

            // Assert
            actual.Should().BeEquivalentTo(allLayouts.Take(countLayoutWithoutLast));
        }

        [Test]
        public void DeleteAsync_WhenIdEqualZeroLayout_ShouldThrowInvalidOperationException()
        {
            // Arrange
            var layout = new Layout { Id = 0 };
            var repository = new EfRepository<Layout>(DbContext);

            // Assert
            Assert.ThrowsAsync<InvalidOperationException>(async () => await repository.DeleteAsync(layout));
        }

        [Test]
        public void DeleteAsync_WhenNullLayout_ShouldThrowArgumentNullException()
        {
            // Arrange
            var repository = new EfRepository<Layout>(DbContext);

            // Assert
            Assert.ThrowsAsync<ArgumentNullException>(async () => await repository.DeleteAsync(null));
        }

        [Test]
        public async Task UpdateAsync_WhenExistLayout_ShouldUpdateLastLayoutAsync()
        {
            // Arrange
            var dbContext = new EfDbContext(connectionString: DefaultConnectionString);
            var repository = new EfRepository<Layout>(dbContext);
            var expected = new Layout { VenueId = 2, Description = "Updated Layout" };

            // Act
            var lastLayout = repository.GetAllAsQueryable().OrderBy(x => x.Id).Last();
            var idLastLayout = lastLayout.Id;
            expected.Id = idLastLayout;

            await repository.UpdateAsync(expected);
            var actual = repository.GetAllAsQueryable().OrderBy(x => x.Id).Last();

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void UpdateAsync_WhenNullLayout_ShouldThrowArgumentNullException()
        {
            // Arrange
            var repository = new EfRepository<Layout>(DbContext);

            // Act & Assert
            Assert.ThrowsAsync<ArgumentNullException>(async () => await repository.UpdateAsync(null));
        }

        [Test]
        public void UpdateAsync_WhenIdEqualZeroLayout_ShouldThrowDbUpdateConcurrencyException()
        {
            // Arrange
            var layout = new Layout { Id = 0 };
            var repository = new EfRepository<Layout>(DbContext);

            // Act & Assert
            Assert.ThrowsAsync<DbUpdateConcurrencyException>(async () => await repository.UpdateAsync(layout));
        }

        [Test]
        public async Task GetByIdAsync_WhenExistLayout_ShouldReturnLayoutAsync()
        {
            // Arrange
            var repository = new EfRepository<Layout>(DbContext);

            // Act
            var lastLayout = repository.GetAllAsQueryable().OrderBy(x => x.Id).LastOrDefault();
            Layout expectedLayout = new Layout { Id = lastLayout.Id, VenueId = lastLayout.VenueId, Description = lastLayout.Description };

            int actualId = expectedLayout.Id;
            var actual = await repository.GetByIDAsync(actualId);

            // Assert
            actual.Should().BeEquivalentTo(expectedLayout);
        }

        [Test]
        public async Task GetByIdAsync_WhenNonExistLayout_ShouldReturnNullAsync()
        {
            // Arrange
            Layout expected = null;
            var repository = new EfRepository<Layout>(DbContext);

            // Act
            var lastLayout = repository.GetAllAsQueryable().OrderBy(x => x.Id).Last();
            int nonExistId = lastLayout.Id + 10;
            var actual = await repository.GetByIDAsync(nonExistId);

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void UpdateAsync_WhenIdLessThenZero_ShouldThrowDbUpdateConcurrencyException()
        {
            // Arrange
            var layout = new Layout { Id = -1 };
            var repository = new EfRepository<Layout>(DbContext);

            // Act & Assert
            Assert.ThrowsAsync<DbUpdateConcurrencyException>(async () => await repository.UpdateAsync(layout));
        }

        [Test]
        public void DeleteAsync_WhenIdLessThenZero_ShouldThrowDbUpdateConcurrencyException()
        {
            // Arrange
            var layout = new Layout { Id = -1 };
            var repository = new EfRepository<Layout>(DbContext);

            // Act & Assert
            Assert.ThrowsAsync<DbUpdateConcurrencyException>(async () => await repository.DeleteAsync(layout));
        }
    }
}
