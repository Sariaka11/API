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
    public class AgencesController : ControllerBase
    {
        private readonly IAgenceService _agenceService;

        public AgencesController(IAgenceService agenceService)
        {
            _agenceService = agenceService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AgenceDto>>> GetAgences()
        {
            try
            {
                var agences = await _agenceService.GetAllAgencesAsync();
                return Ok(agences);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erreur interne: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AgenceDto>> GetAgence(int id)
        {
            try
            {
                var agence = await _agenceService.GetAgenceByIdAsync(id);
                if (agence == null)
                    return NotFound($"Agence avec ID {id} non trouvée");

                return Ok(agence);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erreur interne: {ex.Message}");
            }
        }

        [HttpGet("numero/{numero}")]
        public async Task<ActionResult<AgenceDto>> GetAgenceByNumero(string numero)
        {
            try
            {
                var agence = await _agenceService.GetAgenceByNumeroAsync(numero);
                if (agence == null)
                    return NotFound($"Agence avec numéro {numero} non trouvée");

                return Ok(agence);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erreur interne: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult<AgenceDto>> CreateAgence(CreateAgenceDto agenceDto)
        {
            try
            {
                var agence = await _agenceService.CreateAgenceAsync(agenceDto);
                return CreatedAtAction(nameof(GetAgence), new { id = agence.Id }, agence);
            }
            catch (Exception ex)
            {
                return BadRequest($"Erreur lors de la création de l'agence: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<AgenceDto>> UpdateAgence(int id, UpdateAgenceDto agenceDto)
        {
            try
            {
                var agence = await _agenceService.UpdateAgenceAsync(id, agenceDto);
                if (agence == null)
                    return NotFound($"Agence avec ID {id} non trouvée");

                return Ok(agence);
            }
            catch (Exception ex)
            {
                return BadRequest($"Erreur lors de la mise à jour de l'agence: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAgence(int id)
        {
            try
            {
                var result = await _agenceService.DeleteAgenceAsync(id);
                if (!result)
                    return NotFound($"Agence avec ID {id} non trouvée");

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest($"Erreur lors de la suppression de l'agence: {ex.Message}");
            }
        }
    }
}

