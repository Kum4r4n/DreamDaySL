using DreamDay.Data;
using DreamDay.Entites;
using DreamDay.Enums;
using DreamDay.Models;
using Microsoft.EntityFrameworkCore;

namespace DreamDay.Services
{
    public class PlannerNotesService
    {
        private readonly ApplicaitonDbContext _dbContext;

        public PlannerNotesService(ApplicaitonDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<WeddingNoteModel>> GetWeeddingNotes(Guid weddingId)
        {
            var notes = await _dbContext.WeddingNotes
                .Where(n => n.WeddingId == weddingId)
                .ToListAsync();
            
            return notes.Select(n => new WeddingNoteModel
            {
                Id = n.Id,
                WeddingId = n.WeddingId,
                Message = n.Message,
                SenderRole = n.SenderRole,
                Timestamp = n.Timestamp
            }).ToList();
        }

        public async Task<WeddingNoteModel> CreateWeddingNote(WeddingNoteModel weddingNoteModel)
        {
            var weddingNote = new WeddingNote
            {
                WeddingId = weddingNoteModel.WeddingId,
                Message = weddingNoteModel.Message,
                SenderRole = weddingNoteModel.SenderRole,
                Timestamp = DateTime.UtcNow
            };

            _dbContext.WeddingNotes.Add(weddingNote);
            await _dbContext.SaveChangesAsync();

            return new WeddingNoteModel
            {
                Id = weddingNote.Id,
                WeddingId = weddingNote.WeddingId,
                Message = weddingNote.Message,
                SenderRole = weddingNote.SenderRole,
                Timestamp = weddingNote.Timestamp
            };

        }

    }
}
