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
    public class SpecialitiesController : Controller
    {
        private readonly EdProContext _context;

        public SpecialitiesController(EdProContext context)
        {
            _context = context;
        }

        // GET: Specialities
        public async Task<IActionResult> Index()
        {
              return _context.Specialities != null ? 
                          View(await _context.Specialities.ToListAsync()) :
                          Problem("Entity set 'EdProContext.Specialities'  is null.");
        }

        // GET: Specialities/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Specialities == null)
            {
                return NotFound();
            }

            var speciality = await _context.Specialities
                .FirstOrDefaultAsync(m => m.Id == id);
            if (speciality == null)
            {
                return NotFound();
            }

            return RedirectToAction("Index", "SpecialityCompetences", new
            {
                id = speciality.Id,
                name = speciality.Name
            });
        }

        // GET: Specialities/Create
        [Authorize(Roles = "admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Specialities/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Speciality speciality)
        {
            if (IsUnique(speciality.Name))
            {
                if (ModelState.IsValid)
                {
                    _context.Add(speciality);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            else
            {
                ViewData["ErrorMessage"] = "Така спеціальність вже додана!";
            }
            return View(speciality);
        }

        bool IsUnique(string name)
        {
            var specialities = _context.Specialities.Where(b => b.Name == name).ToList();
            if (specialities.Count == 0) return true;
            return false;
        }

        // GET: Specialities/Edit/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Specialities == null)
            {
                return NotFound();
            }

            var speciality = await _context.Specialities.FindAsync(id);
            if (speciality == null)
            {
                return NotFound();
            }
            return View(speciality);
        }

        // POST: Specialities/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Speciality speciality)
        {
            if (id != speciality.Id)
            {
                return NotFound();
            }

            if (IsUniqueEdit(speciality.Id, speciality.Name))
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        _context.Update(speciality);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!SpecialityExists(speciality.Id))
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
                ViewData["ErrorMessage"] = "Така спеціальність вже додана!";
            }
            return View(speciality);
        }

        bool IsUniqueEdit(int id, string name)
        {
            var specialities = _context.Specialities.Where(b => b.Name == name && b.Id != id).ToList();
            if (specialities.Count == 0) return true;
            return false;
        }

        // GET: Specialities/Delete/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Specialities == null)
            {
                return NotFound();
            }

            var speciality = await _context.Specialities
                .FirstOrDefaultAsync(m => m.Id == id);
            if (speciality == null)
            {
                return NotFound();
            }

            return View(speciality);
        }

        // POST: Specialities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Specialities == null)
            {
                return Problem("Entity set 'EdProContext.Specialities'  is null.");
            }
            var speciality = await _context.Specialities.FindAsync(id);
            if (speciality != null)
            {
                _context.Specialities.Remove(speciality);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SpecialityExists(int id)
        {
          return (_context.Specialities?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
