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
    public class CompetencesController : Controller
    {
        private readonly EdProContext _context;

        public CompetencesController(EdProContext context)
        {
            _context = context;
        }

        // GET: Competences
        public async Task<IActionResult> Index()
        {
            var edProContext = _context.Competences.Include(c => c.CompetenceType);
            return View(await edProContext.ToListAsync());
        }

        // GET: Competences/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Competences == null)
            {
                return NotFound();
            }

            var competence = await _context.Competences
                .Include(c => c.CompetenceType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (competence == null)
            {
                return NotFound();
            }

            return View(competence);
        }

        // GET: Competences/Create
        [Authorize(Roles = "admin")]
        public IActionResult Create()
        {
            ViewData["CompetenceTypeId"] = new SelectList(_context.CompetencesTypes, "Id", "CompType");
            return View();
        }

        // POST: Competences/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Competence1,CompetenceTypeId")] Competence competence)
        {
            if (ModelState.IsValid)
            {
                _context.Add(competence);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CompetenceTypeId"] = new SelectList(_context.CompetencesTypes, "Id", "CompType", competence.CompetenceTypeId);
            return View(competence);
        }

        // GET: Competences/Edit/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Competences == null)
            {
                return NotFound();
            }

            var competence = await _context.Competences.FindAsync(id);
            if (competence == null)
            {
                return NotFound();
            }
            ViewData["CompetenceTypeId"] = new SelectList(_context.CompetencesTypes, "Id", "CompType", competence.CompetenceTypeId);
            return View(competence);
        }

        // POST: Competences/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Competence1,CompetenceTypeId")] Competence competence)
        {
            if (id != competence.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(competence);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CompetenceExists(competence.Id))
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
            ViewData["CompetenceTypeId"] = new SelectList(_context.CompetencesTypes, "Id", "CompType", competence.CompetenceTypeId);
            return View(competence);
        }

        // GET: Competences/Delete/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Competences == null)
            {
                return NotFound();
            }

            var competence = await _context.Competences
                .Include(c => c.CompetenceType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (competence == null)
            {
                return NotFound();
            }

            return View(competence);
        }

        // POST: Competences/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Competences == null)
            {
                return Problem("Entity set 'EdProContext.Competences'  is null.");
            }
            var competence = await _context.Competences.FindAsync(id);
            if (competence != null)
            {
                _context.Competences.Remove(competence);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CompetenceExists(int id)
        {
          return (_context.Competences?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
