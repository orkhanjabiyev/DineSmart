using DineSmart.Data;
using DineSmart.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DineSmart.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserPreferencesController : ControllerBase
    {
        private readonly DineSmartDbContext _context;

        public UserPreferencesController(DineSmartDbContext context)
        {
            _context = context;
        }

        // Получить предпочтения по Id пользователя
        [HttpGet("{userId}")]
        public async Task<ActionResult<UserPreference>> GetUserPreferences(string userId)
        {
            var preference = await _context.UserPreferences
                .FirstOrDefaultAsync(up => up.UserId == userId);

            if (preference == null)
                return NotFound();

            return preference;
        }

        // Обновить предпочтения пользователя
        [HttpPut("{userId}")]
        public async Task<IActionResult> UpdateUserPreferences(string userId, UserPreference preference)
        {
            if (userId != preference.UserId)
                return BadRequest();

            _context.Entry(preference).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.UserPreferences.Any(up => up.UserId == userId))
                    return NotFound();
                throw;
            }

            return NoContent();
        }

        // Создать новые предпочтения для пользователя
        [HttpPost]
        public async Task<ActionResult<UserPreference>> CreateUserPreferences(UserPreference preference)
        {
            _context.UserPreferences.Add(preference);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetUserPreferences), new { userId = preference.UserId }, preference);
        }
    }
}
