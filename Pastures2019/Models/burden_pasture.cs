using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pastures2019.Models
{
    public class burden_pasture
    {
        public int gid { get; set; }
        public int objectid { get; set; }
        public int num_vydel { get; set; }
        public int rule_grazi { get; set; }
        public int bur_otdel { get; set; }
        public int bur_type_i { get; set; }
        public int bur_subotd { get; set; }
        public int bur_class_ { get; set; }
        public int bur_group_ { get; set; }
        public decimal average_yi { get; set; }
        public decimal burden_gro { get; set; }
        public decimal burden_deg { get; set; }
        public decimal shape_leng { get; set; }
        public decimal shape_area { get; set; }
    }
}
