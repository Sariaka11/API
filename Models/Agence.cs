using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    public class Agence
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Numero { get; set; }

        [Required]
        public string Nom { get; set; }

        // Relations
        public virtual ICollection<User> Users { get; set; }
        public virtual ICollection<Fourniture> Fournitures { get; set; }

        public Agence()
        {
            Users = new HashSet<User>();
            Fournitures = new HashSet<Fourniture>();
        }
    }
}

