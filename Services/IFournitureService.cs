using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using API.DTOs;

namespace API.Services
{
  public interface IFournitureService
  {
      Task<IEnumerable<FournitureDto>> GetAllFournituresAsync();
      Task<FournitureDto> GetFournitureByIdAsync(int id);
      Task<IEnumerable<FournitureDto>> GetFournituresByAgenceIdAsync(int agenceId);
      Task<IEnumerable<FournitureDto>> GetFournituresByAgenceNumeroAsync(string numeroAgence);
      Task<FournitureDto> CreateFournitureAsync(CreateFournitureDto fournitureDto);
      Task<FournitureDto> UpdateFournitureAsync(int id, UpdateFournitureDto fournitureDto);
      Task<bool> DeleteFournitureAsync(int id);
  }
}

