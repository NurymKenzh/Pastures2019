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
    public class RecomCattlesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RecomCattlesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: RecomCattles
        public async Task<IActionResult> Index()
        {
            return View(await _context.RecomCattle.ToListAsync());
        }

        // GET: RecomCattles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recomCattle = await _context.RecomCattle
                .FirstOrDefaultAsync(m => m.Id == id);
            if (recomCattle == null)
            {
                return NotFound();
            }

            return View(recomCattle);
        }

        // GET: RecomCattles/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: RecomCattles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Code,DescriptionRU,DescriptionKK,DescriptionEN")] RecomCattle recomCattle)
        {
            if (ModelState.IsValid)
            {
                _context.Add(recomCattle);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(recomCattle);
        }

        // GET: RecomCattles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recomCattle = await _context.RecomCattle.FindAsync(id);
            if (recomCattle == null)
            {
                return NotFound();
            }
            return View(recomCattle);
        }

        // POST: RecomCattles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Code,DescriptionRU,DescriptionKK,DescriptionEN")] RecomCattle recomCattle)
        {
            if (id != recomCattle.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(recomCattle);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RecomCattleExists(recomCattle.Id))
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
            return View(recomCattle);
        }

        // GET: RecomCattles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recomCattle = await _context.RecomCattle
                .FirstOrDefaultAsync(m => m.Id == id);
            if (recomCattle == null)
            {
                return NotFound();
            }

            return View(recomCattle);
        }

        // POST: RecomCattles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var recomCattle = await _context.RecomCattle.FindAsync(id);
            _context.RecomCattle.Remove(recomCattle);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RecomCattleExists(int id)
        {
            return _context.RecomCattle.Any(e => e.Id == id);
        }
    }
}
