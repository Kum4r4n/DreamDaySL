using DreamDay.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DreamDay.Controllers
{
    [Authorize(Roles = "PLANNER")]
    public class PlannerController : Controller
    {
        private readonly PlannerService _plannerService;

        public PlannerController(PlannerService plannerService)
        {
            _plannerService = plannerService;
        }

        public async Task<IActionResult> Dashboard()
        {
            var plannerId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var data = await _plannerService.GetWeddingAsync(plannerId);
            return View(data);
        }
    }
}
