using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GimenaCreations.Data.Migrations
{
    public partial class PurchaseHistoryFk : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_PurchaseHistories_PurchaseId",
                table: "PurchaseHistories",
                column: "PurchaseId");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseHistories_Purchases_PurchaseId",
                table: "PurchaseHistories",
                column: "PurchaseId",
                principalTable: "Purchases",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseHistories_Purchases_PurchaseId",
                table: "PurchaseHistories");

            migrationBuilder.DropIndex(
                name: "IX_PurchaseHistories_PurchaseId",
                table: "PurchaseHistories");
        }
    }
}
