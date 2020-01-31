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
    public class WSubTypesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public WSubTypesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: WSubTypes
        public async Task<IActionResult> Index()
        {
            return View(await _context.WSubType.ToListAsync());
        }

        // GET: WSubTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var wSubType = await _context.WSubType
                .FirstOrDefaultAsync(m => m.Id == id);
            if (wSubType == null)
            {
                return NotFound();
            }

            return View(wSubType);
        }

        // GET: WSubTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: WSubTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Code,DescriptionRU,DescriptionKK,DescriptionEN")] WSubType wSubType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(wSubType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(wSubType);
        }

        // GET: WSubTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var wSubType = await _context.WSubType.FindAsync(id);
            if (wSubType == null)
            {
                return NotFound();
            }
            return View(wSubType);
        }

        // POST: WSubTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Code,DescriptionRU,DescriptionKK,DescriptionEN")] WSubType wSubType)
        {
            if (id != wSubType.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(wSubType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WSubTypeExists(wSubType.Id))
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
            return View(wSubType);
        }

        // GET: WSubTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var wSubType = await _context.WSubType
                .FirstOrDefaultAsync(m => m.Id == id);
            if (wSubType == null)
            {
                return NotFound();
            }

            return View(wSubType);
        }

        // POST: WSubTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var wSubType = await _context.WSubType.FindAsync(id);
            _context.WSubType.Remove(wSubType);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WSubTypeExists(int id)
        {
            return _context.WSubType.Any(e => e.Id == id);
        }
    }
}
