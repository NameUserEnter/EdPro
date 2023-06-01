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
using NuGet.Packaging;

namespace EdPro.Controllers
{
    [Authorize(Roles = "admin, user, worker")]
    public class EducationProgramsController : Controller
    {
        private readonly EdProContext _context;

        public EducationProgramsController(EdProContext context)
        {
            _context = context;
        }

        // GET: EducationPrograms
        public async Task<IActionResult> Index()
        {
            var edProContext = _context.EducationPrograms.Include(e => e.EdPrType).Include(e => e.Faculty).Include(e => e.Speciality);
            return View(await edProContext.ToListAsync());
        }

        // GET: EducationPrograms/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.EducationPrograms == null)
            {
                return NotFound();
            }

            var educationProgram = await _context.EducationPrograms
                .Include(e => e.EdPrType)
                .Include(e => e.Faculty)
                .Include(e => e.Speciality)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (educationProgram == null)
            {
                return NotFound();
            }

            return RedirectToAction("Index", "Subjects", new { id = educationProgram.Id, name = educationProgram.Name, specialityId = educationProgram.SpecialityId,
                edbo = educationProgram.Edbo, edPrTypeId = educationProgram.EdPrTypeId, facultyId = educationProgram.FacultyId, implementationDate = educationProgram.ImplementationDate  });
        }

        // GET: EducationPrograms/Create
        [Authorize(Roles = "admin, worker")]
        public IActionResult Create()
        {
            ViewData["EdPrTypeId"] = new SelectList(_context.EdProgramTypes, "Id", "TypeName");
            ViewData["FacultyId"] = new SelectList(_context.Faculties, "Id", "Name");
            ViewData["SpecialityId"] = new SelectList(_context.Specialities, "Id", "Name");
            return View();
        }

        // POST: EducationPrograms/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,SpecialityId,Edbo,EdPrTypeId,FacultyId,ImplementationDate")] EducationProgram educationProgram)
        {
            if (!Time(educationProgram.ImplementationDate))
            {
                ViewData["ErrorMessage"] = "Рік впровадження старіший за 1960!";
            }
            else 
            { 

                if (ModelState.IsValid)
                {
                    _context.Add(educationProgram);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }

            ViewData["EdPrTypeId"] = new SelectList(_context.EdProgramTypes, "Id", "TypeName", educationProgram.EdPrTypeId);
            ViewData["FacultyId"] = new SelectList(_context.Faculties, "Id", "Name", educationProgram.FacultyId);
            ViewData["SpecialityId"] = new SelectList(_context.Specialities, "Id", "Name", educationProgram.SpecialityId);
            return View(educationProgram);
        }

        //bool IsUnique(int specialityId, int edPrTypeId, int facultyId, DateTime implementationDate)
        //{
        //    var educationPrograms = _context.EducationPrograms.Where(b => b.SpecialityId == specialityId && b.FacultyId == facultyId && b.EdPrTypeId == edPrTypeId && b.ImplementationDate == implementationDate).ToList();
        //    if (educationPrograms.Count == 0) return true;
        //    return false;
        //}

        public bool Time(DateTime time)
        {
            if (time.Year > 1960)
                return true;
            else
                return false;
        }
        // GET: EducationPrograms/Edit/5
        [Authorize(Roles = "admin, worker")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.EducationPrograms == null)
            {
                return NotFound();
            }

            var educationProgram = await _context.EducationPrograms.FindAsync(id);
            if (educationProgram == null)
            {
                return NotFound();
            }
            ViewData["EdPrTypeId"] = new SelectList(_context.EdProgramTypes, "Id", "TypeName", educationProgram.EdPrTypeId);
            ViewData["FacultyId"] = new SelectList(_context.Faculties, "Id", "Name", educationProgram.FacultyId);
            ViewData["SpecialityId"] = new SelectList(_context.Specialities, "Id", "Name", educationProgram.SpecialityId);
            return View(educationProgram);
        }

        // POST: EducationPrograms/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,SpecialityId,Edbo,EdPrTypeId,FacultyId,ImplementationDate")] EducationProgram educationProgram)
        {
            if (id != educationProgram.Id)
            {
                return NotFound();
            }

            if (!Time(educationProgram.ImplementationDate))
            {
                ViewData["ErrorMessage"] = "Рік впровадження старіший за 1960!";
            }
            else
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        _context.Update(educationProgram);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!EducationProgramExists(educationProgram.Id))
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
            ViewData["EdPrTypeId"] = new SelectList(_context.EdProgramTypes, "Id", "TypeName", educationProgram.EdPrTypeId);
            ViewData["FacultyId"] = new SelectList(_context.Faculties, "Id", "Name", educationProgram.FacultyId);
            ViewData["SpecialityId"] = new SelectList(_context.Specialities, "Id", "Name", educationProgram.SpecialityId);
            return View(educationProgram);
        }

        // GET: EducationPrograms/Delete/5
        [Authorize(Roles = "admin, worker")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.EducationPrograms == null)
            {
                return NotFound();
            }

            var educationProgram = await _context.EducationPrograms
                .Include(e => e.EdPrType)
                .Include(e => e.Faculty)
                .Include(e => e.Speciality)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (educationProgram == null)
            {
                return NotFound();
            }

            return View(educationProgram);
        }

        // POST: EducationPrograms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.EducationPrograms == null)
            {
                return Problem("Entity set 'EdProContext.EducationPrograms'  is null.");
            }
            var educationProgram = await _context.EducationPrograms.FindAsync(id);
            
            var subjects = _context.Subjects.Where(f => f.EprogramId == id);
            if (subjects.Any())
            {
                foreach (var subject in subjects)
                {
                    _context.Subjects.Remove(subject);
                }
            }
            if (educationProgram != null)
            {
                _context.EducationPrograms.Remove(educationProgram);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Analyze(int? id)
        {
            ViewBag.d1 = Details1(id);
            
            ViewBag.d2 = Details2(id);

            ViewBag.d3 = Details3(id);
           
            ViewBag.d4 = Details4(id);
            return View();

        }

        public string Details1(int? id)
        {
            HashSet<string> str = new HashSet<string>();
            var subjects = _context.Subjects.Where(b => b.EprogramId == id).ToList();
            foreach (var subject in subjects)
            {
                var epSubjectLoutcomes = _context.EpSubjectLoutcomes.Where(b => b.SubjectId == subject.Id);
                if (!epSubjectLoutcomes.Any())
                {
                    str.Add(subject.Name + "; ");
                }
            }
            if(str.Count() == 0) 
            {
                ViewBag.d1 = " <h5> Всі дисципліни закривають хоча б один навчальний результат </h5>";
            }
            else
            {
                var strRes = String.Join("\n", str);
                ViewBag.d1 = $" <h5> Ці предмети не закривають жоден навчальний результат: </h5> {strRes}";
                ViewBag.d1 = ViewBag.d1.Replace("\n", "<br>");
            }
            return ViewBag.d1;
        }

        public string Details2(int? id)
        {
            HashSet<string> str = new HashSet<string>();
            var subjects = _context.Subjects.Where(b => b.EprogramId == id).ToList();
            foreach (var subject in subjects)
            {
                var epSubjectCompetences = _context.EpSubjectCompetences.Where(b => b.SubjectId == subject.Id);
                if (!epSubjectCompetences.Any())
                {
                    str.Add(subject.Name + "; ");
                }
            }
            if (str.Count() == 0)
            {
                ViewBag.d2 = " <h5> Всі дисципліни закривають хоча б одину компетентність </h5>";
            }
            else
            {
                var strRes = String.Join("\n", str);
                ViewBag.d2 = $" <h5> Ці предмети не закривають жодену компетентність: </h5> {strRes}";
                ViewBag.d2 = ViewBag.d2.Replace("\n", "<br>");
            }
            return ViewBag.d2;
        }

        public string Details3(int? id)
        {
            HashSet<string> str = new HashSet<string>();
            int specialityId = _context.EducationPrograms.Find(id).SpecialityId;
            var learningOutcomes = _context.LearningOutcomes.Where(b => b.SpecialityId == specialityId).ToList();
            foreach (var learningOutcome in learningOutcomes)
            {
                var epSubjectLoutcomes = _context.EpSubjectLoutcomes.Where(b => b.LearningOutcomeId == learningOutcome.Id).ToList();
                if (!epSubjectLoutcomes.Any())
                {
                    str.Add(learningOutcome.Loname + "; ");
                }
                else
                {
                    List<Subject> subjects = new List<Subject>();
                    foreach(var epSubjectLoutcome in epSubjectLoutcomes)
                    {
                        var sub = _context.Subjects.Find(epSubjectLoutcome.SubjectId);
                        if(sub.EprogramId == id)
                        {
                            subjects.Add(sub);
                        }
                    }
                    if(subjects.Count() == 0)
                    {
                        str.Add(learningOutcome.Loname + "; ");
                    }
                }
            }
            if (str.Count() == 0)
            {
                ViewBag.d3 = "<h5> Всі навчальні результати закриваються хоча б одніє дисципліною  </h5>";
            }
            else
            {
                var strRes = String.Join("\n", str);
                ViewBag.d3 = $"<h5>Ці навчальні результати не закриваються жодною дисципліною: </h5> \n {strRes}";
            }
            return ViewBag.d3;
        }

        public string Details4(int? id)
        {
            HashSet<string> str = new HashSet<string>();
            int specialityId = _context.EducationPrograms.Find(id).SpecialityId;
            var specialityCompetences = _context.SpecialityCompetences.Where(b => b.SpecialityId == specialityId).ToList();
            foreach (var specialityCompetence in specialityCompetences)
            {
                var epSubjectCompetences = _context.EpSubjectCompetences.Where(b => b.SpecialityCompetenceId == specialityCompetence.Id).ToList();
                if (!epSubjectCompetences.Any())
                {
                    var competence = _context.Competences.Find(specialityCompetence.CompetenceId);
                    str.Add(competence.Competence1 + "; ");
                }
                else
                {
                    List<Subject> subjects = new List<Subject>();
                    foreach (var epSubjectCompetence in epSubjectCompetences)
                    {
                        var sub = _context.Subjects.Find(epSubjectCompetence.SubjectId);
                        if (sub.EprogramId == id)
                        {
                            subjects.Add(sub);
                        }
                    }
                    if (subjects.Count() == 0)
                    {
                        var competence = _context.Competences.Find(specialityCompetence.CompetenceId);
                        str.Add(competence.Competence1 + "; ");
                    }
                }
            }
            if (str.Count() == 0)
            {
                ViewBag.d4 = "<h5> Всі компетентності закриваються хоча б одніє дисципліною </h5>";
            }
            else
            {
                var strRes = String.Join("\n \n", str);
                ViewBag.d4 = $"<h5> Ці компетентності не закриваються жодною дисципліною: </h5> {strRes}";
                ViewBag.d4 = ViewBag.d4.Replace("\n", "<br>");
            }
            return ViewBag.d4;
        }
        private bool EducationProgramExists(int id)
        {
          return (_context.EducationPrograms?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
