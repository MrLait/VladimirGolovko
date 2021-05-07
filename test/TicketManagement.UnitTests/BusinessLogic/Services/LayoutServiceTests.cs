using System;
using System.Linq;
using System.Threading.Tasks;
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
    /// Layout service tests.
    /// </summary>
    [TestFixture]
    public class LayoutServiceTests : MockEntites
    {
        [Test]
        public async Task CreateAsync_WhenLayoutExist_ShouldReturnCreatedLayout()
        {
            // Arrange
            var expected = new Layout { VenueId = 2, Description = "Created Description" };
            Mock.Setup(x => x.Layouts.CreateAsync(It.IsAny<Layout>())).Callback<Layout>(v => Layouts.Add(v));
            var layoutService = new LayoutService(Mock.Object);

            // Act
            await layoutService.CreateAsync(new LayoutDto { VenueId = 2, Description = "Created Description" });
            var actual = Layouts.Last();

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void CreateAsync_WhenLayoutEmpty_ShouldThrowValidationException()
        {
            // Arrange
            var layoutService = new LayoutService(Mock.Object);

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () => await layoutService.CreateAsync(null));
        }

        [Test]
        public void CreateAsync_WhenLayoutAlreadyExist_ShouldThrowValidationException()
        {
            // Arrange
            var layoutFirst = Layouts.First();
            var layoutDto = new LayoutDto { Id = layoutFirst.Id, Description = layoutFirst.Description, VenueId = layoutFirst.VenueId };
            var layoutService = new LayoutService(Mock.Object);
            Mock.Setup(x => x.Layouts.CreateAsync(It.IsAny<Layout>())).Callback<Layout>(v => Layouts.Add(v));

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () => await layoutService.CreateAsync(layoutDto));
        }

        [Test]
        public async Task DeleteAsync_WhenLayoutExist_ShouldDeleteLastLayout()
        {
            // Arrange
            var expected = Layouts.Last();
            Mock.Setup(x => x.Layouts.DeleteAsync(It.IsAny<Layout>())).Callback<Layout>(v => Layouts.RemoveAt(v.Id - 1));
            var layoutService = new LayoutService(Mock.Object);
            var layoutLast = Layouts.Last();

            // Act
            await layoutService.DeleteAsync(new LayoutDto { Id = layoutLast.Id, VenueId = layoutLast.VenueId, Description = layoutLast.Description });

            // Assert
            expected.Should().NotBeEquivalentTo(Layouts.Last());
        }

        [Test]
        public void DeleteAsync_WhenLayoutEmpty_ShouldThrowValidationException()
        {
            // Arrange
            var layoutService = new LayoutService(Mock.Object);

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () => await layoutService.DeleteAsync(null));
        }

        [Test]
        public void DeleteAsync_WhenIdEqualZero_ShouldThrowValidationException()
        {
            // Arrange
            var layoutService = new LayoutService(Mock.Object);

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () => await layoutService.DeleteAsync(new LayoutDto { Id = 0 }));
        }

        [Test]
        public void DeleteAsync_WhenIdEqualLeesThanZero_ShouldThrowValidationException()
        {
            // Arrange
            var layoutService = new LayoutService(Mock.Object);

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () => await layoutService.DeleteAsync(new LayoutDto { Id = -1 }));
        }

        [Test]
        public async Task UpdateAsync_WhenLayoutExist_ShouldUpdateLastLayout()
        {
            // Arrange
            var layoutLast = Layouts.Last();
            var expected = new Layout { Id = layoutLast.Id, Description = "Updated Description", VenueId = layoutLast.VenueId };
            var layoutService = new LayoutService(Mock.Object);

            // Act
            Action<Layout> updateLastAction = venues => Layouts.RemoveAt(layoutLast.Id - 1);
            updateLastAction += v => Layouts.Insert(v.Id - 1, v);
            Mock.Setup(x => x.Layouts.UpdateAsync(It.IsAny<Layout>())).Callback(updateLastAction);

            await layoutService.UpdateAsync(new LayoutDto { Id = layoutLast.Id, VenueId = expected.VenueId, Description = expected.Description });
            var actual = Layouts[layoutLast.Id - 1];

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void UpdateAsync_WhenLayoutEmpty_ShouldThrowValidationException()
        {
            // Arrange
            var layoutService = new LayoutService(Mock.Object);

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () => await layoutService.UpdateAsync(null));
        }

        [Test]
        public void UpdateAsync_WhenIdEqualZero_ShouldThrowValidationException()
        {
            // Arrange
            var layoutService = new LayoutService(Mock.Object);

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () => await layoutService.UpdateAsync(new LayoutDto { Id = 0 }));
        }

        [Test]
        public void UpdateAsync_WhenIdEqualLeesThanZero_ShouldThrowValidationException()
        {
            // Arrange
            var layoutService = new LayoutService(Mock.Object);

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () => await layoutService.UpdateAsync(new LayoutDto { Id = -1 }));
        }

        [Test]
        public void UpdateAsync_WhenLayoutWithThisDescriptionAlreadyExist_ShouldThrowValidationException()
        {
            // Arrange
            var layoutFirst = Layouts.First();
            var layoutDto = new LayoutDto { Id = layoutFirst.Id, Description = layoutFirst.Description, VenueId = layoutFirst.VenueId };
            var layoutService = new LayoutService(Mock.Object);
            Mock.Setup(x => x.Layouts.UpdateAsync(It.IsAny<Layout>())).Callback<Layout>(v => Layouts.Add(v));

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () => await layoutService.UpdateAsync(layoutDto));
        }

        [Test]
        public async Task GetAllAsync_WhenLayoutsExist_ShouldReturnLayouts()
        {
            // Arrange
            var expected = Layouts;
            Mock.Setup(x => x.Layouts.GetAllAsync()).ReturnsAsync(Layouts.AsQueryable());
            var layoutService = new LayoutService(Mock.Object);

            // Act
            var actual = await layoutService.GetAll();

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public async Task GetByIdAsync_WhenLayoutExist_ShouldReturnLastLayout()
        {
            // Arrange
            var expected = Layouts.Last();
            var expectedId = expected.Id - 1;
            Mock.Setup(x => x.Layouts.GetByIDAsync(expectedId)).ReturnsAsync(Layouts.Last());
            var layoutService = new LayoutService(Mock.Object);

            // Act
            var actual = await layoutService.GetByIDAsync(expectedId);

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void GetByIDAsync_WhenIdEqualZero_ShouldThrowValidationException()
        {
            // Arrange
            var layoutService = new LayoutService(Mock.Object);

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () => await layoutService.GetByIDAsync(0));
        }

        [Test]
        public void GetByIDAsync_WhenIdEqualLeesThanZero_ShouldThrowValidationException()
        {
            // Arrange
            var layoutService = new LayoutService(Mock.Object);

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () => await layoutService.GetByIDAsync(-1));
        }
    }
}
