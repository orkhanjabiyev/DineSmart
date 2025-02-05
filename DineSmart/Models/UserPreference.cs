namespace DineSmart.Models
{
    public class UserPreference
    {
        public int Id { get; set; }
        public string UserId { get; set; } 
        public string FavoriteCuisine { get; set; }
        public double MinimumRating { get; set; }
    }
}
