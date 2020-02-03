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
    public class CATOesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CATOesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: CATOes
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Index(
            string SortOrder,
            string NameFilter,
            string ABFilter,
            string CDFilter,
            string EFFilter,
            string HIJFilter,
            int? PageNumber)
        {
            var cATO = _context.CATO
                .Where(b => true);

            ViewBag.NameFilter = NameFilter;
            ViewBag.ABFilter = ABFilter;
            ViewBag.CDFilter = CDFilter;
            ViewBag.EFFilter = EFFilter;
            ViewBag.HIJFilter = HIJFilter;

            ViewBag.NameSort = SortOrder == "Name" ? "NameDesc" : "Name";

            if (!string.IsNullOrEmpty(NameFilter))
            {
                cATO = cATO.Where(b => b.Name.ToLower().Contains(NameFilter.ToLower()));
            }
            if (!string.IsNullOrEmpty(ABFilter))
            {
                cATO = cATO.Where(b => b.AB.ToLower().Contains(ABFilter.ToLower()));
            }
            if (!string.IsNullOrEmpty(CDFilter))
            {
                cATO = cATO.Where(b => b.CD.ToLower().Contains(CDFilter.ToLower()));
            }
            if (!string.IsNullOrEmpty(EFFilter))
            {
                cATO = cATO.Where(b => b.EF.ToLower().Contains(EFFilter.ToLower()));
            }
            if (!string.IsNullOrEmpty(HIJFilter))
            {
                cATO = cATO.Where(b => b.HIJ.ToLower().Contains(HIJFilter.ToLower()));
            }

            switch (SortOrder)
            {
                case "Name":
                    cATO = cATO.OrderBy(b => b.Name);
                    break;
                case "NameDesc":
                    cATO = cATO.OrderByDescending(b => b.Name);
                    break;
                default:
                    cATO = cATO.OrderBy(b => b.Id);
                    break;
            }

            ViewBag.SortOrder = SortOrder;

            var pager = new Pager(cATO.Count(), PageNumber);

            var viewModel = new CATOIndexPageViewModel
            {
                Items = cATO.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
                Pager = pager
            };

            return View(viewModel);
        }

        // GET: CATOes/Details/5
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cATO = await _context.CATO
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cATO == null)
            {
                return NotFound();
            }

            return View(cATO);
        }

        // GET: CATOes/Create
        [Authorize(Roles = "Administrator, Moderator")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: CATOes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Create([Bind("Id,AB,CD,EF,HIJ,K,NameRU,NameKK")] CATO cATO)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cATO);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(cATO);
        }

        // GET: CATOes/Edit/5
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cATO = await _context.CATO.FindAsync(id);
            if (cATO == null)
            {
                return NotFound();
            }
            return View(cATO);
        }

        // POST: CATOes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,AB,CD,EF,HIJ,K,NameRU,NameKK")] CATO cATO)
        {
            if (id != cATO.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cATO);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CATOExists(cATO.Id))
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
            return View(cATO);
        }

        // GET: CATOes/Delete/5
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cATO = await _context.CATO
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cATO == null)
            {
                return NotFound();
            }

            return View(cATO);
        }

        // POST: CATOes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cATO = await _context.CATO.FindAsync(id);
            _context.CATO.Remove(cATO);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CATOExists(int id)
        {
            return _context.CATO.Any(e => e.Id == id);
        }
    }
}
