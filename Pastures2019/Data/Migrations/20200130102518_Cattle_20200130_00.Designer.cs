﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Pastures2019.Data;

namespace Pastures2019.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20200130102518_Cattle_20200130_00")]
    partial class Cattle_20200130_00
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128);

                    b.Property<string>("ProviderKey")
                        .HasMaxLength(128);

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128);

                    b.Property<string>("Name")
                        .HasMaxLength(128);

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("Pastures2019.Models.BClass", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Code");

                    b.Property<string>("DescriptionEN");

                    b.Property<string>("DescriptionKK");

                    b.Property<string>("DescriptionRU");

                    b.HasKey("Id");

                    b.ToTable("BClass");
                });

            modelBuilder.Entity("Pastures2019.Models.BGroup", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Code");

                    b.Property<string>("DescriptionEN");

                    b.Property<string>("DescriptionKK");

                    b.Property<string>("DescriptionRU");

                    b.HasKey("Id");

                    b.ToTable("BGroup");
                });

            modelBuilder.Entity("Pastures2019.Models.BType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Code");

                    b.Property<string>("DescriptionEN");

                    b.Property<string>("DescriptionKK");

                    b.Property<string>("DescriptionRU");

                    b.HasKey("Id");

                    b.ToTable("BType");
                });

            modelBuilder.Entity("Pastures2019.Models.BurOtdel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Code");

                    b.Property<string>("DescriptionEN");

                    b.Property<string>("DescriptionKK");

                    b.Property<string>("DescriptionRU");

                    b.HasKey("Id");

                    b.ToTable("BurOtdel");
                });

            modelBuilder.Entity("Pastures2019.Models.BurSubOtdel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Code");

                    b.Property<string>("DescriptionEN");

                    b.Property<string>("DescriptionKK");

                    b.Property<string>("DescriptionRU");

                    b.HasKey("Id");

                    b.ToTable("BurSubOtdel");
                });

            modelBuilder.Entity("Pastures2019.Models.Camel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("BreedEN");

                    b.Property<string>("BreedKK");

                    b.Property<string>("BreedRU");

                    b.Property<int>("Code");

                    b.Property<string>("DescriptionEN");

                    b.Property<string>("DescriptionKK");

                    b.Property<string>("DescriptionRU");

                    b.Property<string>("EwesYieldEN");

                    b.Property<string>("EwesYieldKK");

                    b.Property<string>("EwesYieldRU");

                    b.Property<string>("MilkFatContent");

                    b.Property<byte[]>("Photo");

                    b.Property<string>("RangeEN");

                    b.Property<string>("RangeKK");

                    b.Property<string>("RangeRU");

                    b.Property<decimal>("SlaughterYield");

                    b.Property<int>("TotalGoals");

                    b.Property<string>("WeightEN");

                    b.Property<string>("WeightKK");

                    b.Property<string>("WeightRU");

                    b.HasKey("Id");

                    b.ToTable("Camel");
                });

            modelBuilder.Entity("Pastures2019.Models.Cattle", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("BredEN");

                    b.Property<string>("BredKK");

                    b.Property<string>("BredRU");

                    b.Property<string>("BreedEN");

                    b.Property<string>("BreedKK");

                    b.Property<string>("BreedRU");

                    b.Property<int>("Code");

                    b.Property<string>("DescriptionEN");

                    b.Property<string>("DescriptionKK");

                    b.Property<string>("DescriptionRU");

                    b.Property<string>("DirectionEN");

                    b.Property<string>("DirectionKK");

                    b.Property<string>("DirectionRU");

                    b.Property<string>("EwesYieldEN");

                    b.Property<string>("EwesYieldKK");

                    b.Property<string>("EwesYieldRU");

                    b.Property<string>("MilkFatContent");

                    b.Property<byte[]>("Photo");

                    b.Property<string>("RangeEN");

                    b.Property<string>("RangeKK");

                    b.Property<string>("RangeRU");

                    b.Property<int>("TotalGoals");

                    b.Property<string>("WeightEN");

                    b.Property<string>("WeightKK");

                    b.Property<string>("WeightRU");

                    b.HasKey("Id");

                    b.ToTable("Cattle");
                });

            modelBuilder.Entity("Pastures2019.Models.Horse", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("BodyLengthEN");

                    b.Property<string>("BodyLengthKK");

                    b.Property<string>("BodyLengthRU");

                    b.Property<string>("BredEN");

                    b.Property<string>("BredKK");

                    b.Property<string>("BredRU");

                    b.Property<string>("BreedEN");

                    b.Property<string>("BreedKK");

                    b.Property<string>("BreedRU");

                    b.Property<string>("BustEN");

                    b.Property<string>("BustKK");

                    b.Property<string>("BustRU");

                    b.Property<int>("Code");

                    b.Property<string>("DescriptionEN");

                    b.Property<string>("DescriptionKK");

                    b.Property<string>("DescriptionRU");

                    b.Property<string>("DirectionEN");

                    b.Property<string>("DirectionKK");

                    b.Property<string>("DirectionRU");

                    b.Property<string>("HeightEN");

                    b.Property<string>("HeightKK");

                    b.Property<string>("HeightRU");

                    b.Property<string>("MetacarpusEN");

                    b.Property<string>("MetacarpusKK");

                    b.Property<string>("MetacarpusRU");

                    b.Property<string>("MilkYieldEN");

                    b.Property<string>("MilkYieldKK");

                    b.Property<string>("MilkYieldRU");

                    b.Property<byte[]>("Photo");

                    b.Property<string>("RangeEN");

                    b.Property<string>("RangeKK");

                    b.Property<string>("RangeRU");

                    b.Property<int>("TotalGoals");

                    b.Property<string>("WeightEN");

                    b.Property<string>("WeightKK");

                    b.Property<string>("WeightRU");

                    b.HasKey("Id");

                    b.ToTable("Horse");
                });

            modelBuilder.Entity("Pastures2019.Models.MODISDataSet", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Index");

                    b.Property<int>("MODISProductId");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.HasIndex("MODISProductId");

                    b.ToTable("MODISDataSet");
                });

            modelBuilder.Entity("Pastures2019.Models.MODISProduct", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("MODISSourceId");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.HasIndex("MODISSourceId");

                    b.ToTable("MODISProduct");
                });

            modelBuilder.Entity("Pastures2019.Models.MODISSource", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("MODISSource");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Pastures2019.Models.MODISDataSet", b =>
                {
                    b.HasOne("Pastures2019.Models.MODISProduct", "MODISProduct")
                        .WithMany()
                        .HasForeignKey("MODISProductId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Pastures2019.Models.MODISProduct", b =>
                {
                    b.HasOne("Pastures2019.Models.MODISSource", "MODISSource")
                        .WithMany()
                        .HasForeignKey("MODISSourceId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
