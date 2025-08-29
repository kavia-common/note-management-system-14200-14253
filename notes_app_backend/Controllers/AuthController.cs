using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NotesAppBackend.DTOs;
using NotesAppBackend.Services;
using System.Security.Claims;

namespace NotesAppBackend.Controllers
{
    [ApiController]
    [Route("api/auth")]
    [Produces("application/json")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _users;

        public AuthController(IUserService users)
        {
            _users = users;
        }

        /// <summary>
        /// Registers a new user account.
        /// </summary>
        /// <param name="request">Registration data (username, email, password).</param>
        /// <returns>The created user summary.</returns>
        /// <response code="201">User registered successfully.</response>
        /// <response code="400">Validation error or username/email already exists.</response>
        [HttpPost("register")]
        [ProducesResponseType(typeof(object), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            try
            {
                var user = await _users.RegisterAsync(request);
                return CreatedAtAction(nameof(Me), new { }, new
                {
                    user.Id,
                    user.Username,
                    user.Email,
                    user.CreatedAt
                });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Authenticates a user and returns a JWT token.
        /// </summary>
        /// <param name="request">Login data (username or email, password).</param>
        /// <returns>JWT token and user info.</returns>
        /// <response code="200">Logged in successfully.</response>
        /// <response code="401">Invalid credentials.</response>
        [HttpPost("login")]
        [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(object), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                var resp = await _users.LoginAsync(request);
                return Ok(resp);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Returns the current authenticated user's profile.
        /// </summary>
        /// <returns>Current user profile.</returns>
        /// <response code="200">Profile returned.</response>
        /// <response code="401">Unauthorized.</response>
        [HttpGet("me")]
        [Authorize]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Me()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ??
                         User.FindFirstValue(ClaimTypes.Name) ??
                         User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Our token uses sub for user id; JwtSecurityTokenHandler maps to ClaimTypes.NameIdentifier?
            var sub = User.FindFirst("sub")?.Value ?? string.Empty;

            if (!Guid.TryParse(sub, out var id))
            {
                return Unauthorized();
            }

            var user = await _users.GetByIdAsync(id);
            if (user == null) return Unauthorized();

            return Ok(new { user.Id, user.Username, user.Email, user.CreatedAt });
        }
    }
}
