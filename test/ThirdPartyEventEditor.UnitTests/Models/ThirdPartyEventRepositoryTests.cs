using System;
using System.Linq;
using ClassicMvc.Services;
using FluentAssertions;
using NUnit.Framework;
using ThirdPartyEventEditor.Models;

namespace ThirdPartyEventEditor.UnitTests.Models
{
    [TestFixture]
    public class ThirdPartyEventRepositoryTests
    {
        [Test]
        public void Create_WhenModelExist_ShouldCreate()
        {
            // Arrange
            var serializer = new JsonSerializerService<ThirdPartyEvent>();
            var thirdPartyEventRepository = new ThirdPartyEventRepository(serializer);

            var expected = new ThirdPartyEvent
            {
                Description = "actual",
                EndDate = new DateTime(3021, 03, 01, 00, 00, 00),
                Name = "Name",
                PosterImage = "Image",
                StartDate = new DateTime(3021, 02, 01, 00, 00, 00),
            };

            // Act
            thirdPartyEventRepository.Create(expected);
            var actual = thirdPartyEventRepository.GetAll().Last();

            // Clear
            thirdPartyEventRepository.Delete(actual.Id);

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void Create_WhenModelNull_ShouldReturnNullReferenceException()
        {
            // Arrange
            var serializer = new JsonSerializerService<ThirdPartyEvent>();
            var thirdPartyEventRepository = new ThirdPartyEventRepository(serializer);

            // Act & Assert
            Assert.Throws<NullReferenceException>(() => thirdPartyEventRepository.Create(null));
        }

        [Test]
        public void Delete_WhenModelExist_ShouldDelete()
        {
            // Arrange
            var serializer = new JsonSerializerService<ThirdPartyEvent>();
            var thirdPartyEventRepository = new ThirdPartyEventRepository(serializer);

            var model = new ThirdPartyEvent
            {
                Description = "actual",
                EndDate = new DateTime(3021, 03, 01, 00, 00, 00),
                Name = "Name",
                PosterImage = "Image",
                StartDate = new DateTime(3021, 02, 01, 00, 00, 00),
            };

            // Act
            var expected = thirdPartyEventRepository.GetAll().Count();
            thirdPartyEventRepository.Create(model);
            thirdPartyEventRepository.Delete(model.Id);
            var actual = thirdPartyEventRepository.GetAll().Count();

            // Assert
            actual.Should().Be(expected);
        }

        [Test]
        public void Delete_WhenModelNotExist_ShouldReturnArgumentOutOfRangeException()
        {
            // Arrange
            var serializer = new JsonSerializerService<ThirdPartyEvent>();
            var thirdPartyEventRepository = new ThirdPartyEventRepository(serializer);

            // Act
            var expected = thirdPartyEventRepository.GetAll().Count();

            // Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => thirdPartyEventRepository.Delete(expected + 100));
        }

        [Test]
        public void GetAll_WhenModelExist_ShouldGetAll()
        {
            // Arrange
            var serializer = new JsonSerializerService<ThirdPartyEvent>();
            var thirdPartyEventRepository = new ThirdPartyEventRepository(serializer);

            var model = new ThirdPartyEvent
            {
                Description = "actual",
                EndDate = new DateTime(3021, 03, 01, 00, 00, 00),
                Name = "Name",
                PosterImage = "Image",
                StartDate = new DateTime(3021, 02, 01, 00, 00, 00),
            };

            // Act
            var initialState = thirdPartyEventRepository.GetAll().ToList();
            var expected = thirdPartyEventRepository.GetAll().ToList();

            for (int i = 0; i < 10; i++)
            {
                thirdPartyEventRepository.Create(model);
                expected.Add(thirdPartyEventRepository.GetAll().Last());
            }

            var actual = thirdPartyEventRepository.GetAll().ToList();

            // Clear
            for (int i = initialState.Count; i < actual.Count; i++)
            {
                thirdPartyEventRepository.Delete(actual[i].Id);
            }

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void Update_WhenModelExist_ShouldUpdate()
        {
            // Arrange
            var serializer = new JsonSerializerService<ThirdPartyEvent>();
            var thirdPartyEventRepository = new ThirdPartyEventRepository(serializer);

            var model = new ThirdPartyEvent
            {
                Description = "actual",
                EndDate = new DateTime(3021, 03, 01, 00, 00, 00),
                Name = "Name",
                PosterImage = "Image",
                StartDate = new DateTime(3021, 02, 01, 00, 00, 00),
            };

            // Act
            thirdPartyEventRepository.Create(model);
            var expected = thirdPartyEventRepository.GetAll().Last();
            expected.Description = "New";
            thirdPartyEventRepository.Update(expected);

            var actual = new ThirdPartyEvent
            {
                Id = expected.Id,
                Description = "New",
                EndDate = new DateTime(3021, 03, 01, 00, 00, 00),
                Name = "Name",
                PosterImage = "Image",
                StartDate = new DateTime(3021, 02, 01, 00, 00, 00),
            };

            // Clear
            thirdPartyEventRepository.Delete(expected.Id);

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void GetById_WhenModelExist_ShouldUpdate()
        {
            // Arrange
            var serializer = new JsonSerializerService<ThirdPartyEvent>();
            var thirdPartyEventRepository = new ThirdPartyEventRepository(serializer);

            var model = new ThirdPartyEvent
            {
                Description = "actual",
                EndDate = new DateTime(3021, 03, 01, 00, 00, 00),
                Name = "Name",
                PosterImage = "Image",
                StartDate = new DateTime(3021, 02, 01, 00, 00, 00),
            };

            // Act
            thirdPartyEventRepository.Create(model);
            var lastId = thirdPartyEventRepository.GetAll().Last().Id;
            var expected = model;
            expected.Id = lastId;

            var actual = thirdPartyEventRepository.GetById(lastId);

            // Clear
            thirdPartyEventRepository.Delete(lastId);

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }
    }
}
