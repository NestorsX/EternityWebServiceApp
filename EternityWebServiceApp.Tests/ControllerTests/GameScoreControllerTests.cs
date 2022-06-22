using EternityWebServiceApp.Controllers;
using EternityWebServiceApp.Interfaces;
using EternityWebServiceApp.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using Xunit;

namespace EternityWebServiceApp.Tests.ControllerTests
{
    public class GameScoreControllerTests
    {
        private readonly Mock<IGameScoreRepository> _mockRepo;
        private readonly GameScoreController _controller;

        public GameScoreControllerTests()
        {
            _mockRepo = new Mock<IGameScoreRepository>();
            _controller = new GameScoreController(_mockRepo.Object);
            _mockRepo.Setup(repo => repo.Get()).Returns(new List<GameScore>()
            {
                new GameScore
                {
                    GameScoreId = 1,
                    GameId = 1,
                    UserId = 1,
                    Score = "Score1",
                    Game = new Game(),
                    User = new User()
                },
                new GameScore
                {
                    GameScoreId = 2,
                    GameId = 2,
                    UserId = 2,
                    Score = "Score2",
                    Game = new Game(),
                    User = new User()
                }
            });

            _mockRepo.Setup(repo => repo.Get(1)).Returns(new GameScore
            {
                GameScoreId = 1,
                GameId = 1,
                UserId = 1,
                Score = "Score1",
                Game = new Game(),
                User = new User()
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
            var objects = Assert.IsType<List<GameScore>>(viewResult.Model);
            Assert.Equal(2, objects.Count);
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
