using DreamDay.Data;
using DreamDay.Models;
using Microsoft.EntityFrameworkCore;

namespace DreamDay.Services
{
    public class PlannerService
    {
        private readonly ApplicaitonDbContext _dbContext;

        public PlannerService(ApplicaitonDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<PlannerWeddingViewModel>> GetWeddingAsync(Guid plannerId)
        {
            var wedding = await _dbContext.Weddings
                .Where(w => w.PlannerId == plannerId)
                .Select(w => new PlannerWeddingViewModel
                {
                    Id = w.Id,
                    PartnerOneName = w.PartnerOneName,
                    PartnerTwoName = w.PartnerTwoName,
                    WeddingDate = w.WeddingDate,
                    TotalBudget = w.TotalBudget
                })
                .ToListAsync();

            return wedding;
        }
    }
}
