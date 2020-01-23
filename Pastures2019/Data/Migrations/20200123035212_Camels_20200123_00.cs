using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Pastures2019.Data.Migrations
{
    public partial class Camels_20200123_00 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Camel",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Code = table.Column<int>(nullable: false),
                    BreedRU = table.Column<string>(nullable: true),
                    BreedKK = table.Column<string>(nullable: true),
                    BreedEN = table.Column<string>(nullable: true),
                    WeightRU = table.Column<string>(nullable: true),
                    WeightKK = table.Column<string>(nullable: true),
                    WeightEN = table.Column<string>(nullable: true),
                    SlaughterYield = table.Column<decimal>(nullable: false),
                    EwesYieldRU = table.Column<string>(nullable: true),
                    EwesYieldKK = table.Column<string>(nullable: true),
                    EwesYieldEN = table.Column<string>(nullable: true),
                    TotalGoals = table.Column<int>(nullable: false),
                    MilkFatContent = table.Column<string>(nullable: true),
                    RangeRU = table.Column<string>(nullable: true),
                    RangeKK = table.Column<string>(nullable: true),
                    RangeEN = table.Column<string>(nullable: true),
                    Photo = table.Column<byte[]>(nullable: true),
                    DescriptionRU = table.Column<string>(nullable: true),
                    DescriptionKK = table.Column<string>(nullable: true),
                    DescriptionEN = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Camel", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Camel");
        }
    }
}
