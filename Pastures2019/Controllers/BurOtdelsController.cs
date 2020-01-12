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
    public class BurOtdelsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BurOtdelsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: BurOtdels
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Index(
            string SortOrder,
            int? CodeFilter,
            string DescriptionFilter,
            int? PageNumber)
        {
            var burOtdel = _context.BurOtdel
                .Where(b => true);

            ViewBag.CodeFilter = CodeFilter;
            ViewBag.DescriptionFilter = DescriptionFilter;

            ViewBag.CodeSort = SortOrder == "Code" ? "CodeDesc" : "Code";
            ViewBag.DescriptionSort = SortOrder == "Description" ? "DescriptionDesc" : "Description";

            if (CodeFilter != null)
            {
                burOtdel = burOtdel.Where(b => b.Code == CodeFilter);
            }
            if (!string.IsNullOrEmpty(DescriptionFilter))
            {
                burOtdel = burOtdel.Where(b => b.Description.Contains(DescriptionFilter));
            }

            switch (SortOrder)
            {
                case "Code":
                    burOtdel = burOtdel.OrderBy(b => b.Code);
                    break;
                case "CodeDesc":
                    burOtdel = burOtdel.OrderByDescending(b => b.Code);
                    break;
                case "Description":
                    burOtdel = burOtdel.OrderBy(b => b.Description);
                    break;
                case "DescriptionDesc":
                    burOtdel = burOtdel.OrderByDescending(b => b.Description);
                    break;
                default:
                    burOtdel = burOtdel.OrderBy(b => b.Id);
                    break;
            }

            ViewBag.SortOrder = SortOrder;

            var pager = new Pager(burOtdel.Count(), PageNumber);

            var viewModel = new BurOtdelIndexPageViewModel
            {
                Items = burOtdel.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
                Pager = pager
            };

            return View(viewModel);
        }

        // GET: BurOtdels/Details/5
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var burOtdel = await _context.BurOtdel
                .FirstOrDefaultAsync(m => m.Id == id);
            if (burOtdel == null)
            {
                return NotFound();
            }

            return View(burOtdel);
        }

        // GET: BurOtdels/Create
        [Authorize(Roles = "Administrator, Moderator")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: BurOtdels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Create([Bind("Id,Code,DescriptionRU,DescriptionKK,DescriptionEN")] BurOtdel burOtdel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(burOtdel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(burOtdel);
        }

        // GET: BurOtdels/Edit/5
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var burOtdel = await _context.BurOtdel.FindAsync(id);
            if (burOtdel == null)
            {
                return NotFound();
            }
            return View(burOtdel);
        }

        // POST: BurOtdels/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Code,DescriptionRU,DescriptionKK,DescriptionEN")] BurOtdel burOtdel)
        {
            if (id != burOtdel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(burOtdel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BurOtdelExists(burOtdel.Id))
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
            return View(burOtdel);
        }

        // GET: BurOtdels/Delete/5
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var burOtdel = await _context.BurOtdel
                .FirstOrDefaultAsync(m => m.Id == id);
            if (burOtdel == null)
            {
                return NotFound();
            }

            return View(burOtdel);
        }

        // POST: BurOtdels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var burOtdel = await _context.BurOtdel.FindAsync(id);
            _context.BurOtdel.Remove(burOtdel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BurOtdelExists(int id)
        {
            return _context.BurOtdel.Any(e => e.Id == id);
        }
    }
}
