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
    public class STypesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public STypesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: STypes
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Index(
            string SortOrder,
            int? CodeFilter,
            string DescriptionFilter,
            int? PageNumber)
        {
            var sType = _context.SType
                .Where(b => true);

            ViewBag.CodeFilter = CodeFilter;
            ViewBag.DescriptionFilter = DescriptionFilter;

            ViewBag.CodeSort = SortOrder == "Code" ? "CodeDesc" : "Code";
            ViewBag.DescriptionSort = SortOrder == "Description" ? "DescriptionDesc" : "Description";

            if (CodeFilter != null)
            {
                sType = sType.Where(b => b.Code == CodeFilter);
            }
            if (!string.IsNullOrEmpty(DescriptionFilter))
            {
                sType = sType.Where(b => b.Description.Contains(DescriptionFilter));
            }

            switch (SortOrder)
            {
                case "Code":
                    sType = sType.OrderBy(b => b.Code);
                    break;
                case "CodeDesc":
                    sType = sType.OrderByDescending(b => b.Code);
                    break;
                case "Description":
                    sType = sType.OrderBy(b => b.Description);
                    break;
                case "DescriptionDesc":
                    sType = sType.OrderByDescending(b => b.Description);
                    break;
                default:
                    sType = sType.OrderBy(b => b.Id);
                    break;
            }

            ViewBag.SortOrder = SortOrder;

            var pager = new Pager(sType.Count(), PageNumber);

            var viewModel = new STypeIndexPageViewModel
            {
                Items = sType.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
                Pager = pager
            };

            return View(viewModel);
        }

        // GET: STypes/Details/5
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sType = await _context.SType
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sType == null)
            {
                return NotFound();
            }

            return View(sType);
        }

        // GET: STypes/Create
        [Authorize(Roles = "Administrator, Moderator")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: STypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Create([Bind("Id,Code,DescriptionRU,DescriptionKK,DescriptionEN")] SType sType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(sType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(sType);
        }

        // GET: STypes/Edit/5
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sType = await _context.SType.FindAsync(id);
            if (sType == null)
            {
                return NotFound();
            }
            return View(sType);
        }

        // POST: STypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Code,DescriptionRU,DescriptionKK,DescriptionEN")] SType sType)
        {
            if (id != sType.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!STypeExists(sType.Id))
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
            return View(sType);
        }

        // GET: STypes/Delete/5
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sType = await _context.SType
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sType == null)
            {
                return NotFound();
            }

            return View(sType);
        }

        // POST: STypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var sType = await _context.SType.FindAsync(id);
            _context.SType.Remove(sType);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool STypeExists(int id)
        {
            return _context.SType.Any(e => e.Id == id);
        }
    }
}
