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
    public class SoobsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SoobsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Soobs
        public async Task<IActionResult> Index()
        {
            return View(await _context.Soob.ToListAsync());
        }

        // GET: Soobs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var soob = await _context.Soob
                .FirstOrDefaultAsync(m => m.Id == id);
            if (soob == null)
            {
                return NotFound();
            }

            return View(soob);
        }

        // GET: Soobs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Soobs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Code,DescriptionRU,DescriptionKK,DescriptionEN,DescriptionLat")] Soob soob)
        {
            if (ModelState.IsValid)
            {
                _context.Add(soob);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(soob);
        }

        // GET: Soobs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var soob = await _context.Soob.FindAsync(id);
            if (soob == null)
            {
                return NotFound();
            }
            return View(soob);
        }

        // POST: Soobs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Code,DescriptionRU,DescriptionKK,DescriptionEN,DescriptionLat")] Soob soob)
        {
            if (id != soob.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(soob);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SoobExists(soob.Id))
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
            return View(soob);
        }

        // GET: Soobs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var soob = await _context.Soob
                .FirstOrDefaultAsync(m => m.Id == id);
            if (soob == null)
            {
                return NotFound();
            }

            return View(soob);
        }

        // POST: Soobs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var soob = await _context.Soob.FindAsync(id);
            _context.Soob.Remove(soob);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SoobExists(int id)
        {
            return _context.Soob.Any(e => e.Id == id);
        }
    }
}
