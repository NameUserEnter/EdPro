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
    //[Authorize(Roles = "admin, user, worker")]
    public class UniversitiesController : Controller
    {
        private readonly EdProContext _context;

        public UniversitiesController(EdProContext context)
        {
            _context = context;
        }

        // GET: Universities
        public async Task<IActionResult> Index()
        {
              return _context.Universities != null ? 
                          View(await _context.Universities.ToListAsync()) :
                          Problem("Entity set 'EdProContext.Universities'  is null.");
        }

        // GET: Universities/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Universities == null)
            {
                return NotFound();
            }

            var university = await _context.Universities
                .FirstOrDefaultAsync(m => m.Id == id);
            if (university == null)
            {
                return NotFound();
            }

            return RedirectToAction("Index", "Faculties", new { id = university.Id, name = university.Name, edbo = university.Edbo });
        }

        // GET: Universities/Create
        [Authorize(Roles = "admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Universities/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Edbo")] University university)
        {
            if (IsUnique(university.Name))
            {
                if (ModelState.IsValid)
                {
                    _context.Add(university);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            else
            {
                ViewData["ErrorMessage"] = "Такий університет вже доданий!";
            }
            return View(university);
        }

        bool IsUnique(string name)
        {
            var universities = _context.Universities.Where(b => b.Name == name).ToList();
            if (universities.Count == 0) return true;
            return false;
        }

        // GET: Universities/Edit/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Universities == null)
            {
                return NotFound();
            }

            var university = await _context.Universities.FindAsync(id);
            if (university == null)
            {
                return NotFound();
            }
            return View(university);
        }

        // POST: Universities/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Edbo")] University university)
        {
            if (id != university.Id)
            {
                return NotFound();
            }

            if (IsUniqueEdit(university.Id, university.Name))
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        _context.Update(university);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!UniversityExists(university.Id))
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
                ViewData["ErrorMessage"] = "Такий університет вже доданий!";
            }
            return View(university);
        }

        bool IsUniqueEdit(int id, string name)
        {
            var universities = _context.Universities.Where(b => b.Name == name && b.Id != id).ToList();
            if (universities.Count == 0) return true;
            return false;
        }

        // GET: Universities/Delete/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Universities == null)
            {
                return NotFound();
            }

            var university = await _context.Universities
                .FirstOrDefaultAsync(m => m.Id == id);
            if (university == null)
            {
                return NotFound();
            }

            return View(university);
        }

        // POST: Universities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Universities == null)
            {
                return Problem("Entity set 'EdProContext.Universities'  is null.");
            }
            var university = await _context.Universities.FindAsync(id);
            //var faculties = _context.Faculties.Where(b => b.UniversityId == id).ToList();
            //if (faculties.Any())
            //{
            //    foreach (var faculty in faculties)
            //    {
            //        var educationPrograms = _context.EducationPrograms.Where(f => f.FacultyId == faculty.Id).ToList();
            //        if (educationPrograms.Any())
            //        {
            //            foreach(var educationProgram in educationPrograms)
            //            {
            //                var subjects = _context.Subjects.Where(f => f.EprogramId == educationProgram.Id).ToList();
            //                if (subjects.Any())
            //                {
            //                    foreach (var subject in subjects) 
            //                    {
            //                        var epSubjectLoucomes = _context.EpSubjectLoutcomes.Where(f => f.SubjectId == subject.Id).ToList();
            //                        if (epSubjectLoucomes.Any())
            //                        {
            //                            foreach (var epSubjectLoucome in epSubjectLoucomes)
            //                            {
            //                                _context.EpSubjectLoutcomes.Remove(epSubjectLoucome);
            //                            }
            //                        }
            //                        var epSubjectCompetences = _context.EpSubjectCompetences.Where(f => f.SubjectId == subject.Id).ToList();
            //                        if (epSubjectCompetences.Any())
            //                        {
            //                            foreach (var epSubjectCompetence in epSubjectCompetences)
            //                            {
            //                                _context.EpSubjectCompetences.Remove(epSubjectCompetence);
            //                            }
            //                        }
            //                        _context.Subjects.Remove(subject);
            //                    }
            //                }
            //                _context.EducationPrograms.Remove(educationProgram);
            //            }
            //        }
            //        _context.Faculties.Remove(faculty);
            //    }
            //}
            if (university != null)
            {
                _context.Universities.Remove(university);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UniversityExists(int id)
        {
          return (_context.Universities?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
