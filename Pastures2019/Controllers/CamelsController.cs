using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Pastures2019.Data;
using Pastures2019.Models;

namespace Pastures2019.Controllers
{
    public class CamelsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CamelsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Camels
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Index(
            string SortOrder,
            int? CodeFilter,
            string RangeFilter,
            int? PageNumber)
        {
            var camel = _context.Camel
                .Where(c => true);

            ViewBag.CodeFilter = CodeFilter;
            ViewBag.RangeFilter = RangeFilter;

            ViewBag.CodeSort = SortOrder == "Code" ? "CodeDesc" : "Code";
            ViewBag.RangeSort = SortOrder == "Range" ? "RangeDesc" : "Range";

            if (CodeFilter != null)
            {
                camel = camel.Where(c => c.Code == CodeFilter);
            }
            if (!string.IsNullOrEmpty(RangeFilter))
            {
                camel = camel.Where(c => c.Range.Contains(RangeFilter));
            }

            switch (SortOrder)
            {
                case "Code":
                    camel = camel.OrderBy(c => c.Code);
                    break;
                case "CodeDesc":
                    camel = camel.OrderByDescending(c => c.Code);
                    break;
                case "Range":
                    camel = camel.OrderBy(c => c.Range);
                    break;
                case "RangeDesc":
                    camel = camel.OrderByDescending(c => c.Range);
                    break;
                default:
                    camel = camel.OrderBy(c => c.Id);
                    break;
            }

            ViewBag.SortOrder = SortOrder;

            var pager = new Pager(camel.Count(), PageNumber);

            var viewModel = new CamelIndexPageViewModel
            {
                Items = camel.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
                Pager = pager
            };

            return View(viewModel);
        }

        // GET: Camels/Details/5
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var camel = await _context.Camel
                .FirstOrDefaultAsync(m => m.Id == id);
            if (camel == null)
            {
                return NotFound();
            }

            return View(camel);
        }

        // GET: Camels/Create
        [Authorize(Roles = "Administrator, Moderator")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Camels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Code,BreedRU,BreedKK,BreedEN,WeightRU,WeightKK,WeightEN,SlaughterYield,EwesYieldRU,EwesYieldKK,EwesYieldEN,TotalGoals,MilkFatContent,RangeRU,RangeKK,RangeEN,FormFile,DescriptionRU,DescriptionKK,DescriptionEN")] Camel camel)
        {
            if (ModelState.IsValid)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await camel.FormFile.CopyToAsync(memoryStream);
                    camel.Photo = memoryStream.ToArray();
                }

                _context.Add(camel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(camel);
        }

        // GET: Camels/Edit/5
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var camel = await _context.Camel.FindAsync(id);
            if (camel == null)
            {
                return NotFound();
            }
            return View(camel);
        }

        // POST: Camels/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Code,BreedRU,BreedKK,BreedEN,WeightRU,WeightKK,WeightEN,SlaughterYield,EwesYieldRU,EwesYieldKK,EwesYieldEN,TotalGoals,MilkFatContent,RangeRU,RangeKK,RangeEN,FormFile,DescriptionRU,DescriptionKK,DescriptionEN")] Camel camel)
        {
            if (id != camel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (camel.FormFile != null && camel.FormFile.Length > 0)
                    {
                        camel.Photo = null;
                        using (var memoryStream = new MemoryStream())
                        {
                            await camel.FormFile.CopyToAsync(memoryStream);
                            camel.Photo = memoryStream.ToArray();
                        }
                    }

                    _context.Update(camel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CamelExists(camel.Id))
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
            return View(camel);
        }

        // GET: Camels/Delete/5
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var camel = await _context.Camel
                .FirstOrDefaultAsync(m => m.Id == id);
            if (camel == null)
            {
                return NotFound();
            }

            return View(camel);
        }

        // POST: Camels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var camel = await _context.Camel.FindAsync(id);
            _context.Camel.Remove(camel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CamelExists(int id)
        {
            return _context.Camel.Any(e => e.Id == id);
        }
    }
}
