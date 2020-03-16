using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Pastures2019.Models
{
    public class FarmPasture
    {
        public int Id { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "tid")]
        public int tid { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "CATO")]
        public string CATOTE { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "Farm")]
        public string Farm { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "NaturalArea")]
        public string NaturalArea { get; set; }
        
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "PType")]
        public string PType { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "Relief")]
        public string Relief { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "ThePresenceOfEconomicallySignificantContours")]
        public string ThePresenceOfEconomicallySignificantContours { get; set; }
        
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "LandAreaAccordingToLandUseAct")]
        public string LandAreaAccordingToLandUseAct { get; set; }        

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "ProjectiveCoverageFrom")]
        public int? ProjectiveCoverageFrom { get; set; }        

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "ProjectiveCoverageTo")]
        public int? ProjectiveCoverageTo { get; set; }        

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "AveragePastureProductivitySpring")]
        public decimal? AveragePastureProductivitySpring { get; set; }
        
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "AveragePastureProductivitySummer")]
        public decimal? AveragePastureProductivitySummer { get; set; }
        
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "AveragePastureProductivityAutumn")]
        public decimal? AveragePastureProductivityAutumn { get; set; }
        
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "TypeOfGrazedAnimalsBreed")]
        public string TypeOfGrazedAnimalsBreed { get; set; }
        
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "TheNumberOfGrazedAnimalsGoals")]
        public string TheNumberOfGrazedAnimalsGoals { get; set; }
        
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "NumberOfGrazingDaysSpring")]
        public int? NumberOfGrazingDaysSpring { get; set; }
        
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "NumberOfGrazingDaysSummer")]
        public int? NumberOfGrazingDaysSummer { get; set; }
        
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "NumberOfGrazingDaysFall")]
        public int? NumberOfGrazingDaysFall { get; set; }
        
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "FloodingEatSourcesWells")]
        public string FloodingEatSourcesWells { get; set; }
        
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "TheNeedForPastureFeedSpring")]
        public int? TheNeedForPastureFeedSpring { get; set; }
        
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "TheNeedForPastureFeedSummer")]
        public int? TheNeedForPastureFeedSummer { get; set; }
        
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "TheNeedForPastureFeedAutumn")]
        public int? TheNeedForPastureFeedAutumn { get; set; }
        
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "FeedStockOfUsedPasturesSpring")]
        public int? FeedStockOfUsedPasturesSpring { get; set; }
        
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "FeedStockOfUsedPasturesSummer")]
        public int? FeedStockOfUsedPasturesSummer { get; set; }
        
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "FeedStockOfUsedPasturesAutumn")]
        public int? FeedStockOfUsedPasturesAutumn { get; set; }
        
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "TheCostPer1HeadForTheBillingPeriodSpring")]
        public decimal? TheCostPer1HeadForTheBillingPeriodSpring { get; set; }
        
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "TheCostPer1HeadForTheBillingPeriodSummer")]
        public decimal? TheCostPer1HeadForTheBillingPeriodSummer { get; set; }
        
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "TheCostPer1HeadForTheBillingPeriodFall")]
        public decimal? TheCostPer1HeadForTheBillingPeriodFall { get; set; }
        
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "LoadSpring")]
        public decimal? LoadSpring { get; set; }
        
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "LoadSummer")]
        public decimal? LoadSummer { get; set; }
        
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "LoadFall")]
        public decimal? LoadFall { get; set; }
        
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "ShortageSurplusOfPastureFeedSpring")]
        public int? ShortageSurplusOfPastureFeedSpring { get; set; }
        
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "ShortageSurplusOfPastureFeedSummer")]
        public int? ShortageSurplusOfPastureFeedSummer { get; set; }
        
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "ShortageSurplusOfPastureFeedAutumn")]
        public int? ShortageSurplusOfPastureFeedAutumn { get; set; }
        
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "RequiredAdditionalAreaIfNecessaryPasturesSpring")]
        public int? RequiredAdditionalAreaIfNecessaryPasturesSpring { get; set; }
        
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "RequiredAdditionalAreaIfNecessaryPasturesSummer")]
        public int? RequiredAdditionalAreaIfNecessaryPasturesSummer { get; set; }
        
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "RequiredAdditionalAreaIfNecessaryPasturesAutumn")]
        public int? RequiredAdditionalAreaIfNecessaryPasturesAutumn { get; set; }
        
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "ThePresenceOfDegradedSitesIfAvailable")]
        public string ThePresenceOfDegradedSitesIfAvailable { get; set; }
        
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "RecommendationsForImprovingThisDegradedArea")]
        public string RecommendationsForImprovingThisDegradedArea { get; set; }
    }

    public class FarmPastureIndexPageViewModel
    {
        public IEnumerable<FarmPasture> Items { get; set; }
        public Pager Pager { get; set; }
    }
}
