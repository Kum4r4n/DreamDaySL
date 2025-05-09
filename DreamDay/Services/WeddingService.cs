using DreamDay.Data;
using DreamDay.Entites;
using DreamDay.Models;
using Microsoft.EntityFrameworkCore;

namespace DreamDay.Services
{
    public class WeddingService
    {
        private readonly ApplicaitonDbContext _dbContext;

        public WeddingService(ApplicaitonDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        public async Task<Wedding> CreateAsync(Guid userId, WeddingFormModel weddingFormModel )
        {
            var wedding = new Wedding
            {
                PartnerOneName = weddingFormModel.PartnerOneName,
                PartnerTwoName = weddingFormModel.PartnerTwoName,
                WeddingDate = weddingFormModel.WeddingDate,
                TotalBudget = weddingFormModel.TotalBudget,
                UserId = userId
            };

            _dbContext.Weddings.Add(wedding);
            _dbContext.SaveChanges();

            return wedding;
        }

        public async Task<Wedding> UpdateAsync(WeddingFormModel weddingFormModel)
        {
            var wedding = _dbContext.Weddings.Find(weddingFormModel.Id);
            if (wedding == null) return null;

            wedding.PartnerOneName = weddingFormModel.PartnerOneName;
            wedding.PartnerTwoName = weddingFormModel.PartnerTwoName;
            wedding.WeddingDate = weddingFormModel.WeddingDate;
            wedding.TotalBudget = weddingFormModel.TotalBudget;

            _dbContext.SaveChanges();
            return wedding;
        }

        public async Task DeleteAsync(Guid weddingId)
        {
            var wedding = _dbContext.Weddings.Find(weddingId);
            if (wedding != null)
            {
                _dbContext.Weddings.Remove(wedding);
                _dbContext.SaveChanges();
            }
        }


        public async Task<WeddingFormModel> GetWeddingByIdAsync(Guid weddingId)
        {
            var wedding = await _dbContext.Weddings
                .FirstOrDefaultAsync(w => w.Id == weddingId);

            if (wedding == null)
                return null;

            var model = new WeddingFormModel
            {
                Id = wedding.Id,
                PartnerOneName = wedding.PartnerOneName,
                PartnerTwoName = wedding.PartnerTwoName,
                WeddingDate = wedding.WeddingDate,
                TotalBudget = wedding.TotalBudget
            };
            return model;
        }

    }
}
