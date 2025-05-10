using DreamDay.Data;
using DreamDay.Models;
using Microsoft.EntityFrameworkCore;

namespace DreamDay.Services
{
    public class DashboardService
    {
        private readonly ApplicaitonDbContext _dbContext;

        public DashboardService(ApplicaitonDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<WeddingDashboard> GetDashboardDataAsync(Guid userId)
        {
            var wedding = await _dbContext.Weddings.Include(x=>x.CheckListItems)
                                  .FirstOrDefaultAsync(w => w.UserId == userId).ConfigureAwait(false);

            if (wedding == null)
                return null;

            var dashboard = new WeddingDashboard
            {
                WeddingId = wedding.Id,
                PartnerOneName = wedding.PartnerOneName,
                PartnerTwoName = wedding.PartnerTwoName,
                WeddingDate = wedding.WeddingDate,
                TotalBudget = wedding.TotalBudget,
                PlannerId = wedding.PlannerId,
                ToDoList = wedding.CheckListItems
                                .Select(c => new ChecklistItemModel() {
                                    
                                    Id = c.Id,
                                    Name = c.Name,
                                    IsCompleted = c.IsCompleted,
                                    DueDate = c.DueDate,
                                    Description = c.Description,
                                    WeddingId = c.WeddingId
                                })
                                .ToList()
            };

            return dashboard;
        }
    }
}
