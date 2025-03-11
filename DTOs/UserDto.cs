using System;
using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string Email { get; set; }
        public int AgenceId { get; set; }
        public string AgenceNom { get; set; }
    }

    public class CreateUserDto
    {
        [Required]
        public string Nom { get; set; }

        [Required]
        public string Prenom { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(6)]
        public string MotDePasse { get; set; }

        [Required]
        public int AgenceId { get; set; }
    }

    public class UpdateUserDto
    {
        [Required]
        public string Nom { get; set; }

        [Required]
        public string Prenom { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public string MotDePasse { get; set; }

        [Required]
        public int AgenceId { get; set; }
    }

    public class LoginDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string MotDePasse { get; set; }
    }
}

