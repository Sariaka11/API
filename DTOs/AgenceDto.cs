using System;
using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class AgenceDto
    {
        public int Id { get; set; }
        public string Numero { get; set; }
        public string Nom { get; set; }
    }

    public class CreateAgenceDto
    {
        [Required]
        public string Numero { get; set; }

        [Required]
        public string Nom { get; set; }
    }

    public class UpdateAgenceDto
    {
        [Required]
        public string Numero { get; set; }

        [Required]
        public string Nom { get; set; }
    }
}

