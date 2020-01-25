using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Pastures2019.Data.Migrations
{
    public partial class MODISProduct_20200125_00 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MODISProduct",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    MODISSourceId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MODISProduct", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MODISProduct_MODISSource_MODISSourceId",
                        column: x => x.MODISSourceId,
                        principalTable: "MODISSource",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MODISProduct_MODISSourceId",
                table: "MODISProduct",
                column: "MODISSourceId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MODISProduct");
        }
    }
}
