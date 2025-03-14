using Microsoft.AspNetCore.Mvc;
using API.DTOs;
using API.Models;
using API.Services;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var user = await _authService.ValidateUserAsync(loginDto.Email, loginDto.MotDePasse);
            if (user == null)
                return Unauthorized(new { message = "Identifiants invalides" });

            return Ok(new UserDto
            {
                Id = user.Id,
                Nom = user.Nom,
                Prenom = user.Prenom,
                Email = user.Email,
                AgenceId = user.AgenceId,
                AgenceNom = user.Agence?.Nom ?? "Non défini"
            });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] CreateUserDto createUserDto)
        {
            var user = new User
            {
                Nom = createUserDto.Nom,
                Prenom = createUserDto.Prenom,
                Email = createUserDto.Email,
                MotDePasse = createUserDto.MotDePasse, // Il sera hashé dans le service
                AgenceId = createUserDto.AgenceId
            };

            var success = await _authService.RegisterUserAsync(user);
            if (!success)
                return BadRequest(new { message = "L'email est déjà utilisé" });

            return Ok(new { message = "Utilisateur créé avec succès" });
        }
    }
}
