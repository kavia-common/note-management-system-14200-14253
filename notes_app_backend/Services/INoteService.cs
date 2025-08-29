using NotesAppBackend.DTOs;
using NotesAppBackend.Models;

namespace NotesAppBackend.Services
{
    // PUBLIC_INTERFACE
    public interface INoteService
    {
        Task<NoteResponse> CreateAsync(Guid userId, NoteCreateRequest request);
        Task<List<NoteResponse>> GetAllAsync(Guid userId);
        Task<NoteResponse?> GetByIdAsync(Guid userId, Guid id);
        Task<NoteResponse?> UpdateAsync(Guid userId, Guid id, NoteUpdateRequest request);
        Task<bool> DeleteAsync(Guid userId, Guid id);
    }
}
