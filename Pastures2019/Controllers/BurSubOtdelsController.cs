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
    public class BurSubOtdelsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BurSubOtdelsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: BurSubOtdels
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Index(
            string SortOrder,
            int? CodeFilter,
            string DescriptionFilter,
            int? PageNumber)
        {
            var burSubOtdel = _context.BurSubOtdel
                .Where(b => true);

            ViewBag.CodeFilter = CodeFilter;
            ViewBag.DescriptionFilter = DescriptionFilter;

            ViewBag.CodeSort = SortOrder == "Code" ? "CodeDesc" : "Code";
            ViewBag.DescriptionSort = SortOrder == "Description" ? "DescriptionDesc" : "Description";

            if (CodeFilter != null)
            {
                burSubOtdel = burSubOtdel.Where(b => b.Code == CodeFilter);
            }
            if (!string.IsNullOrEmpty(DescriptionFilter))
            {
                burSubOtdel = burSubOtdel.Where(b => b.Description.Contains(DescriptionFilter));
            }

            switch (SortOrder)
            {
                case "Code":
                    burSubOtdel = burSubOtdel.OrderBy(b => b.Code);
                    break;
                case "CodeDesc":
                    burSubOtdel = burSubOtdel.OrderByDescending(b => b.Code);
                    break;
                case "Description":
                    burSubOtdel = burSubOtdel.OrderBy(b => b.Description);
                    break;
                case "DescriptionDesc":
                    burSubOtdel = burSubOtdel.OrderByDescending(b => b.Description);
                    break;
                default:
                    burSubOtdel = burSubOtdel.OrderBy(b => b.Id);
                    break;
            }

            ViewBag.SortOrder = SortOrder;

            var pager = new Pager(burSubOtdel.Count(), PageNumber);

            var viewModel = new BurSubOtdelIndexPageViewModel
            {
                Items = burSubOtdel.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
                Pager = pager
            };

            return View(viewModel);
        }

        // GET: BurSubOtdels/Details/5
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var burSubOtdel = await _context.BurSubOtdel
                .FirstOrDefaultAsync(m => m.Id == id);
            if (burSubOtdel == null)
            {
                return NotFound();
            }

            return View(burSubOtdel);
        }

        // GET: BurSubOtdels/Create
        [Authorize(Roles = "Administrator, Moderator")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: BurSubOtdels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Create([Bind("Id,Code,DescriptionRU,DescriptionKK,DescriptionEN")] BurSubOtdel burSubOtdel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(burSubOtdel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(burSubOtdel);
        }

        // GET: BurSubOtdels/Edit/5
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var burSubOtdel = await _context.BurSubOtdel.FindAsync(id);
            if (burSubOtdel == null)
            {
                return NotFound();
            }
            return View(burSubOtdel);
        }

        // POST: BurSubOtdels/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Code,DescriptionRU,DescriptionKK,DescriptionEN")] BurSubOtdel burSubOtdel)
        {
            if (id != burSubOtdel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(burSubOtdel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BurSubOtdelExists(burSubOtdel.Id))
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
            return View(burSubOtdel);
        }

        // GET: BurSubOtdels/Delete/5
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var burSubOtdel = await _context.BurSubOtdel
                .FirstOrDefaultAsync(m => m.Id == id);
            if (burSubOtdel == null)
            {
                return NotFound();
            }

            return View(burSubOtdel);
        }

        // POST: BurSubOtdels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var burSubOtdel = await _context.BurSubOtdel.FindAsync(id);
            _context.BurSubOtdel.Remove(burSubOtdel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BurSubOtdelExists(int id)
        {
            return _context.BurSubOtdel.Any(e => e.Id == id);
        }
    }
}
