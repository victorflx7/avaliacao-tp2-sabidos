using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using StockApp.Application.DTOs;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace StockApp.Domain.Test
{
    public class IntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public IntegrationTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task RegisterAndLogin_ValidCredentials_ReturnsToken()
        {
            // Arrange
            var userRegisterDto = new UserRegisterDTO
            {
                Username = "testuser",
                Password = "password",
                Role = "User"
            };

            var userLoginDto = new UserLoginDTO
            {
                Username = "testuser",
                Password = "password"
            };

            // Register
            var registerResponse = await _client.PostAsJsonAsync("/api/user/register", userRegisterDto);
            registerResponse.EnsureSuccessStatusCode();

            // Login
            var loginResponse = await _client.PostAsJsonAsync("/api/token/login", userLoginDto);
            loginResponse.EnsureSuccessStatusCode();

            var tokenResponse = await loginResponse.Content.ReadFromJsonAsync<TokenResponseDTO>();

            // Assert
            Assert.NotNull(tokenResponse);
            Assert.NotNull(tokenResponse.Token);
            Assert.True(tokenResponse.Expiration > DateTime.UtcNow);
        }
    }
}