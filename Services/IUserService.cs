// Services/IUserService.cs
using MBStream.DTOs;

namespace MBStream.Services
{
    public interface IUserService
    {
        Task<List<UserResponseDTO>> GetAllUsersAsync();
        Task<UserResponseDTO> GetUserByIdAsync(int id);
        Task<UserResponseDTO> CreateUserAsync(RegisterDTO registerDto);
        Task<UserResponseDTO> UpdateUserAsync(int id, UserDTO userDto);
        Task<bool> DeleteUserAsync(int id);
    }
}
