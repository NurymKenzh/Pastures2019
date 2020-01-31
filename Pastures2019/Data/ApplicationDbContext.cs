using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Pastures2019.Models;

namespace Pastures2019.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Pastures2019.Models.BClass> BClass { get; set; }
        public DbSet<Pastures2019.Models.BGroup> BGroup { get; set; }
        public DbSet<Pastures2019.Models.BType> BType { get; set; }
        public DbSet<Pastures2019.Models.BurOtdel> BurOtdel { get; set; }
        public DbSet<Pastures2019.Models.BurSubOtdel> BurSubOtdel { get; set; }
        public DbSet<Pastures2019.Models.Camel> Camel { get; set; }
        public DbSet<Pastures2019.Models.MODISSource> MODISSource { get; set; }
        public DbSet<Pastures2019.Models.MODISProduct> MODISProduct { get; set; }
        public DbSet<Pastures2019.Models.MODISDataSet> MODISDataSet { get; set; }
        public DbSet<Pastures2019.Models.Cattle> Cattle { get; set; }
        public DbSet<Pastures2019.Models.Horse> Horse { get; set; }
        public DbSet<Pastures2019.Models.SmallCattle> SmallCattle { get; set; }
        public DbSet<Pastures2019.Models.CATO> CATO { get; set; }
        public DbSet<Pastures2019.Models.CATOSpecies> CATOSpecies { get; set; }
        public DbSet<Pastures2019.Models.ChemicalComp> ChemicalComp { get; set; }
        public DbSet<Pastures2019.Models.DominantType> DominantType { get; set; }
        public DbSet<Pastures2019.Models.Haying> Haying { get; set; }
        public DbSet<Pastures2019.Models.Otdel> Otdel { get; set; }
        public DbSet<Pastures2019.Models.PSubType> PSubType { get; set; }
        public DbSet<Pastures2019.Models.PType> PType { get; set; }
        public DbSet<Pastures2019.Models.RecomCattle> RecomCattle { get; set; }
        public DbSet<Pastures2019.Models.Recommend> Recommend { get; set; }
        public DbSet<Pastures2019.Models.Relief> Relief { get; set; }
        public DbSet<Pastures2019.Models.Soob> Soob { get; set; }
        public DbSet<Pastures2019.Models.SType> SType { get; set; }
        public DbSet<Pastures2019.Models.SupplyRecommend> SupplyRecommend { get; set; }
        public DbSet<Pastures2019.Models.WClass> WClass { get; set; }
        public DbSet<Pastures2019.Models.WSubType> WSubType { get; set; }
        public DbSet<Pastures2019.Models.WType> WType { get; set; }
        public DbSet<Pastures2019.Models.Zone> Zone { get; set; }
        public DbSet<Pastures2019.Models.ZSubType> ZSubType { get; set; }
        public DbSet<Pastures2019.Models.ZType> ZType { get; set; }
    }
}
