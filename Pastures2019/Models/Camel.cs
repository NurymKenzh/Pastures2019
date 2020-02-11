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
    public class Camel
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

        [Required(ErrorMessageResourceType = typeof(Resources.Controllers.SharedResources), ErrorMessageResourceName = "TheFieldIsRequired")]
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "SlaughterYield")]
        public decimal SlaughterYield { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "EwesYieldRU")]
        public string EwesYieldRU { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "EwesYieldKK")]
        public string EwesYieldKK { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "EwesYieldEN")]
        public string EwesYieldEN { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "EwesYield")]
        public string EwesYield
        {
            get
            {
                string language = new RequestLocalizationOptions().DefaultRequestCulture.Culture.Name,
                    name = EwesYieldRU;
                if (language == "kk")
                {
                    name = EwesYieldKK;
                }
                if (language == "en")
                {
                    name = EwesYieldEN;
                }
                return name;
            }
        }

        [Required(ErrorMessageResourceType = typeof(Resources.Controllers.SharedResources), ErrorMessageResourceName = "TheFieldIsRequired")]
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "TotalGoals")]
        public int TotalGoals { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "MilkFatContent")]
        public string MilkFatContent { get; set; }

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
        public string Img { get; set; }

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

    public class CamelIndexPageViewModel
    {
        public IEnumerable<Camel> Items { get; set; }
        public Pager Pager { get; set; }
    }
}
