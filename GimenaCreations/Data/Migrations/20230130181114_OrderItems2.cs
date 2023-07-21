using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GimenaCreations.Data.Migrations
{
    public partial class OrderItems2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_Catalog_CatalogItemId",
                table: "OrderItems");

            migrationBuilder.AlterColumn<int>(
                name: "CatalogItemId",
                table: "OrderItems",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_Catalog_CatalogItemId",
                table: "OrderItems",
                column: "CatalogItemId",
                principalTable: "Catalog",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_Catalog_CatalogItemId",
                table: "OrderItems");

            migrationBuilder.AlterColumn<int>(
                name: "CatalogItemId",
                table: "OrderItems",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_Catalog_CatalogItemId",
                table: "OrderItems",
                column: "CatalogItemId",
                principalTable: "Catalog",
                principalColumn: "Id");
        }
    }
}
