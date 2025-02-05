using DineSmart.Models;

namespace DineSmart.Data
{
    public static class DbInitializer
    {
        public static void Initialize(DineSmartDbContext context)
        {
            if (context.Restaurants.Any()) return;

            var restaurants = new[]
            {
                new Restaurant { Name = "Fusion Club Sea Breeze", Address = "White residence, Sea Breeze Resort, Nardaran, Baku, Azerbaijan", Cuisine = "Italian", Rating = 4.9, PhoneNumber = "+994554230000" },
                new Restaurant { Name = "Bahar Cafe", Address = "İsmayıl bəy Qutqaşınlı, Baku, Azerbaijan", Cuisine = "Azerbaijani", Rating = 4.7, PhoneNumber = "+994506989950" },
                new Restaurant { Name = "Kefli Wine Bar", Address = "Terlan Eliyarbeyov, 4a, Baku, Azerbaijan", Cuisine = "Wine & Snacks", Rating = 4.8, PhoneNumber = "+994513089909" }
            };

            context.Restaurants.AddRange(restaurants);
            context.SaveChanges();
        }
    }
}
