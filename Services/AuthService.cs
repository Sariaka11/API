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
        private readonly IPasswordHasher _passwordHasher;

        public AuthService(AppDbContext context, IPasswordHasher passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
        }

        public async Task<User> ValidateUserAsync(string email, string motDePasse)
        {
            var user = await _context.Users
                                     .Include(u => u.Agence)
                                     .FirstOrDefaultAsync(u => u.Email == email);

            if (user == null || !_passwordHasher.VerifyPassword(motDePasse, user.MotDePasse))
                return null;

            return user;
        }

        public async Task<bool> RegisterUserAsync(User user)
        {
            if (await _context.Users.AnyAsync(u => u.Email == user.Email))
                return false;

            user.MotDePasse = _passwordHasher.HashPassword(user.MotDePasse); // Hashing the password

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}

