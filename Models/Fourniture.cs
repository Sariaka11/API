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

        // Nouveaux attributs
        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal PrixUnitaire { get; set; }

        [Required]
        public int Quantite { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal PrixTotal { get; private set; }

        [Required]
        public int QuantiteRestante { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Montant { get; private set; }

        // Méthode pour calculer les valeurs dérivées
        public void CalculerValeurs()
        {
            PrixTotal = PrixUnitaire * Quantite;
            Montant = PrixUnitaire * QuantiteRestante;
        }
    }
}

