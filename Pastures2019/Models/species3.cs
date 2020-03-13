using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pastures2019.Models
{
    public class species3
    {
        public int gid { get; set; }
        public int objectid { get; set; }
        public string name_adm1 { get; set; }
        public string name_adm2 { get; set; }
        public string kato_te { get; set; }
        public int type_k { get; set; }
        public decimal shape_leng { get; set; }
        public decimal shape_area { get; set; }
        public string name_adm3 { get; set; }
        public int? totalgoals { get; set; }
        public int? cattle { get; set; }
        public int? horses { get; set; }
        public int? smallcattle { get; set; }
        public int? camels { get; set; }
        public decimal? conditional { get; set; }
        public string date { get; set; }
        public int? source { get; set; }
        public int? population { get; set; }
        public int? pastures { get; set; }
    }
}
