// Controllers/AuthController.cs
using Microsoft.AspNetCore.Mvc;
using MBStream.DTOs;
using MBStream.Services;

namespace MBStream.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO loginDto)
        {
            var token = await _authService.LoginAsync(loginDto);
            return token == null ? Unauthorized() : Ok(new { Token = token });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDTO registerDto)
        {
            var user = await _authService.RegisterAsync(registerDto);
            return user == null ? BadRequest("Email already exists") : Ok(user);
        }
    }
}
