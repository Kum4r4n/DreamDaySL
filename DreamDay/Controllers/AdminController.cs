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

        public IActionResult Dashboard()
        {
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
