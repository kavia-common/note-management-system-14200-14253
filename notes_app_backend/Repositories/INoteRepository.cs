using NotesAppBackend.Models;

namespace NotesAppBackend.Repositories
{
    // PUBLIC_INTERFACE
    public interface INoteRepository
    {
        Task<Note?> GetByIdAsync(Guid id, Guid ownerId);
        Task<List<Note>> GetAllForUserAsync(Guid ownerId);
        Task AddAsync(Note note);
        void Update(Note note);
        void Remove(Note note);
        Task<bool> SaveChangesAsync();
    }
}
