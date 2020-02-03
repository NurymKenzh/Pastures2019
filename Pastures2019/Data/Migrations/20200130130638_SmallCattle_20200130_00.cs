using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Pastures2019.Data.Migrations
{
    public partial class SmallCattle_20200130_00 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SmallCattle",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Code = table.Column<int>(nullable: false),
                    BreedRU = table.Column<string>(nullable: true),
                    BreedKK = table.Column<string>(nullable: true),
                    BreedEN = table.Column<string>(nullable: true),
                    DirectionRU = table.Column<string>(nullable: true),
                    DirectionKK = table.Column<string>(nullable: true),
                    DirectionEN = table.Column<string>(nullable: true),
                    WeightRU = table.Column<string>(nullable: true),
                    WeightKK = table.Column<string>(nullable: true),
                    WeightEN = table.Column<string>(nullable: true),
                    ShearingsRU = table.Column<string>(nullable: true),
                    ShearingsKK = table.Column<string>(nullable: true),
                    ShearingsEN = table.Column<string>(nullable: true),
                    WashedWoolYieldRU = table.Column<string>(nullable: true),
                    WashedWoolYieldKK = table.Column<string>(nullable: true),
                    WashedWoolYieldEN = table.Column<string>(nullable: true),
                    FertilityRU = table.Column<string>(nullable: true),
                    FertilityKK = table.Column<string>(nullable: true),
                    FertilityEN = table.Column<string>(nullable: true),
                    WoolLengthRU = table.Column<string>(nullable: true),
                    WoolLengthKK = table.Column<string>(nullable: true),
                    WoolLengthEN = table.Column<string>(nullable: true),
                    TotalGoals = table.Column<int>(nullable: false),
                    BredRU = table.Column<string>(nullable: true),
                    BredKK = table.Column<string>(nullable: true),
                    BredEN = table.Column<string>(nullable: true),
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
                    table.PrimaryKey("PK_SmallCattle", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SmallCattle");
        }
    }
}
