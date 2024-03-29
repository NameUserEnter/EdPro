﻿using System;
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
    public class CompetencesTypesController : Controller
    {
        private readonly EdProContext _context;

        public CompetencesTypesController(EdProContext context)
        {
            _context = context;
        }

        // GET: CompetencesTypes
        public async Task<IActionResult> Index()
        {
              return _context.CompetencesTypes != null ? 
                          View(await _context.CompetencesTypes.ToListAsync()) :
                          Problem("Entity set 'EdProContext.CompetencesTypes'  is null.");
        }

        // GET: CompetencesTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.CompetencesTypes == null)
            {
                return NotFound();
            }

            var competencesType = await _context.CompetencesTypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (competencesType == null)
            {
                return NotFound();
            }

            return View(competencesType);
        }

        // GET: CompetencesTypes/Create
        [Authorize(Roles = "admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: CompetencesTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CompType")] CompetencesType competencesType)
        {
            if (IsUnique(competencesType.CompType))
            {
                if (ModelState.IsValid)
                {
                    _context.Add(competencesType);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            else
            {
                ViewData["ErrorMessage"] = "Такий тип компетентності вже доданий!";
            }
            return View(competencesType);
        }

        bool IsUnique(string compType)
        {
            var  competencesTypes = _context.CompetencesTypes.Where(b => b.CompType == compType).ToList();
            if (competencesTypes.Count == 0) return true;
            return false;
        }

        // GET: CompetencesTypes/Edit/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.CompetencesTypes == null)
            {
                return NotFound();
            }

            var competencesType = await _context.CompetencesTypes.FindAsync(id);
            if (competencesType == null)
            {
                return NotFound();
            }
            return View(competencesType);
        }

        // POST: CompetencesTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CompType")] CompetencesType competencesType)
        {
            if (id != competencesType.Id)
            {
                return NotFound();
            }

            if (IsUniqueEdit(competencesType.Id, competencesType.CompType))
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        _context.Update(competencesType);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!CompetencesTypeExists(competencesType.Id))
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
                ViewData["ErrorMessage"] = "Такий тип компетентності вже доданий!";
            }
            return View(competencesType);
        }

        bool IsUniqueEdit(int id, string compType)
        {
            var competencesTypes = _context.CompetencesTypes.Where(b => b.CompType == compType && b.Id != id).ToList();
            if (competencesTypes.Count == 0) return true;
            return false;
        }

        // GET: CompetencesTypes/Delete/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.CompetencesTypes == null)
            {
                return NotFound();
            }

            var competencesType = await _context.CompetencesTypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (competencesType == null)
            {
                return NotFound();
            }

            return View(competencesType);
        }

        // POST: CompetencesTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.CompetencesTypes == null)
            {
                return Problem("Entity set 'EdProContext.CompetencesTypes'  is null.");
            }
            var competencesType = await _context.CompetencesTypes.FindAsync(id);
            if (competencesType != null)
            {
                _context.CompetencesTypes.Remove(competencesType);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CompetencesTypeExists(int id)
        {
          return (_context.CompetencesTypes?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
