using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ClassicMvc.Services;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using ThirdPartyEventEditor.Controllers;
using ThirdPartyEventEditor.Models;

namespace ThirdPartyEventEditor.UnitTests.Controllers
{
    [TestFixture]
    public class HomeControllerTests
    {
        [Test]
        public void Index_WhenIndexExist_ShouldReturnNotNull()
        {
            // Arrange
            var controller = new HomeController(Mock.Of<IThirdPartyEventRepository>(), Mock.Of<IJsonSerializerService<ThirdPartyEvent>>());

            // Act
            ViewResult result = controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public void Index_WhenControllerMocked_ShouldReturnListOfModels()
        {
            // Arrange
            var mock = new Mock<IThirdPartyEventRepository>();
            var expected = new List<ThirdPartyEvent>
            {
                new ThirdPartyEvent { Id = 123 },
                new ThirdPartyEvent { Id = 1230 },
            };
            mock.Setup(a => a.GetAll()).Returns(expected);
            var controller = new HomeController(mock.Object, Mock.Of<IJsonSerializerService<ThirdPartyEvent>>());

            // Act
            ViewResult result = controller.Index() as ViewResult;
            var actual = result.Model;

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void Create_WhenCreateExist_ShouldReturnNotNull()
        {
            // Arrange
            var controller = new HomeController(Mock.Of<IThirdPartyEventRepository>(), Mock.Of<IJsonSerializerService<ThirdPartyEvent>>());

            // Act
            ViewResult result = controller.Create() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public void Create_WhenObjectExist_ShouldReturnCreatedObject()
        {
            // Arrange
            var serializer = new JsonSerializerService<ThirdPartyEvent>();
            var thirdPartyEventRepository = new ThirdPartyEventRepository(serializer);

            string filePath = Path.GetFullPath(@"..\..\test\ThirdPartyEventEditor.UnitTests\App_Data\VangogTestImage.PNG");
            var stream = new FileStream(filePath, FileMode.Open);
            var file = new Mock<HttpPostedFileBase>();
            file.Setup(x => x.InputStream).Returns(stream);
            file.Setup(x => x.ContentLength).Returns((int)stream.Length);
            file.Setup(x => x.FileName).Returns(stream.Name);

            var expected = new ThirdPartyEvent
            {
                Description = "asd",
                EndDate = new DateTime(3022, 03, 01, 00, 00, 00),
                Name = "asd",
                PosterImage = "asd",
                StartDate = new DateTime(3021, 03, 01, 00, 00, 00),
            };

            var controller = new HomeController(thirdPartyEventRepository, Mock.Of<IJsonSerializerService<ThirdPartyEvent>>());

            // Act
            controller.Create(expected, file.Object);
            expected.Id = thirdPartyEventRepository.GetAll().Last().Id;
            expected.PosterImage = thirdPartyEventRepository.GetAll().Last().PosterImage;
            var actual = thirdPartyEventRepository.GetAll().Last();

            // Clear
            thirdPartyEventRepository.Delete(actual.Id);

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void Edit_WhenIdExist_ShouldReturnModel()
        {
            // Arrange
            var mock = new Mock<IThirdPartyEventRepository>();
            var expected = new ThirdPartyEvent { Id = 123 };

            mock.Setup(a => a.GetById(expected.Id)).Returns(expected);
            var controller = new HomeController(mock.Object, Mock.Of<IJsonSerializerService<ThirdPartyEvent>>());

            // Act
            ViewResult result = controller.Edit(expected.Id) as ViewResult;
            var actual = result.Model;

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void Edit_WhenObjectExist_ShouldReturnUpdateObject()
        {
            // Arrange
            var serializer = new JsonSerializerService<ThirdPartyEvent>();
            var thirdPartyEventRepository = new ThirdPartyEventRepository(serializer);
            var controller = new HomeController(thirdPartyEventRepository, Mock.Of<IJsonSerializerService<ThirdPartyEvent>>());

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

            controller.Edit(expected);

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
    }
}
