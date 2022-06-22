using EternityWebServiceApp.Controllers;
using EternityWebServiceApp.Interfaces;
using EternityWebServiceApp.Models;
using EternityWebServiceApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Collections.Generic;
using Xunit;

namespace EternityWebServiceApp.Tests.ControllerTests
{
    public class DataReferenceControllerTests
    {
        private readonly Mock<IRepository<DataReferenceViewModel>> _mockRepo;
        private readonly DataReferenceController _controller;

        public DataReferenceControllerTests()
        {
            _mockRepo = new Mock<IRepository<DataReferenceViewModel>>();
            var optionsBuilder = new DbContextOptionsBuilder<EternityDBContext>();
            var options = optionsBuilder.UseSqlServer("Server=ASUSROG;Initial Catalog=Eternity;Integrated Security=True; TrustServerCertificate=true").Options;
            _controller = new DataReferenceController(new EternityDBContext(options), _mockRepo.Object);
            _mockRepo.Setup(repo => repo.Get()).Returns(new List<DataReferenceViewModel>()
            {
                new DataReferenceViewModel
                {
                    DataReferenceId = 1,
                    CityId = 1,
                    CityName = "City1",
                    AttractionId = 1,
                    AttractionName = "Attraction1"
                },
                new DataReferenceViewModel
                {
                    DataReferenceId = 2,
                    CityId = 2,
                    CityName = "City2",
                    AttractionId = 2,
                    AttractionName = "Attraction2"
                }
            });

            _mockRepo.Setup(repo => repo.Get(2)).Returns(new DataReferenceViewModel
            {
                DataReferenceId = 2,
                CityId = 2,
                CityName = "City2",
                AttractionId = 2,
                AttractionName = "Attraction2"
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
            var objects = Assert.IsType<List<DataReferenceViewModel>>(viewResult.Model);
            Assert.Equal(2, objects.Count);
        }

        [Fact]
        public void Create_ReturnsView()
        {
            var result = _controller.Create();
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Create_InvalidModelState_ReturnsView()
        {
            _controller.ModelState.AddModelError("AttractionId", "Такая связь уже существует");
            var obj = new DataReferenceViewModel
            {
                DataReferenceId = null,
                CityId = 3,
                CityName = "City3",
                AttractionId = 3,
                AttractionName = "Attraction3"
            };

            var result = _controller.Create(obj);
            var viewResult = Assert.IsType<ViewResult>(result);
            var testObj = Assert.IsType<DataReferenceViewModel>(viewResult.Model);
            Assert.Equal(obj.CityId, testObj.CityId);
            Assert.Equal(obj.AttractionId, testObj.AttractionId);
        }

        [Fact]
        public void Create_RedirectsToIndex()
        {
            var obj = new DataReferenceViewModel
            {
                DataReferenceId = null,
                CityId = 3,
                CityName = "City3",
                AttractionId = 3,
                AttractionName = "Attraction3"
            };

            var result = _controller.Create(obj);
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
        }

        [Fact]
        public void Edit_ReturnsView()
        {
            var result = _controller.Edit(2);
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Edit_InvalidModelState_ReturnsView()
        {
            _controller.ModelState.AddModelError("AttractionId", "Такая связь уже существует");
            var newObj = new DataReferenceViewModel
            {
                DataReferenceId = 2,
                CityId = 3,
                CityName = "City3",
                AttractionId = 3,
                AttractionName = "Attraction3"
            };

            var result = _controller.Edit(newObj);
            var viewResult = Assert.IsType<ViewResult>(result);
            var testEmployee = Assert.IsType<DataReferenceViewModel>(viewResult.Model);
            Assert.Equal(newObj.CityId, testEmployee.CityId);
            Assert.Equal(newObj.AttractionId, testEmployee.AttractionId);
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
            var newObj = new DataReferenceViewModel
            {
                DataReferenceId = 2,
                CityId = 3,
                CityName = "City3",
                AttractionId = 3,
                AttractionName = "Attraction3"
            };

            var result = _controller.Edit(newObj);
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
        }

        [Fact]
        public void Delete_ReturnsView()
        {
            var result = _controller.ConfirmDelete(2);
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
            var result = _controller.Delete(2);
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
        }
    }
}
