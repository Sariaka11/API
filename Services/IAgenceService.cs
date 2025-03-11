using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using API.DTOs;

namespace API.Services
{
    public interface IAgenceService
    {
        Task<IEnumerable<AgenceDto>> GetAllAgencesAsync();
        Task<AgenceDto> GetAgenceByIdAsync(int id);
        Task<AgenceDto> GetAgenceByNumeroAsync(string numero);
        Task<AgenceDto> CreateAgenceAsync(CreateAgenceDto agenceDto);
        Task<AgenceDto> UpdateAgenceAsync(int id, UpdateAgenceDto agenceDto);
        Task<bool> DeleteAgenceAsync(int id);
    }
}

