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
        public async Task<IActionResult> Index(int? id, string? name, int? eprogramId, int? credit, int? controlId)
        {
            if (id == 0) return RedirectToAction("Subjects", "Index");
            ViewBag.SubjectId = id;
            ViewBag.SubjectName = name;
            ViewBag.SubjectEprogramId = eprogramId;
            ViewBag.SubjectCredit = credit;
            ViewBag.SubjectControlId = controlId;
            var epSubjectLoutcomesBySubject = _context.EpSubjectLoutcomes.Where(f => f.SubjectId == id).Include(f => f.Subject).Include(f => f.LearningOutcome);
            return View(await epSubjectLoutcomesBySubject.ToListAsync());
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
        public IActionResult Create(int subjectId)
        {
            ViewBag.SubjectId = subjectId;
            ViewBag.SubjectName = _context.Subjects.Where(c => c.Id == subjectId).FirstOrDefault().Name;
            ViewBag.SubjectEprogramId = _context.Subjects.Where(c => c.Id == subjectId).FirstOrDefault().EprogramId;
            ViewBag.SubjectCredit = _context.Subjects.Where(c => c.Id == subjectId).FirstOrDefault().Credit;
            ViewBag.SubjectConrolId = _context.Subjects.Where(c => c.Id == subjectId).FirstOrDefault().ControlId;

            int educationProgramId = _context.Subjects.Where(c => c.Id == subjectId).FirstOrDefault().EprogramId;
            int specialityId = _context.EducationPrograms.Where(c => c.Id == educationProgramId).FirstOrDefault().SpecialityId;

            ViewData["LearningOutcomeId"] = new SelectList(_context.LearningOutcomes.Where(b => b.SpecialityId == specialityId), "Id", "Loname");
            return View();
        }

        // POST: EpSubjectLoutcomes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int subjectId, [Bind("Id,SubjectId,LearningOutcomeId")] EpSubjectLoutcome epSubjectLoutcome)
        {
            epSubjectLoutcome.SubjectId = subjectId;
            if (IsUnique(epSubjectLoutcome.SubjectId, epSubjectLoutcome.LearningOutcomeId))
            {
                if (ModelState.IsValid)
                {
                    _context.Add(epSubjectLoutcome);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index", "EpSubjectLoutcomes", new
                    {
                        id = subjectId,
                        name = _context.Subjects.Where(c => c.Id == subjectId).FirstOrDefault().Name,
                        eprogramId = _context.Subjects.Where(c => c.Id == subjectId).FirstOrDefault().EprogramId,
                        credit = _context.Subjects.Where(c => c.Id == subjectId).FirstOrDefault().Credit,
                        controlId = _context.Subjects.Where(c => c.Id == subjectId).FirstOrDefault().ControlId
                    });
                }
            }
            else
            {
                ViewData["ErrorMessage"] = "Такий результат до цього предмету вже доданий!";
            }
            ViewBag.SubjectId = subjectId;
            ViewBag.SubjectName = _context.Subjects.Where(c => c.Id == subjectId).FirstOrDefault().Name;
            ViewBag.SubjectEprogramId = _context.Subjects.Where(c => c.Id == subjectId).FirstOrDefault().EprogramId;
            ViewBag.SubjectCredit = _context.Subjects.Where(c => c.Id == subjectId).FirstOrDefault().Credit;
            ViewBag.SubjectConrolId = _context.Subjects.Where(c => c.Id == subjectId).FirstOrDefault().ControlId;

            int educationProgramId = _context.Subjects.Where(c => c.Id == subjectId).FirstOrDefault().EprogramId;
            int specialityId = _context.EducationPrograms.Where(c => c.Id == educationProgramId).FirstOrDefault().SpecialityId;

            ViewData["LearningOutcomeId"] = new SelectList(_context.LearningOutcomes.Where(b => b.SpecialityId == specialityId), "Id", "Loname");
            return View(epSubjectLoutcome);
        }

        bool IsUnique(int subjectId, int learningOutcomeId)
        {
            var epSubjectLoutcomes = _context.EpSubjectLoutcomes.Where(b => b.SubjectId == subjectId && b.LearningOutcomeId == learningOutcomeId).ToList();
            if (epSubjectLoutcomes.Count == 0) return true;
            return false;
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
            var subjectId = epSubjectLoutcome.SubjectId;
            ViewBag.SubjectId = subjectId;
            ViewBag.SubjectName = _context.Subjects.Where(c => c.Id == subjectId).FirstOrDefault().Name;
            ViewBag.SubjectEprogramId = _context.Subjects.Where(c => c.Id == subjectId).FirstOrDefault().EprogramId;
            ViewBag.SubjectCredit = _context.Subjects.Where(c => c.Id == subjectId).FirstOrDefault().Credit;
            ViewBag.SubjectConrolId = _context.Subjects.Where(c => c.Id == subjectId).FirstOrDefault().ControlId;

            int educationProgramId = _context.Subjects.Where(c => c.Id == subjectId).FirstOrDefault().EprogramId;
            int specialityId = _context.EducationPrograms.Where(c => c.Id == educationProgramId).FirstOrDefault().SpecialityId;

            ViewData["LearningOutcomeId"] = new SelectList(_context.LearningOutcomes.Where(b => b.SpecialityId == specialityId), "Id", "Loname");
            //ViewData["SubjectId"] = new SelectList(_context.Subjects, "Id", "Name", epSubjectLoutcome.SubjectId);
            return View(epSubjectLoutcome);
        }

        // POST: EpSubjectLoutcomes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, int subjectId, [Bind("Id,SubjectId,LearningOutcomeId")] EpSubjectLoutcome epSubjectLoutcome)
        {
            if (id != epSubjectLoutcome.Id)
            {
                return NotFound();
            }

            if (IsUniqueEdit(epSubjectLoutcome.Id, subjectId, epSubjectLoutcome.LearningOutcomeId))
            {
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
                    //return RedirectToAction(nameof(Index));
                    return RedirectToAction("Index", "EpSubjectLoutcomes", new
                    {
                        id = subjectId,
                        name = _context.Subjects.Where(c => c.Id == subjectId).FirstOrDefault().Name,
                        eprogramId = _context.Subjects.Where(c => c.Id == subjectId).FirstOrDefault().EprogramId,
                        credit = _context.Subjects.Where(c => c.Id == subjectId).FirstOrDefault().Credit,
                        controlId = _context.Subjects.Where(c => c.Id == subjectId).FirstOrDefault().ControlId
                    });
                }
            }
            else
            {
                ViewData["ErrorMessage"] = "Такий результат до цього предмету вже доданий!";
            }
            ViewBag.SubjectId = subjectId;
            ViewBag.SubjectName = _context.Subjects.Where(c => c.Id == subjectId).FirstOrDefault().Name;
            ViewBag.SubjectEprogramId = _context.Subjects.Where(c => c.Id == subjectId).FirstOrDefault().EprogramId;
            ViewBag.SubjectCredit = _context.Subjects.Where(c => c.Id == subjectId).FirstOrDefault().Credit;
            ViewBag.SubjectConrolId = _context.Subjects.Where(c => c.Id == subjectId).FirstOrDefault().ControlId;

            int educationProgramId = _context.Subjects.Where(c => c.Id == subjectId).FirstOrDefault().EprogramId;
            int specialityId = _context.EducationPrograms.Where(c => c.Id == educationProgramId).FirstOrDefault().SpecialityId;

            ViewData["LearningOutcomeId"] = new SelectList(_context.LearningOutcomes.Where(b => b.SpecialityId == specialityId), "Id", "Loname");
            return View(epSubjectLoutcome);
        }

        bool IsUniqueEdit(int id, int subjectId, int learningOutcomeId)
        {
            var epSubjectLoutcomes = _context.EpSubjectLoutcomes.Where(b => b.SubjectId == subjectId && b.LearningOutcomeId == learningOutcomeId && b.Id != id).ToList();
            if (epSubjectLoutcomes.Count == 0) return true;
            return false;
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

            var subjectId = epSubjectLoutcome.SubjectId;
            ViewBag.SubjectId = subjectId;
            ViewBag.SubjectName = _context.Subjects.Where(c => c.Id == subjectId).FirstOrDefault().Name;
            ViewBag.SubjectEprogramId = _context.Subjects.Where(c => c.Id == subjectId).FirstOrDefault().EprogramId;
            ViewBag.SubjectCredit = _context.Subjects.Where(c => c.Id == subjectId).FirstOrDefault().Credit;
            ViewBag.SubjectConrolId = _context.Subjects.Where(c => c.Id == subjectId).FirstOrDefault().ControlId;
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
            //return RedirectToAction(nameof(Index));
            var subjectId = epSubjectLoutcome.SubjectId;
            return RedirectToAction("Index", "EpSubjectLoutcomes", new
            {
                id = subjectId,
                name = _context.Subjects.Where(c => c.Id == subjectId).FirstOrDefault().Name,
                eprogramId = _context.Subjects.Where(c => c.Id == subjectId).FirstOrDefault().EprogramId,
                credit = _context.Subjects.Where(c => c.Id == subjectId).FirstOrDefault().Credit,
                controlId = _context.Subjects.Where(c => c.Id == subjectId).FirstOrDefault().ControlId
            });
        }

        private bool EpSubjectLoutcomeExists(int id)
        {
          return (_context.EpSubjectLoutcomes?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
