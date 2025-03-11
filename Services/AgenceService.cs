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
    public class AgenceService : IAgenceService
    {
        private readonly AppDbContext _context;

        public AgenceService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<AgenceDto>> GetAllAgencesAsync()
        {
            var agences = await _context.Agences.ToListAsync();
            return agences.Select(a => new AgenceDto
            {
                Id = a.Id,
                Numero = a.Numero,
                Nom = a.Nom
            });
        }

        public async Task<AgenceDto> GetAgenceByIdAsync(int id)
        {
            var agence = await _context.Agences.FindAsync(id);
            if (agence == null)
                return null;

            return new AgenceDto
            {
                Id = agence.Id,
                Numero = agence.Numero,
                Nom = agence.Nom
            };
        }

        public async Task<AgenceDto> GetAgenceByNumeroAsync(string numero)
        {
            var agence = await _context.Agences.FirstOrDefaultAsync(a => a.Numero == numero);
            if (agence == null)
                return null;

            return new AgenceDto
            {
                Id = agence.Id,
                Numero = agence.Numero,
                Nom = agence.Nom
            };
        }

        public async Task<AgenceDto> CreateAgenceAsync(CreateAgenceDto agenceDto)
        {
            // Vérifier si le numéro existe déjà
            if (await _context.Agences.AnyAsync(a => a.Numero == agenceDto.Numero))
                throw new Exception("Une agence avec ce numéro existe déjà.");

            // Créer la nouvelle agence
            var agence = new Agence
            {
                Numero = agenceDto.Numero,
                Nom = agenceDto.Nom
            };

            _context.Agences.Add(agence);
            await _context.SaveChangesAsync();

            return new AgenceDto
            {
                Id = agence.Id,
                Numero = agence.Numero,
                Nom = agence.Nom
            };
        }

        public async Task<AgenceDto> UpdateAgenceAsync(int id, UpdateAgenceDto agenceDto)
        {
            var agence = await _context.Agences.FindAsync(id);
            if (agence == null)
                return null;

            // Vérifier si le numéro existe déjà pour une autre agence
            if (await _context.Agences.AnyAsync(a => a.Numero == agenceDto.Numero && a.Id != id))
                throw new Exception("Une autre agence avec ce numéro existe déjà.");

            // Mettre à jour l'agence
            agence.Numero = agenceDto.Numero;
            agence.Nom = agenceDto.Nom;

            await _context.SaveChangesAsync();

            return new AgenceDto
            {
                Id = agence.Id,
                Numero = agence.Numero,
                Nom = agence.Nom
            };
        }

        public async Task<bool> DeleteAgenceAsync(int id)
        {
            var agence = await _context.Agences.FindAsync(id);
            if (agence == null)
                return false;

            // Vérifier si l'agence a des utilisateurs ou des fournitures
            bool hasUsers = await _context.Users.AnyAsync(u => u.AgenceId == id);
            bool hasFournitures = await _context.Fournitures.AnyAsync(f => f.AgenceId == id);

            if (hasUsers || hasFournitures)
                throw new Exception("Impossible de supprimer l'agence car elle est liée à des utilisateurs ou des fournitures.");

            _context.Agences.Remove(agence);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}

