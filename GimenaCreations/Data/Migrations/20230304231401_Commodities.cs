using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GimenaCreations.Data.Migrations
{
    /// <inheritdoc />
    public partial class Commodities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Commodities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Supplier = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PurchaseId = table.Column<int>(type: "int", nullable: false),
                    InputNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InputDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    InvoiceNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InvoiceDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    InvoiceReceptionDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Commodities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Commodities_Purchases_PurchaseId",
                        column: x => x.PurchaseId,
                        principalTable: "Purchases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CommodityItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CatalogItemId = table.Column<int>(type: "int", nullable: false),
                    CommodityId = table.Column<int>(type: "int", nullable: false),
                    ProductName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProductType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    IsSatisfied = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommodityItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CommodityItems_Catalog_CatalogItemId",
                        column: x => x.CatalogItemId,
                        principalTable: "Catalog",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CommodityItems_Commodities_CommodityId",
                        column: x => x.CommodityId,
                        principalTable: "Commodities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Commodities_PurchaseId",
                table: "Commodities",
                column: "PurchaseId");

            migrationBuilder.CreateIndex(
                name: "IX_CommodityItems_CatalogItemId",
                table: "CommodityItems",
                column: "CatalogItemId");

            migrationBuilder.CreateIndex(
                name: "IX_CommodityItems_CommodityId",
                table: "CommodityItems",
                column: "CommodityId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CommodityItems");

            migrationBuilder.DropTable(
                name: "Commodities");
        }
    }
}
