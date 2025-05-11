using DreamDay.Data;
using DreamDay.Entites;
using DreamDay.Models;
using Microsoft.EntityFrameworkCore;

namespace DreamDay.Services
{
    public class GuestService
    {
        private readonly ApplicaitonDbContext _dbContext;

        public GuestService(ApplicaitonDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<GuestModel>> GetListAsync(Guid userId)
        {
            var wedding = _dbContext.Weddings.Include(x=>x.Guests).FirstOrDefault(w => w.UserId == userId);
            if (wedding == null)
                return null;

            return wedding.Guests.Select(x => new GuestModel()
            {
                Id = x.Id,
                Name = x.Name,
                Email = x.Email,
                MealPreference = x.MealPreference,
                WeddingId = wedding.Id,
                IsAttending = x.IsAttending

            }).ToList();
        }

        public async Task<GuestModel> GetByIdAsync(Guid id)
        {
            var guest = await _dbContext.Guests.FindAsync(id);
            if (guest == null)
                return null;

            return new GuestModel()
            {
                Id = guest.Id,
                Name = guest.Name,
                Email = guest.Email,
                MealPreference = guest.MealPreference,
                WeddingId = guest.WeddingId,
                IsAttending = guest.IsAttending
            };
        }

        public async Task<GuestModel> AddAsync(Guid userId, GuestModel model)
        {
            var wedding = _dbContext.Weddings.Include(x=>x.Guests).FirstOrDefault(w => w.UserId == userId);

            if (wedding == null)
                return null;

            var guest = new Guest()
            {
                Name = model.Name,
                Email = model.Email,
                MealPreference = model.MealPreference,
                WeddingId = wedding.Id,
                IsAttending = model.IsAttending
            };

            await _dbContext.Guests.AddAsync(guest);
            await _dbContext.SaveChangesAsync();
            
            var guestModel = new GuestModel()
            {
                Id = guest.Id,
                Name = guest.Name,
                Email = guest.Email,
                MealPreference = guest.MealPreference,
                WeddingId = guest.WeddingId,
                IsAttending = guest.IsAttending
            };

            return guestModel;
        }

        public async Task<GuestModel> UpdateAsync(GuestModel model)
        {
         
            var guest = await _dbContext.Guests.FindAsync(model.Id);
            if (guest == null)
                return null;

            guest.Name = model.Name;
            guest.Email = model.Email;
            guest.MealPreference = model.MealPreference;
            guest.IsAttending = model.IsAttending;

            await _dbContext.SaveChangesAsync();

            return new GuestModel()
            {
                Id = guest.Id,
                Name = guest.Name,
                Email = guest.Email,
                MealPreference = guest.MealPreference,
                WeddingId = guest.WeddingId,
                IsAttending = guest.IsAttending
            };
        }

        public async Task DeleteAsync(Guid id)
        {
            var guest = await _dbContext.Guests.FindAsync(id);
            if (guest != null)
            {
                _dbContext.Guests.Remove(guest);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
