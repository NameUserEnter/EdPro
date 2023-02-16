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

            return View(educationProgram);
        }

        // GET: EducationPrograms/Create
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
            if(!Time(educationProgram.ImplementationDate))
            {
                return View(educationProgram);
            }
            if (ModelState.IsValid)
            {
                _context.Add(educationProgram);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EdPrTypeId"] = new SelectList(_context.EdProgramTypes, "Id", "TypeName", educationProgram.EdPrTypeId);
            ViewData["FacultyId"] = new SelectList(_context.Faculties, "Id", "Name", educationProgram.FacultyId);
            ViewData["SpecialityId"] = new SelectList(_context.Specialities, "Id", "Name", educationProgram.SpecialityId);
            return View(educationProgram);
        }
        public bool Time(DateTime time)
        {
            if (time.Year > 1960)
                return true;
            else
                return false;
        }
            // GET: EducationPrograms/Edit/5
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
            ViewData["EdPrTypeId"] = new SelectList(_context.EdProgramTypes, "Id", "TypeName", educationProgram.EdPrTypeId);
            ViewData["FacultyId"] = new SelectList(_context.Faculties, "Id", "Name", educationProgram.FacultyId);
            ViewData["SpecialityId"] = new SelectList(_context.Specialities, "Id", "Name", educationProgram.SpecialityId);
            return View(educationProgram);
        }

        // GET: EducationPrograms/Delete/5
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
            if (educationProgram != null)
            {
                _context.EducationPrograms.Remove(educationProgram);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EducationProgramExists(int id)
        {
          return (_context.EducationPrograms?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
