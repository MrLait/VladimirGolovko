using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using TicketManagement.DataAccess.Ado;
using TicketManagement.DataAccess.Domain.Models;
using TicketManagement.DataAccess.Repositories.EfRepositories;

namespace TicketManagement.IntegrationTests.DataAccess.Repositories.EfRepository
{
    [TestFixture]
    internal class VenueTests : TestDatabaseLoader
    {
        private readonly List<Venue> _venues = new List<Venue>();

        public EfDbContext DbContext { get; set; }

        [OneTimeSetUp]
        public async Task InitVenuesAsync()
        {
            DbContext = new EfDbContext(connectionString: MainConnectionString);
            var repository = new EfRepository<Venue>(DbContext);
            var countAllVenues = repository.GetAllAsQueryable().OrderBy(x => x.Id).Last().Id;

            for (int i = 1; i <= countAllVenues; i++)
            {
                _venues.Add(await repository.GetByIDAsync(i));
            }
        }

        [Test]
        public void GetAllAsQueryable_WhenVenuesExist_ShouldReturnVenueList()
        {
            // Arrange
            var expected = _venues;

            // Act
            var actual = new EfRepository<Venue>(DbContext).GetAllAsQueryable();

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public async Task CreateAsync_WhenAddVenue_ShouldReturnVenueWithNewVenue()
        {
            // Arrange
            var repository = new EfRepository<Venue>(DbContext);
            Venue venue = new Venue { Description = "New", Address = "pl. Lenin 1, Gomel 246050", Phone = "+375232757763" };
            var expected = new List<Venue>(_venues)
            {
                venue,
            };

            // Act
            await repository.CreateAsync(venue);
            var actual = repository.GetAllAsQueryable();

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void CreateAsync_WhenVenueEmpty_ShouldThrowArgumentNullException()
        {
            // Arrange
            var repository = new EfRepository<Venue>(DbContext);

            // Act & Assert
            Assert.ThrowsAsync<ArgumentNullException>(async () => await repository.CreateAsync(null));
        }

        [Test]
        public async Task DeleteAsync_WhenExistVenue_ShouldReturnVenueListWithoutDeletedVenueAsync()
        {
            // Arrange
            var dbContext = new EfDbContext(connectionString: MainConnectionString);
            var repository = new EfRepository<Venue>(dbContext);

            // Act
            var allVenues = repository.GetAllAsQueryable().OrderBy(x=> x.Id).ToList();
            var lastVenue = allVenues.LastOrDefault();
            await repository.DeleteAsync(lastVenue);

            var actual = repository.GetAllAsQueryable().OrderBy(x=>x.Id);
            int countVenueWithoutLast = allVenues.ToList().Count - 1;

            // Assert
            actual.Should().BeEquivalentTo(allVenues.Take(countVenueWithoutLast));
        }

        [Test]
        public void DeleteAsync_WhenIdEqualZeroVenue_ShouldThrowInvalidOperationException()
        {
            // Arrange
            var venue = new Venue { Id = 0 };
            var repository = new EfRepository<Venue>(DbContext);

            // Assert
            Assert.ThrowsAsync<InvalidOperationException>(async () => await repository.DeleteAsync(venue));
        }

        [Test]
        public void DeleteAsync_WhenNullVenue_ShouldThrowArgumentNullException()
        {
            // Arrange
            var repository = new EfRepository<Venue>(DbContext);

            // Assert
            Assert.ThrowsAsync<ArgumentNullException>(async () => await repository.DeleteAsync(null));
        }

        [Test]
        public async Task UpdateAsync_WhenExistVenue_ShouldUpdateLastVenueAsync()
        {
            // Arrange
            var dbContext = new EfDbContext(connectionString: MainConnectionString);
            var repository = new EfRepository<Venue>(dbContext);
            var expected = new Venue { Description = "DescriptionUpdated", Address = "AddressUpdated", Phone = "+375232757763" };

            // Act
            var lastVenue = repository.GetAllAsQueryable().OrderBy(x => x.Id).Last();
            var idLastVenue = lastVenue.Id;
            expected.Id = idLastVenue;

            await repository.UpdateAsync(expected);
            var actual = repository.GetAllAsQueryable().OrderBy(x=>x.Id).Last();

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void UpdateAsync_WhenNullVenue_ShouldThrowArgumentNullException()
        {
            // Arrange
            var repository = new EfRepository<Venue>(DbContext);

            // Act & Assert
            Assert.ThrowsAsync<ArgumentNullException>(async () => await repository.UpdateAsync(null));
        }

        [Test]
        public void UpdateAsync_WhenIdEqualZeroVenue_ShouldThrowDbUpdateConcurrencyException()
        {
            // Arrange
            var venue = new Venue { Id = 0 };
            var repository = new EfRepository<Venue>(DbContext);

            // Act & Assert
            Assert.ThrowsAsync<DbUpdateConcurrencyException>(async () => await repository.UpdateAsync(venue));
        }

        [Test]
        public async Task GetByIdAsync_WhenExistVenue_ShouldReturnVenueAsync()
        {
            // Arrange
            var repository = new EfRepository<Venue>(DbContext);

            // Act
            var lastVenue = repository.GetAllAsQueryable().OrderBy(x => x.Id).LastOrDefault();
            var expectedVenue = new Venue { Phone = lastVenue.Phone, Id = lastVenue.Id, Address = lastVenue.Address, Description = lastVenue.Description };

            int actualId = expectedVenue.Id;
            var actual = await repository.GetByIDAsync(actualId);

            // Assert
            actual.Should().BeEquivalentTo(expectedVenue);
        }

        [Test]
        public async Task GetByIdAsync_WhenNonExistVenue_ShouldReturnNullAsync()
        {
            // Arrange
            Venue expected = null;
            var repository = new EfRepository<Venue>(DbContext);

            // Act
            var lastVenue = repository.GetAllAsQueryable().OrderBy(x => x.Id).Last();
            int nonExistId = lastVenue.Id + 10;
            var actual = await repository.GetByIDAsync(nonExistId);

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void UpdateAsync_WhenIdLessThenZero_ShouldThrowDbUpdateConcurrencyException()
        {
            // Arrange
            var venue = new Venue { Id = -1 };
            var repository = new EfRepository<Venue>(DbContext);

            // Act & Assert
            Assert.ThrowsAsync<DbUpdateConcurrencyException>(async () => await repository.UpdateAsync(venue));
        }

        [Test]
        public void DeleteAsync_WhenIdLessThenZero_ShouldThrowDbUpdateConcurrencyException()
        {
            // Arrange
            var venue = new Venue { Id = -1 };
            var repository = new EfRepository<Venue>(DbContext);

            // Act & Assert
            Assert.ThrowsAsync<DbUpdateConcurrencyException>(async () => await repository.DeleteAsync(venue));
        }
    }
}
