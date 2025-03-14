using BCrypt.Net;
using API.Models;
using API.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace API.Services
{
    public class AuthService
    {
        private readonly AppDbContext _context;

        public AuthService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<User> ValidateUserAsync(string email, string motDePasse)
        {
            var user = await _context.Users
                                     .Include(u => u.Agence)
                                     .FirstOrDefaultAsync(u => u.Email == email);

            if (user == null || !VerifyPassword(motDePasse, user.MotDePasse))
                return null;

            return user;
        }

        public async Task<bool> RegisterUserAsync(User user)
        {
            if (await _context.Users.AnyAsync(u => u.Email == user.Email))
                return false;

            user.MotDePasse = HashPassword(user.MotDePasse); // Hashing the password

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return true;
        }

        // Utilisation de BCrypt pour le hachage du mot de passe
        private string HashPassword(string motDePasse)
        {
            return BCrypt.Net.BCrypt.HashPassword(motDePasse);
        }

        // Utilisation de BCrypt pour vérifier le mot de passe
        private bool VerifyPassword(string motDePasse, string storedHash)
        {
            return BCrypt.Net.BCrypt.Verify(motDePasse, storedHash);
        }
    }
}
