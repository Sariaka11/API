using System;
using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class FournitureDto
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public DateTime Date { get; set; }
        public int AgenceId { get; set; }
        public string AgenceNom { get; set; }
    }

    public class CreateFournitureDto
    {
        [Required]
        public string Nom { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public int AgenceId { get; set; }
    }

    public class UpdateFournitureDto
    {
        [Required]
        public string Nom { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public int AgenceId { get; set; }
    }
}

