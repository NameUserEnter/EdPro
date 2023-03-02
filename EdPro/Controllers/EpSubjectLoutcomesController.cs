using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EdPro.Models;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace EdPro.Controllers
{
    [Authorize(Roles = "admin, user, worker")]
    public class EpSubjectLoutcomesController : Controller
    {
        private readonly EdProContext _context;

        public EpSubjectLoutcomesController(EdProContext context)
        {
            _context = context;
        }

        // GET: EpSubjectLoutcomes
        public async Task<IActionResult> Index(int? id, string? learningOutcome1, string? loname, int? specialtyId)
        {
            if (id == 0) return RedirectToAction("LearningOutcomes", "Index");
            ViewBag.LearningOutcomeId = id;
            ViewBag.LearningOutcomeLearningOutcome1 = learningOutcome1;
            ViewBag.LearningOutcomeLoname = loname;
            ViewBag.LearningOutcomeSpecialtyId = specialtyId;
            var epSubjectLoutcomesByLearningOutcome = _context.EpSubjectLoutcomes.Where(f => f.LearningOutcomeId == id).Include(f => f.Subject).Include(f => f.LearningOutcome);
            return View(await epSubjectLoutcomesByLearningOutcome.ToListAsync());
        }

        // GET: EpSubjectLoutcomes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.EpSubjectLoutcomes == null)
            {
                return NotFound();
            }

            var epSubjectLoutcome = await _context.EpSubjectLoutcomes
                .Include(e => e.LearningOutcome)
                .Include(e => e.Subject)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (epSubjectLoutcome == null)
            {
                return NotFound();
            }

            return View(epSubjectLoutcome);
        }

        // GET: EpSubjectLoutcomes/Create
        [Authorize(Roles = "admin")]
        public IActionResult Create(int learningOutcomeId)
        {
            ViewBag.LearningOutcomeId = learningOutcomeId;
            ViewBag.LearningOutcomeLearningOutcome1 = _context.LearningOutcomes.Where(c => c.Id == learningOutcomeId).FirstOrDefault().LearningOutcome1;
            ViewBag.LearningOutcomeLoname = _context.LearningOutcomes.Where(c => c.Id == learningOutcomeId).FirstOrDefault().Loname;
            ViewBag.LearningOutcomeSpecialtyId = _context.LearningOutcomes.Where(c => c.Id == learningOutcomeId).FirstOrDefault().SpecialityId;

            int specialityId = _context.LearningOutcomes.Where(c => c.Id == learningOutcomeId).FirstOrDefault().SpecialityId;
            List<int> educationProgramsId = _context.EducationPrograms.Where(c => c.SpecialityId == specialityId).Select(c => c.Id).ToList();

            ViewData["SubjectId"] = new SelectList(_context.Subjects.Where(b => educationProgramsId.Contains(b.EprogramId)), "Id", "Name");
            return View();
        }

        // POST: EpSubjectLoutcomes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int learningOutcomeId, [Bind("Id,SubjectId,LearningOutcomeId")] EpSubjectLoutcome epSubjectLoutcome)
        {
            epSubjectLoutcome.LearningOutcomeId = learningOutcomeId;
            if (ModelState.IsValid)
            {
                _context.Add(epSubjectLoutcome);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "EpSubjectLoutcomes", new
                {
                    id = learningOutcomeId,
                    learningOutcome1 = _context.LearningOutcomes.Where(c => c.Id == learningOutcomeId).FirstOrDefault().LearningOutcome1,
                    loname = _context.LearningOutcomes.Where(c => c.Id == learningOutcomeId).FirstOrDefault().Loname,
                    specialtyId = _context.LearningOutcomes.Where(c => c.Id == learningOutcomeId).FirstOrDefault().SpecialityId
                });
            }
            ViewBag.LearningOutcomeId = learningOutcomeId;
            ViewBag.LearningOutcomeLearningOutcome1 = _context.LearningOutcomes.Where(c => c.Id == learningOutcomeId).FirstOrDefault().LearningOutcome1;
            ViewBag.LearningOutcomeLoname = _context.LearningOutcomes.Where(c => c.Id == learningOutcomeId).FirstOrDefault().Loname;
            ViewBag.LearningOutcomeSpecialtyId = _context.LearningOutcomes.Where(c => c.Id == learningOutcomeId).FirstOrDefault().SpecialityId;
            int specialtyId = _context.LearningOutcomes.Where(c => c.Id == learningOutcomeId).FirstOrDefault().SpecialityId;
            List<int> educationProgramsId = _context.EducationPrograms.Where(c => c.SpecialityId == specialtyId).Select(c => c.Id).ToList();

            ViewData["SubjectId"] = new SelectList(_context.Subjects.Where(b => educationProgramsId.Contains(b.EprogramId)), "Id", "Name");
            return View(epSubjectLoutcome);
        }

        // GET: EpSubjectLoutcomes/Edit/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.EpSubjectLoutcomes == null)
            {
                return NotFound();
            }

            var epSubjectLoutcome = await _context.EpSubjectLoutcomes.FindAsync(id);
            if (epSubjectLoutcome == null)
            {
                return NotFound();
            }
            ViewData["LearningOutcomeId"] = new SelectList(_context.LearningOutcomes, "Id", "LearningOutcome1", epSubjectLoutcome.LearningOutcomeId);
            ViewData["SubjectId"] = new SelectList(_context.Subjects, "Id", "Name", epSubjectLoutcome.SubjectId);
            return View(epSubjectLoutcome);
        }

        // POST: EpSubjectLoutcomes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,SubjectId,LearningOutcomeId")] EpSubjectLoutcome epSubjectLoutcome)
        {
            if (id != epSubjectLoutcome.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(epSubjectLoutcome);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EpSubjectLoutcomeExists(epSubjectLoutcome.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["LearningOutcomeId"] = new SelectList(_context.LearningOutcomes, "Id", "LearningOutcome1", epSubjectLoutcome.LearningOutcomeId);
            ViewData["SubjectId"] = new SelectList(_context.Subjects, "Id", "Name", epSubjectLoutcome.SubjectId);
            return View(epSubjectLoutcome);
        }

        // GET: EpSubjectLoutcomes/Delete/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.EpSubjectLoutcomes == null)
            {
                return NotFound();
            }

            var epSubjectLoutcome = await _context.EpSubjectLoutcomes
                .Include(e => e.LearningOutcome)
                .Include(e => e.Subject)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (epSubjectLoutcome == null)
            {
                return NotFound();
            }

            return View(epSubjectLoutcome);
        }

        // POST: EpSubjectLoutcomes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.EpSubjectLoutcomes == null)
            {
                return Problem("Entity set 'EdProContext.EpSubjectLoutcomes'  is null.");
            }
            var epSubjectLoutcome = await _context.EpSubjectLoutcomes.FindAsync(id);
            if (epSubjectLoutcome != null)
            {
                _context.EpSubjectLoutcomes.Remove(epSubjectLoutcome);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EpSubjectLoutcomeExists(int id)
        {
          return (_context.EpSubjectLoutcomes?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
