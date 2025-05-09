using DreamDay.Entites;
using DreamDay.Models;
using DreamDay.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace DreamDay.Controllers
{
    [Authorize]
    public class WeddingController : Controller
    {

        private readonly WeddingService _weddingService;

        public WeddingController(WeddingService weddingService)
        {
            _weddingService = weddingService;
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(WeddingFormModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            await _weddingService.CreateAsync(userId, model).ConfigureAwait(false);
            return RedirectToAction("Index", "Dashboard");
        }


        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
           var model = await _weddingService.GetWeddingByIdAsync(id).ConfigureAwait(false);
            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> Edit(WeddingFormModel model)
        {
            if (!ModelState.IsValid) return View(model);

            await _weddingService.UpdateAsync(model).ConfigureAwait(false);
            return RedirectToAction("Index", "Dashboard");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _weddingService.DeleteAsync(id).ConfigureAwait(false);
            return RedirectToAction("Index", "Dashboard");
        }
    }
}
