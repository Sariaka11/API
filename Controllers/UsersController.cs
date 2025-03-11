using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using API.DTOs;
using API.Services;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
        {
            try
            {
                var users = await _userService.GetAllUsersAsync();
                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erreur interne: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetUser(int id)
        {
            try
            {
                var user = await _userService.GetUserByIdAsync(id);
                if (user == null)
                    return NotFound($"Utilisateur avec ID {id} non trouvé");

                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erreur interne: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult<UserDto>> CreateUser(CreateUserDto userDto)
        {
            try
            {
                var user = await _userService.CreateUserAsync(userDto);
                return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
            }
            catch (Exception ex)
            {
                return BadRequest($"Erreur lors de la création de l'utilisateur: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<UserDto>> UpdateUser(int id, UpdateUserDto userDto)
        {
            try
            {
                var user = await _userService.UpdateUserAsync(id, userDto);
                if (user == null)
                    return NotFound($"Utilisateur avec ID {id} non trouvé");

                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest($"Erreur lors de la mise à jour de l'utilisateur: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUser(int id)
        {
            try
            {
                var result = await _userService.DeleteUserAsync(id);
                if (!result)
                    return NotFound($"Utilisateur avec ID {id} non trouvé");

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erreur interne: {ex.Message}");
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            try
            {
                var user = await _userService.AuthenticateAsync(loginDto);
                if (user == null)
                    return Unauthorized("Email ou mot de passe incorrect");

                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erreur interne: {ex.Message}");
            }
        }
    }
}

