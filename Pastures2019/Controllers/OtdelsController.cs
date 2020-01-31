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
    public class OtdelsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OtdelsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Otdels
        public async Task<IActionResult> Index()
        {
            return View(await _context.Otdel.ToListAsync());
        }

        // GET: Otdels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var otdel = await _context.Otdel
                .FirstOrDefaultAsync(m => m.Id == id);
            if (otdel == null)
            {
                return NotFound();
            }

            return View(otdel);
        }

        // GET: Otdels/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Otdels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Code,DescriptionRU,DescriptionKK,DescriptionEN")] Otdel otdel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(otdel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(otdel);
        }

        // GET: Otdels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var otdel = await _context.Otdel.FindAsync(id);
            if (otdel == null)
            {
                return NotFound();
            }
            return View(otdel);
        }

        // POST: Otdels/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Code,DescriptionRU,DescriptionKK,DescriptionEN")] Otdel otdel)
        {
            if (id != otdel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(otdel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OtdelExists(otdel.Id))
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
            return View(otdel);
        }

        // GET: Otdels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var otdel = await _context.Otdel
                .FirstOrDefaultAsync(m => m.Id == id);
            if (otdel == null)
            {
                return NotFound();
            }

            return View(otdel);
        }

        // POST: Otdels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var otdel = await _context.Otdel.FindAsync(id);
            _context.Otdel.Remove(otdel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OtdelExists(int id)
        {
            return _context.Otdel.Any(e => e.Id == id);
        }
    }
}
