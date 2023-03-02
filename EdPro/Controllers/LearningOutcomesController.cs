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
    public class LearningOutcomesController : Controller
    {
        private readonly EdProContext _context;

        public LearningOutcomesController(EdProContext context)
        {
            _context = context;
        }

        // GET: LearningOutcomes
        public async Task<IActionResult> Index()
        {
            var edProContext = _context.LearningOutcomes.Include(l => l.Speciality);
            return View(await edProContext.ToListAsync());
        }

        // GET: LearningOutcomes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.LearningOutcomes == null)
            {
                return NotFound();
            }

            var learningOutcome = await _context.LearningOutcomes
                .Include(l => l.Speciality)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (learningOutcome == null)
            {
                return NotFound();
            }

            return View(learningOutcome);

            //return RedirectToAction("Index", "EpSubjectLoutcomes", new
            //{
            //    id = learningOutcome.Id,
            //    learningOutcome1 = learningOutcome.LearningOutcome1,
            //    loname = learningOutcome.Loname,
            //    specialtyId = learningOutcome.SpecialityId
            //});
        }

        // GET: LearningOutcomes/Create
        [Authorize(Roles = "admin")]
        public IActionResult Create()
        {
            ViewData["SpecialityId"] = new SelectList(_context.Specialities, "Id", "Name");
            return View();
        }

        // POST: LearningOutcomes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,LearningOutcome1,Loname,SpecialityId")] LearningOutcome learningOutcome)
        {
            if (IsUnique(learningOutcome.Loname, learningOutcome.SpecialityId))
            {
                if (ModelState.IsValid)
                {
                    _context.Add(learningOutcome);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            else
            {
                ViewData["ErrorMessage"] = "Такий навчальний результат в цій спеціальності є!";
            }
            ViewData["SpecialityId"] = new SelectList(_context.Specialities, "Id", "Name", learningOutcome.SpecialityId);
            return View(learningOutcome);
        }

        bool IsUnique(string loname, int specialityId)
        {
            var learningOutcomes = _context.LearningOutcomes.Where(b => b.SpecialityId == specialityId && b.Loname == loname).ToList();
            if (learningOutcomes.Count == 0) return true;
            return false;
        }

        // GET: LearningOutcomes/Edit/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.LearningOutcomes == null)
            {
                return NotFound();
            }

            var learningOutcome = await _context.LearningOutcomes.FindAsync(id);
            if (learningOutcome == null)
            {
                return NotFound();
            }
            ViewData["SpecialityId"] = new SelectList(_context.Specialities, "Id", "Name", learningOutcome.SpecialityId);
            return View(learningOutcome);
        }

        // POST: LearningOutcomes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,LearningOutcome1,Loname,SpecialityId")] LearningOutcome learningOutcome)
        {
            if (id != learningOutcome.Id)
            {
                return NotFound();
            }

            if (IsUniqueEdit(learningOutcome.Id, learningOutcome.Loname, learningOutcome.SpecialityId))
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        _context.Update(learningOutcome);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!LearningOutcomeExists(learningOutcome.Id))
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
            }
            else
            {
                ViewData["ErrorMessage"] = "Такий навчальний результат в цій спеціальності є!";
            }
            ViewData["SpecialityId"] = new SelectList(_context.Specialities, "Id", "Name", learningOutcome.SpecialityId);
            return View(learningOutcome);
        }

        bool IsUniqueEdit(int id, string loname, int specialityId)
        {
            var learningOutcomes = _context.LearningOutcomes.Where(b => b.SpecialityId == specialityId && b.Loname == loname && b.Id != id).ToList();
            if (learningOutcomes.Count == 0) return true;
            return false;
        }

        // GET: LearningOutcomes/Delete/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.LearningOutcomes == null)
            {
                return NotFound();
            }

            var learningOutcome = await _context.LearningOutcomes
                .Include(l => l.Speciality)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (learningOutcome == null)
            {
                return NotFound();
            }

            return View(learningOutcome);
        }

        // POST: LearningOutcomes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.LearningOutcomes == null)
            {
                return Problem("Entity set 'EdProContext.LearningOutcomes'  is null.");
            }
            var learningOutcome = await _context.LearningOutcomes.FindAsync(id);
            var epSubjectLoutcomes = _context.EpSubjectLoutcomes.Where(f => f.LearningOutcomeId == learningOutcome.Id);
            if(epSubjectLoutcomes.Any())
            {
                foreach(var epSubjectOutcome in epSubjectLoutcomes)
                {
                    _context.EpSubjectLoutcomes.Remove(epSubjectOutcome);
                }
            }
            if (learningOutcome != null)
            {
                _context.LearningOutcomes.Remove(learningOutcome);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LearningOutcomeExists(int id)
        {
          return (_context.LearningOutcomes?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
