using DineSmart.Models;
using DineSmart.Services;
using Microsoft.AspNetCore.Mvc;

namespace DineSmart.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest loginRequest)
        {
            // Проверяем пользователя и получаем его роль
            var role = _authService.AuthenticateUser(loginRequest.Username, loginRequest.Password);

            if (role == null)
            {
                return Unauthorized(new { message = "Invalid credentials" });
            }

            // Создаем пользователя с ролью
            var user = new User
            {
                Id = 1, // В реальном приложении ID берется из базы данных
                Username = loginRequest.Username,
                Role = role
            };

            // Генерируем JWT токен
            var token = _authService.GenerateJwtToken(user);

            return Ok(new { token, role });
        }
    }

    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
