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
    public class WTypesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public WTypesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: WTypes
        public async Task<IActionResult> Index()
        {
            return View(await _context.WType.ToListAsync());
        }

        // GET: WTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var wType = await _context.WType
                .FirstOrDefaultAsync(m => m.Id == id);
            if (wType == null)
            {
                return NotFound();
            }

            return View(wType);
        }

        // GET: WTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: WTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Code,DescriptionRU,DescriptionKK,DescriptionEN")] WType wType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(wType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(wType);
        }

        // GET: WTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var wType = await _context.WType.FindAsync(id);
            if (wType == null)
            {
                return NotFound();
            }
            return View(wType);
        }

        // POST: WTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Code,DescriptionRU,DescriptionKK,DescriptionEN")] WType wType)
        {
            if (id != wType.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(wType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WTypeExists(wType.Id))
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
            return View(wType);
        }

        // GET: WTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var wType = await _context.WType
                .FirstOrDefaultAsync(m => m.Id == id);
            if (wType == null)
            {
                return NotFound();
            }

            return View(wType);
        }

        // POST: WTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var wType = await _context.WType.FindAsync(id);
            _context.WType.Remove(wType);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WTypeExists(int id)
        {
            return _context.WType.Any(e => e.Id == id);
        }
    }
}
