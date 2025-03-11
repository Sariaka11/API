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
                AgenceNom = f.Agence?.Nom
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
                AgenceNom = fourniture.Agence?.Nom
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
                AgenceNom = f.Agence?.Nom
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
                AgenceNom = f.Agence?.Nom
            });
        }

        public async Task<FournitureDto> CreateFournitureAsync(CreateFournitureDto fournitureDto)
        {
            // Vérifier si l'agence existe
            var agence = await _context.Agences.FindAsync(fournitureDto.AgenceId);
            if (agence == null)
                throw new Exception("L'agence spécifiée n'existe pas.");

            // Créer la nouvelle fourniture
            var fourniture = new Fourniture
            {
                Nom = fournitureDto.Nom,
                Date = fournitureDto.Date,
                AgenceId = fournitureDto.AgenceId
            };

            _context.Fournitures.Add(fourniture);
            await _context.SaveChangesAsync();

            return new FournitureDto
            {
                Id = fourniture.Id,
                Nom = fourniture.Nom,
                Date = fourniture.Date,
                AgenceId = fourniture.AgenceId,
                AgenceNom = agence.Nom
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

            // Mettre à jour la fourniture
            fourniture.Nom = fournitureDto.Nom;
            fourniture.Date = fournitureDto.Date;
            fourniture.AgenceId = fournitureDto.AgenceId;

            await _context.SaveChangesAsync();

            return new FournitureDto
            {
                Id = fourniture.Id,
                Nom = fourniture.Nom,
                Date = fourniture.Date,
                AgenceId = fourniture.AgenceId,
                AgenceNom = agence.Nom
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

