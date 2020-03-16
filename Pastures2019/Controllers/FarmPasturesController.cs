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
    public class FarmPasturesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FarmPasturesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: FarmPastures
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Index(
            string SortOrder,
            int? tidFilter,
            int? PageNumber)
        {
            var farmPastures = _context.FarmPasture
                .Where(f => true);

            ViewBag.tidFilter = tidFilter;

            ViewBag.tidSort = SortOrder == "tid" ? "tidDesc" : "tid";

            if (tidFilter != null)
            {
                farmPastures = farmPastures.Where(f => f.tid == tidFilter);
            }

            switch (SortOrder)
            {
                case "tid":
                    farmPastures = farmPastures.OrderBy(b => b.tid);
                    break;
                case "tidDesc":
                    farmPastures = farmPastures.OrderByDescending(b => b.tid);
                    break;
                default:
                    farmPastures = farmPastures.OrderBy(b => b.Id);
                    break;
            }

            ViewBag.SortOrder = SortOrder;

            var pager = new Pager(farmPastures.Count(), PageNumber);

            var viewModel = new FarmPastureIndexPageViewModel
            {
                Items = farmPastures.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
                Pager = pager
            };

            return View(viewModel);
        }

        // GET: FarmPastures/Details/5
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var farmPasture = await _context.FarmPasture
                .FirstOrDefaultAsync(m => m.Id == id);
            if (farmPasture == null)
            {
                return NotFound();
            }

            return View(farmPasture);
        }

        // GET: FarmPastures/Create
        [Authorize(Roles = "Administrator, Moderator")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: FarmPastures/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Create([Bind("Id,tid,CATOTE,Farm,NaturalArea,PType,Relief,ThePresenceOfEconomicallySignificantContours,LandAreaAccordingToLandUseAct,ProjectiveCoverageFrom,ProjectiveCoverageTo,AveragePastureProductivitySpring,AveragePastureProductivitySummer,AveragePastureProductivityAutumn,TypeOfGrazedAnimalsBreed,TheNumberOfGrazedAnimalsGoals,NumberOfGrazingDaysSpring,NumberOfGrazingDaysSummer,NumberOfGrazingDaysFall,FloodingEatSourcesWells,TheNeedForPastureFeedSpring,TheNeedForPastureFeedSummer,TheNeedForPastureFeedAutumn,FeedStockOfUsedPasturesSpring,FeedStockOfUsedPasturesSummer,FeedStockOfUsedPasturesAutumn,TheCostPer1HeadForTheBillingPeriodSpring,TheCostPer1HeadForTheBillingPeriodSummer,TheCostPer1HeadForTheBillingPeriodFall,LoadSpring,LoadSummer,LoadFall,ShortageSurplusOfPastureFeedSpring,ShortageSurplusOfPastureFeedSummer,ShortageSurplusOfPastureFeedAutumn,RequiredAdditionalAreaIfNecessaryPasturesSpring,RequiredAdditionalAreaIfNecessaryPasturesSummer,RequiredAdditionalAreaIfNecessaryPasturesAutumn,ThePresenceOfDegradedSitesIfAvailable,RecommendationsForImprovingThisDegradedArea")] FarmPasture farmPasture)
        {
            if (ModelState.IsValid)
            {
                _context.Add(farmPasture);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(farmPasture);
        }

        // GET: FarmPastures/Edit/5
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var farmPasture = await _context.FarmPasture.FindAsync(id);
            if (farmPasture == null)
            {
                return NotFound();
            }
            return View(farmPasture);
        }

        // POST: FarmPastures/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,tid,CATOTE,Farm,NaturalArea,PType,Relief,ThePresenceOfEconomicallySignificantContours,LandAreaAccordingToLandUseAct,ProjectiveCoverageFrom,ProjectiveCoverageTo,AveragePastureProductivitySpring,AveragePastureProductivitySummer,AveragePastureProductivityAutumn,TypeOfGrazedAnimalsBreed,TheNumberOfGrazedAnimalsGoals,NumberOfGrazingDaysSpring,NumberOfGrazingDaysSummer,NumberOfGrazingDaysFall,FloodingEatSourcesWells,TheNeedForPastureFeedSpring,TheNeedForPastureFeedSummer,TheNeedForPastureFeedAutumn,FeedStockOfUsedPasturesSpring,FeedStockOfUsedPasturesSummer,FeedStockOfUsedPasturesAutumn,TheCostPer1HeadForTheBillingPeriodSpring,TheCostPer1HeadForTheBillingPeriodSummer,TheCostPer1HeadForTheBillingPeriodFall,LoadSpring,LoadSummer,LoadFall,ShortageSurplusOfPastureFeedSpring,ShortageSurplusOfPastureFeedSummer,ShortageSurplusOfPastureFeedAutumn,RequiredAdditionalAreaIfNecessaryPasturesSpring,RequiredAdditionalAreaIfNecessaryPasturesSummer,RequiredAdditionalAreaIfNecessaryPasturesAutumn,ThePresenceOfDegradedSitesIfAvailable,RecommendationsForImprovingThisDegradedArea")] FarmPasture farmPasture)
        {
            if (id != farmPasture.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(farmPasture);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FarmPastureExists(farmPasture.Id))
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
            return View(farmPasture);
        }

        // GET: FarmPastures/Delete/5
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var farmPasture = await _context.FarmPasture
                .FirstOrDefaultAsync(m => m.Id == id);
            if (farmPasture == null)
            {
                return NotFound();
            }

            return View(farmPasture);
        }

        // POST: FarmPastures/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var farmPasture = await _context.FarmPasture.FindAsync(id);
            _context.FarmPasture.Remove(farmPasture);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FarmPastureExists(int id)
        {
            return _context.FarmPasture.Any(e => e.Id == id);
        }
    }
}
