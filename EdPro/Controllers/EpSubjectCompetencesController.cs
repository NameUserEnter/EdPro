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
    public class EpSubjectCompetencesController : Controller
    {
        private readonly EdProContext _context;

        public EpSubjectCompetencesController(EdProContext context)
        {
            _context = context;
        }

        // GET: EpSubjectCompetences
        public async Task<IActionResult> Index(int? id, int? specialityId, int? competenceId)
        {
            if (id == 0) return RedirectToAction("SpecialityCompetences", "Index");
            ViewBag.SpecialityCompetenceId = id;
            ViewBag.SpecialityCompetenceSpecialityId = specialityId;
            ViewBag.SpecialityCompetenceCompetenceId = competenceId;
            var subjectCompetencesBySpecialityCompetence = _context.EpSubjectCompetences.Where(f => f.SpecialityCompetenceId == id).Include(f => f.Subject);
            return View(await subjectCompetencesBySpecialityCompetence.ToListAsync());
        }

        // GET: EpSubjectCompetences/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.EpSubjectCompetences == null)
            {
                return NotFound();
            }

            var epSubjectCompetence = await _context.EpSubjectCompetences
                .Include(e => e.SpecialityCompetence)
                .Include(e => e.Subject)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (epSubjectCompetence == null)
            {
                return NotFound();
            }

            return View(epSubjectCompetence);
        }

        // GET: EpSubjectCompetences/Create
        [Authorize(Roles = "admin")]
        public IActionResult Create(int specialityCompetenceId)
        {
            ViewBag.SpecialityCompetenceId = specialityCompetenceId;
            ViewBag.SpecialityCompetenceSpecialityId = _context.SpecialityCompetences.Where(c => c.Id == specialityCompetenceId).FirstOrDefault().SpecialityId;
            ViewBag.SpecialityCompetenceCompetenceId = _context.SpecialityCompetences.Where(c => c.Id == specialityCompetenceId).FirstOrDefault().CompetenceId;
            int specialityId = _context.SpecialityCompetences.Where(c => c.Id == specialityCompetenceId).FirstOrDefault().SpecialityId;
            List<int> educationProgramsId = _context.EducationPrograms.Where(c => c.SpecialityId == specialityId).Select(c => c.Id).ToList();

            ViewData["SubjectId"] = new SelectList(_context.Subjects.Where(b => educationProgramsId.Contains(b.EprogramId)), "Id", "Name");
            return View();
        }

        // POST: EpSubjectCompetences/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int specialityCompetenceId, [Bind("Id,SubjectId,SpecialityCompetenceId")] EpSubjectCompetence epSubjectCompetence)
        {
            epSubjectCompetence.SpecialityCompetenceId = specialityCompetenceId;
            if (ModelState.IsValid)
            {
                _context.Add(epSubjectCompetence);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "EpSubjectCompetences", new
                {
                    id = specialityCompetenceId,
                    specialityId = _context.SpecialityCompetences.Where(c => c.Id == specialityCompetenceId).FirstOrDefault().SpecialityId,
                    competenceId = _context.SpecialityCompetences.Where(c => c.Id == specialityCompetenceId).FirstOrDefault().CompetenceId
                });
            }
            ViewBag.SpecialityCompetenceId = specialityCompetenceId;
            ViewBag.SpecialityCompetenceSpecialityId = _context.SpecialityCompetences.Where(c => c.Id == specialityCompetenceId).FirstOrDefault().SpecialityId;
            ViewBag.SpecialityCompetenceCompetenceId = _context.SpecialityCompetences.Where(c => c.Id == specialityCompetenceId).FirstOrDefault().CompetenceId;
            int specialityId = _context.SpecialityCompetences.Where(c => c.Id == specialityCompetenceId).FirstOrDefault().SpecialityId;
            List<int> educationProgramsId = _context.EducationPrograms.Where(c => c.SpecialityId == specialityId).Select(c => c.Id).ToList();

            ViewData["SubjectId"] = new SelectList(_context.Subjects.Where(b => educationProgramsId.Contains(b.EprogramId)), "Id", "Name");
            return View(epSubjectCompetence);
        }

        // GET: EpSubjectCompetences/Edit/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.EpSubjectCompetences == null)
            {
                return NotFound();
            }

            var epSubjectCompetence = await _context.EpSubjectCompetences.FindAsync(id);
            if (epSubjectCompetence == null)
            {
                return NotFound();
            }
            ViewData["SpecialityCompetenceId"] = new SelectList(_context.SpecialityCompetences, "Id", "Id", epSubjectCompetence.SpecialityCompetenceId);
            ViewData["SubjectId"] = new SelectList(_context.Subjects, "Id", "Name", epSubjectCompetence.SubjectId);
            return View(epSubjectCompetence);
        }

        // POST: EpSubjectCompetences/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,SubjectId,SpecialityCompetenceId")] EpSubjectCompetence epSubjectCompetence)
        {
            if (id != epSubjectCompetence.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(epSubjectCompetence);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EpSubjectCompetenceExists(epSubjectCompetence.Id))
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
            ViewData["SpecialityCompetenceId"] = new SelectList(_context.SpecialityCompetences, "Id", "Id", epSubjectCompetence.SpecialityCompetenceId);
            ViewData["SubjectId"] = new SelectList(_context.Subjects, "Id", "Name", epSubjectCompetence.SubjectId);
            return View(epSubjectCompetence);
        }

        // GET: EpSubjectCompetences/Delete/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.EpSubjectCompetences == null)
            {
                return NotFound();
            }

            var epSubjectCompetence = await _context.EpSubjectCompetences
                .Include(e => e.SpecialityCompetence)
                .Include(e => e.Subject)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (epSubjectCompetence == null)
            {
                return NotFound();
            }

            return View(epSubjectCompetence);
        }

        // POST: EpSubjectCompetences/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.EpSubjectCompetences == null)
            {
                return Problem("Entity set 'EdProContext.EpSubjectCompetences'  is null.");
            }
            var epSubjectCompetence = await _context.EpSubjectCompetences.FindAsync(id);
            if (epSubjectCompetence != null)
            {
                _context.EpSubjectCompetences.Remove(epSubjectCompetence);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EpSubjectCompetenceExists(int id)
        {
          return (_context.EpSubjectCompetences?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
