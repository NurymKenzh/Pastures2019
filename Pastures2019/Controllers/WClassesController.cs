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
    public class WClassesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public WClassesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: WClasses
        public async Task<IActionResult> Index()
        {
            return View(await _context.WClass.ToListAsync());
        }

        // GET: WClasses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var wClass = await _context.WClass
                .FirstOrDefaultAsync(m => m.Id == id);
            if (wClass == null)
            {
                return NotFound();
            }

            return View(wClass);
        }

        // GET: WClasses/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: WClasses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Code,DescriptionRU,DescriptionKK,DescriptionEN")] WClass wClass)
        {
            if (ModelState.IsValid)
            {
                _context.Add(wClass);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(wClass);
        }

        // GET: WClasses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var wClass = await _context.WClass.FindAsync(id);
            if (wClass == null)
            {
                return NotFound();
            }
            return View(wClass);
        }

        // POST: WClasses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Code,DescriptionRU,DescriptionKK,DescriptionEN")] WClass wClass)
        {
            if (id != wClass.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(wClass);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WClassExists(wClass.Id))
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
            return View(wClass);
        }

        // GET: WClasses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var wClass = await _context.WClass
                .FirstOrDefaultAsync(m => m.Id == id);
            if (wClass == null)
            {
                return NotFound();
            }

            return View(wClass);
        }

        // POST: WClasses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var wClass = await _context.WClass.FindAsync(id);
            _context.WClass.Remove(wClass);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WClassExists(int id)
        {
            return _context.WClass.Any(e => e.Id == id);
        }
    }
}
