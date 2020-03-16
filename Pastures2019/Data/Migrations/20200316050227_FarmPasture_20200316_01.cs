using Microsoft.EntityFrameworkCore.Migrations;

namespace Pastures2019.Data.Migrations
{
    public partial class FarmPasture_20200316_01 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "AveragePastureProductivitySummer",
                table: "FarmPasture",
                nullable: true,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<decimal>(
                name: "AveragePastureProductivityAutumn",
                table: "FarmPasture",
                nullable: true,
                oldClrType: typeof(decimal));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "AveragePastureProductivitySummer",
                table: "FarmPasture",
                nullable: false,
                oldClrType: typeof(decimal),
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "AveragePastureProductivityAutumn",
                table: "FarmPasture",
                nullable: false,
                oldClrType: typeof(decimal),
                oldNullable: true);
        }
    }
}
