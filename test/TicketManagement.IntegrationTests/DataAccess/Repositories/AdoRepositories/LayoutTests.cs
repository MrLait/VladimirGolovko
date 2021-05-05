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
        private readonly List<Layout> _layouts = new List<Layout>();

        [OneTimeSetUp]
        public async Task InitLayoutsAsync()
        {
            var layoutRepository = new AdoUsingParametersRepository<Layout>(MainConnectionString);
            var countAllLayouts = (await layoutRepository.GetAllAsync()).Last().Id;

            for (int i = 1; i <= countAllLayouts; i++)
            {
                _layouts.Add(await layoutRepository.GetByIDAsync(i));
            }
        }

        [Test]
        public async Task GetAllAsync_WhenLayoutsExist_ShouldReturnLayoutList()
        {
            // Arrange
            var expected = _layouts;

            // Act
            var actual = await new AdoUsingParametersRepository<Layout>(MainConnectionString).GetAllAsync();

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void GetAllAsync_WhenLayoutsIncorrectConnectionSting_ShouldThrowArgumentException()
        {
            // Arrange
            var actual = new AdoUsingParametersRepository<Layout>("Server=myServerAddress;Database=myDataBase;User Id=myUsername;Password=myPassword;");

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await actual.GetAllAsync());
        }

        [Test]
        public async Task CreateAsync_WhenAddLayout_ShouldReturnLayoutWithNewLayout()
        {
            // Arrange
            var repository = new AdoUsingParametersRepository<Layout>(MainConnectionString);
            Layout layout = new Layout { Id = (await repository.GetAllAsync()).ToList().Count + 1, VenueId = 2, Description = "Created layout" };
            List<Layout> expected = new List<Layout>(_layouts)
            {
                layout,
            };

            // Act
            await repository.CreateAsync(layout);
            var actual = await repository.GetAllAsync();

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void CreateAsync_WhenLayoutEmpty_ShouldThrowArgumentException()
        {
            // Arrange
            var repository = new AdoUsingParametersRepository<Layout>(MainConnectionString);

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await repository.CreateAsync(null));
        }

        [Test]
        public async Task DeleteAsync_WhenExistLayout_ShouldReturnLayoutListWithoutDeletedLayout()
        {
            // Arrange
            var repository = new AdoUsingParametersRepository<Layout>(MainConnectionString);

            // Act
            var allLayouts = await repository.GetAllAsync();
            var lastLayout = allLayouts.Last();
            await repository.DeleteAsync(lastLayout);

            var actual = await repository.GetAllAsync();
            int countLayoutWithoutLast = allLayouts.ToList().Count - 1;

            // Assert
            actual.Should().BeEquivalentTo(allLayouts.Take(countLayoutWithoutLast));
        }

        [Test]
        public void DeleteAsync_WhenIdEqualZeroLayout_ShouldThrowArgumentException()
        {
            // Arrange
            Layout layout = new Layout { Id = 0 };
            var repository = new AdoUsingParametersRepository<Layout>(MainConnectionString);

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await repository.DeleteAsync(layout));
        }

        [Test]
        public void DeleteAsync_WhenNullLayout_ShouldThrowArgumentException()
        {
            // Arrange
            var repository = new AdoUsingParametersRepository<Layout>(MainConnectionString);

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await repository.DeleteAsync(null));
        }

        [Test]
        public void DeleteAsync_WhenIncorrectConnectionStringLayout_ShouldThrowArgumentException()
        {
            // Arrange
            Layout layout = new Layout { Id = 3 };
            var repository = new AdoUsingParametersRepository<Layout>("Server=myServerAddress;Database=myDataBase;User Id=myUsername;Password=myPassword;");

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await repository.DeleteAsync(layout));
        }

        [Test]
        public async Task UpdateAsync_WhenExistLayout_ShouldUpdateLastLayout()
        {
            // Arrange
            var repository = new AdoUsingParametersRepository<Layout>(MainConnectionString);
            var expected = new Layout { VenueId = 2, Description = "Updated Layout" };

            // Act
            var lastLayout = (await repository.GetAllAsync()).Last();
            var idLastLayout = lastLayout.Id;
            expected.Id = idLastLayout;

            await repository.UpdateAsync(expected);
            var actual = (await repository.GetAllAsync()).Last();

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void UpdateAsync_WhenNullLayout_ShouldThrowArgumentException()
        {
            // Arrange
            var repository = new AdoUsingParametersRepository<Layout>(MainConnectionString);

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await repository.UpdateAsync(null));
        }

        [Test]
        public void UpdateAsync_WhenIdEqualZeroLayout_ShouldThrowArgumentException()
        {
            // Arrange
            Layout layout = new Layout { Id = 0 };
            var repository = new AdoUsingParametersRepository<Layout>(MainConnectionString);

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await repository.UpdateAsync(layout));
        }

        [Test]
        public async Task GetByIdAsync_WhenExistLayout_ShouldReturnLayout()
        {
            // Arrange
            var repository = new AdoUsingParametersRepository<Layout>(MainConnectionString);

            // Act
            var lastLayout = (await repository.GetAllAsync()).Last();
            Layout expectedLayout = new Layout { Id = lastLayout.Id,  VenueId = lastLayout.VenueId, Description = lastLayout.Description };

            int actualId = expectedLayout.Id;
            var actual = await repository.GetByIDAsync(actualId);

            // Assert
            actual.Should().BeEquivalentTo(expectedLayout);
        }

        [Test]
        public async Task GetByIdAsync_WhenNonExistLayout_ShouldReturnNull()
        {
            // Arrange
            Layout expected = null;
            var repository = new AdoUsingParametersRepository<Layout>(MainConnectionString);

            // Act
            var lastLayout = (await repository.GetAllAsync()).Last();
            int nonExistId = lastLayout.Id + 1;
            var actual = await repository.GetByIDAsync(nonExistId);

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void GetByIdAsync_WhenIdEqualZeroLayout_ShouldThrowArgumentException()
        {
            // Arrange
            Layout layout = new Layout { Id = 0 };
            var repository = new AdoUsingParametersRepository<Layout>(MainConnectionString);

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await repository.GetByIDAsync(layout.Id));
        }

        [Test]
        public void GetByIdAsync_WhenIdLessThenZero_ShouldThrowArgumentException()
        {
            // Arrange
            Layout layout = new Layout { Id = -1 };
            var repository = new AdoUsingParametersRepository<Layout>(MainConnectionString);

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await repository.GetByIDAsync(layout.Id));
        }

        [Test]
        public void UpdateAsync_WhenIdLessThenZero_ShouldThrowArgumentException()
        {
            // Arrange
            Layout layout = new Layout { Id = -1 };
            var repository = new AdoUsingParametersRepository<Layout>(MainConnectionString);

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await repository.UpdateAsync(layout));
        }

        [Test]
        public void DeleteAsync_WhenIdLessThenZero_ShouldThrowArgumentException()
        {
            // Arrange
            Layout layout = new Layout { Id = -1 };
            var repository = new AdoUsingParametersRepository<Layout>(MainConnectionString);

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await repository.DeleteAsync(layout));
        }
    }
}
