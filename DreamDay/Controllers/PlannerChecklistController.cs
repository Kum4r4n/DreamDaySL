using DreamDay.Entites;
using DreamDay.Models;
using DreamDay.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DreamDay.Controllers
{
    [Authorize(Roles = "PLANNER")]
    public class PlannerChecklistController : Controller
    {
        private readonly PlannerChecklistService _plannerChecklistService;

        public PlannerChecklistController(PlannerChecklistService plannerChecklistService)
        {
            _plannerChecklistService = plannerChecklistService;
        }

        public async Task<IActionResult> Index(Guid weddingId)
        {
            var data = await _plannerChecklistService.GetChecklistAsync(weddingId).ConfigureAwait(false);
            ViewBag.WeddingId = weddingId;
            return View(data);
        }

        [HttpGet]
        public async Task<IActionResult> Create(Guid weddingId)
        {
            ViewBag.WeddingId = weddingId;
            return View(new ChecklistItemModel { WeddingId = weddingId });
        }

        [HttpPost]
        public async Task<IActionResult> Create(ChecklistItemModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var data = await _plannerChecklistService.CreateCheckListAsync(model).ConfigureAwait(false);
            return RedirectToAction("Index", new { weddingId = model.WeddingId });
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var item = await _plannerChecklistService.GetCheckListItemAsync(id).ConfigureAwait(false);
            if (item == null) return NotFound();
            return View(item);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ChecklistItemModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var data = await _plannerChecklistService.UpdateCheckListAsync(model).ConfigureAwait(false);

            return RedirectToAction("Index", new { weddingId = model.WeddingId });
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            var data = await _plannerChecklistService.DeleteCheckListItemAsync(id).ConfigureAwait(false);

            if(data == null)
                return NotFound();

            return RedirectToAction("Index", new { data });
        }
    }
}
