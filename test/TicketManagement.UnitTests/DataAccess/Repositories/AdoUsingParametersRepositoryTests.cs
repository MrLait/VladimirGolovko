using System.Collections.Generic;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using TicketManagement.DataAccess.Domain.Models;
using TicketManagement.DataAccess.Interfaces;
using TicketManagement.DataAccess.Repositories.AdoRepositories;

namespace TicketManagement.UnitTests.DataAccess.Repositories
{
    ////[TestFixture]
    ////public class AdoUsingParametersRepositoryTests : MockEntites
    ////{
    ////    ////[Test]
    ////    ////[System.Diagnostics.CodeAnalysis.SuppressMessage("Minor Code Smell", "S1481:Unused local variables should be removed", Justification = "<Ожидание>")]
    ////    ////public void GivenGetAll_WhenVenueExist_ShouldReturnVenueList()
    ////    ////{
    ////    ////    ////var mock = new Mock<IRepository<Venue>>();
    ////    ////    ////mock.Setup(x => x.GetAll()).Returns(Venues);
    ////    ////    ////var test = mock.Object.GetAll();
    ////    ////    ////var mockDbContext = new Mock<IDbContext>();
    ////    ////    ////mockDbContext.Setup(x => x.Areas.GetAll()).Returns(Areas);
    ////    ////    ////AdoUsingParametersRepository<Venue> adoUsingParametersRepository = new AdoUsingParametersRepository<Venue>(mockDbContext.Object);
    ////    ////    ////var test2 = adoUsingParametersRepository.GetAll();

    ////    ////    //////////// Assert
    ////    ////    //////////actual.Should().BeEquivalentTo(expected);
    ////    ////    //////////actual.Should().BeEquivalentTo(actualTwo);
    ////    ////    ////Assert.Fail();
    ////    ////}
    ////}
}
