using NotesAppBackend.DTOs;
using NotesAppBackend.Models;

namespace NotesAppBackend.Services
{
    // PUBLIC_INTERFACE
    public interface IUserService
    {
        /// <summary>
        /// Registers a new user. Throws InvalidOperationException if username/email exists.
        /// </summary>
        Task<User> RegisterAsync(RegisterRequest request);

        /// <summary>
        /// Validates credentials and returns JWT token response.
        /// </summary>
        Task<AuthResponse> LoginAsync(LoginRequest request);

        /// <summary>
        /// Retrieves the current user by id.
        /// </summary>
        Task<User?> GetByIdAsync(Guid id);
    }
}
