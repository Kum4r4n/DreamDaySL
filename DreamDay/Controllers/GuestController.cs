using DreamDay.Entites;
using DreamDay.Enums;
using DreamDay.Models;
using DreamDay.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace DreamDay.Controllers
{
    [Authorize]
    public class GuestController : Controller
    {
        private readonly Services.GuestService _guestService;

        public GuestController(GuestService guestService)
        {
            _guestService = guestService;
        }

        public async Task<IActionResult> Index()
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var data = await _guestService.GetListAsync(userId);

            if(data == null)
                return RedirectToAction("Index", "Wedding");

            return View(data);
        }

        [HttpGet]
        public IActionResult Create() {

            ViewBag.MealOptions = new SelectList(Enum.GetValues(typeof(MealPreference)));
            return View();

        }


        [HttpPost]
        public async Task<IActionResult> Create(GuestModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var data = await _guestService.AddAsync(userId, model);

            if(data == null)
                return RedirectToAction("Index", "Wedding");

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            ViewBag.MealOptions = new SelectList(Enum.GetValues(typeof(MealPreference)));
            var data = await _guestService.GetByIdAsync(id);
            if (data == null) return NotFound();
            return View(data);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(GuestModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var data = await _guestService.UpdateAsync(model);
            if (data == null) return NotFound();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _guestService.DeleteAsync(id);
            return RedirectToAction("Index");
        }
    }
}
