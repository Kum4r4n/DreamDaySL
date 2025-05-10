using DreamDay.Data;
using DreamDay.Entites;
using DreamDay.Models;
using Microsoft.EntityFrameworkCore;

namespace DreamDay.Services
{
    public class PlannerChecklistService
    {
        private readonly ApplicaitonDbContext _dbContext;

        public PlannerChecklistService(ApplicaitonDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<ChecklistItemModel>> GetChecklistAsync(Guid weddingId)
        {
            var checklist = await _dbContext.CheckListItems
                .Where(c => c.WeddingId == weddingId)
                .Select(c => new ChecklistItemModel
                {
                    Id = c.Id,
                    Name = c.Name,
                    IsCompleted = c.IsCompleted,
                    DueDate = c.DueDate,
                    Description = c.Description,
                    WeddingId = c.WeddingId
                })
                .ToListAsync();

            return checklist;
        }

        public async Task<Guid> CreateCheckListAsync(ChecklistItemModel checklistItemModel)
        {
            var item = new CheckListItem
            {
                Name = checklistItemModel.Name,
                IsCompleted = checklistItemModel.IsCompleted,
                DueDate = checklistItemModel.DueDate,
                Description = checklistItemModel.Description,
                WeddingId = checklistItemModel.WeddingId
            };
            
            _dbContext.CheckListItems.Add(item);
            await _dbContext.SaveChangesAsync().ConfigureAwait(false);

            return item.WeddingId;
        }

        public async Task<Guid?> UpdateCheckListAsync(ChecklistItemModel checklistItemModel)
        {
            var item = await _dbContext.CheckListItems
                .FirstOrDefaultAsync(c => c.Id == checklistItemModel.Id)
                .ConfigureAwait(false);

            if (item == null)
                return null;

            item.Name = checklistItemModel.Name;
            item.IsCompleted = checklistItemModel.IsCompleted;
            item.DueDate = checklistItemModel.DueDate;
            item.Description = checklistItemModel.Description;

            _dbContext.CheckListItems.Update(item);
            await _dbContext.SaveChangesAsync().ConfigureAwait(false);

            return item.WeddingId;
        }

        public async Task<ChecklistItemModel?> GetCheckListItemAsync(Guid id)
        {
            var item = await _dbContext.CheckListItems
                .FirstOrDefaultAsync(c => c.Id == id);

            if (item == null)
                return null;

            var checklistItemModel = new ChecklistItemModel()
            {
                Id = item.Id,
                Name = item.Name,
                IsCompleted = item.IsCompleted,
                DueDate = item.DueDate,
                Description = item.Description,
                WeddingId = item.WeddingId
            };

            return checklistItemModel;
        }

        public async Task<Guid?> DeleteCheckListItemAsync(Guid id)
        {
            var item = await _dbContext.CheckListItems
                .FirstOrDefaultAsync(c => c.Id == id);

            if (item != null)
            {
                var weddingId = item.WeddingId;
                _dbContext.CheckListItems.Remove(item);
                await _dbContext.SaveChangesAsync().ConfigureAwait(false);
                return weddingId;
            }

            return null;
        }
    }
}
