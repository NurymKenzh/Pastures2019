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
    public class ZTypesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ZTypesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ZTypes
        public async Task<IActionResult> Index()
        {
            return View(await _context.ZType.ToListAsync());
        }

        // GET: ZTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var zType = await _context.ZType
                .FirstOrDefaultAsync(m => m.Id == id);
            if (zType == null)
            {
                return NotFound();
            }

            return View(zType);
        }

        // GET: ZTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ZTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Code,DescriptionRU,DescriptionKK,DescriptionEN,Color")] ZType zType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(zType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(zType);
        }

        // GET: ZTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var zType = await _context.ZType.FindAsync(id);
            if (zType == null)
            {
                return NotFound();
            }
            return View(zType);
        }

        // POST: ZTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Code,DescriptionRU,DescriptionKK,DescriptionEN,Color")] ZType zType)
        {
            if (id != zType.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(zType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ZTypeExists(zType.Id))
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
            return View(zType);
        }

        // GET: ZTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var zType = await _context.ZType
                .FirstOrDefaultAsync(m => m.Id == id);
            if (zType == null)
            {
                return NotFound();
            }

            return View(zType);
        }

        // POST: ZTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var zType = await _context.ZType.FindAsync(id);
            _context.ZType.Remove(zType);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ZTypeExists(int id)
        {
            return _context.ZType.Any(e => e.Id == id);
        }
    }
}
