using NotesAppBackend.Models;

namespace NotesAppBackend.Repositories
{
    // PUBLIC_INTERFACE
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(Guid id);
        Task<User?> GetByUsernameAsync(string username);
        Task<User?> GetByEmailAsync(string email);
        Task AddAsync(User user);
        Task<bool> SaveChangesAsync();
    }
}
