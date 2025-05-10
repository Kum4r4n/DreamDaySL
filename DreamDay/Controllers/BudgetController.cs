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
    public class BudgetController : Controller
    {
        private readonly BudgetService _budgetService;

        public BudgetController(BudgetService budgetService)
        {
            _budgetService = budgetService;
        }

        public async Task<IActionResult> Index()
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var data = await _budgetService.GetListAsync(userId);
            if(data == null)
            {
                return RedirectToAction("Create", "Wedding");
            }

            ViewBag.TotalBudget = data.Value.Item2;
            ViewBag.TotalAllocated = data.Value.Item1.Sum(x => x.AllocatedAmount);
            ViewBag.TotalSpent = data.Value.Item1.Sum(x => x.SpentAmount);

            return View(data.Value.Item1);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(BudgetItemModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var data = await _budgetService.CreateAsync(userId, model);
            if (data == null) return RedirectToAction("Create", "Wedding");

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var data = await _budgetService.GetBudgetItemAsync(id);
            if (data == null) return NotFound();
            return View(data);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(BudgetItemModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var data = await _budgetService.UpdateAsync(model);
            if (data == null)
                return NotFound();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _budgetService.DeleteAsync(id);
            return RedirectToAction("Index");
        }
    }
}
