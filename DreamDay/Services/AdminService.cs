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


        public async Task<VendorModel> AddVendor(VendorModel vendorModel)
        {
            var vendor = new Vendor()
            {
               
                Name = vendorModel.Name,
                Category = vendorModel.Category,
                ContactInfo = vendorModel.ContactInfo,
                PriceEstimate = vendorModel.PriceEstimate,
                Description = vendorModel.Description
            };

            _dbContext.Vendors.Add(vendor);
            await _dbContext.SaveChangesAsync();

            return new VendorModel()
            {
                Id = vendor.Id,
                Name = vendor.Name,
                Category = vendor.Category,
                ContactInfo = vendor.ContactInfo,
                PriceEstimate = vendor.PriceEstimate,
                Description = vendor.Description
            };

        }


        public async Task<VendorModel> UpdateVendor(VendorModel vendorModel)
        {
            var vendor = await _dbContext.Vendors.FirstOrDefaultAsync(x => x.Id == vendorModel.Id);
            if (vendor == null)
                return null;

            vendor.Name = vendorModel.Name;
            vendor.Category = vendorModel.Category;
            vendor.ContactInfo = vendorModel.ContactInfo;
            vendor.PriceEstimate = vendorModel.PriceEstimate;
            vendor.Description = vendorModel.Description;

            await _dbContext.SaveChangesAsync();

            return new VendorModel()
            {
                Id = vendor.Id,
                Name = vendor.Name,
                Category = vendor.Category,
                ContactInfo = vendor.ContactInfo,
                PriceEstimate = vendor.PriceEstimate,
                Description = vendor.Description
            };
        }

        public async Task<VendorModel> GetVendorById(Guid id)
        {
            var vendor = await _dbContext.Vendors.FirstOrDefaultAsync(x => x.Id == id);
            if (vendor == null)
                return null;

            return new VendorModel()
            {
                Id = vendor.Id,
                Name = vendor.Name,
                Category = vendor.Category,
                ContactInfo = vendor.ContactInfo,
                PriceEstimate = vendor.PriceEstimate,
                Description = vendor.Description

            };

        }


        public async Task<bool> DeleteVendor(Guid id)
        {
            var vendor = await _dbContext.Vendors.FirstOrDefaultAsync(x => x.Id == id);
            if (vendor == null)
                return false;

            _dbContext.Vendors.Remove(vendor);
            await _dbContext.SaveChangesAsync();

            return true;
        }




        public async Task<List<VendorModel>> GetVendors()
        {
            var data = _dbContext.Vendors.ToList().Select(s => new VendorModel()
            {
                Id = s.Id,
                Name = s.Name,
                Category = s.Category,
                ContactInfo = s.ContactInfo,
                PriceEstimate = s.PriceEstimate,
                Description = s.Description
            }).ToList();

            return data;
        }

        public async Task<(int,int,int,int)> GetAdminDashboardStatsAsync()
        {
            var userStats = await GetUserRoleCountsAsync();
            var vendorCount = await GetVendorCountAsync();

            return new(

                userStats.Total,
                userStats.Couples,
                userStats.Planners,
                vendorCount
            );
        }

        private async Task<(int Total, int Couples, int Planners)> GetUserRoleCountsAsync()
        {
            var stats = await _dbContext.Users
                .GroupBy(u => u.Role)
                .Select(g => new { Role = g.Key, Count = g.Count() })
                .ToListAsync();

            var total = stats.Sum(x => x.Count);
            var couples = stats.FirstOrDefault(x => x.Role == Enums.Role.COUPLE)?.Count ?? 0;
            var planners = stats.FirstOrDefault(x => x.Role == Enums.Role.PLANNER)?.Count ?? 0;

            return (total, couples, planners);
        }

        private async Task<int> GetVendorCountAsync()
        {
            return await _dbContext.Vendors.CountAsync();
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
