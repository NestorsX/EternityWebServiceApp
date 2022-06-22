using EternityWebServiceApp.Controllers;
using EternityWebServiceApp.Interfaces;
using EternityWebServiceApp.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using Xunit;

namespace EternityWebServiceApp.Tests.ControllerTests
{
    public class CityControllerTests
    {
        private readonly Mock<IImageRepository<City>> _mockRepo;
        private readonly CityController _controller;

        public CityControllerTests()
        {
            _mockRepo = new Mock<IImageRepository<City>>();
            _controller = new CityController(_mockRepo.Object);
            _mockRepo.Setup(repo => repo.Get()).Returns(new List<City>()
            {
                new City
                {
                    CityId = 1,
                    Title = "TestTitle1",
                    Description = "TestDescription1"
                },
                new City
                {
                    CityId = 2,
                    Title = "TestTitle2",
                    Description = "TestDescription2"
                }
            });

            _mockRepo.Setup(repo => repo.Get(1)).Returns(new City
            {
                CityId = 1,
                Title = "TestTitle1",
                Description = "TestDescription1"
            });
        }

        [Fact]
        public void Index_ReturnsView()
        {
            var result = _controller.Index();
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Index_ReturnsExactNumberOfObjects()
        {
            var result = _controller.Index();
            var viewResult = Assert.IsType<ViewResult>(result);
            var objects = Assert.IsType<List<City>>(viewResult.Model);
            Assert.Equal(2, objects.Count);
        }

        [Fact]
        public void Create_ReturnsView()
        {
            var result = _controller.Create();
            Assert.IsType<ViewResult>(result);
        }


        [Fact]
        public void Create_RedirectsToIndex()
        {
            var obj = new City
            {
                CityId = null,
                Title = "TestTitle3",
                Description = "TestDescription3"
            };

            var result = _controller.Create(obj, null);
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
        }

        [Fact]
        public void Edit_ReturnsView()
        {
            var result = _controller.Edit(1);
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Edit_InvalidId_ReturnsNotFoundResult()
        {
            var result = _controller.Edit(-1);
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void Edit_RedirectsToIndex()
        {
            var newObj = new City
            {
                CityId = 1,
                Title = "TestTitle3",
                Description = "TestDescription3"
            };

            var result = _controller.Edit(newObj, null);
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
        }

        [Fact]
        public void Delete_ReturnsView()
        {
            var result = _controller.ConfirmDelete(1);
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Delete_InvalidId_ReturnsNotFoundResult()
        {
            var result = _controller.ConfirmDelete(-1);
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void Delete_ActionExecuted_RedirectsToIndexAction()
        {
            var result = _controller.Delete(1);
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
        }
    }
}
