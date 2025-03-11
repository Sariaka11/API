using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    public class Fourniture
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Nom { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public int AgenceId { get; set; }

        [ForeignKey("AgenceId")]
        public virtual Agence Agence { get; set; }
    }
}

