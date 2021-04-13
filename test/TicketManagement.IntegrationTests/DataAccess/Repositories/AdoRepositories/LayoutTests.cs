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
    internal class LayoutTests : TestDatabaseLoader
    {
        private readonly List<Layout> _layouts = new List<Layout>();

        [OneTimeSetUp]
        public void InitLayouts()
        {
            var layoutRepository = new AdoUsingParametersRepository<Layout>(MainConnectionString);
            var countAllLayouts = layoutRepository.GetAll().Last().Id;

            for (int i = 1; i <= countAllLayouts; i++)
            {
                _layouts.Add(layoutRepository.GetByID(i));
            }
        }

        [Test]
        public void GetAll_WhenLayoutsExist_ShouldReturnLayoutList()
        {
            // Arrange
            var expected = _layouts;

            // Act
            var actual = new AdoUsingParametersRepository<Layout>(MainConnectionString).GetAll();

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void GetAll_WhenLayoutsIncorrectConnectionSting_ShouldThrowArgumentException()
        {
            // Arrange
            var actual = new AdoUsingParametersRepository<Layout>("Server=myServerAddress;Database=myDataBase;User Id=myUsername;Password=myPassword;");

            // Act & Assert
            Assert.Throws<ArgumentException>(() => actual.GetAll());
        }

        [Test]
        public void Create_WhenAddLayout_ShouldReturnLayoutWithNewLayout()
        {
            // Arrange
            var repository = new AdoUsingParametersRepository<Layout>(MainConnectionString);
            Layout layout = new Layout { Id = repository.GetAll().ToList().Count + 1, VenueId = 2, Description = "Created layout" };
            List<Layout> expected = new List<Layout>(_layouts)
            {
                layout,
            };

            // Act
            repository.Create(layout);
            var actual = repository.GetAll();

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void Create_WhenLayoutEmpty_ShouldThrowArgumentException()
        {
            // Arrange
            var repository = new AdoUsingParametersRepository<Layout>(MainConnectionString);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => repository.Create(null));
        }

        [Test]
        public void Delete_WhenExistLayout_ShouldReturnLayoutListWithoutDeletedLayout()
        {
            // Arrange
            var repository = new AdoUsingParametersRepository<Layout>(MainConnectionString);

            // Act
            var allLayouts = repository.GetAll();
            var lastLayout = allLayouts.Last();
            repository.Delete(lastLayout);

            var actual = repository.GetAll();
            int countLayoutWithoutLast = allLayouts.ToList().Count - 1;

            // Assert
            actual.Should().BeEquivalentTo(allLayouts.Take(countLayoutWithoutLast));
        }

        [Test]
        public void Delete_WhenIdEqualZeroLayout_ShouldThrowArgumentException()
        {
            // Arrange
            Layout layout = new Layout { Id = 0 };
            var repository = new AdoUsingParametersRepository<Layout>(MainConnectionString);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => repository.Delete(layout));
        }

        [Test]
        public void Delete_WhenNullLayout_ShouldThrowArgumentException()
        {
            // Arrange
            var repository = new AdoUsingParametersRepository<Layout>(MainConnectionString);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => repository.Delete(null));
        }

        [Test]
        public void Delete_WhenIncorrectConnectionStringLayout_ShouldThrowArgumentException()
        {
            // Arrange
            Layout layout = new Layout { Id = 3 };
            var repository = new AdoUsingParametersRepository<Layout>("Server=myServerAddress;Database=myDataBase;User Id=myUsername;Password=myPassword;");

            // Act & Assert
            Assert.Throws<ArgumentException>(() => repository.Delete(layout));
        }

        [Test]
        public void Update_WhenExistLayout_ShouldReturnListWithUpdateLayout()
        {
            // Arrange
            var repository = new AdoUsingParametersRepository<Layout>(MainConnectionString);
            var expected = new Layout { VenueId = 2, Description = "Updated Layout" };

            // Act
            var lastLayout = repository.GetAll().Last();
            var idLastLayout = lastLayout.Id;
            expected.Id = idLastLayout;

            repository.Update(expected);
            var actual = repository.GetAll().Last();

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void Update_WhenNullLayout_ShouldThrowArgumentException()
        {
            // Arrange
            var repository = new AdoUsingParametersRepository<Layout>(MainConnectionString);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => repository.Update(null));
        }

        [Test]
        public void Update_WhenIdEqualZeroLayout_ShouldThrowArgumentException()
        {
            // Arrange
            Layout layout = new Layout { Id = 0 };
            var repository = new AdoUsingParametersRepository<Layout>(MainConnectionString);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => repository.Update(layout));
        }

        [Test]
        public void GetById_WhenExistLayout_ShouldReturnLayout()
        {
            // Arrange
            var repository = new AdoUsingParametersRepository<Layout>(MainConnectionString);

            // Act
            var lastLayout = repository.GetAll().Last();
            Layout expectedLayout = new Layout { Id = lastLayout.Id,  VenueId = lastLayout.VenueId, Description = lastLayout.Description };

            int actualId = expectedLayout.Id;
            var actual = repository.GetByID(actualId);

            // Assert
            actual.Should().BeEquivalentTo(expectedLayout);
        }

        [Test]
        public void GetById_WhenNonExistLayout_ShouldReturnNull()
        {
            // Arrange
            Layout expected = null;
            var repository = new AdoUsingParametersRepository<Layout>(MainConnectionString);

            // Act
            var lastLayout = repository.GetAll().Last();
            int nonExistId = lastLayout.Id + 1;
            var actual = repository.GetByID(nonExistId);

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void GetById_WhenIdEqualZeroLayout_ShouldThrowArgumentException()
        {
            // Arrange
            Layout layout = new Layout { Id = 0 };
            var repository = new AdoUsingParametersRepository<Layout>(MainConnectionString);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => repository.GetByID(layout.Id));
        }

        [Test]
        public void GetById_WhenIdLessThenZero_ShouldThrowArgumentException()
        {
            // Arrange
            Layout layout = new Layout { Id = -1 };
            var repository = new AdoUsingParametersRepository<Layout>(MainConnectionString);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => repository.GetByID(layout.Id));
        }

        [Test]
        public void Update_WhenIdLessThenZero_ShouldThrowArgumentException()
        {
            // Arrange
            Layout layout = new Layout { Id = -1 };
            var repository = new AdoUsingParametersRepository<Layout>(MainConnectionString);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => repository.Update(layout));
        }

        [Test]
        public void Delete_WhenIdLessThenZero_ShouldThrowArgumentException()
        {
            // Arrange
            Layout layout = new Layout { Id = -1 };
            var repository = new AdoUsingParametersRepository<Layout>(MainConnectionString);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => repository.Delete(layout));
        }
    }
}
