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
    public class SupplyRecommendsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SupplyRecommendsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: SupplyRecommends
        public async Task<IActionResult> Index()
        {
            return View(await _context.SupplyRecommend.ToListAsync());
        }

        // GET: SupplyRecommends/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var supplyRecommend = await _context.SupplyRecommend
                .FirstOrDefaultAsync(m => m.Id == id);
            if (supplyRecommend == null)
            {
                return NotFound();
            }

            return View(supplyRecommend);
        }

        // GET: SupplyRecommends/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: SupplyRecommends/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Code,DescriptionRU,DescriptionKK,DescriptionEN")] SupplyRecommend supplyRecommend)
        {
            if (ModelState.IsValid)
            {
                _context.Add(supplyRecommend);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(supplyRecommend);
        }

        // GET: SupplyRecommends/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var supplyRecommend = await _context.SupplyRecommend.FindAsync(id);
            if (supplyRecommend == null)
            {
                return NotFound();
            }
            return View(supplyRecommend);
        }

        // POST: SupplyRecommends/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Code,DescriptionRU,DescriptionKK,DescriptionEN")] SupplyRecommend supplyRecommend)
        {
            if (id != supplyRecommend.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(supplyRecommend);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SupplyRecommendExists(supplyRecommend.Id))
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
            return View(supplyRecommend);
        }

        // GET: SupplyRecommends/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var supplyRecommend = await _context.SupplyRecommend
                .FirstOrDefaultAsync(m => m.Id == id);
            if (supplyRecommend == null)
            {
                return NotFound();
            }

            return View(supplyRecommend);
        }

        // POST: SupplyRecommends/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var supplyRecommend = await _context.SupplyRecommend.FindAsync(id);
            _context.SupplyRecommend.Remove(supplyRecommend);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SupplyRecommendExists(int id)
        {
            return _context.SupplyRecommend.Any(e => e.Id == id);
        }
    }
}
