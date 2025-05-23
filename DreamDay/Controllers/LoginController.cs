﻿using DreamDay.Enums;
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
                if(User.Claims.First(x=>x.Type == ClaimTypes.Role).Value == Role.PLANNER.ToString())
                {
                    return RedirectToAction("Dashboard", "Planner");
                }else if(User.Claims.First(x => x.Type == ClaimTypes.Role).Value == Role.ADMIN.ToString())
                {
                    return RedirectToAction("Dashboard", "Admin");
                }
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

            if (user.Role == Role.PLANNER)
            {
                return RedirectToAction("Dashboard", "Planner");

            }else if (user.Role == Role.ADMIN)
            {
                return RedirectToAction("Dashboard", "Admin");
            }

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
