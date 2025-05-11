using DreamDay.Entites;
using DreamDay.Models;
using DreamDay.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace DreamDay.Controllers
{
    public class AdminController : Controller
    {

        private readonly AdminService _adminService;

        public AdminController(AdminService adminService)
        {
            _adminService = adminService;
        }

        public async Task<IActionResult> Vendors()
        {
            var vendors = await _adminService.GetVendors();
            return View(vendors);
        }

        public IActionResult AddVendor() => View();

        [HttpPost]
        public async Task<IActionResult> AddVendor(VendorModel vendor)
        {
            if (!ModelState.IsValid) return View(vendor);
            await _adminService.AddVendor(vendor);
            return RedirectToAction("Vendors");
        }


        public async Task<IActionResult> EditVendor(Guid id)
        {
            var vendor = await _adminService.GetVendorById(id);
            return vendor == null ? NotFound() : View(vendor);
        }

        [HttpPost]
        public async Task<IActionResult> EditVendor(VendorModel vendor)
        {
            if (!ModelState.IsValid) return View(vendor);
            await _adminService.UpdateVendor(vendor);
            return RedirectToAction("Vendors");
        }

        public async Task<IActionResult> DeleteVendor(Guid id)
        {
            await _adminService.DeleteVendor(id);
            return RedirectToAction("Vendors");
        }

        public async Task<IActionResult> Dashboard()
        {
            var stats = await _adminService.GetAdminDashboardStatsAsync();
            ViewBag.UserCount = stats.Item1;
            ViewBag.CoupleCount = stats.Item2;
            ViewBag.PlannerCount = stats.Item3;
            ViewBag.VendorCount = stats.Item4;
            return View();
        }

        public async Task<IActionResult> AssignPlanner()
        {
            var weddings =  await _adminService.GetAllWeddingsWithoutPlannerAsync();
            return View(weddings);
        }


        public async Task<IActionResult> Assign(Guid weddingId)
        {
            var planners = await _adminService.GetPlannersListsAsync(weddingId);

            ViewBag.Planners = planners.Select(s=> new SelectListItem { Value = s.Id.ToString(), Text = s.Name }).ToList();
            ViewBag.WeddingId = weddingId;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Assign(Guid weddingId, Guid plannerId)
        {
            await _adminService.AssignPlannerToWeddingAsync(weddingId, plannerId);

            return RedirectToAction("AssignPlanner");
        }
    }
}
