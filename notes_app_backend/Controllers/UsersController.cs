using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NotesAppBackend.Services;
using System.Security.Claims;

namespace NotesAppBackend.Controllers
{
    [ApiController]
    [Route("api/users")]
    [Authorize]
    [Produces("application/json")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _users;

        public UsersController(IUserService users)
        {
            _users = users;
        }

        /// <summary>
        /// Get the current user's profile (alias of /api/auth/me).
        /// </summary>
        [HttpGet("me")]
        public async Task<IActionResult> Me()
        {
            var sub = User.FindFirst("sub")?.Value ?? string.Empty;
            if (!Guid.TryParse(sub, out var id)) return Unauthorized();

            var user = await _users.GetByIdAsync(id);
            if (user == null) return Unauthorized();

            return Ok(new { user.Id, user.Username, user.Email, user.CreatedAt });
        }
    }
}
