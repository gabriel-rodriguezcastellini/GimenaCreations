using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GimenaCreations.Data.Migrations
{
    public partial class RemovedCatalogBrand : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Catalog_CatalogBrand_CatalogBrandId",
                table: "Catalog");

            migrationBuilder.DropTable(
                name: "CatalogBrand");

            migrationBuilder.DropIndex(
                name: "IX_Catalog_CatalogBrandId",
                table: "Catalog");

            migrationBuilder.DropSequence(
                name: "catalog_brand_hilo");

            migrationBuilder.DropColumn(
                name: "CatalogBrandId",
                table: "Catalog");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateSequence(
                name: "catalog_brand_hilo",
                incrementBy: 10);

            migrationBuilder.AddColumn<int>(
                name: "CatalogBrandId",
                table: "Catalog",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "CatalogBrand",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Brand = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CatalogBrand", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Catalog_CatalogBrandId",
                table: "Catalog",
                column: "CatalogBrandId");

            migrationBuilder.AddForeignKey(
                name: "FK_Catalog_CatalogBrand_CatalogBrandId",
                table: "Catalog",
                column: "CatalogBrandId",
                principalTable: "CatalogBrand",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
