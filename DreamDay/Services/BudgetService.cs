using DreamDay.Data;
using DreamDay.Entites;
using DreamDay.Models;
using Microsoft.EntityFrameworkCore;

namespace DreamDay.Services
{
    public class BudgetService
    {
        private readonly ApplicaitonDbContext _dbContext;

        public BudgetService(ApplicaitonDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        public async Task<(List<BudgetItemModel>, decimal)?> GetListAsync(Guid userId)
        {
            var wedding = await _dbContext.Weddings.Include(w => w.BudgetItems)
                .FirstOrDefaultAsync(w => w.UserId == userId);

            if (wedding == null)
            {
                return null;
            }

            var budgetItems = wedding.BudgetItems.Select(s=> new BudgetItemModel()
            {
                Id = s.Id,
                Category = s.Category,
                AllocatedAmount = s.AllocatedAmount,
                SpentAmount = s.SpentAmount,
                Description = s.Description,
                WeddingId = s.WeddingId,
                VendorId = s.VendorId

            }).ToList();

            return new (budgetItems, wedding.TotalBudget);
        }

        public async Task<BudgetItemModel> CreateAsync(Guid userId, BudgetItemModel budgetItemModel)
        {
            var wedding = await _dbContext.Weddings.Include(w => w.BudgetItems)
               .FirstOrDefaultAsync(w => w.UserId == userId);

            if (wedding == null)
            {
                return null;
            }

            var item = new BudgetItem
            {
                Category = budgetItemModel.Category,
                Description = budgetItemModel.Description,
                AllocatedAmount = budgetItemModel.AllocatedAmount,
                SpentAmount = budgetItemModel.SpentAmount,
                WeddingId = wedding.Id
            };

            wedding.BudgetItems.Add(item);
            await _dbContext.SaveChangesAsync();

            var budgetItem = new BudgetItemModel
            {
                Id = item.Id,
                Category = item.Category,
                AllocatedAmount = item.AllocatedAmount,
                SpentAmount = item.SpentAmount,
                Description = item.Description,
                WeddingId = item.WeddingId
            };

            return budgetItem;
        }

        public async Task<BudgetItemModel> GetBudgetItemAsync(Guid budgetItemId)
        {
            var budgetItem = await _dbContext.BudgetItems.FindAsync(budgetItemId);
            if (budgetItem == null)
            {
                return null;
            }

            var model = new BudgetItemModel
            {
                Id = budgetItem.Id,
                Category = budgetItem.Category,
                AllocatedAmount = budgetItem.AllocatedAmount,
                SpentAmount = budgetItem.SpentAmount,
                Description = budgetItem.Description,
                WeddingId = budgetItem.WeddingId,
                VendorId = budgetItem.VendorId
            };

            return model;
        }

        public async Task<BudgetItemModel> UpdateAsync(BudgetItemModel budgetItemModel)
        {
            var budgetItem = await _dbContext.BudgetItems.FindAsync(budgetItemModel.Id);
            if (budgetItem == null)
            {
                return null;
            }

            budgetItem.Category = budgetItemModel.Category;
            budgetItem.Description = budgetItemModel.Description;
            budgetItem.AllocatedAmount = budgetItemModel.AllocatedAmount;
            budgetItem.SpentAmount = budgetItemModel.SpentAmount;

            await _dbContext.SaveChangesAsync();

            var model = new BudgetItemModel
            {
                Id = budgetItem.Id,
                Category = budgetItem.Category,
                AllocatedAmount = budgetItem.AllocatedAmount,
                SpentAmount = budgetItem.SpentAmount,
                Description = budgetItem.Description,
                WeddingId = budgetItem.WeddingId
            };

            return model;
        }

        public async Task DeleteAsync(Guid id)
        {
            var item = _dbContext.BudgetItems.Find(id);
            if (item != null)
            {
                _dbContext.BudgetItems.Remove(item);
                _dbContext.SaveChanges();
            }
        }
    }
}
