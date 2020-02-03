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
    public class CATOSpeciesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CATOSpeciesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: CATOSpecies
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Index(
            string SortOrder,
            string CATOTEFilter,
            int? CodeFilter,
            int? PageNumber)
        {
            var cATOSpecies = _context.CATOSpecies
                .Where(b => true);

            ViewBag.CodeFilter = CodeFilter;
            ViewBag.CATOTEFilter = CATOTEFilter;

            ViewBag.CodeSort = SortOrder == "Code" ? "CodeDesc" : "Code";
            ViewBag.CATOTESort = SortOrder == "CATOTE" ? "CATOTEDesc" : "CATOTE";

            if (CodeFilter != null)
            {
                cATOSpecies = cATOSpecies.Where(b => b.Code == CodeFilter);
            }
            if (!string.IsNullOrEmpty(CATOTEFilter))
            {
                cATOSpecies = cATOSpecies.Where(b => b.CATOTE.Contains(CATOTEFilter));
            }

            switch (SortOrder)
            {
                case "Code":
                    cATOSpecies = cATOSpecies.OrderBy(b => b.Code);
                    break;
                case "CodeDesc":
                    cATOSpecies = cATOSpecies.OrderByDescending(b => b.Code);
                    break;
                case "CATOTE":
                    cATOSpecies = cATOSpecies.OrderBy(b => b.CATOTE);
                    break;
                case "CATOTEDesc":
                    cATOSpecies = cATOSpecies.OrderByDescending(b => b.CATOTE);
                    break;
                default:
                    cATOSpecies = cATOSpecies.OrderBy(b => b.Id);
                    break;
            }

            ViewBag.SortOrder = SortOrder;

            var pager = new Pager(cATOSpecies.Count(), PageNumber);

            var viewModel = new CATOSpeciesIndexPageViewModel
            {
                Items = cATOSpecies.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
                Pager = pager
            };

            return View(viewModel);
        }

        // GET: CATOSpecies/Details/5
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cATOSpecies = await _context.CATOSpecies
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cATOSpecies == null)
            {
                return NotFound();
            }

            return View(cATOSpecies);
        }

        // GET: CATOSpecies/Create
        [Authorize(Roles = "Administrator, Moderator")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: CATOSpecies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Create([Bind("Id,CATOTE,Code")] CATOSpecies cATOSpecies)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cATOSpecies);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(cATOSpecies);
        }

        // GET: CATOSpecies/Edit/5
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cATOSpecies = await _context.CATOSpecies.FindAsync(id);
            if (cATOSpecies == null)
            {
                return NotFound();
            }
            return View(cATOSpecies);
        }

        // POST: CATOSpecies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CATOTE,Code")] CATOSpecies cATOSpecies)
        {
            if (id != cATOSpecies.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cATOSpecies);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CATOSpeciesExists(cATOSpecies.Id))
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
            return View(cATOSpecies);
        }

        // GET: CATOSpecies/Delete/5
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cATOSpecies = await _context.CATOSpecies
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cATOSpecies == null)
            {
                return NotFound();
            }

            return View(cATOSpecies);
        }

        // POST: CATOSpecies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cATOSpecies = await _context.CATOSpecies.FindAsync(id);
            _context.CATOSpecies.Remove(cATOSpecies);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CATOSpeciesExists(int id)
        {
            return _context.CATOSpecies.Any(e => e.Id == id);
        }
    }
}
