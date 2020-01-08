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
    public class BClassesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BClassesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: BClasses
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Index(
            string SortOrder,
            int? CodeFilter,
            string DescriptionFilter,
            int? Page)
        {
            var bclass = _context.BClass
                .Where(b => true);

            ViewBag.CodeFilter = CodeFilter;
            ViewBag.DescriptionFilter = DescriptionFilter;

            ViewBag.CodeSort = SortOrder == "Code" ? "CodeDesc" : "Code";
            ViewBag.DescriptionSort = SortOrder == "Description" ? "DescriptionDesc" : "Description";

            if (CodeFilter != null)
            {
                bclass = bclass.Where(b => b.Code == CodeFilter);
            }
            if (!string.IsNullOrEmpty(DescriptionFilter))
            {
                bclass = bclass.Where(b => b.Description.Contains(DescriptionFilter));
            }

            switch (SortOrder)
            {
                case "Code":
                    bclass = bclass.OrderBy(b => b.Code);
                    break;
                case "CodeDesc":
                    bclass = bclass.OrderByDescending(b => b.Code);
                    break;
                case "Description":
                    bclass = bclass.OrderBy(b => b.Description);
                    break;
                case "DescriptionDesc":
                    bclass = bclass.OrderByDescending(b => b.Description);
                    break;
                default:
                    bclass = bclass.OrderBy(b => b.Id);
                    break;
            }

            ViewBag.SortOrder = SortOrder;

            var pager = new Pager(bclass.Count(), Page);

            var viewModel = new BClassIndexPageViewModel
            {
                Items = bclass.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
                Pager = pager
            };

            return View(viewModel);
        }

        // GET: BClasses/Details/5
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bClass = await _context.BClass
                .FirstOrDefaultAsync(m => m.Id == id);
            if (bClass == null)
            {
                return NotFound();
            }

            return View(bClass);
        }

        // GET: BClasses/Create
        [Authorize(Roles = "Administrator, Moderator")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: BClasses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Create([Bind("Id,Code,DescriptionRU,DescriptionKK,DescriptionEN")] BClass bClass)
        {
            if (ModelState.IsValid)
            {
                _context.Add(bClass);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(bClass);
        }

        // GET: BClasses/Edit/5
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bClass = await _context.BClass.FindAsync(id);
            if (bClass == null)
            {
                return NotFound();
            }
            return View(bClass);
        }

        // POST: BClasses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Code,DescriptionRU,DescriptionKK,DescriptionEN")] BClass bClass)
        {
            if (id != bClass.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(bClass);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BClassExists(bClass.Id))
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
            return View(bClass);
        }

        // GET: BClasses/Delete/5
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bClass = await _context.BClass
                .FirstOrDefaultAsync(m => m.Id == id);
            if (bClass == null)
            {
                return NotFound();
            }

            return View(bClass);
        }

        // POST: BClasses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var bClass = await _context.BClass.FindAsync(id);
            _context.BClass.Remove(bClass);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BClassExists(int id)
        {
            return _context.BClass.Any(e => e.Id == id);
        }
    }
}
