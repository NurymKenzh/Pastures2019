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
    public class MODISSourcesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MODISSourcesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: MODISSources
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Index(
            string SortOrder,
            string NameFilter,
            int? PageNumber)
        {
            var mODISSource = _context.MODISSource
                .Where(m => true);

            ViewBag.NameFilter = NameFilter;

            ViewBag.NameSort = SortOrder == "Name" ? "NameDesc" : "Name";

            if (!string.IsNullOrEmpty(NameFilter))
            {
                mODISSource = mODISSource.Where(m => m.Name.Contains(NameFilter));
            }

            switch (SortOrder)
            {
                case "Name":
                    mODISSource = mODISSource.OrderBy(m => m.Name);
                    break;
                case "NameDesc":
                    mODISSource = mODISSource.OrderByDescending(m => m.Name);
                    break;
                default:
                    mODISSource = mODISSource.OrderBy(m => m.Id);
                    break;
            }

            ViewBag.SortOrder = SortOrder;

            var pager = new Pager(mODISSource.Count(), PageNumber);

            var viewModel = new MODISSourceIndexPageViewModel
            {
                Items = mODISSource.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
                Pager = pager
            };

            return View(viewModel);
        }

        // GET: MODISSources/Details/5
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mODISSource = await _context.MODISSource
                .FirstOrDefaultAsync(m => m.Id == id);
            if (mODISSource == null)
            {
                return NotFound();
            }

            return View(mODISSource);
        }

        // GET: MODISSources/Create
        [Authorize(Roles = "Administrator, Moderator")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: MODISSources/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Create([Bind("Id,Name")] MODISSource mODISSource)
        {
            if (ModelState.IsValid)
            {
                _context.Add(mODISSource);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(mODISSource);
        }

        // GET: MODISSources/Edit/5
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mODISSource = await _context.MODISSource.FindAsync(id);
            if (mODISSource == null)
            {
                return NotFound();
            }
            return View(mODISSource);
        }

        // POST: MODISSources/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] MODISSource mODISSource)
        {
            if (id != mODISSource.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(mODISSource);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MODISSourceExists(mODISSource.Id))
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
            return View(mODISSource);
        }

        // GET: MODISSources/Delete/5
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mODISSource = await _context.MODISSource
                .FirstOrDefaultAsync(m => m.Id == id);
            if (mODISSource == null)
            {
                return NotFound();
            }

            return View(mODISSource);
        }

        // POST: MODISSources/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var mODISSource = await _context.MODISSource.FindAsync(id);
            _context.MODISSource.Remove(mODISSource);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MODISSourceExists(int id)
        {
            return _context.MODISSource.Any(e => e.Id == id);
        }
    }
}
