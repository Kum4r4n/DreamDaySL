using DreamDay.Entites;
using DreamDay.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace DreamDay.Controllers
{
    [Authorize(Roles = "COUPLE")]
    public class VendorController : Controller
    {
        private readonly Services.VendorService _vendorService;

        public VendorController(VendorService vendorService)
        {
            _vendorService = vendorService;
        }

        public async Task<IActionResult> Index(string? search)
        {
            var weddingId = await _vendorService.GetWeddingIdByUser(Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!));

            if (weddingId == null)
            {
                return RedirectToAction("Create", "Wedding");
            }

            ViewBag.WeddingId = weddingId;
            ViewBag.Search = search;

            var assaginedVendors = await _vendorService.GetAssginedVendorForWeddingAsync((Guid)weddingId);
            var availableVendors = await _vendorService.GetAvailableVendorsAsync((Guid)weddingId, search);

            return View(Tuple.Create(assaginedVendors, availableVendors));
        }


        [HttpPost]
        public async Task<IActionResult> Assign(Guid weddingId, Guid vendorId)
        {

            await _vendorService.AssignVendorToWeddingAsync(weddingId, vendorId);

            return RedirectToAction("Index", new { weddingId });
        }


        [HttpPost]
        public async Task<IActionResult> Remove(Guid weddingId, Guid vendorId)
        {
            await _vendorService.RemoveVendorFromWeddingAsync(weddingId, vendorId);
            return RedirectToAction("Index", new { weddingId });
        }
    }
}
