using Microsoft.EntityFrameworkCore.Migrations;

namespace Pastures2019.Data.Migrations
{
    public partial class Cattle_20200130_02 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "SlaughterYield",
                table: "Cattle",
                nullable: true,
                oldClrType: typeof(string));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "SlaughterYield",
                table: "Cattle",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
