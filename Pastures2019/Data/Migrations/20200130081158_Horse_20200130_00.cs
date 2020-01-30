using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Pastures2019.Data.Migrations
{
    public partial class Horse_20200130_00 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Horse",
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
                    HeightRU = table.Column<string>(nullable: true),
                    HeightKK = table.Column<string>(nullable: true),
                    HeightEN = table.Column<string>(nullable: true),
                    MilkYieldRU = table.Column<string>(nullable: true),
                    MilkYieldKK = table.Column<string>(nullable: true),
                    MilkYieldEN = table.Column<string>(nullable: true),
                    BodyLengthRU = table.Column<string>(nullable: true),
                    BodyLengthKK = table.Column<string>(nullable: true),
                    BodyLengthEN = table.Column<string>(nullable: true),
                    BustRU = table.Column<string>(nullable: true),
                    BustKK = table.Column<string>(nullable: true),
                    BustEN = table.Column<string>(nullable: true),
                    MetacarpusRU = table.Column<string>(nullable: true),
                    MetacarpusKK = table.Column<string>(nullable: true),
                    MetacarpusEN = table.Column<string>(nullable: true),
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
                    table.PrimaryKey("PK_Horse", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Horse");
        }
    }
}
