using DreamDay.Data;
using DreamDay.Models;
using Microsoft.EntityFrameworkCore;

namespace DreamDay.Services
{
    public class VendorService
    {
        private readonly ApplicaitonDbContext _dbContext;

        public VendorService(ApplicaitonDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<VendorModel>> GetAllVendorsAsync(string? search)
        {
            var data =  await _dbContext.Vendors
                .Where(x => 
                string.IsNullOrEmpty(search) || 
                x.Name.ToLower().Contains(search.ToLower()) || 
                x.Category.ToLower().Contains(search.ToLower()) || 
                x.Description.ToLower().Contains(search.ToLower())
                ).ToListAsync();

            return data.Select(x => new VendorModel()
            {
                Id = x.Id,
                Name = x.Name,
                Category = x.Category,
                Description = x.Description,
                ContactInfo = x.ContactInfo,
                PriceEstimate = x.PriceEstimate

            }).ToList();
        }
    }
}
