using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Pastures2019.Models
{
    public class PSubType
    {
        public int Id { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Controllers.SharedResources), ErrorMessageResourceName = "TheFieldIsRequired")]
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "Code")]
        public int Code { get; set; }


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

    public class PSubTypeIndexPageViewModel
    {
        public IEnumerable<PSubType> Items { get; set; }
        public Pager Pager { get; set; }
    }
}
