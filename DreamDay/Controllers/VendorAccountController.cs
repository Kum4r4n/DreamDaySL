using DreamDay.Models;
using DreamDay.Services;
using DreamDay.Enums;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DreamDay.Controllers;

public class VendorAccountController : Controller
{
    private readonly RegisterService _registerService;
    private readonly VendorService _vendorService;

    public VendorAccountController(RegisterService registerService, VendorService vendorService)
    {
        _registerService = registerService;
        _vendorService = vendorService;
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(VendorRegisterModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var user = await _registerService.RegisterAsync(new RegisterModel
        {
            Name = model.Name,
            Email = model.Email,
            Password = model.Password,
            ConfirmPassword = model.ConfirmPassword,
            Role = Role.VENDOR
        });

        await _vendorService.AddOrUpdateVendorForUserAsync(user.Id, new VendorModel
        {
            Name = model.Name,
            Category = model.Category,
            Description = model.Description ?? string.Empty,
            ContactInfo = model.ContactInfo,
            PriceEstimate = model.PriceEstimate
        });

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Email),
            new Claim(ClaimTypes.Role, user.Role.ToString())
        };

        var identity = new ClaimsIdentity(claims, "DreamDay");
        var principal = new ClaimsPrincipal(identity);
        await HttpContext.SignInAsync("DreamDay", principal);

        return RedirectToAction("Manage");
    }

    [HttpGet]
    public async Task<IActionResult> Manage()
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var vendor = await _vendorService.GetVendorByUserIdAsync(userId) ?? new VendorModel();
        return View(vendor);
    }

    [HttpPost]
    public async Task<IActionResult> Manage(VendorModel model)
    {
        if (!ModelState.IsValid)
            return View(model);
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        await _vendorService.AddOrUpdateVendorForUserAsync(userId, model);
        ViewBag.Message = "Details updated";
        return View(model);
    }
}
