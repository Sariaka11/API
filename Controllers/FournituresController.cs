using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using API.DTOs;
using API.Services;
using Microsoft.AspNetCore.Cors;

namespace API.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  [EnableCors] // Assurez-vous que CORS est activé pour ce contrôleur
  public class FournituresController : ControllerBase
  {
      private readonly IFournitureService _fournitureService;

      public FournituresController(IFournitureService fournitureService)
      {
          _fournitureService = fournitureService;
      }

      [HttpGet]
      public async Task<ActionResult<IEnumerable<FournitureDto>>> GetFournitures()
      {
          try
          {
              var fournitures = await _fournitureService.GetAllFournituresAsync();
              return Ok(fournitures);
          }
          catch (Exception ex)
          {
              return StatusCode(500, $"Erreur interne: {ex.Message}");
          }
      }

      [HttpGet("{id}")]
      public async Task<ActionResult<FournitureDto>> GetFourniture(int id)
      {
          try
          {
              var fourniture = await _fournitureService.GetFournitureByIdAsync(id);
              if (fourniture == null)
                  return NotFound($"Fourniture avec ID {id} non trouvée");

              return Ok(fourniture);
          }
          catch (Exception ex)
          {
              return StatusCode(500, $"Erreur interne: {ex.Message}");
          }
      }

      [HttpGet("agence/{agenceId}")]
      public async Task<ActionResult<IEnumerable<FournitureDto>>> GetFournituresByAgenceId(int agenceId)
      {
          try
          {
              var fournitures = await _fournitureService.GetFournituresByAgenceIdAsync(agenceId);
              return Ok(fournitures);
          }
          catch (Exception ex)
          {
              return StatusCode(500, $"Erreur interne: {ex.Message}");
          }
      }

      [HttpGet("agence/numero/{numeroAgence}")]
      public async Task<ActionResult<IEnumerable<FournitureDto>>> GetFournituresByAgenceNumero(string numeroAgence)
      {
          try
          {
              var fournitures = await _fournitureService.GetFournituresByAgenceNumeroAsync(numeroAgence);
              return Ok(fournitures);
          }
          catch (Exception ex)
          {
              return StatusCode(500, $"Erreur interne: {ex.Message}");
          }
      }

      [HttpPost]
      public async Task<ActionResult<FournitureDto>> CreateFourniture(CreateFournitureDto fournitureDto)
      {
          try
          {
              Console.WriteLine($"Création d'une fourniture: {fournitureDto.Nom}, Catégorie: {fournitureDto.Categorie}");
              var fourniture = await _fournitureService.CreateFournitureAsync(fournitureDto);
              return CreatedAtAction(nameof(GetFourniture), new { id = fourniture.Id }, fourniture);
          }
          catch (Exception ex)
          {
              Console.WriteLine($"Erreur lors de la création de la fourniture: {ex.Message}");
              return BadRequest($"Erreur lors de la création de la fourniture: {ex.Message}");
          }
      }

      [HttpPut("{id}")]
      public async Task<ActionResult<FournitureDto>> UpdateFourniture(int id, UpdateFournitureDto fournitureDto)
      {
          try
          {
              var fourniture = await _fournitureService.UpdateFournitureAsync(id, fournitureDto);
              if (fourniture == null)
                  return NotFound($"Fourniture avec ID {id} non trouvée");

              return Ok(fourniture);
          }
          catch (Exception ex)
          {
              return BadRequest($"Erreur lors de la mise à jour de la fourniture: {ex.Message}");
          }
      }

      [HttpDelete("{id}")]
      public async Task<ActionResult> DeleteFourniture(int id)
      {
          try
          {
              var result = await _fournitureService.DeleteFournitureAsync(id);
              if (!result)
                  return NotFound($"Fourniture avec ID {id} non trouvée");

              return NoContent();
          }
          catch (Exception ex)
          {
              return StatusCode(500, $"Erreur interne: {ex.Message}");
          }
      }
  }
}

