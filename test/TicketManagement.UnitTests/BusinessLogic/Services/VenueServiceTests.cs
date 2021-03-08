namespace TicketManagement.UnitTests.BusinessLogic.Services
{
    using System;
    using System.Linq;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;
    using TicketManagement.BusinessLogic.Infrastructure;
    using TicketManagement.BusinessLogic.Services;
    using TicketManagement.DataAccess.Domain.Models;
    using TicketManagement.Dto;

    [TestFixture]
    public class VenueServiceTests : MockEntites
    {
        [Test]
        public void GivenCreate_WhenVenueExist_ShouldReturnCreatedVenue()
        {
            // Arrange
            var expected = new Venue { Address = "Added Address", Description = "Added Description", Phone = "+375293094300" };
            Mock.Setup(x => x.Venues.Create(It.IsAny<Venue>())).Callback<Venue>(v => Venues.Add(v));
            var venueService = new VenueService(Mock.Object);

            // Act
            venueService.Create(new VenueDto { Address = "Added Address", Description = "Added Description", Phone = "+375293094300" });

            // Assert
            expected.Should().BeEquivalentTo(Venues.Last());
        }

        [Test]
        public void GivenCreate_WhenVenueEmpty_ShouldReturnArgumentException()
        {
            // Arrange
            var venueService = new VenueService(Mock.Object);

            // Assert
            Assert.Throws<ArgumentException>(() => venueService.Create(null));
        }

        [Test]
        public void GivenCreate_WhenDescriptionExist_ShouldReturnValidationException()
        {
            // Arrange
            var venueDto = new VenueDto { Id = 1, Description = "Luzhniki Stadium", Address = "st. Luzhniki, 24, Moscow, Russia, 119048", Phone = "+7 495 780-08-08" };
            var venueService = new VenueService(Mock.Object);
            Mock.Setup(x => x.Venues.Create(It.IsAny<Venue>())).Callback<Venue>(v => Venues.Add(v));

            // Assert
            Assert.Throws<ValidationException>(() => venueService.Create(venueDto));
        }

        [Test]
        public void GivenDelete_WhenVenueExist_ShouldReturnListWithDeletedVenue()
        {
            // Arrange
            var expected = Venues.Last();
            Mock.Setup(x => x.Venues.Delete(It.IsAny<Venue>())).Callback<Venue>(v => Venues.RemoveAt(v.Id - 1));
            var venueService = new VenueService(Mock.Object);
            var venueLast = Venues.Last();

            // Act
            venueService.Delete(new VenueDto { Id = venueLast.Id, Description = venueLast.Description, Address = venueLast.Address, Phone = venueLast.Phone });

            // Assert
            expected.Should().NotBeEquivalentTo(Venues.Last());
        }

        [Test]
        public void GivenDelete_WhenVenueEmpty_ShouldReturnArgumentException()
        {
            // Arrange
            var venueService = new VenueService(Mock.Object);

            // Act
            Assert.Throws<ArgumentException>(() => venueService.Delete(null));
        }

        [Test]
        public void GivenDelete_WhenIdEqualZero_ShouldReturnArgumentException()
        {
            // Arrange
            var venueService = new VenueService(Mock.Object);

            // Act
            Assert.Throws<ArgumentException>(() => venueService.Delete(new VenueDto { Id = 0 }));
        }

        [Test]
        public void GivenDelete_WhenIdEqualLeesThanZero_ShouldReturnArgumentException()
        {
            // Arrange
            var venueService = new VenueService(Mock.Object);

            // Act
            Assert.Throws<ArgumentException>(() => venueService.Delete(new VenueDto { Id = -1 }));
        }

        [Test]
        public void GivenUpdate_WhenVenueExist_ShouldReturnListWithUpdatedVenue()
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

            // Assert
            expected.Should().BeEquivalentTo(Venues[venueLast.Id - 1]);
        }

        [Test]
        public void GivenUpdate_WhenVenueEmpty_ShouldReturnArgumentException()
        {
            // Arrange
            var venueService = new VenueService(Mock.Object);

            // Act
            Assert.Throws<ArgumentException>(() => venueService.Update(null));
        }

        [Test]
        public void GivenUpdate_WhenIdEqualZero_ShouldReturnArgumentException()
        {
            // Arrange
            var venueService = new VenueService(Mock.Object);

            // Act
            Assert.Throws<ArgumentException>(() => venueService.Update(new VenueDto { Id = 0 }));
        }

        [Test]
        public void GivenUpdate_WhenIdEqualLeesThanZero_ShouldReturnArgumentException()
        {
            // Arrange
            var venueService = new VenueService(Mock.Object);

            // Act
            Assert.Throws<ArgumentException>(() => venueService.Update(new VenueDto { Id = -1 }));
        }
    }
}
