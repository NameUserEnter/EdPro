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
    public class SubjectsController : Controller
    {
        private readonly EdProContext _context;

        public SubjectsController(EdProContext context)
        {
            _context = context;
        }

        // GET: Subjects
        public async Task<IActionResult> Index(int? id, string? name, int? specialityId, string? edbo, int? edPrTypeId, int? facultyId, DateTime? implementationDate)
        {
            if (id == 0) return RedirectToAction("EducationPrograms", "Index");
            ViewBag.EducationProgramId = id;
            ViewBag.EducationProgramName = name;
            ViewBag.EducationProgramSpecialityId = specialityId;
            ViewBag.EducationProgramEdbo = edbo;
            ViewBag.EducationProgramEdPrTypeId = edPrTypeId;
            ViewBag.EducationProgramFacultyId = facultyId;
            ViewBag.EducationProgramImplementationDate = implementationDate;
            var subjectsByEducationProgram = _context.Subjects.Where(f => f.EprogramId == id).Include(f => f.Eprogram).Include(b => b.Control).Include(b => b.Eprogram);
            return View(await subjectsByEducationProgram.ToListAsync());
        }

        // GET: Subjects/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Subjects == null)
            {
                return NotFound();
            }

            var subject = await _context.Subjects
                .Include(s => s.Control)
                .Include(s => s.Eprogram)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (subject == null)
            {
                return NotFound();
            }

            return View(subject);
        }

        // GET: Subjects/Create
        [Authorize(Roles = "admin")]
        public IActionResult Create(int educationProgramId)
        {
            ViewBag.EducationProgramId = educationProgramId;
            ViewBag.EducationProgramName = _context.EducationPrograms.Where(c => c.Id == educationProgramId).FirstOrDefault().Name;
            ViewBag.EducationProgramSpecialityId = _context.EducationPrograms.Where(c => c.Id == educationProgramId).FirstOrDefault().SpecialityId;
            ViewBag.EducationProgramEdbo = _context.EducationPrograms.Where(c => c.Id == educationProgramId).FirstOrDefault().Edbo;
            ViewBag.EducationProgramEdPrTypeId = _context.EducationPrograms.Where(c => c.Id == educationProgramId).FirstOrDefault().EdPrTypeId;
            ViewBag.EducationProgramFacultyId = _context.EducationPrograms.Where(c => c.Id == educationProgramId).FirstOrDefault().FacultyId;
            ViewBag.EducationProgramImplementationDate = _context.EducationPrograms.Where(c => c.Id == educationProgramId).FirstOrDefault().ImplementationDate;
            ViewData["ControlId"] = new SelectList(_context.ControlTypes, "Id", "ControlTypeName");
            return View();
        }

        // POST: Subjects/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int educationProgramId, [Bind("Id,Name,EprogramId,Credit,ControlId")] Subject subject)
        {
            subject.EprogramId = educationProgramId;
            if (IsUnique(subject.Name, subject.EprogramId))
            {
                if (ModelState.IsValid)
                {
                    _context.Add(subject);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index", "Subjects", new
                    {
                        id = educationProgramId,
                        name = _context.EducationPrograms.Where(c => c.Id == educationProgramId).FirstOrDefault().Name,
                        specialityId = _context.EducationPrograms.Where(c => c.Id == educationProgramId).FirstOrDefault().SpecialityId,
                        edbo = _context.EducationPrograms.Where(c => c.Id == educationProgramId).FirstOrDefault().Edbo,
                        edPrTypeId = _context.EducationPrograms.Where(c => c.Id == educationProgramId).FirstOrDefault().EdPrTypeId,
                        facultyId = _context.EducationPrograms.Where(c => c.Id == educationProgramId).FirstOrDefault().FacultyId,
                        implementationDate = _context.EducationPrograms.Where(c => c.Id == educationProgramId).FirstOrDefault().ImplementationDate
                    });
                }
            }
            else
            {
                ViewData["ErrorMessage"] = "Такий предмет в цій навчальній програмі вже є!";
            }
            ViewData["ControlId"] = new SelectList(_context.ControlTypes, "Id", "ControlTypeName", subject.ControlId);
            ViewBag.EducationProgramId = educationProgramId;
            ViewBag.EducationProgramName = _context.EducationPrograms.Where(c => c.Id == educationProgramId).FirstOrDefault().Name;
            ViewBag.EducationProgramSpecialityId = _context.EducationPrograms.Where(c => c.Id == educationProgramId).FirstOrDefault().SpecialityId;
            ViewBag.EducationProgramEdbo = _context.EducationPrograms.Where(c => c.Id == educationProgramId).FirstOrDefault().Edbo;
            ViewBag.EducationProgramEdPrTypeId = _context.EducationPrograms.Where(c => c.Id == educationProgramId).FirstOrDefault().EdPrTypeId;
            ViewBag.EducationProgramFacultyId = _context.EducationPrograms.Where(c => c.Id == educationProgramId).FirstOrDefault().FacultyId;
            ViewBag.EducationProgramImplementationDate = _context.EducationPrograms.Where(c => c.Id == educationProgramId).FirstOrDefault().ImplementationDate;
            return View(subject);
        }

        bool IsUnique(string name, int eProgramId)
        {
            var subjects = _context.Subjects.Where(b => b.Name == name && b.EprogramId == eProgramId).ToList();
            if (subjects.Count == 0) return true;
            return false;
        }

        // GET: Subjects/Edit/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Subjects == null)
            {
                return NotFound();
            }

            var subject = await _context.Subjects.FindAsync(id);
            if (subject == null)
            {
                return NotFound();
            }
            ViewData["ControlId"] = new SelectList(_context.ControlTypes, "Id", "ControlTypeName", subject.ControlId);
            ViewData["EprogramId"] = new SelectList(_context.EducationPrograms, "Id", "Edbo", subject.EprogramId);
            return View(subject);
        }

        // POST: Subjects/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,EprogramId,Credit,ControlId")] Subject subject)
        {
            if (id != subject.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(subject);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SubjectExists(subject.Id))
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
            ViewData["ControlId"] = new SelectList(_context.ControlTypes, "Id", "ControlTypeName", subject.ControlId);
            ViewData["EprogramId"] = new SelectList(_context.EducationPrograms, "Id", "Edbo", subject.EprogramId);
            return View(subject);
        }

        // GET: Subjects/Delete/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Subjects == null)
            {
                return NotFound();
            }

            var subject = await _context.Subjects
                .Include(s => s.Control)
                .Include(s => s.Eprogram)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (subject == null)
            {
                return NotFound();
            }

            return View(subject);
        }

        // POST: Subjects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Subjects == null)
            {
                return Problem("Entity set 'EdProContext.Subjects'  is null.");
            }
            var subject = await _context.Subjects.FindAsync(id);
            if (subject != null)
            {
                _context.Subjects.Remove(subject);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SubjectExists(int id)
        {
          return (_context.Subjects?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
