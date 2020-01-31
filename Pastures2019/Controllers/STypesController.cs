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
    public class STypesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public STypesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: STypes
        public async Task<IActionResult> Index()
        {
            return View(await _context.SType.ToListAsync());
        }

        // GET: STypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sType = await _context.SType
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sType == null)
            {
                return NotFound();
            }

            return View(sType);
        }

        // GET: STypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: STypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Code,DescriptionRU,DescriptionKK,DescriptionEN")] SType sType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(sType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(sType);
        }

        // GET: STypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sType = await _context.SType.FindAsync(id);
            if (sType == null)
            {
                return NotFound();
            }
            return View(sType);
        }

        // POST: STypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Code,DescriptionRU,DescriptionKK,DescriptionEN")] SType sType)
        {
            if (id != sType.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!STypeExists(sType.Id))
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
            return View(sType);
        }

        // GET: STypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sType = await _context.SType
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sType == null)
            {
                return NotFound();
            }

            return View(sType);
        }

        // POST: STypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var sType = await _context.SType.FindAsync(id);
            _context.SType.Remove(sType);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool STypeExists(int id)
        {
            return _context.SType.Any(e => e.Id == id);
        }
    }
}
