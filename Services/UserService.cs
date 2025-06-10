// Services/UserService.cs
using AutoMapper;
using MBStream.Models;
using MBStream.DTOs;
using MBStream.Models;
using MBStream.Repositories;
using BCrypt.Net;

namespace MBStream.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepo;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepo, IMapper mapper)
        {
            _userRepo = userRepo;
            _mapper = mapper;
        }

        public async Task<List<UserResponseDTO>> GetAllUsersAsync()
        {
            var users = await _userRepo.GetAllAsync();
            return _mapper.Map<List<UserResponseDTO>>(users);
        }

        public async Task<UserResponseDTO> GetUserByIdAsync(int id)
        {
            var user = await _userRepo.GetByIdAsync(id);
            return _mapper.Map<UserResponseDTO>(user);
        }

        public async Task<UserResponseDTO> CreateUserAsync(RegisterDTO registerDto)
        {
            var user = new User
            {
                UserName = registerDto.UserName,
                UserEmail = registerDto.UserEmail,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password),
                Role = "User"
            };

            var createdUser = await _userRepo.AddAsync(user);
            return _mapper.Map<UserResponseDTO>(createdUser);
        }


        public async Task<UserResponseDTO> UpdateUserAsync(int id, UserDTO userDto)
        {
            var user = await _userRepo.GetByIdAsync(id);
            if (user == null) return null;

            _mapper.Map(userDto, user);
            var updatedUser = await _userRepo.UpdateAsync(user);
            return _mapper.Map<UserResponseDTO>(updatedUser);
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            return await _userRepo.DeleteAsync(id);
        }
    }
}
