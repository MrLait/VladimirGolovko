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
    internal class LayoutTests : TestDatabaseLoader
    {
        private readonly List<Layout> _layouts = new ();

        [OneTimeSetUp]
        public async Task InitLayoutsAsync()
        {
            var layoutRepository = new AdoUsingParametersRepository<Layout>(DefaultConnectionString);
            var countAllLayouts = layoutRepository.GetAllAsQueryable().Last().Id;

            for (var i = 1; i <= countAllLayouts; i++)
            {
                _layouts.Add(await layoutRepository.GetByIdAsync(i));
            }
        }

        [Test]
        public void GetAllAsQueryable_WhenLayoutsExist_ShouldReturnLayoutList()
        {
            // Arrange
            var expected = _layouts;

            // Act
            var actual = new AdoUsingParametersRepository<Layout>(DefaultConnectionString).GetAllAsQueryable();

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void GetAllAsQueryable_WhenLayoutsIncorrectConnectionSting_ShouldThrowArgumentException()
        {
            // Arrange
            var actual = new AdoUsingParametersRepository<Layout>("Server=myServerAddress;Database=myDataBase;User Id=myUsername;Password=myPassword;");

            // Act & Assert
            Assert.Throws<ArgumentException>(() => actual.GetAllAsQueryable());
        }

        [Test]
        public async Task CreateAsync_WhenAddLayout_ShouldReturnLayoutWithNewLayout()
        {
            // Arrange
            var repository = new AdoUsingParametersRepository<Layout>(DefaultConnectionString);
            var layout = new Layout { Id = repository.GetAllAsQueryable().ToList().Count + 1, VenueId = 2, Description = "Created layout" };
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
        public void CreateAsync_WhenLayoutEmpty_ShouldThrowArgumentException()
        {
            // Arrange
            var repository = new AdoUsingParametersRepository<Layout>(DefaultConnectionString);

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await repository.CreateAsync(null));
        }

        [Test]
        public async Task DeleteAsync_WhenExistLayout_ShouldReturnLayoutListWithoutDeletedLayout()
        {
            // Arrange
            var repository = new AdoUsingParametersRepository<Layout>(DefaultConnectionString);

            // Act
            var allLayouts = repository.GetAllAsQueryable();
            var lastLayout = allLayouts.Last();
            await repository.DeleteAsync(lastLayout);

            var actual = repository.GetAllAsQueryable();
            var countLayoutWithoutLast = allLayouts.ToList().Count - 1;

            // Assert
            actual.Should().BeEquivalentTo(allLayouts.Take(countLayoutWithoutLast));
        }

        [Test]
        public void DeleteAsync_WhenIdEqualZeroLayout_ShouldThrowArgumentException()
        {
            // Arrange
            var layout = new Layout { Id = 0 };
            var repository = new AdoUsingParametersRepository<Layout>(DefaultConnectionString);

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await repository.DeleteAsync(layout));
        }

        [Test]
        public void DeleteAsync_WhenNullLayout_ShouldThrowArgumentException()
        {
            // Arrange
            var repository = new AdoUsingParametersRepository<Layout>(DefaultConnectionString);

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await repository.DeleteAsync(null));
        }

        [Test]
        public void DeleteAsync_WhenIncorrectConnectionStringLayout_ShouldThrowArgumentException()
        {
            // Arrange
            var layout = new Layout { Id = 3 };
            var repository = new AdoUsingParametersRepository<Layout>("Server=myServerAddress;Database=myDataBase;User Id=myUsername;Password=myPassword;");

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await repository.DeleteAsync(layout));
        }

        [Test]
        public async Task UpdateAsync_WhenExistLayout_ShouldUpdateLastLayout()
        {
            // Arrange
            var repository = new AdoUsingParametersRepository<Layout>(DefaultConnectionString);
            var expected = new Layout { VenueId = 2, Description = "Updated Layout" };

            // Act
            var lastLayout = repository.GetAllAsQueryable().Last();
            var idLastLayout = lastLayout.Id;
            expected.Id = idLastLayout;

            await repository.UpdateAsync(expected);
            var actual = repository.GetAllAsQueryable().Last();

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void UpdateAsync_WhenNullLayout_ShouldThrowArgumentException()
        {
            // Arrange
            var repository = new AdoUsingParametersRepository<Layout>(DefaultConnectionString);

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await repository.UpdateAsync(null));
        }

        [Test]
        public void UpdateAsync_WhenIdEqualZeroLayout_ShouldThrowArgumentException()
        {
            // Arrange
            var layout = new Layout { Id = 0 };
            var repository = new AdoUsingParametersRepository<Layout>(DefaultConnectionString);

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await repository.UpdateAsync(layout));
        }

        [Test]
        public async Task GetByIdAsync_WhenExistLayout_ShouldReturnLayout()
        {
            // Arrange
            var repository = new AdoUsingParametersRepository<Layout>(DefaultConnectionString);

            // Act
            var lastLayout = repository.GetAllAsQueryable().Last();
            var expectedLayout = new Layout { Id = lastLayout.Id, VenueId = lastLayout.VenueId, Description = lastLayout.Description };

            var actualId = expectedLayout.Id;
            var actual = await repository.GetByIdAsync(actualId);

            // Assert
            actual.Should().BeEquivalentTo(expectedLayout);
        }

        [Test]
        public async Task GetByIdAsync_WhenNonExistLayout_ShouldReturnNull()
        {
            // Arrange
            var repository = new AdoUsingParametersRepository<Layout>(DefaultConnectionString);

            // Act
            var lastLayout = repository.GetAllAsQueryable().Last();
            var nonExistId = lastLayout.Id + 1;
            var actual = await repository.GetByIdAsync(nonExistId);

            // Assert
            actual.Should().BeEquivalentTo((Layout) null);
        }

        [Test]
        public void GetByIdAsync_WhenIdEqualZeroLayout_ShouldThrowArgumentException()
        {
            // Arrange
            var layout = new Layout { Id = 0 };
            var repository = new AdoUsingParametersRepository<Layout>(DefaultConnectionString);

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await repository.GetByIdAsync(layout.Id));
        }

        [Test]
        public void GetByIdAsync_WhenIdLessThenZero_ShouldThrowArgumentException()
        {
            // Arrange
            var layout = new Layout { Id = -1 };
            var repository = new AdoUsingParametersRepository<Layout>(DefaultConnectionString);

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await repository.GetByIdAsync(layout.Id));
        }

        [Test]
        public void UpdateAsync_WhenIdLessThenZero_ShouldThrowArgumentException()
        {
            // Arrange
            var layout = new Layout { Id = -1 };
            var repository = new AdoUsingParametersRepository<Layout>(DefaultConnectionString);

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await repository.UpdateAsync(layout));
        }

        [Test]
        public void DeleteAsync_WhenIdLessThenZero_ShouldThrowArgumentException()
        {
            // Arrange
            var layout = new Layout { Id = -1 };
            var repository = new AdoUsingParametersRepository<Layout>(DefaultConnectionString);

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await repository.DeleteAsync(layout));
        }
    }
}
