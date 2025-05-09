using DreamDay.Models;
using DreamDay.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DreamDay.Controllers
{
    public class LoginController : Controller
    {
        private readonly LoginService _loginService;

        public LoginController(LoginService loginService)
        {
            _loginService = loginService;
        }

        public IActionResult Index()
        {
            if (User.Identity?.IsAuthenticated ?? false)
            {
                return RedirectToAction("Index", "Dashboard");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(LoginModel loginModel)
        {
            if(!ModelState.IsValid)
                return View(loginModel);

            var user = await _loginService.LoginAsync(loginModel).ConfigureAwait(false);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            var identity = new ClaimsIdentity(claims, "DreamDay");
            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync("DreamDay", principal);
            return RedirectToAction("Index","Dashboard");

        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("DreamDay");
            return RedirectToAction("Index", "Login");
        }
    }
}
