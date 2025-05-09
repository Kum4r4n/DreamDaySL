using DreamDay.Data;
using DreamDay.Entites;
using DreamDay.Models;
using Microsoft.EntityFrameworkCore;

namespace DreamDay.Services
{
    public class ChecklistService
    {
        private readonly ApplicaitonDbContext _dbContext;

        public ChecklistService(ApplicaitonDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<ChecklistItemModel>> GetListAsync(Guid userId)
        {
            var wedding = _dbContext.Weddings.Include(x=>x.CheckListItems).FirstOrDefault(w => w.UserId == userId);
            if (wedding == null) return null;

            var tasks = wedding.CheckListItems
                .Select(x => new ChecklistItemModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    IsCompleted = x.IsCompleted,
                    Description = x.Description,
                    DueDate = x.DueDate,
                    WeddingId = x.WeddingId
                }).ToList();

            return tasks;

        }

        public async Task<ChecklistItemModel> CreateCheckList(Guid userId, ChecklistItemModel checklistItemModel)
        {
            var wedding = _dbContext.Weddings.FirstOrDefault(w => w.UserId == userId);
            if (wedding == null) return null;

            var task = new CheckListItem
            {
                Name = checklistItemModel.Name,
                IsCompleted = checklistItemModel.IsCompleted,
                WeddingId = wedding.Id,
                DueDate = checklistItemModel.DueDate,
                Description = checklistItemModel.Description
            };

            _dbContext.CheckListItems.Add(task);
            _dbContext.SaveChanges();


            var checkListModel = new ChecklistItemModel
            {
                Id = task.Id,
                Name = task.Name,
                IsCompleted = task.IsCompleted,
                DueDate = task.DueDate,
                Description = task.Description,
                WeddingId = task.WeddingId
            };

            return checkListModel;
        }


        public async Task<ChecklistItemModel> GetCheckListItemAsync(Guid checkListId)
        {
            var task = _dbContext.CheckListItems.Find(checkListId);
            if (task == null) return null;

            var model = new ChecklistItemModel
            {
                Id = task.Id,
                Name = task.Name,
                IsCompleted = task.IsCompleted,
                WeddingId = task.WeddingId,
                Description = task.Description,
                DueDate = task.DueDate
            };

            return model;
        }


        public async Task<ChecklistItemModel> UpdateAsync(ChecklistItemModel checklistItemModel)
        {
            var task = _dbContext.CheckListItems.Find(checklistItemModel.Id);
            if (task == null) return null;

            task.Name = checklistItemModel.Name;
            task.IsCompleted = checklistItemModel.IsCompleted;
            task.DueDate = checklistItemModel.DueDate;
            task.Description = checklistItemModel.Description;

            _dbContext.SaveChanges();

            var model = new ChecklistItemModel
            {
                Id = task.Id,
                Name = task.Name,
                IsCompleted = task.IsCompleted,
                WeddingId = task.WeddingId,
                Description = task.Description,
                DueDate = task.DueDate
            };

            return model;

        }


        public async Task DeleteAsync(Guid checkListId)
        {
            var task = _dbContext.CheckListItems.Find(checkListId);
            if (task != null)
            {
                _dbContext.CheckListItems.Remove(task);
                _dbContext.SaveChanges();
            }
        }
    }
}
