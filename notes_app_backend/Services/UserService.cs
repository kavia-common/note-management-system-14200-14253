using NotesAppBackend.DTOs;
using NotesAppBackend.Models;
using NotesAppBackend.Repositories;

namespace NotesAppBackend.Services
{
    /// <summary>
    /// Handles user registration and login logic.
    /// </summary>
    public class UserService : IUserService
    {
        private readonly IUserRepository _users;
        private readonly ITokenService _tokens;

        public UserService(IUserRepository users, ITokenService tokens)
        {
            _users = users;
            _tokens = tokens;
        }

        public async Task<User> RegisterAsync(RegisterRequest request)
        {
            var existingByUsername = await _users.GetByUsernameAsync(request.Username);
            if (existingByUsername != null)
            {
                throw new InvalidOperationException("Username is already taken.");
            }
            var existingByEmail = await _users.GetByEmailAsync(request.Email);
            if (existingByEmail != null)
            {
                throw new InvalidOperationException("Email is already registered.");
            }

            var hashed = BCrypt.Net.BCrypt.HashPassword(request.Password);
            var user = new User
            {
                Username = request.Username.Trim(),
                Email = request.Email.Trim(),
                PasswordHash = hashed,
                CreatedAt = DateTime.UtcNow
            };

            await _users.AddAsync(user);
            await _users.SaveChangesAsync();
            return user;
        }

        public async Task<AuthResponse> LoginAsync(LoginRequest request)
        {
            User? user = null;
            // Try by username then email
            user = await _users.GetByUsernameAsync(request.UsernameOrEmail) ??
                   await _users.GetByEmailAsync(request.UsernameOrEmail);

            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            {
                throw new UnauthorizedAccessException("Invalid credentials.");
            }

            var token = _tokens.GenerateToken(user);
            return new AuthResponse
            {
                Token = token,
                Username = user.Username,
                UserId = user.Id,
                Email = user.Email
            };
        }

        public Task<User?> GetByIdAsync(Guid id) => _users.GetByIdAsync(id);
    }
}
