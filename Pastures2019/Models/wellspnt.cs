using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pastures2019.Models
{
    public class wellspnt
    {
        public int gid { get; set; }
        public int objectid { get; set; }
        public int id { get; set; }
        public string num { get; set; }
        public int usl { get; set; }
        public string indeks { get; set; }
        public decimal debit { get; set; }
        public decimal decrease { get; set; }
        public decimal depth { get; set; }
        public decimal minerali { get; set; }
        public int chemical_c { get; set; }
        public string kato { get; set; }
        public int wat_seepag { get; set; }
        public string wtype { get; set; }
        public string wsubtype { get; set; }
        public string chemicalcomp { get; set; }
    }
}
