﻿using System.ComponentModel.DataAnnotations;

namespace dz_GuestBook.Models
{
    public class RegisterModel
    {
        [Required]
        public string? Name { get; set; }

        [Required]
        public string? Login { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public string? PasswordConfirm { get; set; }
    }
}
