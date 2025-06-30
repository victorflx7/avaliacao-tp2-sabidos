using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using StockApp.API.Controllers;
using StockApp.Application.DTOs;
using StockApp.Infra.Data.Identity;
using System;
using System.Threading.Tasks;

namespace StockApp.Domain.Test
{
    public class TokenControllerTests
    {
        [Fact]
        public async Task Login_ValidCredentials_ReturnsToken()
        {
            // Arrange
            var authServiceMock = new Mock<IAuthService>();
            var tokenController = new TokenController(authServiceMock.Object);

            authServiceMock.Setup(service => service.AuthenticateAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(new TokenResponseDTO
                {
                    Token = "token",
                    Expiration = DateTime.UtcNow.AddMinutes(60)
                });

            var userLoginDto = new UserLoginDTO
            {
                Username = "testuser",
                Password = "password"
            };

            // Act
            var result = await tokenController.Login(userLoginDto) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.IsType<TokenResponseDTO>(result.Value);
        }

        [Fact]
        public async Task Login_InvalidCredentials_ReturnsUnauthorized()
        {
            // Arrange
            var authServiceMock = new Mock<IAuthService>();
            var tokenController = new TokenController(authServiceMock.Object);

            authServiceMock.Setup(service => service.AuthenticateAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync((TokenResponseDTO)null);

            var userLoginDto = new UserLoginDTO
            {
                Username = "wronguser",
                Password = "wrongpassword"
            };

            // Act
            var result = await tokenController.Login(userLoginDto) as UnauthorizedObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(401, result.StatusCode);
            Assert.Equal("Invalid username or password", result.Value);
        }
    }
}