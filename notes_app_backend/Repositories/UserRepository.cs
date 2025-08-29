using Microsoft.EntityFrameworkCore;
using NotesAppBackend.Data;
using NotesAppBackend.Models;
using System.Linq;

namespace NotesAppBackend.Repositories
{
    /// <summary>
    /// EF Core implementation of user repository.
    /// </summary>
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _db;

        public UserRepository(AppDbContext db) => _db = db;

        public Task<User?> GetByIdAsync(Guid id) => _db.Users.FirstOrDefaultAsync(u => u.Id == id);

        public Task<User?> GetByUsernameAsync(string username) =>
            _db.Users.FirstOrDefaultAsync(u => u.Username.ToLower() == username.ToLower());

        public Task<User?> GetByEmailAsync(string email) =>
            _db.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());

        public async Task AddAsync(User user)
        {
            await _db.Users.AddAsync(user);
        }

        public Task<bool> SaveChangesAsync() => Task.FromResult(_db.SaveChanges() > 0);
    }
}
