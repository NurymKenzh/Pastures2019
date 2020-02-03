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
    public class WSubTypesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public WSubTypesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: WSubTypes
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Index(
            string SortOrder,
            int? CodeFilter,
            string DescriptionFilter,
            int? PageNumber)
        {
            var wSubType = _context.WSubType
                .Where(b => true);

            ViewBag.CodeFilter = CodeFilter;
            ViewBag.DescriptionFilter = DescriptionFilter;

            ViewBag.CodeSort = SortOrder == "Code" ? "CodeDesc" : "Code";
            ViewBag.DescriptionSort = SortOrder == "Description" ? "DescriptionDesc" : "Description";

            if (CodeFilter != null)
            {
                wSubType = wSubType.Where(b => b.Code == CodeFilter);
            }
            if (!string.IsNullOrEmpty(DescriptionFilter))
            {
                wSubType = wSubType.Where(b => b.Description.Contains(DescriptionFilter));
            }

            switch (SortOrder)
            {
                case "Code":
                    wSubType = wSubType.OrderBy(b => b.Code);
                    break;
                case "CodeDesc":
                    wSubType = wSubType.OrderByDescending(b => b.Code);
                    break;
                case "Description":
                    wSubType = wSubType.OrderBy(b => b.Description);
                    break;
                case "DescriptionDesc":
                    wSubType = wSubType.OrderByDescending(b => b.Description);
                    break;
                default:
                    wSubType = wSubType.OrderBy(b => b.Id);
                    break;
            }

            ViewBag.SortOrder = SortOrder;

            var pager = new Pager(wSubType.Count(), PageNumber);

            var viewModel = new WSubTypeIndexPageViewModel
            {
                Items = wSubType.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
                Pager = pager
            };

            return View(viewModel);
        }

        // GET: WSubTypes/Details/5
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var wSubType = await _context.WSubType
                .FirstOrDefaultAsync(m => m.Id == id);
            if (wSubType == null)
            {
                return NotFound();
            }

            return View(wSubType);
        }

        // GET: WSubTypes/Create
        [Authorize(Roles = "Administrator, Moderator")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: WSubTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Create([Bind("Id,Code,DescriptionRU,DescriptionKK,DescriptionEN")] WSubType wSubType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(wSubType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(wSubType);
        }

        // GET: WSubTypes/Edit/5
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var wSubType = await _context.WSubType.FindAsync(id);
            if (wSubType == null)
            {
                return NotFound();
            }
            return View(wSubType);
        }

        // POST: WSubTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Code,DescriptionRU,DescriptionKK,DescriptionEN")] WSubType wSubType)
        {
            if (id != wSubType.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(wSubType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WSubTypeExists(wSubType.Id))
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
            return View(wSubType);
        }

        // GET: WSubTypes/Delete/5
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var wSubType = await _context.WSubType
                .FirstOrDefaultAsync(m => m.Id == id);
            if (wSubType == null)
            {
                return NotFound();
            }

            return View(wSubType);
        }

        // POST: WSubTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var wSubType = await _context.WSubType.FindAsync(id);
            _context.WSubType.Remove(wSubType);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WSubTypeExists(int id)
        {
            return _context.WSubType.Any(e => e.Id == id);
        }
    }
}
