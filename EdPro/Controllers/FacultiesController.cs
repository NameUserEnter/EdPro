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
    public class FacultiesController : Controller
    {
        private readonly EdProContext _context;

        public FacultiesController(EdProContext context)
        {
            _context = context;
        }

        // GET: Faculties
        public async Task<IActionResult> Index(int? id, string? name, string? edbo)
        {
            if(id == 0) return RedirectToAction("Universities", "Index");
            ViewBag.UniversityId = id;
            ViewBag.UniversityName = name;
            ViewBag.UniversityEdbo = edbo;
            var facultiesByUniversity = _context.Faculties.Where(f => f.UniversityId == id).Include(f=>f.University);
            return View(await facultiesByUniversity.ToListAsync());
        }

        // GET: Faculties/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Faculties == null)
            {
                return NotFound();
            }

            var faculty = await _context.Faculties
                .Include(f => f.University)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (faculty == null)
            {
                return NotFound();
            }

            return View(faculty);
        }

        // GET: Faculties/Create
        public IActionResult Create(int universityId)
        {
            ViewBag.UniversityId = universityId;
            ViewBag.UniversityName = _context.Universities.Where(c => c.Id == universityId).FirstOrDefault().Name;
            ViewBag.UniversityEdbo = _context.Universities.Where(c => c.Id == universityId).FirstOrDefault().Edbo;
            return View();
        }

        // POST: Faculties/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int universityId, [Bind("Id,Name,UniversityId")] Faculty faculty)
        {
            faculty.UniversityId = universityId;
            if (IsUnique(faculty.Name, faculty.UniversityId))
            {
                if (ModelState.IsValid)
                {
                    _context.Add(faculty);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index", "Faculties", new
                    {
                        id = universityId,
                        name = _context.Universities.Where(c => c.Id == universityId).FirstOrDefault().Name,
                        edbo = _context.Universities.Where(c => c.Id == universityId).FirstOrDefault().Edbo
                    });
                }
            }
            else
            {
                ViewData["ErrorMessage"] = "Такий факультет в цьому університеті вже існує!";
            }

            //return RedirectToAction("Index", "Faculties", new
            //{
            //    id = universityId,
            //    name = _context.Universities.Where(c => c.Id == universityId).FirstOrDefault().Name,
            //    edbo = _context.Universities.Where(c => c.Id == universityId).FirstOrDefault().Edbo
            //});
            ViewBag.UniversityId = universityId;
            ViewBag.UniversityName = _context.Universities.Where(c => c.Id == universityId).FirstOrDefault().Name;
            ViewBag.UniversityEdbo = _context.Universities.Where(c => c.Id == universityId).FirstOrDefault().Edbo;
            return View(faculty);
        }

        bool IsUnique(string name, int universityId)
        {
            var faculties = _context.Faculties.Where(b => b.Name == name && b.UniversityId == universityId).ToList();
            if (faculties.Count == 0) return true;
            return false;
        }

        // GET: Faculties/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Faculties == null)
            {
                return NotFound();
            }

            var faculty = await _context.Faculties.FindAsync(id);
            if (faculty == null)
            {
                return NotFound();
            }
            ViewData["UniversityId"] = new SelectList(_context.Universities, "Id", "Edbo", faculty.UniversityId);
            return View(faculty);
        }

        // POST: Faculties/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,UniversityId")] Faculty faculty)
        {
            if (id != faculty.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(faculty);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FacultyExists(faculty.Id))
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
            ViewData["UniversityId"] = new SelectList(_context.Universities, "Id", "Edbo", faculty.UniversityId);
            return View(faculty);
        }

        // GET: Faculties/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Faculties == null)
            {
                return NotFound();
            }

            var faculty = await _context.Faculties
                .Include(f => f.University)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (faculty == null)
            {
                return NotFound();
            }

            return View(faculty);
        }

        // POST: Faculties/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Faculties == null)
            {
                return Problem("Entity set 'EdProContext.Faculties'  is null.");
            }
            var faculty = await _context.Faculties.FindAsync(id);
            if (faculty != null)
            {
                _context.Faculties.Remove(faculty);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FacultyExists(int id)
        {
          return (_context.Faculties?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
