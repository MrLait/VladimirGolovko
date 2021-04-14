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
    /// Venue service tests.
    /// </summary>
    [TestFixture]
    public class VenueServiceTests : MockEntites
    {
        [Test]
        public void Create_WhenVenueExist_ShouldReturnCreatedVenue()
        {
            // Arrange
            var expected = new Venue { Address = "Added Address", Description = "Added Description", Phone = "+375293094300" };
            Mock.Setup(x => x.Venues.Create(It.IsAny<Venue>())).Callback<Venue>(v => Venues.Add(v));
            var venueService = new VenueService(Mock.Object);

            // Act
            venueService.Create(new VenueDto { Address = "Added Address", Description = "Added Description", Phone = "+375293094300" });
            var actual = Venues.Last();

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void Create_WhenVenueEmpty_ShouldThrowValidationException()
        {
            // Arrange
            var venueService = new VenueService(Mock.Object);

            // Act & Assert
            Assert.Throws<ValidationException>(() => venueService.Create(null));
        }

        [Test]
        public void Create_WhenDescriptionExist_ShouldThrowValidationException()
        {
            // Arrange
            var venueDto = new VenueDto { Id = 1, Description = "Luzhniki Stadium", Address = "st. Luzhniki, 24, Moscow, Russia, 119048", Phone = "+7 495 780-08-08" };
            var venueService = new VenueService(Mock.Object);
            Mock.Setup(x => x.Venues.Create(It.IsAny<Venue>())).Callback<Venue>(v => Venues.Add(v));

            // Act & Assert
            Assert.Throws<ValidationException>(() => venueService.Create(venueDto));
        }

        [Test]
        public void Delete_WhenVenueExist_ShouldDeleteLastVenue()
        {
            // Arrange
            var expected = Venues.Last();
            Mock.Setup(x => x.Venues.Delete(It.IsAny<Venue>())).Callback<Venue>(v => Venues.RemoveAt(v.Id - 1));
            var venueService = new VenueService(Mock.Object);
            var venueLast = Venues.Last();

            // Act
            venueService.Delete(new VenueDto { Id = venueLast.Id, Description = venueLast.Description, Address = venueLast.Address, Phone = venueLast.Phone });
            var actual = Venues.Last();

            // Assert
            actual.Should().NotBeEquivalentTo(expected);
        }

        [Test]
        public void Delete_WhenVenueEmpty_ShouldThrowValidationException()
        {
            // Arrange
            var venueService = new VenueService(Mock.Object);

            // Act & Assert
            Assert.Throws<ValidationException>(() => venueService.Delete(null));
        }

        [Test]
        public void Delete_WhenIdEqualZero_ShouldThrowValidationException()
        {
            // Arrange
            var venueService = new VenueService(Mock.Object);

            // Act & Assert
            Assert.Throws<ValidationException>(() => venueService.Delete(new VenueDto { Id = 0 }));
        }

        [Test]
        public void Delete_WhenIdEqualLeesThanZero_ShouldThrowValidationException()
        {
            // Arrange
            var venueService = new VenueService(Mock.Object);

            // Act & Assert
            Assert.Throws<ValidationException>(() => venueService.Delete(new VenueDto { Id = -1 }));
        }

        [Test]
        public void Update_WhenVenueExist_ShouldUpdateLastVenue()
        {
            // Arrange
            var venueLast = Venues.Last();
            var expected = new Venue { Id = venueLast.Id, Address = "Added Address", Description = "Added Description", Phone = "+375293094300" };
            var venueService = new VenueService(Mock.Object);

            // Act
            Action<Venue> updateLastAction = venues => Venues.RemoveAt(venueLast.Id - 1);
            updateLastAction += v => Venues.Insert(v.Id - 1, v);
            Mock.Setup(x => x.Venues.Update(It.IsAny<Venue>())).Callback(updateLastAction);

            venueService.Update(new VenueDto { Id = venueLast.Id, Description = expected.Description, Address = expected.Address, Phone = expected.Phone });
            var actual = Venues[venueLast.Id - 1];

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void Update_WhenVenueEmpty_ShouldThrowValidationException()
        {
            // Arrange
            var venueService = new VenueService(Mock.Object);

            // Act & Assert
            Assert.Throws<ValidationException>(() => venueService.Update(null));
        }

        [Test]
        public void Update_WhenIdEqualZero_ShouldThrowValidationException()
        {
            // Arrange
            var venueService = new VenueService(Mock.Object);

            // Act & Assert
            Assert.Throws<ValidationException>(() => venueService.Update(new VenueDto { Id = 0 }));
        }

        [Test]
        public void Update_WhenIdEqualLeesThanZero_ShouldThrowValidationException()
        {
            // Arrange
            var venueService = new VenueService(Mock.Object);

            // Act & Assert
            Assert.Throws<ValidationException>(() => venueService.Update(new VenueDto { Id = -1 }));
        }

        [Test]
        public void GetAll_WhenVenuesExist_ShouldReturnVenues()
        {
            // Arrange
            var expected = Venues;
            Mock.Setup(x => x.Venues.GetAll()).Returns(Venues);
            var venueService = new VenueService(Mock.Object);

            // Act
            var actual = venueService.GetAll();

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void GetById_WhenVenueExist_ShouldReturnLastVenue()
        {
            // Arrange
            var expected = Venues.Last();
            var expectedId = expected.Id - 1;
            Mock.Setup(x => x.Venues.GetByID(expectedId)).Returns(Venues.Last());
            var venueService = new VenueService(Mock.Object);

            // Act
            var actual = venueService.GetByID(expectedId);

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void GetByID_WhenIdEqualZero_ShouldThrowValidationException()
        {
            // Arrange
            var venueService = new VenueService(Mock.Object);

            // Act & Assert
            Assert.Throws<ValidationException>(() => venueService.GetByID(0));
        }

        [Test]
        public void GetByID_WhenIdEqualLeesThanZero_ShouldThrowValidationException()
        {
            // Arrange
            var venueService = new VenueService(Mock.Object);

            // Act & Assert
            Assert.Throws<ValidationException>(() => venueService.GetByID(-1));
        }
    }
}
