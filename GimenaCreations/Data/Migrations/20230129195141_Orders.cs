using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GimenaCreations.Data.Migrations
{
    public partial class Orders : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Address_Street = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address_City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address_State = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address_Country = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address_ZipCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IdentityUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PaymentMethod = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CatalogItemOrder",
                columns: table => new
                {
                    CatalogItemsId = table.Column<int>(type: "int", nullable: false),
                    OrdersId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CatalogItemOrder", x => new { x.CatalogItemsId, x.OrdersId });
                    table.ForeignKey(
                        name: "FK_CatalogItemOrder_Catalog_CatalogItemsId",
                        column: x => x.CatalogItemsId,
                        principalTable: "Catalog",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CatalogItemOrder_Orders_OrdersId",
                        column: x => x.OrdersId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CatalogItemOrder_OrdersId",
                table: "CatalogItemOrder",
                column: "OrdersId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CatalogItemOrder");

            migrationBuilder.DropTable(
                name: "Orders");
        }
    }
}
