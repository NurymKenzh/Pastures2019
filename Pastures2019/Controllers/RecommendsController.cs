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
    public class RecommendsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RecommendsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Recommends
        public async Task<IActionResult> Index()
        {
            return View(await _context.Recommend.ToListAsync());
        }

        // GET: Recommends/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recommend = await _context.Recommend
                .FirstOrDefaultAsync(m => m.Id == id);
            if (recommend == null)
            {
                return NotFound();
            }

            return View(recommend);
        }

        // GET: Recommends/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Recommends/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Code,DescriptionRU,DescriptionKK,DescriptionEN")] Recommend recommend)
        {
            if (ModelState.IsValid)
            {
                _context.Add(recommend);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(recommend);
        }

        // GET: Recommends/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recommend = await _context.Recommend.FindAsync(id);
            if (recommend == null)
            {
                return NotFound();
            }
            return View(recommend);
        }

        // POST: Recommends/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Code,DescriptionRU,DescriptionKK,DescriptionEN")] Recommend recommend)
        {
            if (id != recommend.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(recommend);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RecommendExists(recommend.Id))
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
            return View(recommend);
        }

        // GET: Recommends/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recommend = await _context.Recommend
                .FirstOrDefaultAsync(m => m.Id == id);
            if (recommend == null)
            {
                return NotFound();
            }

            return View(recommend);
        }

        // POST: Recommends/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var recommend = await _context.Recommend.FindAsync(id);
            _context.Recommend.Remove(recommend);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RecommendExists(int id)
        {
            return _context.Recommend.Any(e => e.Id == id);
        }
    }
}
