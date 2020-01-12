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
    public class BGroupsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BGroupsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: BGroups
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Index(
            string SortOrder,
            int? CodeFilter,
            string DescriptionFilter,
            int? PageNumber)
        {
            var bGroup = _context.BGroup
                .Where(b => true);

            ViewBag.CodeFilter = CodeFilter;
            ViewBag.DescriptionFilter = DescriptionFilter;

            ViewBag.CodeSort = SortOrder == "Code" ? "CodeDesc" : "Code";
            ViewBag.DescriptionSort = SortOrder == "Description" ? "DescriptionDesc" : "Description";

            if (CodeFilter != null)
            {
                bGroup = bGroup.Where(b => b.Code == CodeFilter);
            }
            if (!string.IsNullOrEmpty(DescriptionFilter))
            {
                bGroup = bGroup.Where(b => b.Description.Contains(DescriptionFilter));
            }

            switch (SortOrder)
            {
                case "Code":
                    bGroup = bGroup.OrderBy(b => b.Code);
                    break;
                case "CodeDesc":
                    bGroup = bGroup.OrderByDescending(b => b.Code);
                    break;
                case "Description":
                    bGroup = bGroup.OrderBy(b => b.Description);
                    break;
                case "DescriptionDesc":
                    bGroup = bGroup.OrderByDescending(b => b.Description);
                    break;
                default:
                    bGroup = bGroup.OrderBy(b => b.Id);
                    break;
            }

            ViewBag.SortOrder = SortOrder;

            var pager = new Pager(bGroup.Count(), PageNumber);

            var viewModel = new BGroupIndexPageViewModel
            {
                Items = bGroup.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
                Pager = pager
            };

            return View(viewModel);
        }

        // GET: BGroups/Details/5
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bGroup = await _context.BGroup
                .FirstOrDefaultAsync(m => m.Id == id);
            if (bGroup == null)
            {
                return NotFound();
            }

            return View(bGroup);
        }

        // GET: BGroups/Create
        [Authorize(Roles = "Administrator, Moderator")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: BGroups/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Create([Bind("Id,Code,DescriptionRU,DescriptionKK,DescriptionEN")] BGroup bGroup)
        {
            if (ModelState.IsValid)
            {
                _context.Add(bGroup);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(bGroup);
        }

        // GET: BGroups/Edit/5
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bGroup = await _context.BGroup.FindAsync(id);
            if (bGroup == null)
            {
                return NotFound();
            }
            return View(bGroup);
        }

        // POST: BGroups/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Code,DescriptionRU,DescriptionKK,DescriptionEN")] BGroup bGroup)
        {
            if (id != bGroup.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(bGroup);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BGroupExists(bGroup.Id))
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
            return View(bGroup);
        }

        // GET: BGroups/Delete/5
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bGroup = await _context.BGroup
                .FirstOrDefaultAsync(m => m.Id == id);
            if (bGroup == null)
            {
                return NotFound();
            }

            return View(bGroup);
        }

        // POST: BGroups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var bGroup = await _context.BGroup.FindAsync(id);
            _context.BGroup.Remove(bGroup);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BGroupExists(int id)
        {
            return _context.BGroup.Any(e => e.Id == id);
        }
    }
}
