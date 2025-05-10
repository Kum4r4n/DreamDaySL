using DreamDay.Data;
using DreamDay.Entites;
using DreamDay.Models;
using Microsoft.EntityFrameworkCore;

namespace DreamDay.Services
{
    public class AdminService
    {
        private readonly ApplicaitonDbContext _dbContext;

        public AdminService(ApplicaitonDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<WeddingAdminViewModel>> GetAllWeddingsWithoutPlannerAsync()
        {
            var weddings = await _dbContext.Weddings.Where(x=>x.PlannerId == null)
                .Select(w => new WeddingAdminViewModel
                {
                    WeddingId = w.Id,
                    PartnerOneName = w.PartnerOneName,
                    PartnerTwoName = w.PartnerTwoName,
                    WeddingDate = w.WeddingDate
                })
                .ToListAsync();

            return weddings;
        }

        public async Task<List<User>> GetPlannersListsAsync(Guid weddingId)
        {
            var wedding = await _dbContext.Weddings.FirstOrDefaultAsync(x => x.Id == weddingId);
            if (wedding == null)
                return null;

            var planners = await _dbContext.Users.Where(x => x.Role == Enums.Role.PLANNER).ToListAsync();
            return planners;
        }

        public async Task<Wedding> AssignPlannerToWeddingAsync(Guid weddingId, Guid plannerId)
        {
            var wedding = await _dbContext.Weddings.FirstOrDefaultAsync(x => x.Id == weddingId);
            if (wedding == null)
                return null;

            var planner = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == plannerId);
            if (planner == null)
                return null;

            wedding.PlannerId = plannerId;
            await _dbContext.SaveChangesAsync();

            return wedding;
        }
    }
}
