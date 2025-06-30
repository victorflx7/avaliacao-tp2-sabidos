using StockApp.Application.DTOs;
using Microsoft.Extensions.Configuration;
using StockApp.Domain.Interfaces;
using Microsoft.IdentityModel.Tokens;
using StockApp.Domain.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace StockApp.Infra.Data.Identity
{
    public interface IAuthService
    {
        Task<TokenResponseDTO> AuthenticateAsync(string username, string password);
    }

    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;

        public AuthService(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }

        public async Task<TokenResponseDTO> AuthenticateAsync(string username, string password)
        {
            var user = await _userRepository.GetByUsernameAsync(username , password);

            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.Password))
            {
                return null;
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["JwtSettings:SecretKey"]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Role, user.Role),
                    new Claim("Permission" , "CanManageStock")
                }),
                Expires = DateTime.UtcNow.AddMinutes(
                    double.Parse(_configuration["JwtSettings:AccessTokenExpiration"])),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return new TokenResponseDTO
            {
                Token = tokenHandler.WriteToken(token),
                Expiration = tokenDescriptor.Expires.Value
            };
        }
    }
}
