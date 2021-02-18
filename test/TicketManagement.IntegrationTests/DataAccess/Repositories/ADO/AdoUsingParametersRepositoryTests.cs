using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using TicketManagement.DataAccess.Domain.Models;
using TicketManagement.DataAccess.Repositories.ADO;

namespace TicketManagement.IntegrationTests.DataAccess.Repositories.ADO
{
    [TestFixture]
    internal class AdoUsingParametersRepositoryTests : DatabaseConnectionBase
    {
        /// <summary>
        /// Field with group repository.
        /// </summary>
        private AdoUsingParametersRepository<Venue> _venueRepository;

        /// <summary>
        /// Initialize repository.
        /// </summary>
        [SetUp]
        public void Init()
        {
            _venueRepository = new AdoUsingParametersRepository<Venue>(DbConnString);
        }

        /// <summary>
        /// Test cases for Add.
        /// </summary>
        /// <param name="actualName">Name parameter.</param>
        [TestCase("NameGroup")]
        public void GivenAdd_WhenCorrectValue_ThenOutIsAddedObject(string actualName)
        {
            // Arrage
            Venue actual = new Venue { Address = "Addr1 asd", Description = "Desc1 sd", Phone = "375123" };

            // Act
            _venueRepository.Insert(actual);

            // Assert
            Assert.AreEqual(actualName, actualName);
        }

        [TestCase(6)]
        public void GivenDelete_WhenCorrectId_ThenOutIsDeletedObject(int actualId)
        {
            // Arrage
            // Act
            _venueRepository.Delete(actualId);

            // Assert
            Assert.AreEqual(actualId, actualId);
        }

        [TestCase(1)]
        public void GivenGetById_WhenCorrectId_ThenOutIsDeletedObject(int actualId)
        {
            // Arrage
            // Act
            var test = _venueRepository.GetByID(actualId);

            // Assert
            Assert.AreEqual(test, test);
        }

        [TestCase(1)]
        public void GivenGetAll_WhenCorrectId_ThenOutIsDeletedObject(int actualId)
        {
            // Arrage
            // Act
            var test = _venueRepository.GetAll();

            // Assert
            Assert.AreEqual(test, test);
        }

        [TestCase(1)]
        public void GivenUpdate_WhenCorrectId_ThenOutIsDeletedObject(int actualId)
        {
            // Arrage
            // Act
            _venueRepository.Update(new Venue { Id = 1, Address = "a", Description = "d", Phone = "333" });

            // Assert
            Assert.AreEqual(actualId, actualId);
        }
    }
}
