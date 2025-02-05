using DineSmart.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DineSmart.Services
{
    public class AuthService
    {
        private readonly IConfiguration _configuration;

        public AuthService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // Метод для генерации JWT токена с ролями
        public string GenerateJwtToken(User user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role)  // Добавляем роль пользователя
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        // Метод для аутентификации пользователя (теперь он возвращает роль)
        public string? AuthenticateUser(string username, string password)
        {
            // В реальном проекте здесь должна быть проверка пользователя через БД
            if (username == "admin" && password == "admin123")
            {
                return "Admin";  // Админ
            }
            else if (username == "user" && password == "user123")
            {
                return "User";  // Обычный пользователь
            }

            return null; // Если данные неверные
        }
    }
}
