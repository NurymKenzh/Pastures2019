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
    public class ChemicalCompsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ChemicalCompsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ChemicalComps
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Index(
            string SortOrder,
            int? CodeFilter,
            string DescriptionFilter,
            int? PageNumber)
        {
            var chemicalComp = _context.ChemicalComp
                .Where(b => true);

            ViewBag.CodeFilter = CodeFilter;
            ViewBag.DescriptionFilter = DescriptionFilter;

            ViewBag.CodeSort = SortOrder == "Code" ? "CodeDesc" : "Code";
            ViewBag.DescriptionSort = SortOrder == "Description" ? "DescriptionDesc" : "Description";

            if (CodeFilter != null)
            {
                chemicalComp = chemicalComp.Where(b => b.Code == CodeFilter);
            }
            if (!string.IsNullOrEmpty(DescriptionFilter))
            {
                chemicalComp = chemicalComp.Where(b => b.Description.Contains(DescriptionFilter));
            }

            switch (SortOrder)
            {
                case "Code":
                    chemicalComp = chemicalComp.OrderBy(b => b.Code);
                    break;
                case "CodeDesc":
                    chemicalComp = chemicalComp.OrderByDescending(b => b.Code);
                    break;
                case "Description":
                    chemicalComp = chemicalComp.OrderBy(b => b.Description);
                    break;
                case "DescriptionDesc":
                    chemicalComp = chemicalComp.OrderByDescending(b => b.Description);
                    break;
                default:
                    chemicalComp = chemicalComp.OrderBy(b => b.Id);
                    break;
            }

            ViewBag.SortOrder = SortOrder;

            var pager = new Pager(chemicalComp.Count(), PageNumber);

            var viewModel = new ChemicalCompIndexPageViewModel
            {
                Items = chemicalComp.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
                Pager = pager
            };

            return View(viewModel);
        }

        // GET: ChemicalComps/Details/5
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chemicalComp = await _context.ChemicalComp
                .FirstOrDefaultAsync(m => m.Id == id);
            if (chemicalComp == null)
            {
                return NotFound();
            }

            return View(chemicalComp);
        }

        // GET: ChemicalComps/Create
        [Authorize(Roles = "Administrator, Moderator")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: ChemicalComps/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Create([Bind("Id,Code,DescriptionRU,DescriptionKK,DescriptionEN")] ChemicalComp chemicalComp)
        {
            if (ModelState.IsValid)
            {
                _context.Add(chemicalComp);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(chemicalComp);
        }

        // GET: ChemicalComps/Edit/5
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chemicalComp = await _context.ChemicalComp.FindAsync(id);
            if (chemicalComp == null)
            {
                return NotFound();
            }
            return View(chemicalComp);
        }

        // POST: ChemicalComps/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Code,DescriptionRU,DescriptionKK,DescriptionEN")] ChemicalComp chemicalComp)
        {
            if (id != chemicalComp.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(chemicalComp);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ChemicalCompExists(chemicalComp.Id))
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
            return View(chemicalComp);
        }

        // GET: ChemicalComps/Delete/5
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chemicalComp = await _context.ChemicalComp
                .FirstOrDefaultAsync(m => m.Id == id);
            if (chemicalComp == null)
            {
                return NotFound();
            }

            return View(chemicalComp);
        }

        // POST: ChemicalComps/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var chemicalComp = await _context.ChemicalComp.FindAsync(id);
            _context.ChemicalComp.Remove(chemicalComp);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ChemicalCompExists(int id)
        {
            return _context.ChemicalComp.Any(e => e.Id == id);
        }
    }
}
