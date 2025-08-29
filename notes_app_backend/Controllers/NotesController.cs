using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NotesAppBackend.DTOs;
using NotesAppBackend.Services;
using System.Security.Claims;

namespace NotesAppBackend.Controllers
{
    [ApiController]
    [Route("api/notes")]
    [Authorize]
    [Produces("application/json")]
    public class NotesController : ControllerBase
    {
        private readonly INoteService _notes;

        public NotesController(INoteService notes)
        {
            _notes = notes;
        }

        private Guid GetUserId()
        {
            var sub = User.FindFirst("sub")?.Value ?? string.Empty;
            return Guid.TryParse(sub, out var id) ? id : Guid.Empty;
        }

        /// <summary>
        /// List all notes for the current user.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(List<NoteResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> List()
        {
            var userId = GetUserId();
            var list = await _notes.GetAllAsync(userId);
            return Ok(list);
        }

        /// <summary>
        /// Get a single note by id for the current user.
        /// </summary>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(NoteResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(Guid id)
        {
            var userId = GetUserId();
            var note = await _notes.GetByIdAsync(userId, id);
            if (note == null) return NotFound();
            return Ok(note);
        }

        /// <summary>
        /// Create a new note.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(NoteResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] NoteCreateRequest request)
        {
            var userId = GetUserId();
            var created = await _notes.CreateAsync(userId, request);
            return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
        }

        /// <summary>
        /// Update an existing note.
        /// </summary>
        [HttpPut("{id:guid}")]
        [ProducesResponseType(typeof(NoteResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(Guid id, [FromBody] NoteUpdateRequest request)
        {
            var userId = GetUserId();
            var updated = await _notes.UpdateAsync(userId, id, request);
            if (updated == null) return NotFound();
            return Ok(updated);
        }

        /// <summary>
        /// Delete a note by id.
        /// </summary>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var userId = GetUserId();
            var ok = await _notes.DeleteAsync(userId, id);
            if (!ok) return NotFound();
            return NoContent();
        }
        }
}
