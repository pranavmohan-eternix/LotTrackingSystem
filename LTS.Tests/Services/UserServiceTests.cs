using Xunit;
using Moq;
using System;
using LTS.Application.Services;
using LTS.Common.Interfaces;
using LTS.Common.Models;

namespace LTS.Tests.Services
{
    public class UserServiceTests
    {
        private readonly Mock<IUserRepository>
            _repoMock;

        private readonly UserService
            _service;

        public UserServiceTests()
        {
            _repoMock =
                new Mock<IUserRepository>();

            _service =
                new UserService(
                    _repoMock.Object);
        }

        [Fact]
        public void Register_ShouldThrow_WhenUsernameEmpty()
        {
            var user =
                new User
                {
                    Username = "",
                    Password = "123456"
                };

            var ex =
                Assert.Throws<Exception>(() =>
                    _service.Register(user));

            Assert.Equal(
                "Username required",
                ex.Message);
        }

        [Fact]
        public void Register_ShouldThrow_WhenPasswordEmpty()
        {
            var user =
                new User
                {
                    Username = "admin",
                    Password = ""
                };

            var ex =
                Assert.Throws<Exception>(() =>
                    _service.Register(user));

            Assert.Equal(
                "Password required",
                ex.Message);
        }

        [Fact]
        public void Register_ShouldThrow_WhenPasswordTooShort()
        {
            var user =
                new User
                {
                    Username = "admin",
                    Password = "123"
                };

            var ex =
                Assert.Throws<Exception>(() =>
                    _service.Register(user));

            Assert.Equal(
                "Password must be at least 6 characters",
                ex.Message);
        }

        [Fact]
        public void Register_ShouldThrow_WhenUsernameExists()
        {
            _repoMock
                .Setup(x =>
                    x.GetByUsername("admin"))
                .Returns(new User());

            var user =
                new User
                {
                    Username = "admin",
                    Password = "123456"
                };

            var ex =
                Assert.Throws<Exception>(() =>
                    _service.Register(user));

            Assert.Equal(
                "Username already exists",
                ex.Message);
        }

        [Fact]
        public void Register_ShouldCallRepository()
        {
            _repoMock
                .Setup(x =>
                    x.GetByUsername("admin"))
                .Returns((User)null);

            var user =
                new User
                {
                    Username = "admin",
                    Password = "123456",
                    Role = "Admin"
                };

            _service.Register(user);

            _repoMock.Verify(
                x => x.Add(It.IsAny<User>()),
                Times.Once);
        }

        [Fact]
        public void Login_ShouldReturnUser_WhenValid()
        {
            _repoMock
                .Setup(x =>
                    x.GetByUsername("admin"))
                .Returns(new User
                {
                    Username = "admin",
                    Password = "123456",
                    IsActive = true
                });

            var result =
                _service.Login(
                    "admin",
                    "123456");

            Assert.NotNull(result);

            Assert.Equal(
                "admin",
                result.Username);
        }

        [Fact]
        public void Login_ShouldReturnNull_WhenPasswordWrong()
        {
            _repoMock
                .Setup(x =>
                    x.GetByUsername("admin"))
                .Returns(new User
                {
                    Username = "admin",
                    Password = "123456",
                    IsActive = true
                });

            var result =
                _service.Login(
                    "admin",
                    "wrong");

            Assert.Null(result);
        }

        [Fact]
        public void Login_ShouldReturnNull_WhenUserInactive()
        {
            _repoMock
                .Setup(x =>
                    x.GetByUsername("admin"))
                .Returns(new User
                {
                    Username = "admin",
                    Password = "123456",
                    IsActive = false
                });

            var result =
                _service.Login(
                    "admin",
                    "123456");

            Assert.Null(result);
        }
    }
}