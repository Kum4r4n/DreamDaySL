using DreamDay.Data;
using DreamDay.Entites;
using DreamDay.Models;
using Microsoft.EntityFrameworkCore;

namespace DreamDay.Services
{
    public class CoupleTimelineService
    {
        private readonly ApplicaitonDbContext _dbContext;

        public CoupleTimelineService(ApplicaitonDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<(List<WeddingTimelineItemModel>, Guid)?> GetTimelinesByUser(Guid userId)
        {
            var wedding = _dbContext.Weddings.FirstOrDefault(w => w.UserId == userId);
            if (wedding == null)
                return null;

            var items = _dbContext.WeddingTimelineItems
            .Where(t => t.WeddingId == wedding.Id)
            .OrderBy(t => t.Time)
            .ToList();

            return new (items.Select(t => new WeddingTimelineItemModel
            {
                Id = t.Id,
                WeddingId = t.WeddingId,
                Title = t.Title,
                Description = t.Description,
                Time = t.Time
            }).ToList(), wedding.Id);
        }


        public async Task<WeddingTimelineItemModel> Create(WeddingTimelineItemModel weddingTimelineItemModel )
        {
            var timelineItem = new WeddingTimelineItem
            {
                WeddingId = weddingTimelineItemModel.WeddingId,
                Title = weddingTimelineItemModel.Title,
                Description = weddingTimelineItemModel.Description,
                Time = weddingTimelineItemModel.Time
            };

            _dbContext.WeddingTimelineItems.Add(timelineItem);
            await _dbContext.SaveChangesAsync();

            return new WeddingTimelineItemModel
            {
                Id = timelineItem.Id,
                WeddingId = timelineItem.WeddingId,
                Title = timelineItem.Title,
                Description = timelineItem.Description,
                Time = timelineItem.Time
            };
        }

        public async Task<WeddingTimelineItemModel> GetByIdAsync(Guid id)
        {
            var data = await _dbContext.WeddingTimelineItems.FirstOrDefaultAsync(x => x.Id == id);

            return new WeddingTimelineItemModel
            {
                Id = data.Id,
                WeddingId = data.WeddingId,
                Title = data.Title,
                Description = data.Description,
                Time = data.Time
            };
        }

        public async Task<bool> Update(WeddingTimelineItemModel weddingTimelineItemModel)
        {
            var timelineItem = await _dbContext.WeddingTimelineItems.FirstOrDefaultAsync(x => x.Id == weddingTimelineItemModel.Id);
            if (timelineItem == null)
                return false;

            timelineItem.Title = weddingTimelineItemModel.Title;
            timelineItem.Description = weddingTimelineItemModel.Description;
            timelineItem.Time = weddingTimelineItemModel.Time;

            _dbContext.WeddingTimelineItems.Update(timelineItem);
            await _dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> Delete(Guid id)
        {
            var timelineItem = await _dbContext.WeddingTimelineItems.FirstOrDefaultAsync(x => x.Id == id);
            if (timelineItem == null)
                return false;

            _dbContext.WeddingTimelineItems.Remove(timelineItem);
            await _dbContext.SaveChangesAsync();

            return true;
        }
    }
}
