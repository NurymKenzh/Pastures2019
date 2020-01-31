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
    public class SoobsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SoobsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Soobs
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Index(
            string SortOrder,
            int? CodeFilter,
            string DescriptionFilter,
            int? PageNumber)
        {
            var soob = _context.Soob
                .Where(b => true);

            ViewBag.CodeFilter = CodeFilter;
            ViewBag.DescriptionFilter = DescriptionFilter;

            ViewBag.CodeSort = SortOrder == "Code" ? "CodeDesc" : "Code";
            ViewBag.DescriptionSort = SortOrder == "Description" ? "DescriptionDesc" : "Description";

            if (CodeFilter != null)
            {
                soob = soob.Where(b => b.Code == CodeFilter);
            }
            if (!string.IsNullOrEmpty(DescriptionFilter))
            {
                soob = soob.Where(b => b.Description.Contains(DescriptionFilter));
            }

            switch (SortOrder)
            {
                case "Code":
                    soob = soob.OrderBy(b => b.Code);
                    break;
                case "CodeDesc":
                    soob = soob.OrderByDescending(b => b.Code);
                    break;
                case "Description":
                    soob = soob.OrderBy(b => b.Description);
                    break;
                case "DescriptionDesc":
                    soob = soob.OrderByDescending(b => b.Description);
                    break;
                default:
                    soob = soob.OrderBy(b => b.Id);
                    break;
            }

            ViewBag.SortOrder = SortOrder;

            var pager = new Pager(soob.Count(), PageNumber);

            var viewModel = new SoobIndexPageViewModel
            {
                Items = soob.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
                Pager = pager
            };

            return View(viewModel);
        }

        // GET: Soobs/Details/5
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var soob = await _context.Soob
                .FirstOrDefaultAsync(m => m.Id == id);
            if (soob == null)
            {
                return NotFound();
            }

            return View(soob);
        }

        // GET: Soobs/Create
        [Authorize(Roles = "Administrator, Moderator")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Soobs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Create([Bind("Id,Code,DescriptionRU,DescriptionKK,DescriptionEN,DescriptionLat")] Soob soob)
        {
            if (ModelState.IsValid)
            {
                _context.Add(soob);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(soob);
        }

        // GET: Soobs/Edit/5
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var soob = await _context.Soob.FindAsync(id);
            if (soob == null)
            {
                return NotFound();
            }
            return View(soob);
        }

        // POST: Soobs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Code,DescriptionRU,DescriptionKK,DescriptionEN,DescriptionLat")] Soob soob)
        {
            if (id != soob.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(soob);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SoobExists(soob.Id))
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
            return View(soob);
        }

        // GET: Soobs/Delete/5
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var soob = await _context.Soob
                .FirstOrDefaultAsync(m => m.Id == id);
            if (soob == null)
            {
                return NotFound();
            }

            return View(soob);
        }

        // POST: Soobs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var soob = await _context.Soob.FindAsync(id);
            _context.Soob.Remove(soob);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SoobExists(int id)
        {
            return _context.Soob.Any(e => e.Id == id);
        }
    }
}
