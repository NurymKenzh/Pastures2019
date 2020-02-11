using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Pastures2019.Models
{
    public class SmallCattle
    {
        public int Id { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Controllers.SharedResources), ErrorMessageResourceName = "TheFieldIsRequired")]
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "Code")]
        public int Code { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "BreedRU")]
        public string BreedRU { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "BreedKK")]
        public string BreedKK { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "BreedEN")]
        public string BreedEN { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "Breed")]
        public string Breed
        {
            get
            {
                string language = new RequestLocalizationOptions().DefaultRequestCulture.Culture.Name,
                    name = BreedRU;
                if (language == "kk")
                {
                    name = BreedKK;
                }
                if (language == "en")
                {
                    name = BreedEN;
                }
                return name;
            }
        }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "DirectionRU")]
        public string DirectionRU { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "DirectionKK")]
        public string DirectionKK { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "DirectionEN")]
        public string DirectionEN { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "Direction")]
        public string Direction
        {
            get
            {
                string language = new RequestLocalizationOptions().DefaultRequestCulture.Culture.Name,
                    name = DirectionRU;
                if (language == "kk")
                {
                    name = DirectionKK;
                }
                if (language == "en")
                {
                    name = DirectionEN;
                }
                return name;
            }
        }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "WeightRU")]
        public string WeightRU { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "WeightKK")]
        public string WeightKK { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "WeightEN")]
        public string WeightEN { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "Weight")]
        public string Weight
        {
            get
            {
                string language = new RequestLocalizationOptions().DefaultRequestCulture.Culture.Name,
                    name = WeightRU;
                if (language == "kk")
                {
                    name = WeightKK;
                }
                if (language == "en")
                {
                    name = WeightEN;
                }
                return name;
            }
        }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "ShearingsRU")]
        public string ShearingsRU { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "ShearingsKK")]
        public string ShearingsKK { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "ShearingsEN")]
        public string ShearingsEN { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "Shearings")]
        public string Shearings
        {
            get
            {
                string language = new RequestLocalizationOptions().DefaultRequestCulture.Culture.Name,
                    name = ShearingsRU;
                if (language == "kk")
                {
                    name = ShearingsKK;
                }
                if (language == "en")
                {
                    name = ShearingsEN;
                }
                return name;
            }
        }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "WashedWoolYieldRU")]
        public string WashedWoolYieldRU { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "WashedWoolYieldKK")]
        public string WashedWoolYieldKK { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "WashedWoolYieldEN")]
        public string WashedWoolYieldEN { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "WashedWoolYield")]
        public string WashedWoolYield
        {
            get
            {
                string language = new RequestLocalizationOptions().DefaultRequestCulture.Culture.Name,
                    name = WashedWoolYieldRU;
                if (language == "kk")
                {
                    name = WashedWoolYieldKK;
                }
                if (language == "en")
                {
                    name = WashedWoolYieldEN;
                }
                return name;
            }
        }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "FertilityRU")]
        public string FertilityRU { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "FertilityKK")]
        public string FertilityKK { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "FertilityEN")]
        public string FertilityEN { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "Fertility")]
        public string Fertility
        {
            get
            {
                string language = new RequestLocalizationOptions().DefaultRequestCulture.Culture.Name,
                    name = FertilityRU;
                if (language == "kk")
                {
                    name = FertilityKK;
                }
                if (language == "en")
                {
                    name = FertilityEN;
                }
                return name;
            }
        }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "WoolLengthRU")]
        public string WoolLengthRU { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "WoolLengthKK")]
        public string WoolLengthKK { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "WoolLengthEN")]
        public string WoolLengthEN { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "WoolLength")]
        public string WoolLength
        {
            get
            {
                string language = new RequestLocalizationOptions().DefaultRequestCulture.Culture.Name,
                    name = WoolLengthRU;
                if (language == "kk")
                {
                    name = WoolLengthKK;
                }
                if (language == "en")
                {
                    name = WoolLengthEN;
                }
                return name;
            }
        }

        [Required(ErrorMessageResourceType = typeof(Resources.Controllers.SharedResources), ErrorMessageResourceName = "TheFieldIsRequired")]
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "TotalGoals")]
        public int TotalGoals { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "BredRU")]
        public string BredRU { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "BredKK")]
        public string BredKK { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "BredEN")]
        public string BredEN { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "Bred")]
        public string Bred
        {
            get
            {
                string language = new RequestLocalizationOptions().DefaultRequestCulture.Culture.Name,
                    name = BredRU;
                if (language == "kk")
                {
                    name = BredKK;
                }
                if (language == "en")
                {
                    name = BredEN;
                }
                return name;
            }
        }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "RangeRU")]
        public string RangeRU { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "RangeKK")]
        public string RangeKK { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "RangeEN")]
        public string RangeEN { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "Range")]
        public string Range
        {
            get
            {
                string language = new RequestLocalizationOptions().DefaultRequestCulture.Culture.Name,
                    name = RangeRU;
                if (language == "kk")
                {
                    name = RangeKK;
                }
                if (language == "en")
                {
                    name = RangeEN;
                }
                return name;
            }
        }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "Photo")]
        public byte[] Photo { get; set; }
        [NotMapped]
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "Photo")]
        public IFormFile FormFile { get; set; }
        [NotMapped]
        public string Img{ get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "DescriptionRU")]
        public string DescriptionRU { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "DescriptionKK")]
        public string DescriptionKK { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "DescriptionEN")]
        public string DescriptionEN { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "Description")]
        public string Description
        {
            get
            {
                string language = new RequestLocalizationOptions().DefaultRequestCulture.Culture.Name,
                    name = DescriptionRU;
                if (language == "kk")
                {
                    name = DescriptionKK;
                }
                if (language == "en")
                {
                    name = DescriptionEN;
                }
                return name;
            }
        }
    }

    public class SmallCattleIndexPageViewModel
    {
        public IEnumerable<SmallCattle> Items { get; set; }
        public Pager Pager { get; set; }
    }
}
