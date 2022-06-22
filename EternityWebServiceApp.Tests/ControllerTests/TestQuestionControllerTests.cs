using EternityWebServiceApp.Controllers;
using EternityWebServiceApp.Interfaces;
using EternityWebServiceApp.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using Xunit;

namespace EternityWebServiceApp.Tests.ControllerTests
{
    public class TestQuestionControllerTests
    {
        private readonly Mock<IImageRepository<TestQuestion>> _mockRepo;
        private readonly TestQuestionController _controller;

        public TestQuestionControllerTests()
        {
            _mockRepo = new Mock<IImageRepository<TestQuestion>>();
            _controller = new TestQuestionController(_mockRepo.Object);
            _mockRepo.Setup(repo => repo.Get()).Returns(new List<TestQuestion>()
            {
                new TestQuestion
                {
                    TestQuestionId = 1,
                    Question = "TestQuestion1",
                    RightAnswer = "TestAnswer1",
                    WrongAnswer1 = "TestAnswer2",
                    WrongAnswer2 = "TestAnswer3",
                    WrongAnswer3 = "TestAnswer4",
                },
                new TestQuestion
                {
                    TestQuestionId = 2,
                    Question = "TestQuestion2",
                    RightAnswer = "TestAnswer1",
                    WrongAnswer1 = "TestAnswer2",
                    WrongAnswer2 = "TestAnswer3",
                    WrongAnswer3 = "TestAnswer4",
                }
            });

            _mockRepo.Setup(repo => repo.Get(1)).Returns(new TestQuestion
            {
                TestQuestionId = 1,
                Question = "TestQuestion1",
                RightAnswer = "TestAnswer1",
                WrongAnswer1 = "TestAnswer2",
                WrongAnswer2 = "TestAnswer3",
                WrongAnswer3 = "TestAnswer4",
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
            var objects = Assert.IsType<List<TestQuestion>>(viewResult.Model);
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
            var obj = new TestQuestion
            {
                TestQuestionId = null,
                Question = "TestQuestion3",
                RightAnswer = "TestAnswer1",
                WrongAnswer1 = "TestAnswer2",
                WrongAnswer2 = "TestAnswer3",
                WrongAnswer3 = "TestAnswer4",
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
            var newObj = new TestQuestion
            {
                TestQuestionId = 1,
                Question = "TestQuestion3",
                RightAnswer = "TestAnswer1",
                WrongAnswer1 = "TestAnswer2",
                WrongAnswer2 = "TestAnswer3",
                WrongAnswer3 = "TestAnswer4",
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
