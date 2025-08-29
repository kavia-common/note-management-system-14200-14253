using NotesAppBackend.DTOs;
using NotesAppBackend.Models;
using NotesAppBackend.Repositories;

namespace NotesAppBackend.Services
{
    /// <summary>
    /// Handles notes business logic: ownership enforcement and mapping.
    /// </summary>
    public class NoteService : INoteService
    {
        private readonly INoteRepository _notes;

        public NoteService(INoteRepository notes)
        {
            _notes = notes;
        }

        public async Task<NoteResponse> CreateAsync(Guid userId, NoteCreateRequest request)
        {
            var note = new Note
            {
                Title = request.Title.Trim(),
                Content = request.Content,
                UserId = userId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            await _notes.AddAsync(note);
            await _notes.SaveChangesAsync();
            return Map(note);
        }

        public async Task<List<NoteResponse>> GetAllAsync(Guid userId)
        {
            var list = await _notes.GetAllForUserAsync(userId);
            return list.Select(Map).ToList();
        }

        public async Task<NoteResponse?> GetByIdAsync(Guid userId, Guid id)
        {
            var note = await _notes.GetByIdAsync(id, userId);
            return note == null ? null : Map(note);
        }

        public async Task<NoteResponse?> UpdateAsync(Guid userId, Guid id, NoteUpdateRequest request)
        {
            var note = await _notes.GetByIdAsync(id, userId);
            if (note == null) return null;

            note.Title = request.Title.Trim();
            note.Content = request.Content;
            note.UpdatedAt = DateTime.UtcNow;

            _notes.Update(note);
            await _notes.SaveChangesAsync();
            return Map(note);
        }

        public async Task<bool> DeleteAsync(Guid userId, Guid id)
        {
            var note = await _notes.GetByIdAsync(id, userId);
            if (note == null) return false;

            _notes.Remove(note);
            return await _notes.SaveChangesAsync();
        }

        private static NoteResponse Map(Note note) => new NoteResponse
        {
            Id = note.Id,
            Title = note.Title,
            Content = note.Content,
            CreatedAt = note.CreatedAt,
            UpdatedAt = note.UpdatedAt
        };
    }
}
