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
    public class HorsesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HorsesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Horses
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Index(
            string SortOrder,
            int? CodeFilter,
            string BreedFilter,
            int? PageNumber)
        {
            var horse = _context.Horse
                .Where(h => true);

            ViewBag.CodeFilter = CodeFilter;
            ViewBag.BreedFilter = BreedFilter;

            ViewBag.CodeSort = SortOrder == "Code" ? "CodeDesc" : "Code";
            ViewBag.BreedSort = SortOrder == "Breed" ? "BreedDesc" : "Breed";

            if (CodeFilter != null)
            {
                horse = horse.Where(h => h.Code == CodeFilter);
            }
            if (!string.IsNullOrEmpty(BreedFilter))
            {
                horse = horse.Where(h => h.Breed.Contains(BreedFilter));
            }

            switch (SortOrder)
            {
                case "Code":
                    horse = horse.OrderBy(h => h.Code);
                    break;
                case "CodeDesc":
                    horse = horse.OrderByDescending(h => h.Code);
                    break;
                case "Breed":
                    horse = horse.OrderBy(h => h.Breed);
                    break;
                case "BreedDesc":
                    horse = horse.OrderByDescending(h => h.Breed);
                    break;
                default:
                    horse = horse.OrderBy(h => h.Id);
                    break;
            }

            ViewBag.SortOrder = SortOrder;

            var pager = new Pager(horse.Count(), PageNumber);

            var viewModel = new HorseIndexPageViewModel
            {
                Items = horse.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
                Pager = pager
            };

            return View(viewModel);
        }

        // GET: Horses/Details/5
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var horse = await _context.Horse
                .FirstOrDefaultAsync(m => m.Id == id);
            if (horse == null)
            {
                return NotFound();
            }

            return View(horse);
        }

        // GET: Horses/Create
        [Authorize(Roles = "Administrator, Moderator")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Horses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Create([Bind("Id,Code,BreedRU,BreedKK,BreedEN,DirectionRU,DirectionKK,DirectionEN,WeightRU,WeightKK,WeightEN,HeightRU,HeightKK,HeightEN,MilkYieldRU,MilkYieldKK,MilkYieldEN,BodyLengthRU,BodyLengthKK,BodyLengthEN,BustRU,BustKK,BustEN,MetacarpusRU,MetacarpusKK,MetacarpusEN,TotalGoals,BredRU,BredKK,BredEN,RangeRU,RangeKK,RangeEN,FormFile,DescriptionRU,DescriptionKK,DescriptionEN")] Horse horse)
        {
            if (ModelState.IsValid)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await horse.FormFile.CopyToAsync(memoryStream);
                    horse.Photo = memoryStream.ToArray();
                }

                _context.Add(horse);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(horse);
        }

        // GET: Horses/Edit/5
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var horse = await _context.Horse.FindAsync(id);
            if (horse == null)
            {
                return NotFound();
            }
            return View(horse);
        }

        // POST: Horses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Code,BreedRU,BreedKK,BreedEN,DirectionRU,DirectionKK,DirectionEN,WeightRU,WeightKK,WeightEN,HeightRU,HeightKK,HeightEN,MilkYieldRU,MilkYieldKK,MilkYieldEN,BodyLengthRU,BodyLengthKK,BodyLengthEN,BustRU,BustKK,BustEN,MetacarpusRU,MetacarpusKK,MetacarpusEN,TotalGoals,BredRU,BredKK,BredEN,RangeRU,RangeKK,RangeEN,FormFile,DescriptionRU,DescriptionKK,DescriptionEN")] Horse horse)
        {
            if (id != horse.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (horse.FormFile != null && horse.FormFile.Length > 0)
                    {
                        horse.Photo = null;
                        using (var memoryStream = new MemoryStream())
                        {
                            await horse.FormFile.CopyToAsync(memoryStream);
                            horse.Photo = memoryStream.ToArray();
                        }
                    }

                    _context.Update(horse);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HorseExists(horse.Id))
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
            return View(horse);
        }

        // GET: Horses/Delete/5
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var horse = await _context.Horse
                .FirstOrDefaultAsync(m => m.Id == id);
            if (horse == null)
            {
                return NotFound();
            }

            return View(horse);
        }

        // POST: Horses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var horse = await _context.Horse.FindAsync(id);
            _context.Horse.Remove(horse);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HorseExists(int id)
        {
            return _context.Horse.Any(e => e.Id == id);
        }
    }
}
