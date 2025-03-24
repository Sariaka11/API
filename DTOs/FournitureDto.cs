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
      public string Categorie { get; set; } // Ajout de la propriété Categorie
      public decimal PrixUnitaire { get; set; }
      public int Quantite { get; set; }
      public decimal PrixTotal { get; set; }
      public int QuantiteRestante { get; set; }
      public decimal Montant { get; set; }
  }

  public class CreateFournitureDto
  {
      [Required]
      public string Nom { get; set; }

      [Required]
      public DateTime Date { get; set; }

      [Required]
      public int AgenceId { get; set; }

      [Required]
      [MaxLength(100)]
      public string Categorie { get; set; } // Ajout de la propriété Categorie

      [Required]
      [Range(0.01, double.MaxValue, ErrorMessage = "Le prix unitaire doit être supérieur à 0")]
      public decimal PrixUnitaire { get; set; }

      [Required]
      [Range(1, int.MaxValue, ErrorMessage = "La quantité doit être supérieure à 0")]
      public int Quantite { get; set; }
  }

  public class UpdateFournitureDto
  {
      [Required]
      public string Nom { get; set; }

      [Required]
      public DateTime Date { get; set; }

      [Required]
      public int AgenceId { get; set; }

      [Required]
      [MaxLength(100)]
      public string Categorie { get; set; } // Ajout de la propriété Categorie

      [Required]
      [Range(0.01, double.MaxValue, ErrorMessage = "Le prix unitaire doit être supérieur à 0")]
      public decimal PrixUnitaire { get; set; }

      [Required]
      [Range(1, int.MaxValue, ErrorMessage = "La quantité doit être supérieure à 0")]
      public int Quantite { get; set; }
  }
}

