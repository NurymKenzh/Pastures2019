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
    public class OtdelsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OtdelsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Otdels
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Index(
            string SortOrder,
            int? CodeFilter,
            string DescriptionFilter,
            int? PageNumber)
        {
            var otdel = _context.Otdel
                .Where(b => true);

            ViewBag.CodeFilter = CodeFilter;
            ViewBag.DescriptionFilter = DescriptionFilter;

            ViewBag.CodeSort = SortOrder == "Code" ? "CodeDesc" : "Code";
            ViewBag.DescriptionSort = SortOrder == "Description" ? "DescriptionDesc" : "Description";

            if (CodeFilter != null)
            {
                otdel = otdel.Where(b => b.Code == CodeFilter);
            }
            if (!string.IsNullOrEmpty(DescriptionFilter))
            {
                otdel = otdel.Where(b => b.Description.Contains(DescriptionFilter));
            }

            switch (SortOrder)
            {
                case "Code":
                    otdel = otdel.OrderBy(b => b.Code);
                    break;
                case "CodeDesc":
                    otdel = otdel.OrderByDescending(b => b.Code);
                    break;
                case "Description":
                    otdel = otdel.OrderBy(b => b.Description);
                    break;
                case "DescriptionDesc":
                    otdel = otdel.OrderByDescending(b => b.Description);
                    break;
                default:
                    otdel = otdel.OrderBy(b => b.Id);
                    break;
            }

            ViewBag.SortOrder = SortOrder;

            var pager = new Pager(otdel.Count(), PageNumber);

            var viewModel = new OtdelIndexPageViewModel
            {
                Items = otdel.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
                Pager = pager
            };

            return View(viewModel);
        }

        // GET: Otdels/Details/5
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var otdel = await _context.Otdel
                .FirstOrDefaultAsync(m => m.Id == id);
            if (otdel == null)
            {
                return NotFound();
            }

            return View(otdel);
        }

        // GET: Otdels/Create
        [Authorize(Roles = "Administrator, Moderator")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Otdels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Create([Bind("Id,Code,DescriptionRU,DescriptionKK,DescriptionEN")] Otdel otdel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(otdel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(otdel);
        }

        // GET: Otdels/Edit/5
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var otdel = await _context.Otdel.FindAsync(id);
            if (otdel == null)
            {
                return NotFound();
            }
            return View(otdel);
        }

        // POST: Otdels/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Code,DescriptionRU,DescriptionKK,DescriptionEN")] Otdel otdel)
        {
            if (id != otdel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(otdel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OtdelExists(otdel.Id))
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
            return View(otdel);
        }

        // GET: Otdels/Delete/5
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var otdel = await _context.Otdel
                .FirstOrDefaultAsync(m => m.Id == id);
            if (otdel == null)
            {
                return NotFound();
            }

            return View(otdel);
        }

        // POST: Otdels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var otdel = await _context.Otdel.FindAsync(id);
            _context.Otdel.Remove(otdel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OtdelExists(int id)
        {
            return _context.Otdel.Any(e => e.Id == id);
        }
    }
}
