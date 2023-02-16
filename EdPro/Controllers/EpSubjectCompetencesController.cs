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
    public class EpSubjectCompetencesController : Controller
    {
        private readonly EdProContext _context;

        public EpSubjectCompetencesController(EdProContext context)
        {
            _context = context;
        }

        // GET: EpSubjectCompetences
        public async Task<IActionResult> Index()
        {
            var edProContext = _context.EpSubjectCompetences.Include(e => e.SpecialityCompetence).Include(e => e.Subject);
            return View(await edProContext.ToListAsync());
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
        public IActionResult Create()
        {
            ViewData["SpecialityCompetenceId"] = new SelectList(_context.SpecialityCompetences, "Id", "Id");
            ViewData["SubjectId"] = new SelectList(_context.Subjects, "Id", "Name");
            return View();
        }

        // POST: EpSubjectCompetences/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,SubjectId,SpecialityCompetenceId")] EpSubjectCompetence epSubjectCompetence)
        {
            if (ModelState.IsValid)
            {
                _context.Add(epSubjectCompetence);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["SpecialityCompetenceId"] = new SelectList(_context.SpecialityCompetences, "Id", "Id", epSubjectCompetence.SpecialityCompetenceId);
            ViewData["SubjectId"] = new SelectList(_context.Subjects, "Id", "Name", epSubjectCompetence.SubjectId);
            return View(epSubjectCompetence);
        }

        // GET: EpSubjectCompetences/Edit/5
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
