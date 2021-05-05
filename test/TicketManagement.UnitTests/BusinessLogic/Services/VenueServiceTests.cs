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
    /// Venue service tests.
    /// </summary>
    [TestFixture]
    public class VenueServiceTests : MockEntites
    {
        [Test]
        public async Task CreateAsync_WhenVenueExist_ShouldReturnCreatedVenue()
        {
            // Arrange
            var expected = new Venue { Address = "Added Address", Description = "Added Description", Phone = "+375293094300" };
            Mock.Setup(x => x.Venues.CreateAsync(It.IsAny<Venue>())).Callback<Venue>(v => Venues.Add(v));
            var venueService = new VenueService(Mock.Object);

            // Act
            await venueService.CreateAsync(new VenueDto { Address = "Added Address", Description = "Added Description", Phone = "+375293094300" });
            var actual = Venues.Last();

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void CreateAsync_WhenVenueEmpty_ShouldThrowValidationException()
        {
            // Arrange
            var venueService = new VenueService(Mock.Object);

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () => await venueService.CreateAsync(null));
        }

        [Test]
        public void CreateAsync_WhenDescriptionExist_ShouldThrowValidationException()
        {
            // Arrange
            var venueDto = new VenueDto { Id = 1, Description = "Luzhniki Stadium", Address = "st. Luzhniki, 24, Moscow, Russia, 119048", Phone = "+7 495 780-08-08" };
            var venueService = new VenueService(Mock.Object);
            Mock.Setup(x => x.Venues.CreateAsync(It.IsAny<Venue>())).Callback<Venue>(v => Venues.Add(v));

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () => await venueService.CreateAsync(venueDto));
        }

        [Test]
        public async Task DeleteAsync_WhenVenueExist_ShouldDeleteLastVenue()
        {
            // Arrange
            var expected = Venues.Last();
            Mock.Setup(x => x.Venues.DeleteAsync(It.IsAny<Venue>())).Callback<Venue>(v => Venues.RemoveAt(v.Id - 1));
            var venueService = new VenueService(Mock.Object);
            var venueLast = Venues.Last();

            // Act
            await venueService.DeleteAsync(new VenueDto { Id = venueLast.Id, Description = venueLast.Description, Address = venueLast.Address, Phone = venueLast.Phone });
            var actual = Venues.Last();

            // Assert
            actual.Should().NotBeEquivalentTo(expected);
        }

        [Test]
        public void DeleteAsync_WhenVenueEmpty_ShouldThrowValidationException()
        {
            // Arrange
            var venueService = new VenueService(Mock.Object);

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () => await venueService.DeleteAsync(null));
        }

        [Test]
        public void DeleteAsync_WhenIdEqualZero_ShouldThrowValidationException()
        {
            // Arrange
            var venueService = new VenueService(Mock.Object);

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () => await venueService.DeleteAsync(new VenueDto { Id = 0 }));
        }

        [Test]
        public void DeleteAsync_WhenIdEqualLeesThanZero_ShouldThrowValidationException()
        {
            // Arrange
            var venueService = new VenueService(Mock.Object);

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () => await venueService.DeleteAsync(new VenueDto { Id = -1 }));
        }

        [Test]
        public async Task UpdateAsync_WhenVenueExist_ShouldUpdateLastVenue()
        {
            // Arrange
            var venueLast = Venues.Last();
            var expected = new Venue { Id = venueLast.Id, Address = "Added Address", Description = "Added Description", Phone = "+375293094300" };
            var venueService = new VenueService(Mock.Object);

            // Act
            Action<Venue> updateLastAction = venues => Venues.RemoveAt(venueLast.Id - 1);
            updateLastAction += v => Venues.Insert(v.Id - 1, v);
            Mock.Setup(x => x.Venues.UpdateAsync(It.IsAny<Venue>())).Callback(updateLastAction);

            await venueService.UpdateAsync(new VenueDto { Id = venueLast.Id, Description = expected.Description, Address = expected.Address, Phone = expected.Phone });
            var actual = Venues[venueLast.Id - 1];

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void UpdateAsync_WhenVenueEmpty_ShouldThrowValidationException()
        {
            // Arrange
            var venueService = new VenueService(Mock.Object);

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () => await venueService.UpdateAsync(null));
        }

        [Test]
        public void UpdateAsync_WhenIdEqualZero_ShouldThrowValidationException()
        {
            // Arrange
            var venueService = new VenueService(Mock.Object);

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () => await venueService.UpdateAsync(new VenueDto { Id = 0 }));
        }

        [Test]
        public void UpdateAsync_WhenIdEqualLeesThanZero_ShouldThrowValidationException()
        {
            // Arrange
            var venueService = new VenueService(Mock.Object);

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () => await venueService.UpdateAsync(new VenueDto { Id = -1 }));
        }

        [Test]
        public async Task GetAllAsync_WhenVenuesExist_ShouldReturnVenues()
        {
            // Arrange
            var expected = Venues;
            Mock.Setup(x => x.Venues.GetAllAsync()).ReturnsAsync(Venues.AsQueryable());
            var venueService = new VenueService(Mock.Object);

            // Act
            var actual = await venueService.GetAllAsync();

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public async Task GetByIdAsync_WhenVenueExist_ShouldReturnLastVenue()
        {
            // Arrange
            var expected = Venues.Last();
            var expectedId = expected.Id - 1;
            Mock.Setup(x => x.Venues.GetByIDAsync(expectedId)).ReturnsAsync(Venues.Last());
            var venueService = new VenueService(Mock.Object);

            // Act
            var actual = await venueService.GetByIDAsync(expectedId);

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void GetByIDAsync_WhenIdEqualZero_ShouldThrowValidationException()
        {
            // Arrange
            var venueService = new VenueService(Mock.Object);

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () => await venueService.GetByIDAsync(0));
        }

        [Test]
        public void GetByIDAsync_WhenIdEqualLeesThanZero_ShouldThrowValidationException()
        {
            // Arrange
            var venueService = new VenueService(Mock.Object);

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () => await venueService.GetByIDAsync(-1));
        }
    }
}
