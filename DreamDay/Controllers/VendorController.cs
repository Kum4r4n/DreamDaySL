using DreamDay.Services;
using Microsoft.AspNetCore.Mvc;

namespace DreamDay.Controllers
{
    public class VendorController : Controller
    {
        private readonly Services.VendorService _vendorService;

        public VendorController(VendorService vendorService)
        {
            _vendorService = vendorService;
        }

        public async Task<IActionResult> Index(string? search)
        {
            var data = await _vendorService.GetAllVendorsAsync(search);
            return View(data);
        }
    }
}
