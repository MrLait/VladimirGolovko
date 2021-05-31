using System;
using System.Collections.Generic;
using System.IO;
using ClassicMvc.Services;
using FluentAssertions;
using NUnit.Framework;
using ThirdPartyEventEditor.Models;

namespace ThirdPartyEventEditor.UnitTests.Services
{
    [TestFixture]
    public class JsonSerializerServiceTests
    {
        [Test]
        public void SerializeObjectToJsonString_WhenModelExist_ShouldReturnJsonString()
        {
            // Arrange
            var expected = "{\"Id\":1,\"Name\":\"Name\",\"StartDate\":\"3021-02-01T00:00:00\",\"EndDate\":\"3021-03-01T00:00:00\",\"Description\":\"actual\",\"PosterImage\":\"Image\"}";
            var serializer = new JsonSerializerService<ThirdPartyEvent>();

            var actualModel = new ThirdPartyEvent
            {
                Id = 1,
                Description = "actual",
                EndDate = new DateTime(3021, 03, 01, 00, 00, 00),
                Name = "Name",
                PosterImage = "Image",
                StartDate = new DateTime(3021, 02, 01, 00, 00, 00),
            };

            // Act
            var actual = serializer.SerializeObjectToJsonString(actualModel);

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void SerializeObjectToJsonString_WhenModelNull_ShouldReturnNull()
        {
            // Arrange
            var expected = "null";
            var serializer = new JsonSerializerService<ThirdPartyEvent>();

            // Act
            var actual = serializer.SerializeObjectToJsonString(null);

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void SerializeObjectsToJson_WhenModelExist_ShouldReturnJsonString()
        {
            // Arrange
            var serializer = new JsonSerializerService<ThirdPartyEvent>();
            var fileName = "json";
            string jsonDirectoryPath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\JsonsFiles\", fileName));
            var actualModels = new List<ThirdPartyEvent>
            {
                new ThirdPartyEvent
                {
                    Id = 1,
                    Description = "actual",
                    EndDate = new DateTime(3021, 03, 01, 00, 00, 00),
                    Name = "Name",
                    PosterImage = "Image",
                    StartDate = new DateTime(3021, 02, 01, 00, 00, 00),
                },
                new ThirdPartyEvent
                {
                    Id = 2,
                    Description = "actual2",
                    EndDate = new DateTime(3021, 03, 01, 00, 00, 00),
                    Name = "Name2",
                    PosterImage = "Image2",
                    StartDate = new DateTime(3021, 02, 01, 00, 00, 00),
                },
            };

            // Act
            serializer.SerializeObjectsToJson(actualModels, jsonDirectoryPath);
            var expectedModels = serializer.DeserializeObjectsFromJson(jsonDirectoryPath);

            // Assert
            actualModels.Should().BeEquivalentTo(expectedModels);
        }

        [Test]
        public void DeserializeObjectsFromJson_WhenFileExist_ShouldReturnJsonFile()
        {
            // Arrange
            var serializer = new JsonSerializerService<ThirdPartyEvent>();
            var fileName = "jsonTest";
            string jsonDirectoryPath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\JsonsFiles\", fileName));
            var expectedModels = new List<ThirdPartyEvent>
            {
                new ThirdPartyEvent
                {
                    Id = 1,
                    Description = "actual",
                    EndDate = new DateTime(3021, 03, 01, 00, 00, 00),
                    Name = "Name",
                    PosterImage = "Image",
                    StartDate = new DateTime(3021, 02, 01, 00, 00, 00),
                },
                new ThirdPartyEvent
                {
                    Id = 2,
                    Description = "actual2",
                    EndDate = new DateTime(3021, 03, 01, 00, 00, 00),
                    Name = "Name2",
                    PosterImage = "Image2",
                    StartDate = new DateTime(3021, 02, 01, 00, 00, 00),
                },
            };

            // Act
            var actualModels = serializer.DeserializeObjectsFromJson(jsonDirectoryPath);

            // Assert
            actualModels.Should().BeEquivalentTo(expectedModels);
        }

        [Test]
        public void DeserializeObjectsFromJson_WhenFileNotExist_ShouldReturnDirectoryNotFoundException()
        {
            // Arrange
            var serializer = new JsonSerializerService<ThirdPartyEvent>();
            string jsonDirectoryPath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\JsonsFiles\"));

            // Act & Assert
            Assert.Throws<DirectoryNotFoundException>(() => serializer.DeserializeObjectsFromJson(jsonDirectoryPath));
        }

        [Test]
        public void DeserializeObjectsFromJson_WhenFileNotExist_ShouldReturnFileNotFoundException()
        {
            // Arrange
            var serializer = new JsonSerializerService<ThirdPartyEvent>();
            string jsonDirectoryPath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\JsonsFiles\null"));

            // Act & Assert
            Assert.Throws<FileNotFoundException>(() => serializer.DeserializeObjectsFromJson(jsonDirectoryPath));
        }

        [Test]
        public void SerializeObjectsToJson_WhenFileNotExist_ShouldReturnDirectoryNotFoundException()
        {
            // Arrange
            var serializer = new JsonSerializerService<ThirdPartyEvent>();
            string jsonDirectoryPath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\JsonsFiles\"));
            var models = new List<ThirdPartyEvent>();

            // Act & Assert
            Assert.Throws<DirectoryNotFoundException>(() => serializer.SerializeObjectsToJson(models, jsonDirectoryPath));
        }
    }
}
