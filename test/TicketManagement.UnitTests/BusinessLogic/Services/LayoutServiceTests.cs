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
    [TestFixture]
    public class LayoutServiceTests : MockEntites
    {
        [Test]
        public void GivenCreate_WhenLayoutExist_ShouldReturnCreatedLayout()
        {
            // Arrange
            var expected = new Layout { VenueId = 2, Description = "Created Description" };
            Mock.Setup(x => x.Layouts.Create(It.IsAny<Layout>())).Callback<Layout>(v => Layouts.Add(v));
            var layoutService = new LayoutService(Mock.Object);

            // Act
            layoutService.Create(new LayoutDto { VenueId = 2, Description = "Created Description" });

            // Assert
            expected.Should().BeEquivalentTo(Layouts.Last());
        }

        [Test]
        public void GivenCreate_WhenLayoutEmpty_ShouldReturnArgumentException()
        {
            // Arrange
            var layoutService = new LayoutService(Mock.Object);

            // Act
            TestDelegate testAction = () => layoutService.Create(null);

            // Assert
            Assert.Throws<ArgumentException>(testAction);
        }

        [Test]
        public void GivenCreate_WhenLayoutAlreadyExist_ShouldReturnValidationException()
        {
            // Arrange
            var firstLayout = Layouts.First();
            var layoutDto = new LayoutDto { Id = firstLayout.Id, Description = firstLayout.Description, VenueId = firstLayout.VenueId };
            var layoutService = new LayoutService(Mock.Object);
            Mock.Setup(x => x.Layouts.Create(It.IsAny<Layout>())).Callback<Layout>(v => Layouts.Add(v));

            // Act
            TestDelegate testAction = () => layoutService.Create(layoutDto);

            // Assert
            Assert.Throws<ValidationException>(testAction);
        }

        [Test]
        public void GivenDelete_WhenLayoutExist_ShouldReturnListWithDeletedLayout()
        {
            // Arrange
            var expected = Layouts.Last();
            Mock.Setup(x => x.Layouts.Delete(It.IsAny<Layout>())).Callback<Layout>(v => Layouts.RemoveAt(v.Id - 1));
            var layoutService = new LayoutService(Mock.Object);
            var layoutLast = Layouts.Last();

            // Act
            layoutService.Delete(new LayoutDto { Id = layoutLast.Id, VenueId = layoutLast.VenueId, Description = layoutLast.Description });

            // Assert
            expected.Should().NotBeEquivalentTo(Layouts.Last());
        }

        [Test]
        public void GivenDelete_WhenLayoutEmpty_ShouldReturnArgumentException()
        {
            // Arrange
            var layoutService = new LayoutService(Mock.Object);

            // Act
            TestDelegate testAction = () => layoutService.Delete(null);

            // Assert
            Assert.Throws<ArgumentException>(testAction);
        }

        [Test]
        public void GivenDelete_WhenIdEqualZero_ShouldReturnArgumentException()
        {
            // Arrange
            var layoutService = new LayoutService(Mock.Object);

            // Act
            TestDelegate testAction = () => layoutService.Delete(new LayoutDto { Id = 0 });

            // Assert
            Assert.Throws<ArgumentException>(testAction);
        }

        [Test]
        public void GivenDelete_WhenIdEqualLeesThanZero_ShouldReturnArgumentException()
        {
            // Arrange
            var layoutService = new LayoutService(Mock.Object);

            // Act
            TestDelegate testAction = () => layoutService.Delete(new LayoutDto { Id = -1 });

            // Assert
            Assert.Throws<ArgumentException>(testAction);
        }

        [Test]
        public void GivenUpdate_WhenLayoutExist_ShouldReturnListWithUpdatedLayout()
        {
            // Arrange
            var layoutLast = Layouts.Last();
            var expected = new Layout { Id = layoutLast.Id, Description = "Updated Description", VenueId = layoutLast.VenueId };
            var layoutService = new LayoutService(Mock.Object);

            // Act
            Action<Layout> updateLastAction = venues => Layouts.RemoveAt(layoutLast.Id - 1);
            updateLastAction += v => Layouts.Insert(v.Id - 1, v);
            Mock.Setup(x => x.Layouts.Update(It.IsAny<Layout>())).Callback(updateLastAction);

            layoutService.Update(new LayoutDto { Id = layoutLast.Id, VenueId = expected.VenueId, Description = expected.Description });

            // Assert
            expected.Should().BeEquivalentTo(Layouts[layoutLast.Id - 1]);
        }

        [Test]
        public void GivenUpdate_WhenLayoutEmpty_ShouldReturnArgumentException()
        {
            // Arrange
            var layoutService = new LayoutService(Mock.Object);

            // Act
            TestDelegate testAction = () => layoutService.Update(null);

            // Assert
            Assert.Throws<ArgumentException>(testAction);
        }

        [Test]
        public void GivenUpdate_WhenIdEqualZero_ShouldReturnArgumentException()
        {
            // Arrange
            var layoutService = new LayoutService(Mock.Object);

            // Act
            TestDelegate testAction = () => layoutService.Update(new LayoutDto { Id = 0 });

            // Assert
            Assert.Throws<ArgumentException>(testAction);
        }

        [Test]
        public void GivenUpdate_WhenIdEqualLeesThanZero_ShouldReturnArgumentException()
        {
            // Arrange
            var layoutService = new LayoutService(Mock.Object);

            // Act
            TestDelegate testAction = () => layoutService.Update(new LayoutDto { Id = -1 });

            // Assert
            Assert.Throws<ArgumentException>(testAction);
        }

        [Test]
        public void GivenUpdate_WhenLayoutWithThisDescriptionAlreadyExist_ShouldReturnValidationException()
        {
            // Arrange
            var firstLayout = Layouts.First();
            var layoutDto = new LayoutDto { Id = firstLayout.Id, Description = firstLayout.Description, VenueId = firstLayout.VenueId };
            var layoutService = new LayoutService(Mock.Object);
            Mock.Setup(x => x.Layouts.Update(It.IsAny<Layout>())).Callback<Layout>(v => Layouts.Add(v));

            // Act
            TestDelegate testAction = () => layoutService.Update(layoutDto);

            // Assert
            Assert.Throws<ValidationException>(testAction);
        }
    }
}
