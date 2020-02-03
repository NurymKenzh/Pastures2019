using Microsoft.EntityFrameworkCore.Migrations;

namespace Pastures2019.Data.Migrations
{
    public partial class Horse_20200130_01 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "SlaughterYield",
                table: "Cattle",
                nullable: false,
                oldClrType: typeof(decimal));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "SlaughterYield",
                table: "Cattle",
                nullable: false,
                oldClrType: typeof(string));
        }
    }
}
