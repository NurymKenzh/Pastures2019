using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Pastures2019.Models
{
    public class MODISSource
    {
        public int Id { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "Name")]
        public string Name { get; set; }
    }

    public class MODISSourceIndexPageViewModel
    {
        public IEnumerable<MODISSource> Items { get; set; }
        public Pager Pager { get; set; }
    }
}
