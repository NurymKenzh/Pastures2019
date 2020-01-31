using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Pastures2019.Data;
using Pastures2019.Models;

namespace Pastures2019.Controllers
{
    public class DominantTypesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DominantTypesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: DominantTypes
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Index(
            string SortOrder,
            int? CodeFilter,
            string DescriptionFilter,
            int? PageNumber)
        {
            var dominantTypes = _context.DominantType
                .Where(b => true);

            ViewBag.CodeFilter = CodeFilter;
            ViewBag.DescriptionFilter = DescriptionFilter;

            ViewBag.CodeSort = SortOrder == "Code" ? "CodeDesc" : "Code";
            ViewBag.DescriptionSort = SortOrder == "Description" ? "DescriptionDesc" : "Description";

            if (CodeFilter != null)
            {
                dominantTypes = dominantTypes.Where(b => b.Code == CodeFilter);
            }
            if (!string.IsNullOrEmpty(DescriptionFilter))
            {
                dominantTypes = dominantTypes.Where(b => b.Description.Contains(DescriptionFilter));
            }

            switch (SortOrder)
            {
                case "Code":
                    dominantTypes = dominantTypes.OrderBy(b => b.Code);
                    break;
                case "CodeDesc":
                    dominantTypes = dominantTypes.OrderByDescending(b => b.Code);
                    break;
                case "Description":
                    dominantTypes = dominantTypes.OrderBy(b => b.Description);
                    break;
                case "DescriptionDesc":
                    dominantTypes = dominantTypes.OrderByDescending(b => b.Description);
                    break;
                default:
                    dominantTypes = dominantTypes.OrderBy(b => b.Id);
                    break;
            }

            ViewBag.SortOrder = SortOrder;

            var pager = new Pager(dominantTypes.Count(), PageNumber);

            var viewModel = new DominantTypeIndexPageViewModel
            {
                Items = dominantTypes.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
                Pager = pager
            };

            return View(viewModel);
        }

        // GET: DominantTypes/Details/5
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dominantType = await _context.DominantType
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dominantType == null)
            {
                return NotFound();
            }

            return View(dominantType);
        }

        // GET: DominantTypes/Create
        [Authorize(Roles = "Administrator, Moderator")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: DominantTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Create([Bind("Id,Code,DescriptionRU,DescriptionKK,DescriptionEN")] DominantType dominantType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(dominantType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(dominantType);
        }

        // GET: DominantTypes/Edit/5
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dominantType = await _context.DominantType.FindAsync(id);
            if (dominantType == null)
            {
                return NotFound();
            }
            return View(dominantType);
        }

        // POST: DominantTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Code,DescriptionRU,DescriptionKK,DescriptionEN")] DominantType dominantType)
        {
            if (id != dominantType.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dominantType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DominantTypeExists(dominantType.Id))
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
            return View(dominantType);
        }

        // GET: DominantTypes/Delete/5
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dominantType = await _context.DominantType
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dominantType == null)
            {
                return NotFound();
            }

            return View(dominantType);
        }

        // POST: DominantTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var dominantType = await _context.DominantType.FindAsync(id);
            _context.DominantType.Remove(dominantType);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DominantTypeExists(int id)
        {
            return _context.DominantType.Any(e => e.Id == id);
        }
    }
}
