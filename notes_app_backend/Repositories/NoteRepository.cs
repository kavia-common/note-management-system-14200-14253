using Microsoft.EntityFrameworkCore;
using NotesAppBackend.Data;
using NotesAppBackend.Models;
using System.Linq;
using System.Collections.Generic;

namespace NotesAppBackend.Repositories
{
    /// <summary>
    /// EF Core implementation of notes repository.
    /// </summary>
    public class NoteRepository : INoteRepository
    {
        private readonly AppDbContext _db;

        public NoteRepository(AppDbContext db) => _db = db;

        public Task<Note?> GetByIdAsync(Guid id, Guid ownerId) =>
            _db.Notes.FirstOrDefaultAsync(n => n.Id == id && n.UserId == ownerId);

        public Task<List<Note>> GetAllForUserAsync(Guid ownerId) =>
            _db.Notes.Where(n => n.UserId == ownerId)
                     .OrderByDescending(n => n.UpdatedAt)
                     .ToListAsync();

        public async Task AddAsync(Note note) => await _db.Notes.AddAsync(note);

        public void Update(Note note) => _db.Notes.Update(note);

        public void Remove(Note note) => _db.Notes.Remove(note);

        public Task<bool> SaveChangesAsync() => Task.FromResult(_db.SaveChanges() > 0);
    }
}
