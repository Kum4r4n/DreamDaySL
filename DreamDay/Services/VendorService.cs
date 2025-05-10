using DreamDay.Data;
using DreamDay.Entites;
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

        public async Task<Guid?> GetWeddingIdByUser(Guid userId)
        {
            var wedding = await _dbContext.Weddings.FirstOrDefaultAsync(x => x.UserId == userId);
            return wedding?.Id;
        }


        public async Task<List<VendorModel>> GetAssginedVendorForWeddingAsync(Guid weddingId)
        {
            var data = await _dbContext.WeddingVendors.Include(x => x.Vendor).Where(x => x.WeddingId == weddingId).Select(x => x.Vendor).ToListAsync();

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


        public async Task<List<VendorModel>> GetAvailableVendorsAsync(Guid weddingId, string? search)
        {
            var assgindVendors = await _dbContext.WeddingVendors.Include(x => x.Vendor).Where(x => x.WeddingId == weddingId).Select(x => x.Vendor).ToListAsync();

            var vendorIds = assgindVendors.Select(x => x.Id).ToList();

            var availableVendors = await _dbContext.Vendors
                .Where(x => !vendorIds.Contains(x.Id) &&
                    (string.IsNullOrEmpty(search) ||
                    x.Name.ToLower().Contains(search.ToLower()) ||
                    x.Category.ToLower().Contains(search.ToLower()) ||
                    x.Description.ToLower().Contains(search.ToLower())))
                .ToListAsync();

            return availableVendors.Select(x => new VendorModel()
            {

                Id = x.Id,
                Name = x.Name,
                Category = x.Category,
                Description = x.Description,
                ContactInfo = x.ContactInfo,
                PriceEstimate = x.PriceEstimate

            }).ToList();
        }

        public async Task AssignVendorToWeddingAsync(Guid weddingId, Guid vendorId)
        {
            var weddingVendor = new WeddingVendor()
            {
                WeddingId = weddingId,
                VendorId = vendorId
            };


            var vendor = _dbContext.Vendors.FirstOrDefault(x => x.Id == vendorId);

            //if assaig nthis vendor to wedding this need to add to budget also
            var budget = _dbContext.BudgetItems.Add(new BudgetItem()
            {
                AllocatedAmount = vendor!.PriceEstimate,
                Category = "Vendor",
                Description = "Vendor: " + vendor!.Name,
                SpentAmount = vendor.PriceEstimate,
                WeddingId = weddingId,
                VendorId = vendorId

            });

            await _dbContext.WeddingVendors.AddAsync(weddingVendor);
            await _dbContext.SaveChangesAsync();
        }

        public async Task RemoveVendorFromWeddingAsync(Guid weddingId, Guid vendorId)
        {
            var weddingVendor = await _dbContext.WeddingVendors
                .FirstOrDefaultAsync(x => x.WeddingId == weddingId && x.VendorId == vendorId);

            if (weddingVendor != null)
            {
                // Remove the associated budget item
                var budgetItem = await _dbContext.BudgetItems
                    .FirstOrDefaultAsync(x => x.WeddingId == weddingId && x.VendorId == vendorId);
                if (budgetItem != null)
                {
                    _dbContext.BudgetItems.Remove(budgetItem);
                }

                _dbContext.WeddingVendors.Remove(weddingVendor);
                await _dbContext.SaveChangesAsync();
            }
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
