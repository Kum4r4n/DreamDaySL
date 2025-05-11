using DreamDay.Models;
using DreamDay.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Security.Claims;

namespace DreamDay.Controllers
{
    [Authorize(Roles = "PLANNER")]
    public class PlannerReportsController : Controller
    {
        private readonly PlannerReportsService _plannerReportsService;

        public PlannerReportsController(PlannerReportsService plannerReportsService)
        {
            _plannerReportsService = plannerReportsService;
        }

        public async Task<IActionResult> TopVendors()
        {
            var plannerId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var data = await _plannerReportsService.GetTopVendorsByPlanner(Guid.Parse(plannerId!));

            return View(data);
        }

        public async Task<IActionResult> AverageBudget()
        {
            var plannerId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var data = await _plannerReportsService.GetAverageBudgetByPlanner(Guid.Parse(plannerId!));

            ViewBag.AverageBudget = data?.ToString("C", CultureInfo.CurrentCulture) ?? "N/A";
            return View();
        }

        public async Task<IActionResult> VendorCategoryDistribution()
        {
            var plannerId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var data = await _plannerReportsService.VendorCategoryDistribution(Guid.Parse(plannerId!));
            return View(data);
        }


        public async Task<IActionResult> ChecklistProgressReport()
        {
            var plannerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var data = await _plannerReportsService.ChecklistCompletion(Guid.Parse(plannerId!));
            return View(data);
        }
    }
}
