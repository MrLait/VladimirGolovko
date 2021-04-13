﻿using System;
using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using TicketManagement.DataAccess.Domain.Models;
using TicketManagement.DataAccess.Repositories.AdoRepositories;

namespace TicketManagement.IntegrationTests.DataAccess.Repositories.AdoRepositories
{
    [TestFixture]
    internal class VenueTests : TestDatabaseLoader
    {
        [Test]
        public void GetAll_WhenVenuesExist_ShouldReturnVenueList()
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
        public void GetAll_WhenVenuesIncorrectConnectionSting_ShouldThrowArgumentException()
        {
            // Arrange
            var actual = new AdoUsingParametersRepository<Venue>("Server=myServerAddress;Database=myDataBase;User Id=myUsername;Password=myPassword;");

            // Act & Assert
            Assert.Throws<ArgumentException>(() => actual.GetAll());
        }

        [Test]
        public void Create_WhenAddVenue_ShouldReturnVenueWithNewVenue()
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
        public void Create_WhenVenueEmpty_ShouldThrowArgumentException()
        {
            // Arrange
            var repository = new AdoUsingParametersRepository<Venue>(MainConnectionString);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => repository.Create(null));
        }

        [Test]
        public void Delete_WhenExistVenue_ShouldReturnVenueListWithoutDeletedVenue()
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
        public void Delete_WhenIdEqualZerotVenue_ShouldThrowArgumentException()
        {
            // Arrange
            Venue venue = new Venue { Id = 0, Description = "New", Address = "pl. Lenin 1, Gomel 246050", Phone = "+375232757763" };
            var repository = new AdoUsingParametersRepository<Venue>(MainConnectionString);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => repository.Delete(venue));
        }

        [Test]
        public void Delete_WhenNullVenue_ShouldThrowArgumentException()
        {
            // Arrange
            var repository = new AdoUsingParametersRepository<Venue>(MainConnectionString);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => repository.Delete(null));
        }

        [Test]
        public void Delete_WhenIncorrectConnectionStringVenue_ShouldThrowArgumentException()
        {
            // Arrange
            Venue venue = new Venue { Id = 3, Description = "New", Address = "pl. Lenin 1, Gomel 246050", Phone = "+375232757763" };
            var repository = new AdoUsingParametersRepository<Venue>("Server=myServerAddress;Database=myDataBase;User Id=myUsername;Password=myPassword;");

            // Act & Assert
            Assert.Throws<ArgumentException>(() => repository.Delete(venue));
        }

        [Test]
        public void Update_WhenExistVenue_ShouldReturnListWithUpdateVenue()
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
        public void Update_WhenNullVenue_ShouldThrowArgumentException()
        {
            // Arrange
            var repository = new AdoUsingParametersRepository<Venue>(MainConnectionString);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => repository.Update(null));
        }

        [Test]
        public void Update_WhenIdEqualZeroVenue_ShouldThrowArgumentException()
        {
            // Arrange
            Venue venue = new Venue { Id = 0, Description = "New", Address = "pl. Lenin 1, Gomel 246050", Phone = "+375232757763" };
            var repository = new AdoUsingParametersRepository<Venue>(MainConnectionString);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => repository.Update(venue));
        }

        [Test]
        public void GetById_WhenExistVenue_ShouldReturnVenue()
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
        public void GetById_WhenNonExistVenue_ShouldReturnNull()
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
        public void GetById_WhenIdEqualZeroVenue_ShouldThrowArgumentException()
        {
            // Arrange
            Venue venue = new Venue { Id = 0, Description = "New", Address = "pl. Lenin 1, Gomel 246050", Phone = "+375232757763" };
            var repository = new AdoUsingParametersRepository<Venue>(MainConnectionString);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => repository.GetByID(venue.Id));
        }

        [Test]
        public void GetById_WhenIdLessThenZero_ShouldThrowArgumentException()
        {
            // Arrange
            Venue venue = new Venue { Id = -1 };
            var repository = new AdoUsingParametersRepository<Venue>(MainConnectionString);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => repository.GetByID(venue.Id));
        }

        [Test]
        public void Update_WhenIdLessThenZero_ShouldThrowArgumentException()
        {
            // Arrange
            Venue venue = new Venue { Id = -1 };
            var repository = new AdoUsingParametersRepository<Venue>(MainConnectionString);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => repository.Update(venue));
        }

        [Test]
        public void Delete_WhenIdLessThenZero_ShouldThrowArgumentException()
        {
            // Arrange
            Venue venue = new Venue { Id = -1 };
            var repository = new AdoUsingParametersRepository<Venue>(MainConnectionString);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => repository.Delete(venue));
        }
    }
}
