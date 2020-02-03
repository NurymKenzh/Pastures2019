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
    public class Horse
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

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "HeightRU")]
        public string HeightRU { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "HeightKK")]
        public string HeightKK { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "HeightEN")]
        public string HeightEN { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "Height")]
        public string Height
        {
            get
            {
                string language = new RequestLocalizationOptions().DefaultRequestCulture.Culture.Name,
                    name = HeightRU;
                if (language == "kk")
                {
                    name = HeightKK;
                }
                if (language == "en")
                {
                    name = HeightEN;
                }
                return name;
            }
        }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "MilkYieldRU")]
        public string MilkYieldRU { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "MilkYieldKK")]
        public string MilkYieldKK { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "MilkYieldEN")]
        public string MilkYieldEN { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "MilkYield")]
        public string MilkYield
        {
            get
            {
                string language = new RequestLocalizationOptions().DefaultRequestCulture.Culture.Name,
                    name = MilkYieldRU;
                if (language == "kk")
                {
                    name = MilkYieldKK;
                }
                if (language == "en")
                {
                    name = MilkYieldEN;
                }
                return name;
            }
        }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "BodyLengthRU")]
        public string BodyLengthRU { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "BodyLengthKK")]
        public string BodyLengthKK { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "BodyLengthEN")]
        public string BodyLengthEN { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "BodyLength")]
        public string BodyLength
        {
            get
            {
                string language = new RequestLocalizationOptions().DefaultRequestCulture.Culture.Name,
                    name = BodyLengthRU;
                if (language == "kk")
                {
                    name = BodyLengthKK;
                }
                if (language == "en")
                {
                    name = BodyLengthEN;
                }
                return name;
            }
        }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "BustRU")]
        public string BustRU { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "BustKK")]
        public string BustKK { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "BustEN")]
        public string BustEN { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "Bust")]
        public string Bust
        {
            get
            {
                string language = new RequestLocalizationOptions().DefaultRequestCulture.Culture.Name,
                    name = BustRU;
                if (language == "kk")
                {
                    name = BustKK;
                }
                if (language == "en")
                {
                    name = BustEN;
                }
                return name;
            }
        }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "MetacarpusRU")]
        public string MetacarpusRU { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "MetacarpusKK")]
        public string MetacarpusKK { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "MetacarpusEN")]
        public string MetacarpusEN { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "Metacarpus")]
        public string Metacarpus
        {
            get
            {
                string language = new RequestLocalizationOptions().DefaultRequestCulture.Culture.Name,
                    name = MetacarpusRU;
                if (language == "kk")
                {
                    name = MetacarpusKK;
                }
                if (language == "en")
                {
                    name = MetacarpusEN;
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

    public class HorseIndexPageViewModel
    {
        public IEnumerable<Horse> Items { get; set; }
        public Pager Pager { get; set; }
    }
}
