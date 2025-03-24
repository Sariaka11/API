using Microsoft.EntityFrameworkCore;
using API.Models;

namespace API.Data
{
  public class AppDbContext : DbContext
  {
      public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
      {
      }

 public DbSet<User> Users { get; set; } = null!;
      public DbSet<Agence> Agences { get; set; } = null!;
      public DbSet<Fourniture> Fournitures { get; set; } = null!;
      protected override void OnModelCreating(ModelBuilder modelBuilder)
      {
          base.OnModelCreating(modelBuilder);

          // Configuration de l'entité User
          modelBuilder.Entity<User>(entity =>
          {
              entity.ToTable("USERS");
              entity.HasIndex(e => e.Email).IsUnique();
              entity.Property(e => e.Id).HasColumnName("ID");
              entity.Property(e => e.Nom).HasColumnName("NOM").IsRequired().HasMaxLength(100);
              entity.Property(e => e.Prenom).HasColumnName("PRENOM").IsRequired().HasMaxLength(100);
              entity.Property(e => e.Email).HasColumnName("EMAIL").IsRequired().HasMaxLength(100);
              entity.Property(e => e.MotDePasse).HasColumnName("MOT_DE_PASSE").IsRequired();
              entity.Property(e => e.AgenceId).HasColumnName("AGENCE_ID");

              // Relation avec Agence
              entity.HasOne(d => d.Agence)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.AgenceId)
                    .OnDelete(DeleteBehavior.Restrict);
          });

          // Configuration de l'entité Agence
          modelBuilder.Entity<Agence>(entity =>
          {
              entity.ToTable("AGENCES");
              entity.HasIndex(e => e.Numero).IsUnique();
              entity.Property(e => e.Id).HasColumnName("ID");
              entity.Property(e => e.Numero).HasColumnName("NUMERO").IsRequired().HasMaxLength(50);
              entity.Property(e => e.Nom).HasColumnName("NOM").IsRequired().HasMaxLength(100);
          });

          // Configuration de l'entité Fourniture
          modelBuilder.Entity<Fourniture>(entity =>
          {
              entity.ToTable("FOURNITURES");
              entity.Property(e => e.Id).HasColumnName("ID");
              entity.Property(e => e.Nom).HasColumnName("NOM").IsRequired().HasMaxLength(100);
              entity.Property(e => e.Date).HasColumnName("DATE").IsRequired();
              entity.Property(e => e.AgenceId).HasColumnName("AGENCE_ID");
              // Ajout de la configuration pour la colonne Categorie
              entity.Property(e => e.Categorie).HasColumnName("CATEGORIE").IsRequired().HasMaxLength(100);
              entity.Property(e => e.PrixUnitaire).HasColumnName("PRIX_UNITAIRE").IsRequired().HasColumnType("decimal(18, 2)");
              entity.Property(e => e.Quantite).HasColumnName("QUANTITE").IsRequired();
              entity.Property(e => e.PrixTotal).HasColumnName("PRIX_TOTAL").IsRequired().HasColumnType("decimal(18, 2)");
              entity.Property(e => e.QuantiteRestante).HasColumnName("QUANTITE_RESTANTE").IsRequired();
              entity.Property(e => e.Montant).HasColumnName("MONTANT").IsRequired().HasColumnType("decimal(18, 2)");

              // Relation avec Agence
              entity.HasOne(d => d.Agence)
                    .WithMany(p => p.Fournitures)
                    .HasForeignKey(d => d.AgenceId)
                    .OnDelete(DeleteBehavior.Restrict);
          });
      }
  }
}

