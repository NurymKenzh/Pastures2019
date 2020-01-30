using Microsoft.EntityFrameworkCore.Migrations;

namespace Pastures2019.Data.Migrations
{
    public partial class Cattle_20200130_01 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SlaughterYield",
                table: "Cattle",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SlaughterYield",
                table: "Cattle");
        }
    }
}
