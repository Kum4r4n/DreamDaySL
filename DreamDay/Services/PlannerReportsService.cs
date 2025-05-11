using DreamDay.Data;
using DreamDay.Models;
using Microsoft.EntityFrameworkCore;

namespace DreamDay.Services
{
    public class PlannerReportsService
    {
        private readonly ApplicaitonDbContext _dbContext;

        public PlannerReportsService(ApplicaitonDbContext dbContext)
        {
            _dbContext = dbContext;
        }



        public async Task<List<ChecklistProgressReport>> ChecklistCompletion(Guid plannerId)
        {
            var data = _dbContext.Weddings
               .Where(w => w.PlannerId == plannerId)
               .Select(w => new ChecklistProgressReport
               {
                   WeddingId = w.Id,
                   CoupleName = w.PartnerOneName + " & " + w.PartnerTwoName,
                   Total = _dbContext.CheckListItems.Count(c => c.WeddingId == w.Id),
                   Completed = _dbContext.CheckListItems.Count(c => c.WeddingId == w.Id && c.IsCompleted)
               })
               .ToList();

            foreach (var d in data)
            {
                d.PercentComplete = d.Total > 0 ? (int)Math.Round((double)d.Completed * 100 / d.Total) : 0;
            }

            return data;
        }


        public async Task<List<CategoryReport>> VendorCategoryDistribution(Guid plannerId)
        {
            var categoryData = _dbContext.WeddingVendors
                .Where(wv => _dbContext.Weddings.Any(w => w.Id == wv.WeddingId && w.PlannerId == plannerId))
                .Select(wv => wv.Vendor.Category)
                .GroupBy(c => c)
                .Select(g => new CategoryReport
                {
                    Category = g.Key,
                    Count = g.Count()
                })
                .OrderByDescending(c => c.Count)
                .ToList();

            return categoryData;
        }


        public async Task<decimal?> GetAverageBudgetByPlanner(Guid plannerId)
        {
            var averageBudget = await _dbContext.Weddings
                .Where(w => w.PlannerId == plannerId)
                .Select(w => (decimal?)w.TotalBudget)
                .AverageAsync();

            return averageBudget;
        }


        public async Task<List<VendorReport>> GetTopVendorsByPlanner(Guid plannerId)
        {
            var vendorUsage = _dbContext.WeddingVendors
               .Where(wv => _dbContext.Weddings.Any(w => w.Id == wv.WeddingId && w.PlannerId == plannerId))
               .GroupBy(wv => wv.VendorId)
               .Select(group => new
               {
                   VendorId = group.Key,
                   Count = group.Count()
               })
               .OrderByDescending(x => x.Count)
               .Take(5)
               .ToList();

            var vUsage = vendorUsage.Select(x => x.VendorId);

            var topVendors = _dbContext.Vendors
               .Where(v => vUsage.Contains(v.Id))
               .ToList();

            var report = vendorUsage
                .Join(topVendors, u => u.VendorId, v => v.Id, (u, v) => new VendorReport
                {
                    Name = v.Name,
                    Category = v.Category,
                    Assignments = u.Count
                })
                .ToList();

            return report;
        }
    }
}
