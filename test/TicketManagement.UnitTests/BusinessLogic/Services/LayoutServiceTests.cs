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
    /// <summary>
    /// Layout service tests.
    /// </summary>
    [TestFixture]
    public class LayoutServiceTests : MockEntites
    {
        [Test]
        public void Create_WhenLayoutExist_ShouldReturnCreatedLayout()
        {
            // Arrange
            var expected = new Layout { VenueId = 2, Description = "Created Description" };
            Mock.Setup(x => x.Layouts.Create(It.IsAny<Layout>())).Callback<Layout>(v => Layouts.Add(v));
            var layoutService = new LayoutService(Mock.Object);

            // Act
            layoutService.Create(new LayoutDto { VenueId = 2, Description = "Created Description" });
            var actual = Layouts.Last();

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void Create_WhenLayoutEmpty_ShouldThrowValidationException()
        {
            // Arrange
            var layoutService = new LayoutService(Mock.Object);

            // Act & Assert
            Assert.Throws<ValidationException>(() => layoutService.Create(null));
        }

        [Test]
        public void Create_WhenLayoutAlreadyExist_ShouldThrowValidationException()
        {
            // Arrange
            var layoutFirst = Layouts.First();
            var layoutDto = new LayoutDto { Id = layoutFirst.Id, Description = layoutFirst.Description, VenueId = layoutFirst.VenueId };
            var layoutService = new LayoutService(Mock.Object);
            Mock.Setup(x => x.Layouts.Create(It.IsAny<Layout>())).Callback<Layout>(v => Layouts.Add(v));

            // Act & Assert
            Assert.Throws<ValidationException>(() => layoutService.Create(layoutDto));
        }

        [Test]
        public void Delete_WhenLayoutExist_ShouldDeleteLastLayout()
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
        public void Delete_WhenLayoutEmpty_ShouldThrowValidationException()
        {
            // Arrange
            var layoutService = new LayoutService(Mock.Object);

            // Act & Assert
            Assert.Throws<ValidationException>(() => layoutService.Delete(null));
        }

        [Test]
        public void Delete_WhenIdEqualZero_ShouldThrowValidationException()
        {
            // Arrange
            var layoutService = new LayoutService(Mock.Object);

            // Act & Assert
            Assert.Throws<ValidationException>(() => layoutService.Delete(new LayoutDto { Id = 0 }));
        }

        [Test]
        public void Delete_WhenIdEqualLeesThanZero_ShouldThrowValidationException()
        {
            // Arrange
            var layoutService = new LayoutService(Mock.Object);

            // Act & Assert
            Assert.Throws<ValidationException>(() => layoutService.Delete(new LayoutDto { Id = -1 }));
        }

        [Test]
        public void Update_WhenLayoutExist_ShouldUpdateLastLayout()
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
            var actual = Layouts[layoutLast.Id - 1];

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void Update_WhenLayoutEmpty_ShouldThrowValidationException()
        {
            // Arrange
            var layoutService = new LayoutService(Mock.Object);

            // Act & Assert
            Assert.Throws<ValidationException>(() => layoutService.Update(null));
        }

        [Test]
        public void Update_WhenIdEqualZero_ShouldThrowValidationException()
        {
            // Arrange
            var layoutService = new LayoutService(Mock.Object);

            // Act & Assert
            Assert.Throws<ValidationException>(() => layoutService.Update(new LayoutDto { Id = 0 }));
        }

        [Test]
        public void Update_WhenIdEqualLeesThanZero_ShouldThrowValidationException()
        {
            // Arrange
            var layoutService = new LayoutService(Mock.Object);

            // Act & Assert
            Assert.Throws<ValidationException>(() => layoutService.Update(new LayoutDto { Id = -1 }));
        }

        [Test]
        public void Update_WhenLayoutWithThisDescriptionAlreadyExist_ShouldThrowValidationException()
        {
            // Arrange
            var layoutFirst = Layouts.First();
            var layoutDto = new LayoutDto { Id = layoutFirst.Id, Description = layoutFirst.Description, VenueId = layoutFirst.VenueId };
            var layoutService = new LayoutService(Mock.Object);
            Mock.Setup(x => x.Layouts.Update(It.IsAny<Layout>())).Callback<Layout>(v => Layouts.Add(v));

            // Act & Assert
            Assert.Throws<ValidationException>(() => layoutService.Update(layoutDto));
        }
    }
}
