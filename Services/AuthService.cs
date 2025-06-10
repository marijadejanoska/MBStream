// Services/AuthService.cs
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MBStream.DTOs;
using MBStream.Models;
using MBStream.Repositories;
using MBStream.Models;
using BCrypt.Net;

namespace MBStream.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepo;
        private readonly IConfiguration _config;

        public AuthService(IUserRepository userRepo, IConfiguration config)
        {
            _userRepo = userRepo;
            _config = config;
        }

        public async Task<string> LoginAsync(LoginDTO loginDto)
        {
            var user = await _userRepo.GetByEmailAsync(loginDto.UserEmail);
            if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
                return null;

            return GenerateJwtToken(user);
        }
        public async Task<UserResponseDTO> RegisterAsync(RegisterDTO registerDto)
        {
            var existingUser = await _userRepo.GetByEmailAsync(registerDto.UserEmail);
            if (existingUser != null) return null;

            var user = new User
            {
                UserName = registerDto.UserName,
                UserEmail = registerDto.UserEmail,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password),
                Role = "User" // Hardcoded default role
            };

            var createdUser = await _userRepo.AddAsync(user);
            return new UserResponseDTO
            {
                UserId = createdUser.UserId,
                UserName = createdUser.UserName,
                UserEmail = createdUser.UserEmail
            };
        }


        private string GenerateJwtToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
        new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
        new Claim(ClaimTypes.Email, user.UserEmail),
        new Claim(ClaimTypes.Role, user.Role) // Add role claim
    };

            var token = new JwtSecurityToken(
                _config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(_config["Jwt:ExpiryInMinutes"])),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}