﻿using System.ComponentModel.DataAnnotations;

namespace SuperHeroAPI.Application.DTOs
{
    public class LoginDto
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
