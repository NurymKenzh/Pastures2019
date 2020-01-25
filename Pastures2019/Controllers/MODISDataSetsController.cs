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
    public class MODISDataSetsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MODISDataSetsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: MODISDataSets
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Index(
            string SortOrder,
            string NameFilter,
            int? MODISProductIdFilter,
            int? PageNumber)
        {
            var mODISDataSet = _context.MODISDataSet
                .Include(m => m.MODISProduct)
                .Where(m => true);

            ViewBag.NameFilter = NameFilter;
            ViewBag.MODISProductIdFilter = MODISProductIdFilter;

            ViewBag.NameSort = SortOrder == "Name" ? "NameDesc" : "Name";
            ViewBag.MODISProductNameSort = SortOrder == "MODISProductName" ? "MODISProductNameDesc" : "MODISProductName";

            if (!string.IsNullOrEmpty(NameFilter))
            {
                mODISDataSet = mODISDataSet.Where(m => m.Name.Contains(NameFilter));
            }
            if (MODISProductIdFilter != null)
            {
                mODISDataSet = mODISDataSet.Where(m => m.MODISProduct.Id == MODISProductIdFilter);
            }

            switch (SortOrder)
            {
                case "Name":
                    mODISDataSet = mODISDataSet.OrderBy(m => m.Name);
                    break;
                case "NameDesc":
                    mODISDataSet = mODISDataSet.OrderByDescending(m => m.Name);
                    break;
                case "MODISProductName":
                    mODISDataSet = mODISDataSet.OrderBy(m => m.MODISProduct.Name);
                    break;
                case "MODISProductNameDesc":
                    mODISDataSet = mODISDataSet.OrderByDescending(m => m.MODISProduct.Name);
                    break;
                default:
                    mODISDataSet = mODISDataSet.OrderBy(m => m.Id);
                    break;
            }

            ViewBag.SortOrder = SortOrder;

            var pager = new Pager(mODISDataSet.Count(), PageNumber);

            var viewModel = new MODISDataSetIndexPageViewModel
            {
                Items = mODISDataSet.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
                Pager = pager
            };

            ViewData["MODISProductId"] = new SelectList(_context.MODISProduct.OrderBy(m => m.Name), "Id", "Name");

            return View(viewModel);
        }

        // GET: MODISDataSets/Details/5
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mODISDataSet = await _context.MODISDataSet
                .Include(m => m.MODISProduct)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (mODISDataSet == null)
            {
                return NotFound();
            }

            return View(mODISDataSet);
        }

        // GET: MODISDataSets/Create
        [Authorize(Roles = "Administrator, Moderator")]
        public IActionResult Create()
        {
            ViewData["MODISProductId"] = new SelectList(_context.MODISProduct.OrderBy(m => m.Name), "Id", "Name");
            return View();
        }

        // POST: MODISDataSets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Create([Bind("Id,MODISProductId,Name,Index")] MODISDataSet mODISDataSet)
        {
            if (ModelState.IsValid)
            {
                _context.Add(mODISDataSet);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MODISProductId"] = new SelectList(_context.MODISProduct.OrderBy(m => m.Name), "Id", "Name", mODISDataSet.MODISProductId);
            return View(mODISDataSet);
        }

        // GET: MODISDataSets/Edit/5
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mODISDataSet = await _context.MODISDataSet.FindAsync(id);
            if (mODISDataSet == null)
            {
                return NotFound();
            }
            ViewData["MODISProductId"] = new SelectList(_context.MODISProduct.OrderBy(m => m.Name), "Id", "Name", mODISDataSet.MODISProductId);
            return View(mODISDataSet);
        }

        // POST: MODISDataSets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,MODISProductId,Name,Index")] MODISDataSet mODISDataSet)
        {
            if (id != mODISDataSet.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(mODISDataSet);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MODISDataSetExists(mODISDataSet.Id))
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
            ViewData["MODISProductId"] = new SelectList(_context.MODISProduct.OrderBy(m => m.Name), "Id", "Name", mODISDataSet.MODISProductId);
            return View(mODISDataSet);
        }

        // GET: MODISDataSets/Delete/5
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mODISDataSet = await _context.MODISDataSet
                .Include(m => m.MODISProduct)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (mODISDataSet == null)
            {
                return NotFound();
            }

            return View(mODISDataSet);
        }

        // POST: MODISDataSets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var mODISDataSet = await _context.MODISDataSet.FindAsync(id);
            _context.MODISDataSet.Remove(mODISDataSet);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MODISDataSetExists(int id)
        {
            return _context.MODISDataSet.Any(e => e.Id == id);
        }
    }
}
