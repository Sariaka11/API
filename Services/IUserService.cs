using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using API.DTOs;
using API.Models;

namespace API.Services
{
    public interface IUserService
    {
        Task<IEnumerable<UserDto>> GetAllUsersAsync();
        Task<UserDto> GetUserByIdAsync(int id);
        Task<UserDto> CreateUserAsync(CreateUserDto userDto);
        Task<UserDto> UpdateUserAsync(int id, UpdateUserDto userDto);
        Task<bool> DeleteUserAsync(int id);
        Task<UserDto> AuthenticateAsync(LoginDto loginDto);
    }
}

