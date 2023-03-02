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
    public class EdProgramTypesController : Controller
    {
        private readonly EdProContext _context;

        public EdProgramTypesController(EdProContext context)
        {
            _context = context;
        }

        // GET: EdProgramTypes
        public async Task<IActionResult> Index()
        {
              return _context.EdProgramTypes != null ? 
                          View(await _context.EdProgramTypes.ToListAsync()) :
                          Problem("Entity set 'EdProContext.EdProgramTypes'  is null.");
        }

        // GET: EdProgramTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.EdProgramTypes == null)
            {
                return NotFound();
            }

            var edProgramType = await _context.EdProgramTypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (edProgramType == null)
            {
                return NotFound();
            }

            return View(edProgramType);
        }

        // GET: EdProgramTypes/Create
        [Authorize(Roles = "admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: EdProgramTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TypeName")] EdProgramType edProgramType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(edProgramType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(edProgramType);
        }

        // GET: EdProgramTypes/Edit/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.EdProgramTypes == null)
            {
                return NotFound();
            }

            var edProgramType = await _context.EdProgramTypes.FindAsync(id);
            if (edProgramType == null)
            {
                return NotFound();
            }
            return View(edProgramType);
        }

        // POST: EdProgramTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TypeName")] EdProgramType edProgramType)
        {
            if (id != edProgramType.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(edProgramType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EdProgramTypeExists(edProgramType.Id))
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
            return View(edProgramType);
        }

        // GET: EdProgramTypes/Delete/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.EdProgramTypes == null)
            {
                return NotFound();
            }

            var edProgramType = await _context.EdProgramTypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (edProgramType == null)
            {
                return NotFound();
            }

            return View(edProgramType);
        }

        // POST: EdProgramTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.EdProgramTypes == null)
            {
                return Problem("Entity set 'EdProContext.EdProgramTypes'  is null.");
            }
            var edProgramType = await _context.EdProgramTypes.FindAsync(id);
            if (edProgramType != null)
            {
                _context.EdProgramTypes.Remove(edProgramType);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EdProgramTypeExists(int id)
        {
          return (_context.EdProgramTypes?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
