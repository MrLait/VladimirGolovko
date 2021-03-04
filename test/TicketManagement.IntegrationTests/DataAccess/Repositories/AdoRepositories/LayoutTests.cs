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
    internal class LayoutTests : AdoRepositoryTests
    {
        private readonly List<Layout> _layouts = new List<Layout>();

        [OneTimeSetUp]
        public void InitLayouts()
        {
            var layoutRepository = new AdoUsingParametersRepository<Layout>(MainConnectionString);
            var coutAllLayouts = layoutRepository.GetAll().Last().Id;

            for (int i = 1; i <= coutAllLayouts; i++)
            {
                _layouts.Add(layoutRepository.GetByID(i));
            }
        }

        [Test]
        public void GivenGetAll_WhenLayoutsExist_ShouldReturnLayoutList()
        {
            // Arrange
            var expected = _layouts;

            // Act
            var actual = new AdoUsingParametersRepository<Layout>(MainConnectionString).GetAll();

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void GivenGetAll_WhenLayoutsIncorrectConnectionSting_ShouldReturnArgumentException()
        {
            // Arrange
            var actual = new AdoUsingParametersRepository<Layout>("Server=myServerAddress;Database=myDataBase;User Id=myUsername;Password=myPassword;");

            // Act
            TestDelegate testAction = () => actual.GetAll();

            // Assert
            Assert.Throws<ArgumentException>(testAction);
        }

        [Test]
        public void GivenCreate_WhenAddLayout_ShouldReturnLayoutWithNewLayout()
        {
            // Arrange
            var repository = new AdoUsingParametersRepository<Layout>(MainConnectionString);
            Layout layout = new Layout { Id = repository.GetAll().ToList().Count + 1, VenueId = 2, Description = "Created layout" };
            List<Layout> expected = new List<Layout>(_layouts);

            // Act
            expected.Add(layout);
            repository.Create(layout);
            var actual = repository.GetAll();

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void GivenCreate_WhenLayoutEmpty_ShouldReturnArgumentException()
        {
            // Arrange
            var repository = new AdoUsingParametersRepository<Layout>(MainConnectionString);

            // Act
            TestDelegate testAction = () => repository.Create(null);

            // Assert
            Assert.Throws<ArgumentException>(testAction);
        }

        [Test]
        public void GivenDelete_WhenExistLayout_ShouldReturnLayoutListWithoutDeletedLayout()
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
        public void GivenDelete_WhenIdEqualZeroLayout_ShouldReturnArgumentException()
        {
            // Arrange
            Layout layout = new Layout { Id = 0 };
            var repository = new AdoUsingParametersRepository<Layout>(MainConnectionString);

            // Act
            TestDelegate testAction = () => repository.Delete(layout);

            // Assert
            Assert.Throws<ArgumentException>(testAction);
        }

        [Test]
        public void GivenDelete_WhenNullLayout_ShouldReturnArgumentException()
        {
            // Arrange
            var repository = new AdoUsingParametersRepository<Layout>(MainConnectionString);

            // Act
            TestDelegate testAction = () => repository.Delete(null);

            // Assert
            Assert.Throws<ArgumentException>(testAction);
        }

        [Test]
        public void GivenDelete_WhenIncorrectConnectionStringLayout_ShouldReturnArgumentException()
        {
            // Arrange
            Layout layout = new Layout { Id = 3 };
            var repository = new AdoUsingParametersRepository<Layout>("Server=myServerAddress;Database=myDataBase;User Id=myUsername;Password=myPassword;");

            // Act
            TestDelegate testAction = () => repository.Delete(layout);

            // Assert
            Assert.Throws<ArgumentException>(testAction);
        }

        [Test]
        public void GivenUpdate_WhenExistLayout_ShouldReturnListWithUpdateLayout()
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
        public void GivenUpdate_WhenNullLayout_ShouldReturnArgumentException()
        {
            // Arrange
            var repository = new AdoUsingParametersRepository<Layout>(MainConnectionString);

            // Act
            TestDelegate testAction = () => repository.Update(null);

            // Assert
            Assert.Throws<ArgumentException>(testAction);
        }

        [Test]
        public void GivenUpdate_WhenIdEqualZeroLayout_ShouldReturnArgumentException()
        {
            // Arrange
            Layout layout = new Layout { Id = 0 };
            var repository = new AdoUsingParametersRepository<Layout>(MainConnectionString);

            // Act
            TestDelegate testAction = () => repository.Update(layout);

            // Assert
            Assert.Throws<ArgumentException>(testAction);
        }

        [Test]
        public void GivenGetById_WhenExistLayout_ShouldReturnLayout()
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
        public void GivenGetById_WhenNonExistLayout_ShouldReturnNull()
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
        public void GivenGetById_WhenIdEqualZeroLayout_ShouldReturnArgumentException()
        {
            // Arrange
            Layout layout = new Layout { Id = 0 };
            var repository = new AdoUsingParametersRepository<Layout>(MainConnectionString);

            // Act
            TestDelegate testAction = () => repository.GetByID(layout.Id);

            // Assert
            Assert.Throws<ArgumentException>(testAction);
        }
    }
}
