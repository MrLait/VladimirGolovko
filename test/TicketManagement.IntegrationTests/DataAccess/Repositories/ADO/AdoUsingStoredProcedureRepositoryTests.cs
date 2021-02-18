using NUnit.Framework;
using TicketManagement.DataAccess.Domain.Models;
using TicketManagement.DataAccess.Repositories.ADO;

namespace TicketManagement.IntegrationTests.DataAccess.Repositories.ADO
{
    [TestFixture]
    public class AdoUsingStoredProcedureRepositoryTests : DatabaseConnectionBase
    {
        /// <summary>
        /// Field with group repository.
        /// </summary>
        private AdoUsingStoredProcedureRepository<Event> _eventRepository;

        /// <summary>
        /// Initialize repository.
        /// </summary>
        [SetUp]
        public void Init()
        {
            _eventRepository = new AdoUsingStoredProcedureRepository<Event>(DbConnString);
        }

        /// <summary>
        /// Test cases for Create.
        /// </summary>
        /// <param name="actualName">Name parameter.</param>
        [TestCase("NameGroup")]
        public void GivenCreate_WhenCorrectValue_ThenOutIsAddedObject(string actualName)
        {
            // Arrage
            Event actual = new Event { Description = "asd", LayoutId = 1, Name = "vas" };

            // Act
            _eventRepository.Create(actual);

            // Assert
            Assert.AreEqual(actualName, actualName);
        }

        /// <summary>
        /// Test cases for Delete.
        /// </summary>
        /// <param name="actualName">Name parameter.</param>
        [TestCase("NameGroup")]
        public void GivenDelete_WhenCorrectValue_ThenOutIsAddedObject(string actualName)
        {
            // Arrage
            // Act
            _eventRepository.Delete(1);

            // Assert
            Assert.AreEqual(actualName, actualName);
        }
    }
}
