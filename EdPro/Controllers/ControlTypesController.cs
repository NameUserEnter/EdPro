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
    public class ControlTypesController : Controller
    {
        private readonly EdProContext _context;

        public ControlTypesController(EdProContext context)
        {
            _context = context;
        }

        // GET: ControlTypes
        public async Task<IActionResult> Index()
        {
              return _context.ControlTypes != null ? 
                          View(await _context.ControlTypes.ToListAsync()) :
                          Problem("Entity set 'EdProContext.ControlTypes'  is null.");
        }

        // GET: ControlTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ControlTypes == null)
            {
                return NotFound();
            }

            var controlType = await _context.ControlTypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (controlType == null)
            {
                return NotFound();
            }

            return View(controlType);
        }

        // GET: ControlTypes/Create
        [Authorize(Roles = "admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: ControlTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ControlTypeName")] ControlType controlType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(controlType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(controlType);
        }

        // GET: ControlTypes/Edit/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ControlTypes == null)
            {
                return NotFound();
            }

            var controlType = await _context.ControlTypes.FindAsync(id);
            if (controlType == null)
            {
                return NotFound();
            }
            return View(controlType);
        }

        // POST: ControlTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ControlTypeName")] ControlType controlType)
        {
            if (id != controlType.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(controlType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ControlTypeExists(controlType.Id))
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
            return View(controlType);
        }

        // GET: ControlTypes/Delete/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ControlTypes == null)
            {
                return NotFound();
            }

            var controlType = await _context.ControlTypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (controlType == null)
            {
                return NotFound();
            }

            return View(controlType);
        }

        // POST: ControlTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ControlTypes == null)
            {
                return Problem("Entity set 'EdProContext.ControlTypes'  is null.");
            }
            var controlType = await _context.ControlTypes.FindAsync(id);
            if (controlType != null)
            {
                _context.ControlTypes.Remove(controlType);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ControlTypeExists(int id)
        {
          return (_context.ControlTypes?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
