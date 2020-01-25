using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Pastures2019.Models
{
    public class MODISProduct
    {
        public int Id { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "MODISSource")]
        public MODISSource MODISSource { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "MODISSource")]
        public int MODISSourceId { get; set; }
        
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "Name")]
        public string Name { get; set; }
    }

    public class MODISProductIndexPageViewModel
    {
        public IEnumerable<MODISProduct> Items { get; set; }
        public Pager Pager { get; set; }
    }
}
