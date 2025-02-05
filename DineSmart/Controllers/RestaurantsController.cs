using DineSmart.Data;
using DineSmart.Models;
using DineSmart.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace DineSmart.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]  // По умолчанию все эндпоинты требуют аутентификацию
    public class RestaurantsController : ControllerBase
    {
        private readonly DineSmartDbContext _context;
        private readonly RecommendationService _recommendationService;

        public RestaurantsController(DineSmartDbContext context, RecommendationService recommendationService)
        {
            _context = context;
            _recommendationService = recommendationService;
        }

        // Открытый доступ: поиск ресторанов
        [HttpGet("search")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<Restaurant>>> SearchRestaurants(
            string? cuisine = null,
            double? minRating = null,
            string? name = null)
        {
            var query = _context.Restaurants.AsQueryable();

            if (!string.IsNullOrEmpty(cuisine))
            {
                query = query.Where(r => r.Cuisine.Contains(cuisine));
            }

            if (minRating.HasValue)
            {
                query = query.Where(r => r.Rating >= minRating);
            }

            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(r => r.Name.Contains(name));
            }

            return await query.ToListAsync();
        }

        // Открытый доступ: получение всех ресторанов
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<Restaurant>>> GetAllRestaurants()
        {
            return await _context.Restaurants.ToListAsync();
        }

        // Открытый доступ: получение ресторана по Id
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<Restaurant>> GetRestaurantById(int id)
        {
            var restaurant = await _context.Restaurants.FindAsync(id);
            if (restaurant == null)
                return NotFound();

            return restaurant;
        }

        // Только администраторы могут создавать рестораны
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Restaurant>> CreateRestaurant(Restaurant restaurant)
        {
            _context.Restaurants.Add(restaurant);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetRestaurantById), new { id = restaurant.Id }, restaurant);
        }

        // Только администраторы могут обновлять рестораны
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateRestaurant(int id, Restaurant restaurant)
        {
            if (id != restaurant.Id)
                return BadRequest();

            _context.Entry(restaurant).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Restaurants.Any(r => r.Id == id))
                    return NotFound();
                throw;
            }

            return NoContent();
        }

        // Только администраторы могут удалять рестораны
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteRestaurant(int id)
        {
            var restaurant = await _context.Restaurants.FindAsync(id);
            if (restaurant == null)
                return NotFound();

            _context.Restaurants.Remove(restaurant);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // Открытый доступ: получение рекомендаций
        [HttpGet("recommendations")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<Restaurant>>> GetRecommendations(string? cuisine = null, int count = 5)
        {
            var recommendations = await _recommendationService.GetRecommendations(cuisine, count);
            return Ok(recommendations);
        }
    }
}
