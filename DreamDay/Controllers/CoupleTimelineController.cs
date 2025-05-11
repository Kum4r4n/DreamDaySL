using DreamDay.Entites;
using DreamDay.Models;
using DreamDay.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace DreamDay.Controllers
{
    [Authorize(Roles = "COUPLE")]
    public class CoupleTimelineController : Controller
    {
        private readonly CoupleTimelineService _coupleTimelineService;

        public CoupleTimelineController(CoupleTimelineService coupleTimelineService)
        {
            _coupleTimelineService = coupleTimelineService;
        }

        public async Task<IActionResult> Index()
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var data = await _coupleTimelineService.GetTimelinesByUser(userId).ConfigureAwait(false);
            if(data == null)
            {
                return RedirectToAction("Create", "Wedding");
            }

            ViewBag.WeddingId = data.Value.Item2;
            return View(data.Value.Item1);
        }

        public IActionResult Create(Guid weddingId)
        {
            ViewBag.WeddingId = weddingId;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(WeddingTimelineItemModel item)
        {
            if (!ModelState.IsValid) return View(item);
            var data = await _coupleTimelineService.Create(item).ConfigureAwait(false);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var item = await _coupleTimelineService.GetByIdAsync(id).ConfigureAwait(false);
            return View(item);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(WeddingTimelineItemModel item)
        {
            if (!ModelState.IsValid) return View(item);
            var data = await _coupleTimelineService.Update(item).ConfigureAwait(false);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            var item = await _coupleTimelineService.Delete(id).ConfigureAwait(false);
            return RedirectToAction("Index");
        }
    }
}
