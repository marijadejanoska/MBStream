using MBStream.DTOs;

namespace MBStream.Services
{
    public interface IAuthService
    {
        Task<string> LoginAsync(LoginDTO loginDto);
        Task<UserResponseDTO> RegisterAsync(RegisterDTO registerDto);
    }
}
