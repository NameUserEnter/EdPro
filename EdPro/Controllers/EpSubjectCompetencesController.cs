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
        public async Task<IActionResult> Index(int? id, string? name, int? eprogramId, int? credit, int? controlId)
        {
            if (id == 0) return RedirectToAction("SubjectsCompetences", "Index");
            ViewBag.SubjectId = id;
            ViewBag.SubjectName = name;
            ViewBag.SubjectEprogramId = eprogramId;
            ViewBag.SubjectCredit = credit;
            ViewBag.SubjectControlId = controlId;
            var subjectCompetencesBySpecialitySubject = _context.EpSubjectCompetences.Where(f => f.SubjectId == id).Include(f => f.Subject).Include(f => f.SpecialityCompetence).Include(f => f.SpecialityCompetence.Speciality).Include(f => f.SpecialityCompetence.Competence);

            return View(await subjectCompetencesBySpecialitySubject.ToListAsync());
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
        public IActionResult Create(int subjectId)
        {
            ViewBag.SubjectId = subjectId;
            ViewBag.SubjectName = _context.Subjects.Where(c => c.Id == subjectId).FirstOrDefault().Name;
            ViewBag.SubjectEprogramId = _context.Subjects.Where(c => c.Id == subjectId).FirstOrDefault().EprogramId;
            ViewBag.SubjectCredit = _context.Subjects.Where(c => c.Id == subjectId).FirstOrDefault().Credit;
            ViewBag.SubjectConrolId = _context.Subjects.Where(c => c.Id == subjectId).FirstOrDefault().ControlId;
            
            int educationProgramId = _context.Subjects.Where(c => c.Id == subjectId).FirstOrDefault().EprogramId;
            int specialityId = _context.EducationPrograms.Where(c => c.Id == educationProgramId).FirstOrDefault().SpecialityId;

            ViewData["SpecialityCompetenceId"] = new SelectList(_context.SpecialityCompetences.Where(b => b.SpecialityId == specialityId), "Id", "Id");

            var specialityCompetences = _context.SpecialityCompetences.Where(b => b.SpecialityId == specialityId).Include(b => b.Speciality).Include(b => b.Competence).ToList();
            var competences = specialityCompetences.Select(b => b.Competence).Select(b => b.Competence1).ToList();
            var ids = specialityCompetences.Select(b => b.Id).ToList();

            List<string> competenceString = new List<string> {};

            for(int i = 0; i < competences.Count(); i++)
            {
                competenceString.Add(ids[i] + ". " + competences[i]);
            }

            ViewBag.Competences = competenceString;

            return View();
        }

        // POST: EpSubjectCompetences/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int subjectId, [Bind("Id,SubjectId,SpecialityCompetenceId")] EpSubjectCompetence epSubjectCompetence)
        {
            epSubjectCompetence.SubjectId = subjectId;
            if (IsUnique(epSubjectCompetence.SubjectId, epSubjectCompetence.SpecialityCompetenceId))
            {
                if (ModelState.IsValid)
                {
                    _context.Add(epSubjectCompetence);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index", "EpSubjectCompetences", new
                    {
                        id = subjectId,
                        name = _context.Subjects.Where(c => c.Id == subjectId).FirstOrDefault().Name,
                        eprogramId = _context.Subjects.Where(c => c.Id == subjectId).FirstOrDefault().EprogramId,
                        credit = _context.Subjects.Where(c => c.Id == subjectId).FirstOrDefault().Credit,
                        controlId = _context.Subjects.Where(c => c.Id == subjectId).FirstOrDefault().ControlId
                    });
                }
            }
            else
            {
                ViewData["ErrorMessage"] = "Така компетентність до цього предмету вже додана!";
            }
            ViewBag.SubjectId = subjectId;
            ViewBag.SubjectName = _context.Subjects.Where(c => c.Id == subjectId).FirstOrDefault().Name;
            ViewBag.SubjectEprogramId = _context.Subjects.Where(c => c.Id == subjectId).FirstOrDefault().EprogramId;
            ViewBag.SubjectCredit = _context.Subjects.Where(c => c.Id == subjectId).FirstOrDefault().Credit;
            ViewBag.SubjectConrolId = _context.Subjects.Where(c => c.Id == subjectId).FirstOrDefault().ControlId;

            int educationProgramId = _context.Subjects.Where(c => c.Id == subjectId).FirstOrDefault().EprogramId;
            int specialityId = _context.EducationPrograms.Where(c => c.Id == educationProgramId).FirstOrDefault().SpecialityId;

            ViewData["SpecialityCompetenceId"] = new SelectList(_context.SpecialityCompetences.Where(b => b.SpecialityId == specialityId), "Id", "Id");
            var specialityCompetences = _context.SpecialityCompetences.Where(b => b.SpecialityId == specialityId).Include(b => b.Speciality).Include(b => b.Competence).ToList();
            var competences = specialityCompetences.Select(b => b.Competence).Select(b => b.Competence1).ToList();
            var ids = specialityCompetences.Select(b => b.Id).ToList();

            List<string> competenceString = new List<string> { };

            for (int i = 0; i < competences.Count(); i++)
            {
                competenceString.Add(ids[i] + ". " + competences[i]);
            }

            ViewBag.Competences = competenceString;
            return View(epSubjectCompetence);
        }

        bool IsUnique(int subjectId, int specialityCompetenceId)
        {
            var epSubjectCompetences = _context.EpSubjectCompetences.Where(b => b.SubjectId == subjectId && b.SpecialityCompetenceId == specialityCompetenceId).ToList();
            if (epSubjectCompetences.Count == 0) return true;
            return false;
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
            //ViewData["SpecialityCompetenceId"] = new SelectList(_context.SpecialityCompetences, "Id", "Id", epSubjectCompetence.SpecialityCompetenceId);
            //ViewData["SubjectId"] = new SelectList(_context.Subjects, "Id", "Name", epSubjectCompetence.SubjectId);
            var subjectId = epSubjectCompetence.SubjectId;
            ViewBag.SubjectId = subjectId;
            ViewBag.SubjectName = _context.Subjects.Where(c => c.Id == subjectId).FirstOrDefault().Name;
            ViewBag.SubjectEprogramId = _context.Subjects.Where(c => c.Id == subjectId).FirstOrDefault().EprogramId;
            ViewBag.SubjectCredit = _context.Subjects.Where(c => c.Id == subjectId).FirstOrDefault().Credit;
            ViewBag.SubjectConrolId = _context.Subjects.Where(c => c.Id == subjectId).FirstOrDefault().ControlId;

            int educationProgramId = _context.Subjects.Where(c => c.Id == subjectId).FirstOrDefault().EprogramId;
            int specialityId = _context.EducationPrograms.Where(c => c.Id == educationProgramId).FirstOrDefault().SpecialityId;

            ViewData["SpecialityCompetenceId"] = new SelectList(_context.SpecialityCompetences.Where(b => b.SpecialityId == specialityId), "Id", "Id");
            var specialityCompetences = _context.SpecialityCompetences.Where(b => b.SpecialityId == specialityId).Include(b => b.Speciality).Include(b => b.Competence).ToList();
            var competences = specialityCompetences.Select(b => b.Competence).Select(b => b.Competence1).ToList();
            var ids = specialityCompetences.Select(b => b.Id).ToList();

            List<string> competenceString = new List<string> { };

            for (int i = 0; i < competences.Count(); i++)
            {
                competenceString.Add(ids[i] + ". " + competences[i]);
            }

            ViewBag.Competences = competenceString;
            return View(epSubjectCompetence);
        }

        // POST: EpSubjectCompetences/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, int subjectId, [Bind("Id,SubjectId,SpecialityCompetenceId")] EpSubjectCompetence epSubjectCompetence)
        {
            if (id != epSubjectCompetence.Id)
            {
                return NotFound();
            }

            if (IsUniqueEdit(epSubjectCompetence.Id, subjectId, epSubjectCompetence.SpecialityCompetenceId))
            {
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
                    //return RedirectToAction(nameof(Index));
                    return RedirectToAction("Index", "EpSubjectCompetences", new
                    {
                        id = subjectId,
                        name = _context.Subjects.Where(c => c.Id == subjectId).FirstOrDefault().Name,
                        eprogramId = _context.Subjects.Where(c => c.Id == subjectId).FirstOrDefault().EprogramId,
                        credit = _context.Subjects.Where(c => c.Id == subjectId).FirstOrDefault().Credit,
                        controlId = _context.Subjects.Where(c => c.Id == subjectId).FirstOrDefault().ControlId
                    });
                }
            }
            else
            {
                ViewData["ErrorMessage"] = "Така компетентність до цього предмету вже додана!";
            }
            //ViewData["SpecialityCompetenceId"] = new SelectList(_context.SpecialityCompetences, "Id", "Id", epSubjectCompetence.SpecialityCompetenceId);
            //ViewData["SubjectId"] = new SelectList(_context.Subjects, "Id", "Name", epSubjectCompetence.SubjectId);
            ViewBag.SubjectId = subjectId;
            ViewBag.SubjectName = _context.Subjects.Where(c => c.Id == subjectId).FirstOrDefault().Name;
            ViewBag.SubjectEprogramId = _context.Subjects.Where(c => c.Id == subjectId).FirstOrDefault().EprogramId;
            ViewBag.SubjectCredit = _context.Subjects.Where(c => c.Id == subjectId).FirstOrDefault().Credit;
            ViewBag.SubjectConrolId = _context.Subjects.Where(c => c.Id == subjectId).FirstOrDefault().ControlId;

            int educationProgramId = _context.Subjects.Where(c => c.Id == subjectId).FirstOrDefault().EprogramId;
            int specialityId = _context.EducationPrograms.Where(c => c.Id == educationProgramId).FirstOrDefault().SpecialityId;

            ViewData["SpecialityCompetenceId"] = new SelectList(_context.SpecialityCompetences.Where(b => b.SpecialityId == specialityId), "Id", "Id");
            var specialityCompetences = _context.SpecialityCompetences.Where(b => b.SpecialityId == specialityId).Include(b => b.Speciality).Include(b => b.Competence).ToList();
            var competences = specialityCompetences.Select(b => b.Competence).Select(b => b.Competence1).ToList();
            var ids = specialityCompetences.Select(b => b.Id).ToList();

            List<string> competenceString = new List<string> { };

            for (int i = 0; i < competences.Count(); i++)
            {
                competenceString.Add(ids[i] + ". " + competences[i]);
            }

            ViewBag.Competences = competenceString;
            return View(epSubjectCompetence);
        }

        bool IsUniqueEdit(int id, int subjectId, int specialityCompetenceId)
        {
            var epSubjectCompetences = _context.EpSubjectCompetences.Where(b => b.SubjectId == subjectId && b.SpecialityCompetenceId == specialityCompetenceId && b.Id != id).ToList();
            if (epSubjectCompetences.Count == 0) return true;
            return false;
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

            var subjectId = epSubjectCompetence.SubjectId;
            ViewBag.SubjectId = subjectId;
            ViewBag.SubjectName = _context.Subjects.Where(c => c.Id == subjectId).FirstOrDefault().Name;
            ViewBag.SubjectEprogramId = _context.Subjects.Where(c => c.Id == subjectId).FirstOrDefault().EprogramId;
            ViewBag.SubjectCredit = _context.Subjects.Where(c => c.Id == subjectId).FirstOrDefault().Credit;
            ViewBag.SubjectConrolId = _context.Subjects.Where(c => c.Id == subjectId).FirstOrDefault().ControlId;
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
            //return RedirectToAction(nameof(Index));
            var subjectId = epSubjectCompetence.SubjectId;
            return RedirectToAction("Index", "EpSubjectCompetences", new
            {
                id = subjectId,
                name = _context.Subjects.Where(c => c.Id == subjectId).FirstOrDefault().Name,
                eprogramId = _context.Subjects.Where(c => c.Id == subjectId).FirstOrDefault().EprogramId,
                credit = _context.Subjects.Where(c => c.Id == subjectId).FirstOrDefault().Credit,
                controlId = _context.Subjects.Where(c => c.Id == subjectId).FirstOrDefault().ControlId
            });
        }

        private bool EpSubjectCompetenceExists(int id)
        {
          return (_context.EpSubjectCompetences?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
