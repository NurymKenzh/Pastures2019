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
    public class MODISProductsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MODISProductsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: MODISProducts
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Index(
            string SortOrder,
            string NameFilter,
            int? MODISSourceIdFilter,
            int? PageNumber)
        {
            var mODISProduct = _context.MODISProduct
                .Include(m => m.MODISSource)
                .Where(m => true);

            ViewBag.NameFilter = NameFilter;
            ViewBag.MODISSourceIdFilter = MODISSourceIdFilter;

            ViewBag.NameSort = SortOrder == "Name" ? "NameDesc" : "Name";
            ViewBag.MODISSourceNameSort = SortOrder == "MODISSourceName" ? "MODISSourceNameDesc" : "MODISSourceName";

            if (!string.IsNullOrEmpty(NameFilter))
            {
                mODISProduct = mODISProduct.Where(m => m.Name.Contains(NameFilter));
            }
            if (MODISSourceIdFilter!=null)
            {
                mODISProduct = mODISProduct.Where(m => m.MODISSource.Id == MODISSourceIdFilter);
            }

            switch (SortOrder)
            {
                case "Name":
                    mODISProduct = mODISProduct.OrderBy(m => m.Name);
                    break;
                case "NameDesc":
                    mODISProduct = mODISProduct.OrderByDescending(m => m.Name);
                    break;
                case "MODISSourceName":
                    mODISProduct = mODISProduct.OrderBy(m => m.MODISSource.Name);
                    break;
                case "MODISSourceNameDesc":
                    mODISProduct = mODISProduct.OrderByDescending(m => m.MODISSource.Name);
                    break;
                default:
                    mODISProduct = mODISProduct.OrderBy(m => m.Id);
                    break;
            }

            ViewBag.SortOrder = SortOrder;

            var pager = new Pager(mODISProduct.Count(), PageNumber);

            var viewModel = new MODISProductIndexPageViewModel
            {
                Items = mODISProduct.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
                Pager = pager
            };

            ViewData["MODISSourceId"] = new SelectList(_context.MODISSource.OrderBy(m => m.Name), "Id", "Name");

            return View(viewModel);
        }

        // GET: MODISProducts/Details/5
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mODISProduct = await _context.MODISProduct
                .Include(m => m.MODISSource)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (mODISProduct == null)
            {
                return NotFound();
            }

            return View(mODISProduct);
        }

        // GET: MODISProducts/Create
        [Authorize(Roles = "Administrator, Moderator")]
        public IActionResult Create()
        {
            ViewData["MODISSourceId"] = new SelectList(_context.MODISSource.OrderBy(m => m.Name), "Id", "Name");
            return View();
        }

        // POST: MODISProducts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Create([Bind("Id,MODISSourceId,Name")] MODISProduct mODISProduct)
        {
            if (ModelState.IsValid)
            {
                _context.Add(mODISProduct);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MODISSourceId"] = new SelectList(_context.MODISSource.OrderBy(m => m.Name), "Id", "Name", mODISProduct.MODISSourceId);
            return View(mODISProduct);
        }

        // GET: MODISProducts/Edit/5
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mODISProduct = await _context.MODISProduct.FindAsync(id);
            if (mODISProduct == null)
            {
                return NotFound();
            }
            ViewData["MODISSourceId"] = new SelectList(_context.MODISSource.OrderBy(m => m.Name), "Id", "Name", mODISProduct.MODISSourceId);
            return View(mODISProduct);
        }

        // POST: MODISProducts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,MODISSourceId,Name")] MODISProduct mODISProduct)
        {
            if (id != mODISProduct.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(mODISProduct);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MODISProductExists(mODISProduct.Id))
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
            ViewData["MODISSourceId"] = new SelectList(_context.MODISSource.OrderBy(m => m.Name), "Id", "Name", mODISProduct.MODISSourceId);
            return View(mODISProduct);
        }

        // GET: MODISProducts/Delete/5
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mODISProduct = await _context.MODISProduct
                .Include(m => m.MODISSource)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (mODISProduct == null)
            {
                return NotFound();
            }

            return View(mODISProduct);
        }

        // POST: MODISProducts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var mODISProduct = await _context.MODISProduct.FindAsync(id);
            _context.MODISProduct.Remove(mODISProduct);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MODISProductExists(int id)
        {
            return _context.MODISProduct.Any(e => e.Id == id);
        }
    }
}
