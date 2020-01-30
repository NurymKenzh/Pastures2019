﻿using System;
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
    }
}
