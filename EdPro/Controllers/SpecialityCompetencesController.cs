using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EdPro.Models;

namespace EdPro.Controllers
{
    public class SpecialityCompetencesController : Controller
    {
        private readonly EdProContext _context;

        public SpecialityCompetencesController(EdProContext context)
        {
            _context = context;
        }

        // GET: SpecialityCompetences
        public async Task<IActionResult> Index()
        {
            var edProContext = _context.SpecialityCompetences.Include(s => s.Competence).Include(s => s.Speciality);
            return View(await edProContext.ToListAsync());
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

            return View(specialityCompetence);
        }

        // GET: SpecialityCompetences/Create
        public IActionResult Create()
        {
            ViewData["CompetenceId"] = new SelectList(_context.Competences, "Id", "Competence1");
            ViewData["SpecialityId"] = new SelectList(_context.Specialities, "Id", "Name");
            return View();
        }

        // POST: SpecialityCompetences/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,SpecialityId,CompetenceId")] SpecialityCompetence specialityCompetence)
        {
            if (ModelState.IsValid)
            {
                _context.Add(specialityCompetence);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CompetenceId"] = new SelectList(_context.Competences, "Id", "Competence1", specialityCompetence.CompetenceId);
            ViewData["SpecialityId"] = new SelectList(_context.Specialities, "Id", "Name", specialityCompetence.SpecialityId);
            return View(specialityCompetence);
        }

        // GET: SpecialityCompetences/Edit/5
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
