// Controllers/UsersController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MBStream.DTOs;
using MBStream.Services;

namespace MBStream.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        // GET: api/users
        [HttpGet]
        [Authorize(Roles = "Admin")]

        public async Task<ActionResult<List<UserResponseDTO>>> GetAllUsers()
        {
            return Ok(await _userService.GetAllUsersAsync());
        }

        // GET: api/users/5
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]

        public async Task<ActionResult<UserResponseDTO>> GetUser(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            return user == null ? NotFound() : Ok(user);
        }

        // POST: api/users
        [HttpPost]
        [Authorize(Roles = "Admin")]

        public async Task<ActionResult<UserResponseDTO>> CreateUser(RegisterDTO registerDto)
        {
            var user = await _userService.CreateUserAsync(registerDto);
            return CreatedAtAction(nameof(GetUser), new { id = user.UserId }, user);
        }

        // PUT: api/users/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> UpdateUser(int id, UserDTO userDto)
        {
            var updatedUser = await _userService.UpdateUserAsync(id, userDto);
            return updatedUser == null ? NotFound() : NoContent();
        }

        // DELETE: api/users/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> DeleteUser(int id)
        {
            var result = await _userService.DeleteUserAsync(id);
            return result ? NoContent() : NotFound();
        }
    }
}
