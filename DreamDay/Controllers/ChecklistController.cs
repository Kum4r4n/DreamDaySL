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
    public class ChecklistController : Controller
    {

        private readonly ChecklistService _checklistService;

        public ChecklistController(ChecklistService checklistService)
        {
            _checklistService = checklistService;
        }

        public async Task<IActionResult> Index()
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var data = await _checklistService.GetListAsync(userId).ConfigureAwait(false);
            if(data == null)
            {
                return RedirectToAction("Create", "Wedding");
            }
            return View(data);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ChecklistItemModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var data = await _checklistService.CreateCheckList(userId, model).ConfigureAwait(false);
            if (data == null)
            {
                return RedirectToAction("Create", "Wedding");
            }
            return RedirectToAction("Index");
        }


        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var data = await _checklistService.GetCheckListItemAsync(id).ConfigureAwait(false);

            if(data == null)
                return NotFound();
            return View(data);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ChecklistItemModel model)
        {
            if (!ModelState.IsValid) return View(model);


            var data = await _checklistService.UpdateAsync(model).ConfigureAwait(false);
            if(data == null) 
                return NotFound();
            return RedirectToAction("Index");
        }


        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _checklistService.DeleteAsync(id).ConfigureAwait(false);
            return RedirectToAction("Index");
        }

    }
}
