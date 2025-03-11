using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using API.Data;
using API.DTOs;
using API.Models;

namespace API.Services
{
    public class FournitureService : IFournitureService
    {
        private readonly AppDbContext _context;

        public FournitureService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<FournitureDto>> GetAllFournituresAsync()
        {
            var fournitures = await _context.Fournitures
                .Include(f => f.Agence)
                .ToListAsync();

            return fournitures.Select(f => new FournitureDto
            {
                Id = f.Id,
                Nom = f.Nom,
                Date = f.Date,
                AgenceId = f.AgenceId,
                AgenceNom = f.Agence?.Nom,
                PrixUnitaire = f.PrixUnitaire,
                Quantite = f.Quantite,
                PrixTotal = f.PrixTotal,
                QuantiteRestante = f.QuantiteRestante,
                Montant = f.Montant
            });
        }

        public async Task<FournitureDto> GetFournitureByIdAsync(int id)
        {
            var fourniture = await _context.Fournitures
                .Include(f => f.Agence)
                .FirstOrDefaultAsync(f => f.Id == id);

            if (fourniture == null)
                return null;

            return new FournitureDto
            {
                Id = fourniture.Id,
                Nom = fourniture.Nom,
                Date = fourniture.Date,
                AgenceId = fourniture.AgenceId,
                AgenceNom = fourniture.Agence?.Nom,
                PrixUnitaire = fourniture.PrixUnitaire,
                Quantite = fourniture.Quantite,
                PrixTotal = fourniture.PrixTotal,
                QuantiteRestante = fourniture.QuantiteRestante,
                Montant = fourniture.Montant
            };
        }

        public async Task<IEnumerable<FournitureDto>> GetFournituresByAgenceIdAsync(int agenceId)
        {
            var fournitures = await _context.Fournitures
                .Include(f => f.Agence)
                .Where(f => f.AgenceId == agenceId)
                .ToListAsync();

            return fournitures.Select(f => new FournitureDto
            {
                Id = f.Id,
                Nom = f.Nom,
                Date = f.Date,
                AgenceId = f.AgenceId,
                AgenceNom = f.Agence?.Nom,
                PrixUnitaire = f.PrixUnitaire,
                Quantite = f.Quantite,
                PrixTotal = f.PrixTotal,
                QuantiteRestante = f.QuantiteRestante,
                Montant = f.Montant
            });
        }

        public async Task<IEnumerable<FournitureDto>> GetFournituresByAgenceNumeroAsync(string numeroAgence)
        {
            var fournitures = await _context.Fournitures
                .Include(f => f.Agence)
                .Where(f => f.Agence.Numero == numeroAgence)
                .ToListAsync();

            return fournitures.Select(f => new FournitureDto
            {
                Id = f.Id,
                Nom = f.Nom,
                Date = f.Date,
                AgenceId = f.AgenceId,
                AgenceNom = f.Agence?.Nom,
                PrixUnitaire = f.PrixUnitaire,
                Quantite = f.Quantite,
                PrixTotal = f.PrixTotal,
                QuantiteRestante = f.QuantiteRestante,
                Montant = f.Montant
            });
        }

        public async Task<FournitureDto> CreateFournitureAsync(CreateFournitureDto fournitureDto)
        {
            // Vérifier si l'agence existe
            var agence = await _context.Agences.FindAsync(fournitureDto.AgenceId);
            if (agence == null)
                throw new Exception("L'agence spécifiée n'existe pas.");

            // Vérifier si une fourniture avec le même nom existe déjà
            var existingFourniture = await _context.Fournitures
                .FirstOrDefaultAsync(f => f.Nom == fournitureDto.Nom && f.AgenceId == fournitureDto.AgenceId);

            if (existingFourniture != null)
            {
                // Mettre à jour la fourniture existante
                existingFourniture.Date = fournitureDto.Date;
                existingFourniture.PrixUnitaire = fournitureDto.PrixUnitaire;
                
                // Ajouter la nouvelle quantité à la quantité restante
                existingFourniture.QuantiteRestante += fournitureDto.Quantite;
                existingFourniture.Quantite = fournitureDto.Quantite;
                
                // Recalculer les valeurs dérivées
                existingFourniture.CalculerValeurs();
                
                await _context.SaveChangesAsync();
                
                return new FournitureDto
                {
                    Id = existingFourniture.Id,
                    Nom = existingFourniture.Nom,
                    Date = existingFourniture.Date,
                    AgenceId = existingFourniture.AgenceId,
                    AgenceNom = agence.Nom,
                    PrixUnitaire = existingFourniture.PrixUnitaire,
                    Quantite = existingFourniture.Quantite,
                    PrixTotal = existingFourniture.PrixTotal,
                    QuantiteRestante = existingFourniture.QuantiteRestante,
                    Montant = existingFourniture.Montant
                };
            }
            
            // Créer une nouvelle fourniture
            var fourniture = new Fourniture
            {
                Nom = fournitureDto.Nom,
                Date = fournitureDto.Date,
                AgenceId = fournitureDto.AgenceId,
                PrixUnitaire = fournitureDto.PrixUnitaire,
                Quantite = fournitureDto.Quantite,
                QuantiteRestante = fournitureDto.Quantite // Initialement, la quantité restante est égale à la quantité
            };
            
            // Calculer les valeurs dérivées
            fourniture.CalculerValeurs();

            _context.Fournitures.Add(fourniture);
            await _context.SaveChangesAsync();

            return new FournitureDto
            {
                Id = fourniture.Id,
                Nom = fourniture.Nom,
                Date = fourniture.Date,
                AgenceId = fourniture.AgenceId,
                AgenceNom = agence.Nom,
                PrixUnitaire = fourniture.PrixUnitaire,
                Quantite = fourniture.Quantite,
                PrixTotal = fourniture.PrixTotal,
                QuantiteRestante = fourniture.QuantiteRestante,
                Montant = fourniture.Montant
            };
        }

        public async Task<FournitureDto> UpdateFournitureAsync(int id, UpdateFournitureDto fournitureDto)
        {
            var fourniture = await _context.Fournitures.FindAsync(id);
            if (fourniture == null)
                return null;

            // Vérifier si l'agence existe
            var agence = await _context.Agences.FindAsync(fournitureDto.AgenceId);
            if (agence == null)
                throw new Exception("L'agence spécifiée n'existe pas.");

            // Calculer l'ajustement de la quantité restante
            int quantiteAjustement = fournitureDto.Quantite - fourniture.Quantite;
            
            // Mettre à jour la fourniture
            fourniture.Nom = fournitureDto.Nom;
            fourniture.Date = fournitureDto.Date;
            fourniture.AgenceId = fournitureDto.AgenceId;
            fourniture.PrixUnitaire = fournitureDto.PrixUnitaire;
            fourniture.Quantite = fournitureDto.Quantite;
            
            // Ajuster la quantité restante
            fourniture.QuantiteRestante += quantiteAjustement;
            
            // Recalculer les valeurs dérivées
            fourniture.CalculerValeurs();

            await _context.SaveChangesAsync();

            return new FournitureDto
            {
                Id = fourniture.Id,
                Nom = fourniture.Nom,
                Date = fourniture.Date,
                AgenceId = fourniture.AgenceId,
                AgenceNom = agence.Nom,
                PrixUnitaire = fourniture.PrixUnitaire,
                Quantite = fourniture.Quantite,
                PrixTotal = fourniture.PrixTotal,
                QuantiteRestante = fourniture.QuantiteRestante,
                Montant = fourniture.Montant
            };
        }

        public async Task<bool> DeleteFournitureAsync(int id)
        {
            var fourniture = await _context.Fournitures.FindAsync(id);
            if (fourniture == null)
                return false;

            _context.Fournitures.Remove(fourniture);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}

