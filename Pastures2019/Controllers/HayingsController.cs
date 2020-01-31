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
    public class HayingsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HayingsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Hayings
        public async Task<IActionResult> Index()
        {
            return View(await _context.Haying.ToListAsync());
        }

        // GET: Hayings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var haying = await _context.Haying
                .FirstOrDefaultAsync(m => m.Id == id);
            if (haying == null)
            {
                return NotFound();
            }

            return View(haying);
        }

        // GET: Hayings/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Hayings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Code,DescriptionRU,DescriptionKK,DescriptionEN")] Haying haying)
        {
            if (ModelState.IsValid)
            {
                _context.Add(haying);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(haying);
        }

        // GET: Hayings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var haying = await _context.Haying.FindAsync(id);
            if (haying == null)
            {
                return NotFound();
            }
            return View(haying);
        }

        // POST: Hayings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Code,DescriptionRU,DescriptionKK,DescriptionEN")] Haying haying)
        {
            if (id != haying.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(haying);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HayingExists(haying.Id))
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
            return View(haying);
        }

        // GET: Hayings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var haying = await _context.Haying
                .FirstOrDefaultAsync(m => m.Id == id);
            if (haying == null)
            {
                return NotFound();
            }

            return View(haying);
        }

        // POST: Hayings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var haying = await _context.Haying.FindAsync(id);
            _context.Haying.Remove(haying);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HayingExists(int id)
        {
            return _context.Haying.Any(e => e.Id == id);
        }
    }
}
