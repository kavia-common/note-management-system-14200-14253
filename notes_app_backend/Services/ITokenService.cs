using NotesAppBackend.Models;

namespace NotesAppBackend.Services
{
    // PUBLIC_INTERFACE
    public interface ITokenService
    {
        /// <summary>
        /// Generate a signed JWT token for the given user.
        /// </summary>
        string GenerateToken(User user);
    }
}
