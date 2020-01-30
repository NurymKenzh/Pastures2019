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
    public class SmallCattlesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SmallCattlesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: SmallCattles
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Index(
            string SortOrder,
            int? CodeFilter,
            string BreedFilter,
            int? PageNumber)
        {
            var smallCattles = _context.SmallCattle
                .Where(s => true);

            ViewBag.CodeFilter = CodeFilter;
            ViewBag.BreedFilter = BreedFilter;

            ViewBag.CodeSort = SortOrder == "Code" ? "CodeDesc" : "Code";
            ViewBag.BreedSort = SortOrder == "Breed" ? "BreedDesc" : "Breed";

            if (CodeFilter != null)
            {
                smallCattles = smallCattles.Where(s => s.Code == CodeFilter);
            }
            if (!string.IsNullOrEmpty(BreedFilter))
            {
                smallCattles = smallCattles.Where(s => s.Breed.Contains(BreedFilter));
            }

            switch (SortOrder)
            {
                case "Code":
                    smallCattles = smallCattles.OrderBy(s => s.Code);
                    break;
                case "CodeDesc":
                    smallCattles = smallCattles.OrderByDescending(s => s.Code);
                    break;
                case "Breed":
                    smallCattles = smallCattles.OrderBy(s => s.Breed);
                    break;
                case "BreedDesc":
                    smallCattles = smallCattles.OrderByDescending(s => s.Breed);
                    break;
                default:
                    smallCattles = smallCattles.OrderBy(s => s.Id);
                    break;
            }

            ViewBag.SortOrder = SortOrder;

            var pager = new Pager(smallCattles.Count(), PageNumber);

            var viewModel = new SmallCattleIndexPageViewModel
            {
                Items = smallCattles.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
                Pager = pager
            };

            return View(viewModel);
        }

        // GET: SmallCattles/Details/5
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var smallCattle = await _context.SmallCattle
                .FirstOrDefaultAsync(m => m.Id == id);
            if (smallCattle == null)
            {
                return NotFound();
            }

            return View(smallCattle);
        }

        // GET: SmallCattles/Create
        [Authorize(Roles = "Administrator, Moderator")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: SmallCattles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Create([Bind("Id,Code,BreedRU,BreedKK,BreedEN,DirectionRU,DirectionKK,DirectionEN,WeightRU,WeightKK,WeightEN,ShearingsRU,ShearingsKK,ShearingsEN,WashedWoolYieldRU,WashedWoolYieldKK,WashedWoolYieldEN,FertilityRU,FertilityKK,FertilityEN,WoolLengthRU,WoolLengthKK,WoolLengthEN,TotalGoals,BredRU,BredKK,BredEN,RangeRU,RangeKK,RangeEN,FormFile,DescriptionRU,DescriptionKK,DescriptionEN")] SmallCattle smallCattle)
        {
            if (ModelState.IsValid)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await smallCattle.FormFile.CopyToAsync(memoryStream);
                    smallCattle.Photo = memoryStream.ToArray();
                }

                _context.Add(smallCattle);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(smallCattle);
        }

        // GET: SmallCattles/Edit/5
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var smallCattle = await _context.SmallCattle.FindAsync(id);
            if (smallCattle == null)
            {
                return NotFound();
            }
            return View(smallCattle);
        }

        // POST: SmallCattles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Code,BreedRU,BreedKK,BreedEN,DirectionRU,DirectionKK,DirectionEN,WeightRU,WeightKK,WeightEN,ShearingsRU,ShearingsKK,ShearingsEN,WashedWoolYieldRU,WashedWoolYieldKK,WashedWoolYieldEN,FertilityRU,FertilityKK,FertilityEN,WoolLengthRU,WoolLengthKK,WoolLengthEN,TotalGoals,BredRU,BredKK,BredEN,RangeRU,RangeKK,RangeEN,FormFile,DescriptionRU,DescriptionKK,DescriptionEN")] SmallCattle smallCattle)
        {
            if (id != smallCattle.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (smallCattle.FormFile != null && smallCattle.FormFile.Length > 0)
                    {
                        smallCattle.Photo = null;
                        using (var memoryStream = new MemoryStream())
                        {
                            await smallCattle.FormFile.CopyToAsync(memoryStream);
                            smallCattle.Photo = memoryStream.ToArray();
                        }
                    }

                    _context.Update(smallCattle);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SmallCattleExists(smallCattle.Id))
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
            return View(smallCattle);
        }

        // GET: SmallCattles/Delete/5
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var smallCattle = await _context.SmallCattle
                .FirstOrDefaultAsync(m => m.Id == id);
            if (smallCattle == null)
            {
                return NotFound();
            }

            return View(smallCattle);
        }

        // POST: SmallCattles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var smallCattle = await _context.SmallCattle.FindAsync(id);
            _context.SmallCattle.Remove(smallCattle);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SmallCattleExists(int id)
        {
            return _context.SmallCattle.Any(e => e.Id == id);
        }
    }
}
