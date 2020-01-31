using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Pastures2019.Data;
using Pastures2019.Models;

namespace Pastures2019.Controllers
{
    public class PTypesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PTypesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: PTypes
        public async Task<IActionResult> Index()
        {
            return View(await _context.PType.ToListAsync());
        }

        // GET: PTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pType = await _context.PType
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pType == null)
            {
                return NotFound();
            }

            return View(pType);
        }

        // GET: PTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: PTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Code,DescriptionRU,DescriptionKK,DescriptionEN")] PType pType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(pType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(pType);
        }

        // GET: PTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pType = await _context.PType.FindAsync(id);
            if (pType == null)
            {
                return NotFound();
            }
            return View(pType);
        }

        // POST: PTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Code,DescriptionRU,DescriptionKK,DescriptionEN")] PType pType)
        {
            if (id != pType.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PTypeExists(pType.Id))
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
            return View(pType);
        }

        // GET: PTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pType = await _context.PType
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pType == null)
            {
                return NotFound();
            }

            return View(pType);
        }

        // POST: PTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pType = await _context.PType.FindAsync(id);
            _context.PType.Remove(pType);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PTypeExists(int id)
        {
            return _context.PType.Any(e => e.Id == id);
        }
    }
}
