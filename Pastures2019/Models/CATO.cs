using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Pastures2019.Models
{
    public class CATO
    {
        public int Id { get; set; }
        public string AB { get; set; }
        public string CD { get; set; }
        public string EF { get; set; }
        public string HIJ { get; set; }

        public string K { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "NameRU")]
        public string NameRU { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "NameKK")]
        public string NameKK { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "Name")]
        public string Name
        {
            get
            {
                string language = new RequestLocalizationOptions().DefaultRequestCulture.Culture.Name,
                    name = NameRU;
                if (language == "kk")
                {
                    name = NameKK;
                }
                return name;
            }
        }


        public string TE
        {
            get
            {
                return AB + CD + EF + HIJ;
            }

        }
    }

    public class CATOIndexPageViewModel
    {
        public IEnumerable<CATO> Items { get; set; }
        public Pager Pager { get; set; }
    }
}
