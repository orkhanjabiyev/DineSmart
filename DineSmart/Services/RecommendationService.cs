using DineSmart.Data;
using DineSmart.Models;
using Microsoft.EntityFrameworkCore;

namespace DineSmart.Services
{
    public class RecommendationService
    {
        private readonly DineSmartDbContext _context;

        public RecommendationService(DineSmartDbContext context)
        {
            _context = context;
        }

        // Метод для получения рекомендаций с учётом предпочтений пользователя
        public async Task<IEnumerable<Restaurant>> GetRecommendations(string userId, int count = 5)
        {
            // Получаем предпочтения пользователя из базы
            var userPreference = await _context.UserPreferences
                .FirstOrDefaultAsync(up => up.UserId == userId);

            // Если предпочтений пользователя нет, возвращаем рестораны без фильтра
            if (userPreference == null)
            {
                return await _context.Restaurants
                    .OrderByDescending(r => r.Rating) // Сортировка по убыванию рейтинга
                    .Take(count) // Лимит количества ресторанов
                    .ToListAsync();
            }

            // Если предпочтения есть, фильтруем рестораны по этим предпочтениям
            var query = _context.Restaurants.AsQueryable();

            // Фильтрация по любимой кухне
            if (!string.IsNullOrEmpty(userPreference.FavoriteCuisine))
            {
                query = query.Where(r => r.Cuisine.Contains(userPreference.FavoriteCuisine));
            }

            // Фильтрация по минимальному рейтингу
            if (userPreference.MinimumRating > 0)
            {
                query = query.Where(r => r.Rating >= userPreference.MinimumRating);
            }

            // Возвращаем отфильтрованные рестораны с сортировкой по рейтингу
            return await query
                .OrderByDescending(r => r.Rating) // Сортировка по убыванию рейтинга
                .Take(count) // Лимит количества ресторанов
                .ToListAsync();
        }
    }
}
