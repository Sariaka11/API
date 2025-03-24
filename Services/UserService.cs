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
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;
        private readonly IPasswordHasher _passwordHasher;

        public UserService(AppDbContext context, IPasswordHasher passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
        }

        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            var users = await _context.Users
                .Include(u => u.Agence)
                .ToListAsync();

            return users.Select(u => new UserDto
            {
                Id = u.Id,
                Nom = u.Nom,
                Prenom = u.Prenom,
                Email = u.Email,
                AgenceId = u.AgenceId,
                AgenceNom = u.Agence?.Nom
            });
        }

        public async Task<UserDto> GetUserByIdAsync(int id)
        {
            var user = await _context.Users
                .Include(u => u.Agence)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
                return null;

            return new UserDto
            {
                Id = user.Id,
                Nom = user.Nom,
                Prenom = user.Prenom,
                Email = user.Email,
                AgenceId = user.AgenceId,
                AgenceNom = user.Agence?.Nom
            };
        }

        public async Task<UserDto> CreateUserAsync(CreateUserDto userDto)
        {
            // Vérifier si l'email existe déjà
            if (await _context.Users.AnyAsync(u => u.Email == userDto.Email))
                throw new Exception("Un utilisateur avec cet email existe déjà.");

            // Convertir AgenceId de string à int
            if (!int.TryParse(userDto.AgenceId, out int agenceId))
                throw new Exception("L'ID de l'agence doit être un nombre valide.");

            // Vérifier si l'agence existe
            var agence = await _context.Agences.FindAsync(agenceId);
            if (agence == null)
                throw new Exception("L'agence spécifiée n'existe pas.");

            // Créer le nouvel utilisateur
            var user = new User
            {
                Nom = userDto.Nom,
                Prenom = userDto.Prenom,
                Email = userDto.Email,
                MotDePasse = _passwordHasher.HashPassword(userDto.MotDePasse),
                AgenceId = agenceId
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return new UserDto
            {
                Id = user.Id,
                Nom = user.Nom,
                Prenom = user.Prenom,
                Email = user.Email,
                AgenceId = user.AgenceId,
                AgenceNom = agence.Nom
            };
        }

        public async Task<UserDto> UpdateUserAsync(int id, UpdateUserDto userDto)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return null;

            // Vérifier si l'email existe déjà pour un autre utilisateur
            if (await _context.Users.AnyAsync(u => u.Email == userDto.Email && u.Id != id))
                throw new Exception("Un autre utilisateur avec cet email existe déjà.");

            // Convertir AgenceId de string à int
            if (!int.TryParse(userDto.AgenceId, out int agenceId))
                throw new Exception("L'ID de l'agence doit être un nombre valide.");

            // Vérifier si l'agence existe
            var agence = await _context.Agences.FindAsync(agenceId);
            if (agence == null)
                throw new Exception("L'agence spécifiée n'existe pas.");

            // Mettre à jour l'utilisateur
            user.Nom = userDto.Nom;
            user.Prenom = userDto.Prenom;
            user.Email = userDto.Email;
            user.AgenceId = agenceId;

            // Mettre à jour le mot de passe si fourni
            if (!string.IsNullOrEmpty(userDto.MotDePasse))
            {
                user.MotDePasse = _passwordHasher.HashPassword(userDto.MotDePasse);
            }

            await _context.SaveChangesAsync();

            return new UserDto
            {
                Id = user.Id,
                Nom = user.Nom,
                Prenom = user.Prenom,
                Email = user.Email,
                AgenceId = user.AgenceId,
                AgenceNom = agence.Nom
            };
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return false;

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<UserDto> AuthenticateAsync(LoginDto loginDto)
        {
            var user = await _context.Users
                .Include(u => u.Agence)
                .FirstOrDefaultAsync(u => u.Email == loginDto.Email);

            if (user == null)
                return null;

            // Vérifier le mot de passe
            if (!_passwordHasher.VerifyPassword(loginDto.MotDePasse, user.MotDePasse))
                return null;

            return new UserDto
            {
                Id = user.Id,
                Nom = user.Nom,
                Prenom = user.Prenom,
                Email = user.Email,
                AgenceId = user.AgenceId,
                AgenceNom = user.Agence?.Nom
            };
        }
    }
}

