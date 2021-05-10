﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
        public void GetAllAsQueryable_WhenVenuesExist_ShouldReturnVenueList()
        {
            // Arrange
            var expected = new List<Venue>
            {
                new Venue { Id = 1, Description = "Luzhniki Stadium", Address = "st. Luzhniki, 24, Moscow, Russia, 119048", Phone = "+7 495 780-08-08" },
                new Venue { Id = 2, Description = "Gomel Regional Drama Theater", Address = "pl. Lenin 1, Gomel 246050", Phone = "+375232757763" },
                new Venue { Id = 3, Description = "The circus", Address = "pl. Lenin 1, Brest 246050", Phone = "+375442757763" },
            };

            // Act
            var actual = new AdoUsingParametersRepository<Venue>(MainConnectionString).GetAllAsQueryable();

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void GetAllAsQueryable_WhenVenuesIncorrectConnectionSting_ShouldThrowArgumentException()
        {
            // Arrange
            var actual = new AdoUsingParametersRepository<Venue>("Server=myServerAddress;Database=myDataBase;User Id=myUsername;Password=myPassword;");

            // Act & Assert
            Assert.Throws<ArgumentException>(() => actual.GetAllAsQueryable());
        }

        [Test]
        public async Task CreateAsync_WhenAddVenue_ShouldReturnVenueWithNewVenue()
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
            await repository.CreateAsync(venue);
            var actual = repository.GetAllAsQueryable();

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void CreateAsync_WhenVenueEmpty_ShouldThrowArgumentException()
        {
            // Arrange
            var repository = new AdoUsingParametersRepository<Venue>(MainConnectionString);

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await repository.CreateAsync(null));
        }

        [Test]
        public async Task DeleteAsync_WhenExistVenue_ShouldReturnVenueListWithoutDeletedVenue()
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
            await repository.DeleteAsync(venue);
            var actual = repository.GetAllAsQueryable();

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void DeleteAsync_WhenIdEqualZerotVenue_ShouldThrowArgumentException()
        {
            // Arrange
            Venue venue = new Venue { Id = 0, Description = "New", Address = "pl. Lenin 1, Gomel 246050", Phone = "+375232757763" };
            var repository = new AdoUsingParametersRepository<Venue>(MainConnectionString);

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await repository.DeleteAsync(venue));
        }

        [Test]
        public void DeleteAsync_WhenNullVenue_ShouldThrowArgumentException()
        {
            // Arrange
            var repository = new AdoUsingParametersRepository<Venue>(MainConnectionString);

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await repository.DeleteAsync(null));
        }

        [Test]
        public void DeleteAsync_WhenIncorrectConnectionStringVenue_ShouldThrowArgumentException()
        {
            // Arrange
            Venue venue = new Venue { Id = 3, Description = "New", Address = "pl. Lenin 1, Gomel 246050", Phone = "+375232757763" };
            var repository = new AdoUsingParametersRepository<Venue>("Server=myServerAddress;Database=myDataBase;User Id=myUsername;Password=myPassword;");

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await repository.DeleteAsync(venue));
        }

        [Test]
        public async Task UpdateAsync_WhenExistVenue_ShouldUpdateThirdVenue()
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
            await repository.UpdateAsync(venue);
            var actual = repository.GetAllAsQueryable();

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void UpdateAsync_WhenNullVenue_ShouldThrowArgumentException()
        {
            // Arrange
            var repository = new AdoUsingParametersRepository<Venue>(MainConnectionString);

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await repository.UpdateAsync(null));
        }

        [Test]
        public void UpdateAsync_WhenIdEqualZeroVenue_ShouldThrowArgumentException()
        {
            // Arrange
            Venue venue = new Venue { Id = 0, Description = "New", Address = "pl. Lenin 1, Gomel 246050", Phone = "+375232757763" };
            var repository = new AdoUsingParametersRepository<Venue>(MainConnectionString);

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await repository.UpdateAsync(venue));
        }

        [Test]
        public async Task GetByIdAsync_WhenExistVenue_ShouldReturnVenue()
        {
            // Arrange
            Venue expectedVenue = new Venue { Id = 3, Description = "The circus", Address = "pl. Lenin 1, Brest 246050", Phone = "+375442757763" };
            var repository = new AdoUsingParametersRepository<Venue>(MainConnectionString);

            // Act
            int actualId = expectedVenue.Id;
            var actual = await repository.GetByIDAsync(actualId);

            // Assert
            actual.Should().BeEquivalentTo(expectedVenue);
        }

        [Test]
        public async Task GetByIdAsync_WhenNonExistVenue_ShouldReturnNull()
        {
            // Arrange
            Venue venue = new Venue { Id = 5, Description = "DescriptionUpdated", Address = "AddressUpdated", Phone = "+375232757763" };
            Venue expected = null;
            var repository = new AdoUsingParametersRepository<Venue>(MainConnectionString);

            // Act
            int actualId = venue.Id;
            var actual = await repository.GetByIDAsync(actualId);

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void GetByIdAsync_WhenIdEqualZeroVenue_ShouldThrowArgumentException()
        {
            // Arrange
            Venue venue = new Venue { Id = 0, Description = "New", Address = "pl. Lenin 1, Gomel 246050", Phone = "+375232757763" };
            var repository = new AdoUsingParametersRepository<Venue>(MainConnectionString);

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await repository.GetByIDAsync(venue.Id));
        }

        [Test]
        public void GetByIdAsync_WhenIdLessThenZero_ShouldThrowArgumentException()
        {
            // Arrange
            Venue venue = new Venue { Id = -1 };
            var repository = new AdoUsingParametersRepository<Venue>(MainConnectionString);

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await repository.GetByIDAsync(venue.Id));
        }

        [Test]
        public void UpdateAsync_WhenIdLessThenZero_ShouldThrowArgumentException()
        {
            // Arrange
            Venue venue = new Venue { Id = -1 };
            var repository = new AdoUsingParametersRepository<Venue>(MainConnectionString);

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await repository.UpdateAsync(venue));
        }

        [Test]
        public void DeleteAsync_WhenIdLessThenZero_ShouldThrowArgumentException()
        {
            // Arrange
            Venue venue = new Venue { Id = -1 };
            var repository = new AdoUsingParametersRepository<Venue>(MainConnectionString);

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await repository.DeleteAsync(venue));
        }
    }
}
