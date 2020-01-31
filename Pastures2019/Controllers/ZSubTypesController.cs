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
    public class ZSubTypesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ZSubTypesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ZSubTypes
        public async Task<IActionResult> Index()
        {
            return View(await _context.ZSubType.ToListAsync());
        }

        // GET: ZSubTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var zSubType = await _context.ZSubType
                .FirstOrDefaultAsync(m => m.Id == id);
            if (zSubType == null)
            {
                return NotFound();
            }

            return View(zSubType);
        }

        // GET: ZSubTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ZSubTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Code,DescriptionRU,DescriptionKK,DescriptionEN")] ZSubType zSubType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(zSubType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(zSubType);
        }

        // GET: ZSubTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var zSubType = await _context.ZSubType.FindAsync(id);
            if (zSubType == null)
            {
                return NotFound();
            }
            return View(zSubType);
        }

        // POST: ZSubTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Code,DescriptionRU,DescriptionKK,DescriptionEN")] ZSubType zSubType)
        {
            if (id != zSubType.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(zSubType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ZSubTypeExists(zSubType.Id))
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
            return View(zSubType);
        }

        // GET: ZSubTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var zSubType = await _context.ZSubType
                .FirstOrDefaultAsync(m => m.Id == id);
            if (zSubType == null)
            {
                return NotFound();
            }

            return View(zSubType);
        }

        // POST: ZSubTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var zSubType = await _context.ZSubType.FindAsync(id);
            _context.ZSubType.Remove(zSubType);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ZSubTypeExists(int id)
        {
            return _context.ZSubType.Any(e => e.Id == id);
        }
    }
}
