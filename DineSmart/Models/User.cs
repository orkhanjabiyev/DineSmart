﻿using System.ComponentModel.DataAnnotations;

namespace DineSmart.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Username { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty; // Храним хеш пароля

        [Required]
        public string Role { get; set; } = "User";
    }
}
