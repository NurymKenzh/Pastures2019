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
    public class PSubTypesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PSubTypesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: PSubTypes
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Index(
            string SortOrder,
            int? CodeFilter,
            string DescriptionFilter,
            int? PageNumber)
        {
            var pSubType = _context.PSubType
                .Where(b => true);

            ViewBag.CodeFilter = CodeFilter;
            ViewBag.DescriptionFilter = DescriptionFilter;

            ViewBag.CodeSort = SortOrder == "Code" ? "CodeDesc" : "Code";
            ViewBag.DescriptionSort = SortOrder == "Description" ? "DescriptionDesc" : "Description";

            if (CodeFilter != null)
            {
                pSubType = pSubType.Where(b => b.Code == CodeFilter);
            }
            if (!string.IsNullOrEmpty(DescriptionFilter))
            {
                pSubType = pSubType.Where(b => b.Description.Contains(DescriptionFilter));
            }

            switch (SortOrder)
            {
                case "Code":
                    pSubType = pSubType.OrderBy(b => b.Code);
                    break;
                case "CodeDesc":
                    pSubType = pSubType.OrderByDescending(b => b.Code);
                    break;
                case "Description":
                    pSubType = pSubType.OrderBy(b => b.Description);
                    break;
                case "DescriptionDesc":
                    pSubType = pSubType.OrderByDescending(b => b.Description);
                    break;
                default:
                    pSubType = pSubType.OrderBy(b => b.Id);
                    break;
            }

            ViewBag.SortOrder = SortOrder;

            var pager = new Pager(pSubType.Count(), PageNumber);

            var viewModel = new PSubTypeIndexPageViewModel
            {
                Items = pSubType.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
                Pager = pager
            };

            return View(viewModel);
        }

        // GET: PSubTypes/Details/5
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pSubType = await _context.PSubType
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pSubType == null)
            {
                return NotFound();
            }

            return View(pSubType);
        }

        // GET: PSubTypes/Create
        [Authorize(Roles = "Administrator, Moderator")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: PSubTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Create([Bind("Id,Code,DescriptionRU,DescriptionKK,DescriptionEN")] PSubType pSubType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(pSubType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(pSubType);
        }

        // GET: PSubTypes/Edit/5
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pSubType = await _context.PSubType.FindAsync(id);
            if (pSubType == null)
            {
                return NotFound();
            }
            return View(pSubType);
        }

        // POST: PSubTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Code,DescriptionRU,DescriptionKK,DescriptionEN")] PSubType pSubType)
        {
            if (id != pSubType.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pSubType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PSubTypeExists(pSubType.Id))
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
            return View(pSubType);
        }

        // GET: PSubTypes/Delete/5
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pSubType = await _context.PSubType
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pSubType == null)
            {
                return NotFound();
            }

            return View(pSubType);
        }

        // POST: PSubTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pSubType = await _context.PSubType.FindAsync(id);
            _context.PSubType.Remove(pSubType);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PSubTypeExists(int id)
        {
            return _context.PSubType.Any(e => e.Id == id);
        }
    }
}
