using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Pastures2019.Models
{
    public class MODISDataSet
    {
        public int Id { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "MODISProduct")]
        public MODISProduct MODISProduct { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "MODISProduct")]
        public int MODISProductId { get; set; }
        
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "Name")]
        public string Name { get; set; }
        
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "Index")]
        public int Index { get; set; }
    }

    public class MODISDataSetIndexPageViewModel
    {
        public IEnumerable<MODISDataSet> Items { get; set; }
        public Pager Pager { get; set; }
    }
}
