using DineSmart.Models;
using Microsoft.EntityFrameworkCore;

namespace DineSmart.Data
{
    public class DineSmartDbContext : DbContext
    {
        public DineSmartDbContext(DbContextOptions<DineSmartDbContext> options) : base(options) { }

        public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<UserPreference> UserPreferences { get; set; }
        public DbSet<User> Users { get; set; } = null!;

    }
}
