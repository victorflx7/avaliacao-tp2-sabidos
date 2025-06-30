using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using StockApp.API.Controllers;
using StockApp.Domain.Interfaces;
using StockApp.Application.DTOs;
using StockApp.Domain.Entities;

namespace StockApp.Domain.Test
{
    public class UserControllerTests
    {
        [Fact]
        public async Task Register_ValidUser_ReturnsOk()
        {
            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock
                .Setup(repo => repo.AddAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(new User { Username = "testuser" });

            var userController = new UserController(userRepositoryMock.Object);

            var userRegisterDto = new UserRegisterDTO
            {
                Username = "testuser",
                Password = "password",
                Role = "User"
            };

            var result = await userController.Register(userRegisterDto) as OkObjectResult;

            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.IsType<User>(result.Value);
        }
    }
}