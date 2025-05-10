using DreamDay.Entites;
using DreamDay.Enums;
using DreamDay.Models;
using DreamDay.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DreamDay.Controllers
{
    public class PlannerNotesController : Controller
    {
        private readonly DreamDay.Services.PlannerNotesService _plannerNotesService;

        public PlannerNotesController(PlannerNotesService plannerNotesService)
        {
            _plannerNotesService = plannerNotesService;
        }

        public async Task<IActionResult> Index(Guid weddingId)
        {
            var notes = await _plannerNotesService.GetWeeddingNotes(weddingId);
            ViewBag.WeddingId = weddingId;
            return View(notes);
        }

        [HttpPost]
        public async Task<IActionResult> PostNote(WeddingNoteModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index", new { weddingId = model.WeddingId });
            }
            model.SenderRole = Role.PLANNER.ToString();
            await _plannerNotesService.CreateWeddingNote(model);

            return RedirectToAction("Index", new { weddingId = model.WeddingId });
        }
    }
}
