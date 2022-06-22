using EternityWebServiceApp.Controllers;
using EternityWebServiceApp.Interfaces;
using EternityWebServiceApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Collections.Generic;
using Xunit;

namespace EternityWebServiceApp.Tests.ControllerTests
{
    public class UserControllerTests
    {
        private readonly Mock<IUserRepository> _mockRepo;
        private readonly UserController _controller;

        public UserControllerTests()
        {
            _mockRepo = new Mock<IUserRepository>();
            var optionsBuilder = new DbContextOptionsBuilder<EternityDBContext>();
            var options = optionsBuilder.UseSqlServer("Server=ASUSROG;Initial Catalog=Eternity;Integrated Security=True; TrustServerCertificate=true").Options;
            _controller = new UserController(new EternityDBContext(options), _mockRepo.Object);
            _mockRepo.Setup(repo => repo.Get()).Returns(new List<User>()
            {
                new User
                {
                    UserId = 1,
                    UserName = "TestName1",
                    Email = "test1@mail.com",
                    Password = "testPassword1",
                    RoleId = 1
                },
                new User
                {
                    UserId = 2,
                    UserName = "TestName2",
                    Email = "test2@mail.com",
                    Password = "testPassword2",
                    RoleId = 2
                }
            });

            _mockRepo.Setup(repo => repo.Get(2)).Returns(new User
            {
                UserId = 2,
                UserName = "TestName2",
                Email = "test2@mail.com",
                Password = "testPassword2",
                RoleId = 1
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
            var objects = Assert.IsType<List<User>>(viewResult.Model);
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
            _controller.ModelState.AddModelError("Email", "Email уже используется");
            var obj = new User 
            {
                UserId = null,
                UserName = "TestName",
                Email = "test@mail.com",
                Password = "testPassword",
                RoleId = 2
            };

            var result = _controller.Create(obj, null);
            var viewResult = Assert.IsType<ViewResult>(result);
            var testObj = Assert.IsType<User>(viewResult.Model);
            Assert.Equal(obj.UserName, testObj.UserName);
            Assert.Equal(obj.Email, testObj.Email);
        }

        [Fact]
        public void Create_RedirectsToIndex()
        {
            var obj = new User
            {
                UserId = null,
                UserName = "TestName",
                Email = "test@mail.com",
                Password = "testPassword",
                RoleId = 2
            };

            var result = _controller.Create(obj, null);
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
            _controller.ModelState.AddModelError("Email", "Email уже используется");
            var newObj = new User
            {
                UserId = 2,
                UserName = "TestName3",
                Email = "test1@mail.com",
                Password = "testPassword3",
                RoleId = 1
            };

            var result = _controller.Edit(newObj, null);
            var viewResult = Assert.IsType<ViewResult>(result);
            var testEmployee = Assert.IsType<User>(viewResult.Model);
            Assert.Equal(newObj.UserName, testEmployee.UserName);
            Assert.Equal(newObj.Email, testEmployee.Email);
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
            var newObj = new User
            {
                UserId = 2,
                UserName = "TestName3",
                Email = "test3@mail.com",
                Password = "testPassword3",
                RoleId = 1
            };

            var result = _controller.Edit(newObj, null);
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
