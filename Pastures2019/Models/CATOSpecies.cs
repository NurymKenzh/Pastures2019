using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Pastures2019.Models
{
    public class CATOSpecies
    {
        public int Id { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "CATO")]
        public string CATOTE { get; set; }


        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "Code")]
        public int Code { get; set; }
    }

    public class CATOSpeciesIndexPageViewModel
    {
        public IEnumerable<CATOSpecies> Items { get; set; }
        public Pager Pager { get; set; }
    }
}
