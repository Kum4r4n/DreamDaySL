using DreamDay.Entites;
using DreamDay.Models;
using DreamDay.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DreamDay.Controllers;

public class RegisterController : Controller
{
    private readonly RegisterService _registerService;

    public RegisterController(RegisterService registerService)
    {
        _registerService = registerService;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Index(RegisterModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var data = await _registerService.RegisterAsync(model).ConfigureAwait(false);
        var claims = new List<Claim>
        {
                new Claim(ClaimTypes.NameIdentifier, data.Id.ToString()),
                new Claim(ClaimTypes.Name, data.Email),
                new Claim(ClaimTypes.Role, data.Role.ToString())
            };

        var identity = new ClaimsIdentity(claims, "DreamDay");
        var principal = new ClaimsPrincipal(identity);
        await HttpContext.SignInAsync("DreamDay", principal);
        return RedirectToAction("Index", "Dashboard");
    }
}
