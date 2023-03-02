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
    public class SpecialityCompetencesController : Controller
    {
        private readonly EdProContext _context;

        public SpecialityCompetencesController(EdProContext context)
        {
            _context = context;
        }

        // GET: SpecialityCompetences
        public async Task<IActionResult> Index(int? id, string? name)
        {
            if (id == 0) return RedirectToAction("Specialities", "Index");
            ViewBag.SpecialityId = id;
            ViewBag.SpecialityName = name;
            var specialityCompetencesBySpeciality = _context.SpecialityCompetences.Where(f => f.SpecialityId == id).Include(f => f.Competence).Include(f => f.Speciality);
            return View(await specialityCompetencesBySpeciality.ToListAsync());
        }

        // GET: SpecialityCompetences/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.SpecialityCompetences == null)
            {
                return NotFound();
            }

            var specialityCompetence = await _context.SpecialityCompetences
                .Include(s => s.Competence)
                .Include(s => s.Speciality)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (specialityCompetence == null)
            {
                return NotFound();
            }

            return RedirectToAction("Index", "EpSubjectCompetences", new
            {
                id = specialityCompetence.Id,
                specialityId = specialityCompetence.SpecialityId,
                competenceId = specialityCompetence.CompetenceId
            });
        }

        // GET: SpecialityCompetences/Create
        [Authorize(Roles = "admin")]
        public IActionResult Create(int specialityId)
        {
            ViewBag.SpecialityId = specialityId;
            ViewBag.SpecialityName = _context.Specialities.Where(c => c.Id == specialityId).FirstOrDefault().Name;
            ViewData["CompetenceId"] = new SelectList(_context.Competences, "Id", "Competence1");
            return View();
        }

        // POST: SpecialityCompetences/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int specialityId, [Bind("Id,SpecialityId,CompetenceId")] SpecialityCompetence specialityCompetence)
        {
            specialityCompetence.SpecialityId = specialityId;
            if (ModelState.IsValid)
            {
                _context.Add(specialityCompetence);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "SpecialityCompetences", new
                {
                    id = specialityId,
                    name = _context.Specialities.Where(c => c.Id == specialityId).FirstOrDefault().Name
                });
            }
            ViewBag.SpecialityId = specialityId;
            ViewBag.SpecialityName = _context.Specialities.Where(c => c.Id == specialityId).FirstOrDefault().Name;
            ViewData["CompetenceId"] = new SelectList(_context.Competences, "Id", "Competence1", specialityCompetence.CompetenceId);
            return View(specialityCompetence);
        }

        // GET: SpecialityCompetences/Edit/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.SpecialityCompetences == null)
            {
                return NotFound();
            }

            var specialityCompetence = await _context.SpecialityCompetences.FindAsync(id);
            if (specialityCompetence == null)
            {
                return NotFound();
            }
            ViewData["CompetenceId"] = new SelectList(_context.Competences, "Id", "Competence1", specialityCompetence.CompetenceId);
            ViewData["SpecialityId"] = new SelectList(_context.Specialities, "Id", "Name", specialityCompetence.SpecialityId);
            return View(specialityCompetence);
        }

        // POST: SpecialityCompetences/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,SpecialityId,CompetenceId")] SpecialityCompetence specialityCompetence)
        {
            if (id != specialityCompetence.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(specialityCompetence);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SpecialityCompetenceExists(specialityCompetence.Id))
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
            ViewData["CompetenceId"] = new SelectList(_context.Competences, "Id", "Competence1", specialityCompetence.CompetenceId);
            ViewData["SpecialityId"] = new SelectList(_context.Specialities, "Id", "Name", specialityCompetence.SpecialityId);
            return View(specialityCompetence);
        }

        // GET: SpecialityCompetences/Delete/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.SpecialityCompetences == null)
            {
                return NotFound();
            }

            var specialityCompetence = await _context.SpecialityCompetences
                .Include(s => s.Competence)
                .Include(s => s.Speciality)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (specialityCompetence == null)
            {
                return NotFound();
            }

            return View(specialityCompetence);
        }

        // POST: SpecialityCompetences/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.SpecialityCompetences == null)
            {
                return Problem("Entity set 'EdProContext.SpecialityCompetences'  is null.");
            }
            var specialityCompetence = await _context.SpecialityCompetences.FindAsync(id);
            if (specialityCompetence != null)
            {
                _context.SpecialityCompetences.Remove(specialityCompetence);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SpecialityCompetenceExists(int id)
        {
          return (_context.SpecialityCompetences?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
