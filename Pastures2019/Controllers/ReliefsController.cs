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
    public class ReliefsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReliefsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Reliefs
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Index(
            string SortOrder,
            int? CodeFilter,
            string DescriptionFilter,
            int? PageNumber)
        {
            var relief = _context.Relief
                .Where(b => true);

            ViewBag.CodeFilter = CodeFilter;
            ViewBag.DescriptionFilter = DescriptionFilter;

            ViewBag.CodeSort = SortOrder == "Code" ? "CodeDesc" : "Code";
            ViewBag.DescriptionSort = SortOrder == "Description" ? "DescriptionDesc" : "Description";

            if (CodeFilter != null)
            {
                relief = relief.Where(b => b.Code == CodeFilter);
            }
            if (!string.IsNullOrEmpty(DescriptionFilter))
            {
                relief = relief.Where(b => b.Description.Contains(DescriptionFilter));
            }

            switch (SortOrder)
            {
                case "Code":
                    relief = relief.OrderBy(b => b.Code);
                    break;
                case "CodeDesc":
                    relief = relief.OrderByDescending(b => b.Code);
                    break;
                case "Description":
                    relief = relief.OrderBy(b => b.Description);
                    break;
                case "DescriptionDesc":
                    relief = relief.OrderByDescending(b => b.Description);
                    break;
                default:
                    relief = relief.OrderBy(b => b.Id);
                    break;
            }

            ViewBag.SortOrder = SortOrder;

            var pager = new Pager(relief.Count(), PageNumber);

            var viewModel = new ReliefIndexPageViewModel
            {
                Items = relief.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
                Pager = pager
            };

            return View(viewModel);
        }

        // GET: Reliefs/Details/5
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var relief = await _context.Relief
                .FirstOrDefaultAsync(m => m.Id == id);
            if (relief == null)
            {
                return NotFound();
            }

            return View(relief);
        }

        // GET: Reliefs/Create
        [Authorize(Roles = "Administrator, Moderator")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Reliefs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Create([Bind("Id,Code,DescriptionRU,DescriptionKK,DescriptionEN")] Relief relief)
        {
            if (ModelState.IsValid)
            {
                _context.Add(relief);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(relief);
        }

        // GET: Reliefs/Edit/5
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var relief = await _context.Relief.FindAsync(id);
            if (relief == null)
            {
                return NotFound();
            }
            return View(relief);
        }

        // POST: Reliefs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Code,DescriptionRU,DescriptionKK,DescriptionEN")] Relief relief)
        {
            if (id != relief.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(relief);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReliefExists(relief.Id))
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
            return View(relief);
        }

        // GET: Reliefs/Delete/5
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var relief = await _context.Relief
                .FirstOrDefaultAsync(m => m.Id == id);
            if (relief == null)
            {
                return NotFound();
            }

            return View(relief);
        }

        // POST: Reliefs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var relief = await _context.Relief.FindAsync(id);
            _context.Relief.Remove(relief);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReliefExists(int id)
        {
            return _context.Relief.Any(e => e.Id == id);
        }
    }
}
