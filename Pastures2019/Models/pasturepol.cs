using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pastures2019.Models
{
    public class pasturepol
    {
        public int gid { get; set; }
        public int objectid { get; set; }
        public int class_id { get; set; }
        public int relief_id { get; set; }
        public int zone_id { get; set; }
        public int subtype_id { get; set; }
        public int group_id { get; set; }
        public decimal ur_v { get; set; }
        public decimal ur_l { get; set; }
        public decimal ur_o { get; set; }
        public decimal ur_z { get; set; }
        public decimal korm_v { get; set; }
        public decimal korm_l { get; set; }
        public decimal korm_o { get; set; }
        public decimal korm_z { get; set; }
        public int recommend_ { get; set; }
        public int recom_catt { get; set; }
        public int haying_id { get; set; }
        public int otdely_id { get; set; }
        public decimal shape_leng { get; set; }
        public decimal shape_area { get; set; }

        public string otdel { get; set; }
        public string ptype { get; set; }
        public string group { get; set; }
        public string group_lat { get; set; }
        public string recommend { get; set; }
        public string recomcatt { get; set; }

    }
}
