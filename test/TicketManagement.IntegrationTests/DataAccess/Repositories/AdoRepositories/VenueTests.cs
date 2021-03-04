using System;
using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using TicketManagement.DataAccess.Domain.Models;
using TicketManagement.DataAccess.Repositories.AdoRepositories;

namespace TicketManagement.IntegrationTests.DataAccess.Repositories.AdoRepositories
{
    [TestFixture]
    internal class VenueTests : AdoRepositoryTests
    {
        [Test]
        public void GivenGetAll_WhenVenuesExist_ShouldReturnVenueList()
        {
            // Arrange
            var expected = new List<Venue>
            {
                new Venue { Id = 1, Description = "Luzhniki Stadium", Address = "st. Luzhniki, 24, Moscow, Russia, 119048", Phone = "+7 495 780-08-08" },
                new Venue { Id = 2, Description = "Gomel Regional Drama Theater", Address = "pl. Lenin 1, Gomel 246050", Phone = "+375232757763" },
                new Venue { Id = 3, Description = "The circus", Address = "pl. Lenin 1, Brest 246050", Phone = "+375442757763" },
            };

            // Act
            var actual = new AdoUsingParametersRepository<Venue>(MainConnectionString).GetAll();

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void GivenGetAll_WhenVenuesIncorrectConnectionSting_ShouldReturnArgumentException()
        {
            // Arrange
            var actual = new AdoUsingParametersRepository<Venue>("Server=myServerAddress;Database=myDataBase;User Id=myUsername;Password=myPassword;");

            // Act
            TestDelegate testAction = () => actual.GetAll();

            // Assert
            Assert.Throws<ArgumentException>(testAction);
        }

        [Test]
        public void GivenCreate_WhenAddVenue_ShouldReturnVenueWithNewVenue()
        {
            // Arrange
            Venue venue = new Venue { Id = 3, Description = "New", Address = "pl. Lenin 1, Gomel 246050", Phone = "+375232757763" };
            var expected = new List<Venue>
            {
                new Venue { Id = 1, Description = "Luzhniki Stadium", Address = "st. Luzhniki, 24, Moscow, Russia, 119048", Phone = "+7 495 780-08-08" },
                new Venue { Id = 2, Description = "Gomel Regional Drama Theater", Address = "pl. Lenin 1, Gomel 246050", Phone = "+375232757763" },
                new Venue { Id = 3, Description = "The circus", Address = "pl. Lenin 1, Brest 246050", Phone = "+375442757763" },
                new Venue { Id = 4, Description = "New", Address = "pl. Lenin 1, Gomel 246050", Phone = "+375232757763" },
            };
            var repository = new AdoUsingParametersRepository<Venue>(MainConnectionString);

            // Act
            repository.Create(venue);
            var actual = repository.GetAll();

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void GivenCreate_WhenVenueEmpty_ShouldReturnArgumentException()
        {
            // Arrange
            var repository = new AdoUsingParametersRepository<Venue>(MainConnectionString);

            // Act
            TestDelegate testAction = () => repository.Create(null);

            // Assert
            Assert.Throws<ArgumentException>(testAction);
        }

        [Test]
        public void GivenDelete_WhenExistVenue_ShouldReturnVenueListWithoutDeletedVenue()
        {
            // Arrange
            Venue venue = new Venue { Id = 3, Description = "New", Address = "pl. Lenin 1, Gomel 246050", Phone = "+375232757763" };
            var expected = new List<Venue>
            {
                new Venue { Id = 1, Description = "Luzhniki Stadium", Address = "st. Luzhniki, 24, Moscow, Russia, 119048", Phone = "+7 495 780-08-08" },
                new Venue { Id = 2, Description = "Gomel Regional Drama Theater", Address = "pl. Lenin 1, Gomel 246050", Phone = "+375232757763" },
            };
            var repository = new AdoUsingParametersRepository<Venue>(MainConnectionString);

            // Act
            repository.Delete(venue);
            var actual = repository.GetAll();

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void GivenDelete_WhenIdEqualZerotVenue_ShouldReturnArgumentException()
        {
            // Arrange
            Venue venue = new Venue { Id = 0, Description = "New", Address = "pl. Lenin 1, Gomel 246050", Phone = "+375232757763" };
            var repository = new AdoUsingParametersRepository<Venue>(MainConnectionString);

            // Act
            TestDelegate testAction = () => repository.Delete(venue);

            // Assert
            Assert.Throws<ArgumentException>(testAction);
        }

        [Test]
        public void GivenDelete_WhenNullVenue_ShouldReturnArgumentException()
        {
            // Arrange
            var repository = new AdoUsingParametersRepository<Venue>(MainConnectionString);

            // Act
            TestDelegate testAction = () => repository.Delete(null);

            // Assert
            Assert.Throws<ArgumentException>(testAction);
        }

        [Test]
        public void GivenDelete_WhenIncorrectConnectionStringVenue_ShouldReturnArgumentException()
        {
            // Arrange
            Venue venue = new Venue { Id = 3, Description = "New", Address = "pl. Lenin 1, Gomel 246050", Phone = "+375232757763" };
            var repository = new AdoUsingParametersRepository<Venue>("Server=myServerAddress;Database=myDataBase;User Id=myUsername;Password=myPassword;");

            // Act
            TestDelegate testAction = () => repository.Delete(venue);

            // Assert
            Assert.Throws<ArgumentException>(testAction);
        }

        [Test]
        public void GivenUpdate_WhenExistVenue_ShouldReturnListWithUpdateVenue()
        {
            // Arrange
            Venue venue = new Venue { Id = 3, Description = "DescriptionUpdated", Address = "AddressUpdated", Phone = "+375232757763" };
            var expected = new List<Venue>
            {
                new Venue { Id = 1, Description = "Luzhniki Stadium", Address = "st. Luzhniki, 24, Moscow, Russia, 119048", Phone = "+7 495 780-08-08" },
                new Venue { Id = 2, Description = "Gomel Regional Drama Theater", Address = "pl. Lenin 1, Gomel 246050", Phone = "+375232757763" },
                new Venue { Id = 3, Description = "DescriptionUpdated", Address = "AddressUpdated", Phone = "+375232757763" },
            };
            var repository = new AdoUsingParametersRepository<Venue>(MainConnectionString);

            // Act
            repository.Update(venue);
            var actual = repository.GetAll();

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void GivenUpdate_WhenNullVenue_ShouldReturnArgumentException()
        {
            // Arrange
            var repository = new AdoUsingParametersRepository<Venue>(MainConnectionString);

            // Act
            TestDelegate testAction = () => repository.Update(null);

            // Assert
            Assert.Throws<ArgumentException>(testAction);
        }

        [Test]
        public void GivenUpdate_WhenIdEqualZeroVenue_ShouldReturnArgumentException()
        {
            // Arrange
            Venue venue = new Venue { Id = 0, Description = "New", Address = "pl. Lenin 1, Gomel 246050", Phone = "+375232757763" };
            var repository = new AdoUsingParametersRepository<Venue>(MainConnectionString);

            // Act
            TestDelegate testAction = () => repository.Update(venue);

            // Assert
            Assert.Throws<ArgumentException>(testAction);
        }

        [Test]
        public void GivenGetById_WhenExistVenue_ShouldReturnVenue()
        {
            // Arrange
            Venue expectedVenue = new Venue { Id = 3, Description = "The circus", Address = "pl. Lenin 1, Brest 246050", Phone = "+375442757763" };
            var repository = new AdoUsingParametersRepository<Venue>(MainConnectionString);

            // Act
            int actualId = expectedVenue.Id;
            var actual = repository.GetByID(actualId);

            // Assert
            actual.Should().BeEquivalentTo(expectedVenue);
        }

        [Test]
        public void GivenGetById_WhenNonExistVenue_ShouldReturnNull()
        {
            // Arrange
            Venue venue = new Venue { Id = 5, Description = "DescriptionUpdated", Address = "AddressUpdated", Phone = "+375232757763" };
            Venue expected = null;
            var repository = new AdoUsingParametersRepository<Venue>(MainConnectionString);

            // Act
            int actualId = venue.Id;
            var actual = repository.GetByID(actualId);

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void GivenGetById_WhenIdEqualZeroVenue_ShouldReturnArgumentException()
        {
            // Arrange
            Venue venue = new Venue { Id = 0, Description = "New", Address = "pl. Lenin 1, Gomel 246050", Phone = "+375232757763" };
            var repository = new AdoUsingParametersRepository<Venue>(MainConnectionString);

            // Act
            TestDelegate testAction = () => repository.GetByID(venue.Id);

            // Assert
            Assert.Throws<ArgumentException>(testAction);
        }
    }
}
