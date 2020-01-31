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
    public class RecommendsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RecommendsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Recommends
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Index(
            string SortOrder,
            int? CodeFilter,
            string DescriptionFilter,
            int? PageNumber)
        {
            var recommend = _context.Recommend
                .Where(b => true);

            ViewBag.CodeFilter = CodeFilter;
            ViewBag.DescriptionFilter = DescriptionFilter;

            ViewBag.CodeSort = SortOrder == "Code" ? "CodeDesc" : "Code";
            ViewBag.DescriptionSort = SortOrder == "Description" ? "DescriptionDesc" : "Description";

            if (CodeFilter != null)
            {
                recommend = recommend.Where(b => b.Code == CodeFilter);
            }
            if (!string.IsNullOrEmpty(DescriptionFilter))
            {
                recommend = recommend.Where(b => b.Description.Contains(DescriptionFilter));
            }

            switch (SortOrder)
            {
                case "Code":
                    recommend = recommend.OrderBy(b => b.Code);
                    break;
                case "CodeDesc":
                    recommend = recommend.OrderByDescending(b => b.Code);
                    break;
                case "Description":
                    recommend = recommend.OrderBy(b => b.Description);
                    break;
                case "DescriptionDesc":
                    recommend = recommend.OrderByDescending(b => b.Description);
                    break;
                default:
                    recommend = recommend.OrderBy(b => b.Id);
                    break;
            }

            ViewBag.SortOrder = SortOrder;

            var pager = new Pager(recommend.Count(), PageNumber);

            var viewModel = new RecommendIndexPageViewModel
            {
                Items = recommend.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
                Pager = pager
            };

            return View(viewModel);
        }

        // GET: Recommends/Details/5
        [Authorize(Roles = "Administrator, Moderator")]
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
        [Authorize(Roles = "Administrator, Moderator")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Recommends/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
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
        [Authorize(Roles = "Administrator, Moderator")]
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
        [Authorize(Roles = "Administrator, Moderator")]
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
        [Authorize(Roles = "Administrator, Moderator")]
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
        [Authorize(Roles = "Administrator, Moderator")]
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
