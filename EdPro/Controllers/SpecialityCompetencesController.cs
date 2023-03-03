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

            //return RedirectToAction("Index", "EpSubjectCompetences", new
            //{
            //    id = specialityCompetence.Id,
            //    specialityId = specialityCompetence.SpecialityId,
            //    competenceId = specialityCompetence.CompetenceId
            //});
            return View(specialityCompetence);
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
            if (IsUnique(specialityCompetence.SpecialityId, specialityCompetence.CompetenceId))
            {
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
            }
            else
            {
                ViewData["ErrorMessage"] = "Така компетентність до цієї спеціальності вже додана!";
            }
            ViewBag.SpecialityId = specialityId;
            ViewBag.SpecialityName = _context.Specialities.Where(c => c.Id == specialityId).FirstOrDefault().Name;
            ViewData["CompetenceId"] = new SelectList(_context.Competences, "Id", "Competence1", specialityCompetence.CompetenceId);
            return View(specialityCompetence);
        }

        bool IsUnique(int specialityId, int competenceId)
        {
            var specialityCompetences = _context.SpecialityCompetences.Where(b => b.SpecialityId == specialityId && b.CompetenceId == competenceId).ToList();
            if (specialityCompetences.Count == 0) return true;
            return false;
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
            //ViewData["SpecialityId"] = new SelectList(_context.Specialities, "Id", "Name", specialityCompetence.SpecialityId);
            ViewBag.SpecialityId = specialityCompetence.SpecialityId;
            ViewBag.SpecialityName = _context.Specialities.Where(c => c.Id == specialityCompetence.SpecialityId).FirstOrDefault().Name;
            return View(specialityCompetence);
        }

        // POST: SpecialityCompetences/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, int specialityId, [Bind("Id,SpecialityId,CompetenceId")] SpecialityCompetence specialityCompetence)
        {
            if (id != specialityCompetence.Id)
            {
                return NotFound();
            }

            if (IsUniqueEdit(specialityCompetence.Id, specialityId, specialityCompetence.CompetenceId))
            {
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
                    //return RedirectToAction(nameof(Index));
                    return RedirectToAction("Index", "SpecialityCompetences", new
                    {
                        id = specialityId,
                        name = _context.Specialities.Where(c => c.Id == specialityId).FirstOrDefault().Name
                    });
                }
            }
            else
            {
                ViewData["ErrorMessage"] = "Така компетентність до цієї спеціальності вже додана!";
            }
            ViewData["CompetenceId"] = new SelectList(_context.Competences, "Id", "Competence1", specialityCompetence.CompetenceId);
            //ViewData["SpecialityId"] = new SelectList(_context.Specialities, "Id", "Name", specialityCompetence.SpecialityId);
            //ViewData["SpecialityId"] = new SelectList(_context.Specialities, "Id", "Name", specialityCompetence.SpecialityId);
            ViewBag.SpecialityId = specialityId;
            ViewBag.SpecialityName = _context.Specialities.Where(c => c.Id == specialityId).FirstOrDefault().Name;
            return View(specialityCompetence);
        }

        bool IsUniqueEdit(int id, int specialityId, int competenceId)
        {
            var specialityCompetences = _context.SpecialityCompetences.Where(b => b.SpecialityId == specialityId && b.CompetenceId == competenceId && b.Id != id).ToList();
            if (specialityCompetences.Count == 0) return true;
            return false;
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

            ViewBag.SpecialityId = specialityCompetence.SpecialityId;
            ViewBag.SpecialityName = _context.Specialities.Where(c => c.Id == specialityCompetence.SpecialityId).FirstOrDefault().Name;
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
            var epSubjectCompetences = _context.EpSubjectCompetences.Where(f => f.SpecialityCompetenceId == id).ToList();
            if(epSubjectCompetences.Any())
            {
                foreach(var epSubjectCompetence in epSubjectCompetences)
                {
                    _context.EpSubjectCompetences.Remove(epSubjectCompetence);
                }
            }
            if (specialityCompetence != null)
            {
                _context.SpecialityCompetences.Remove(specialityCompetence);
            }
            
            await _context.SaveChangesAsync();
            //return RedirectToAction(nameof(Index));
            var specialityId = specialityCompetence.SpecialityId;
            return RedirectToAction("Index", "SpecialityCompetences", new
            {
                id = specialityId,
                name = _context.Specialities.Where(c => c.Id == specialityId).FirstOrDefault().Name
            });
        }

        private bool SpecialityCompetenceExists(int id)
        {
          return (_context.SpecialityCompetences?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
