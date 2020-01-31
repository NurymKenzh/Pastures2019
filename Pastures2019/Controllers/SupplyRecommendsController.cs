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
    public class SupplyRecommendsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SupplyRecommendsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: SupplyRecommends
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Index(
            string SortOrder,
            int? CodeFilter,
            string DescriptionFilter,
            int? PageNumber)
        {
            var supplyRecommend = _context.SupplyRecommend
                .Where(b => true);

            ViewBag.CodeFilter = CodeFilter;
            ViewBag.DescriptionFilter = DescriptionFilter;

            ViewBag.CodeSort = SortOrder == "Code" ? "CodeDesc" : "Code";
            ViewBag.DescriptionSort = SortOrder == "Description" ? "DescriptionDesc" : "Description";

            if (CodeFilter != null)
            {
                supplyRecommend = supplyRecommend.Where(b => b.Code == CodeFilter);
            }
            if (!string.IsNullOrEmpty(DescriptionFilter))
            {
                supplyRecommend = supplyRecommend.Where(b => b.Description.Contains(DescriptionFilter));
            }

            switch (SortOrder)
            {
                case "Code":
                    supplyRecommend = supplyRecommend.OrderBy(b => b.Code);
                    break;
                case "CodeDesc":
                    supplyRecommend = supplyRecommend.OrderByDescending(b => b.Code);
                    break;
                case "Description":
                    supplyRecommend = supplyRecommend.OrderBy(b => b.Description);
                    break;
                case "DescriptionDesc":
                    supplyRecommend = supplyRecommend.OrderByDescending(b => b.Description);
                    break;
                default:
                    supplyRecommend = supplyRecommend.OrderBy(b => b.Id);
                    break;
            }

            ViewBag.SortOrder = SortOrder;

            var pager = new Pager(supplyRecommend.Count(), PageNumber);

            var viewModel = new SupplyRecommendIndexPageViewModel
            {
                Items = supplyRecommend.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
                Pager = pager
            };

            return View(viewModel);
        }

        // GET: SupplyRecommends/Details/5
        [Authorize(Roles = "Administrator, Moderator")]
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
        [Authorize(Roles = "Administrator, Moderator")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: SupplyRecommends/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
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
        [Authorize(Roles = "Administrator, Moderator")]
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
        [Authorize(Roles = "Administrator, Moderator")]
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
        [Authorize(Roles = "Administrator, Moderator")]
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
        [Authorize(Roles = "Administrator, Moderator")]
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
