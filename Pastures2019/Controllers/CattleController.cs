using System;
using System.Collections.Generic;
using System.IO;
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
    public class CattleController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CattleController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Cattle
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Index(
            string SortOrder,
            int? CodeFilter,
            string BreedFilter,
            int? PageNumber)
        {
            var cattle = _context.Cattle
                .Where(c => true);

            ViewBag.CodeFilter = CodeFilter;
            ViewBag.BreedFilter = BreedFilter;

            ViewBag.CodeSort = SortOrder == "Code" ? "CodeDesc" : "Code";
            ViewBag.BreedSort = SortOrder == "Breed" ? "BreedDesc" : "Breed";

            if (CodeFilter != null)
            {
                cattle = cattle.Where(c => c.Code == CodeFilter);
            }
            if (!string.IsNullOrEmpty(BreedFilter))
            {
                cattle = cattle.Where(c => c.Breed.Contains(BreedFilter));
            }

            switch (SortOrder)
            {
                case "Code":
                    cattle = cattle.OrderBy(c => c.Code);
                    break;
                case "CodeDesc":
                    cattle = cattle.OrderByDescending(c => c.Code);
                    break;
                case "Breed":
                    cattle = cattle.OrderBy(c => c.Breed);
                    break;
                case "BreedDesc":
                    cattle = cattle.OrderByDescending(c => c.Breed);
                    break;
                default:
                    cattle = cattle.OrderBy(c => c.Id);
                    break;
            }

            ViewBag.SortOrder = SortOrder;

            var pager = new Pager(cattle.Count(), PageNumber);

            var viewModel = new CattleIndexPageViewModel
            {
                Items = cattle.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
                Pager = pager
            };

            return View(viewModel);
        }

        // GET: Cattle/Details/5
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cattle = await _context.Cattle
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cattle == null)
            {
                return NotFound();
            }

            return View(cattle);
        }

        // GET: Cattle/Create
        [Authorize(Roles = "Administrator, Moderator")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Cattle/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Create([Bind("Id,Code,BreedRU,BreedKK,BreedEN,DirectionRU,DirectionKK,DirectionEN,WeightRU,WeightKK,WeightEN,SlaughterYield,EwesYieldRU,EwesYieldKK,EwesYieldEN,TotalGoals,MilkFatContent,BredRU,BredKK,BredEN,RangeRU,RangeKK,RangeEN,FormFile,DescriptionRU,DescriptionKK,DescriptionEN")] Cattle cattle)
        {
            if (ModelState.IsValid)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await cattle.FormFile.CopyToAsync(memoryStream);
                    cattle.Photo = memoryStream.ToArray();
                }

                _context.Add(cattle);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(cattle);
        }

        // GET: Cattle/Edit/5
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cattle = await _context.Cattle.FindAsync(id);
            if (cattle == null)
            {
                return NotFound();
            }
            return View(cattle);
        }

        // POST: Cattle/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Code,BreedRU,BreedKK,BreedEN,DirectionRU,DirectionKK,DirectionEN,WeightRU,WeightKK,WeightEN,SlaughterYield,EwesYieldRU,EwesYieldKK,EwesYieldEN,TotalGoals,MilkFatContent,BredRU,BredKK,BredEN,RangeRU,RangeKK,RangeEN,FormFile,DescriptionRU,DescriptionKK,DescriptionEN")] Cattle cattle)
        {
            if (id != cattle.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (cattle.FormFile != null && cattle.FormFile.Length > 0)
                    {
                        cattle.Photo = null;
                        using (var memoryStream = new MemoryStream())
                        {
                            await cattle.FormFile.CopyToAsync(memoryStream);
                            cattle.Photo = memoryStream.ToArray();
                        }
                    }

                    _context.Update(cattle);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CattleExists(cattle.Id))
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
            return View(cattle);
        }

        // GET: Cattle/Delete/5
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cattle = await _context.Cattle
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cattle == null)
            {
                return NotFound();
            }

            return View(cattle);
        }

        // POST: Cattle/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cattle = await _context.Cattle.FindAsync(id);
            _context.Cattle.Remove(cattle);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CattleExists(int id)
        {
            return _context.Cattle.Any(e => e.Id == id);
        }
    }
}
