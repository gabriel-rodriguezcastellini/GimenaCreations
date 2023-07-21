using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GimenaCreations.Data.Migrations
{
    public partial class PurchaseHistory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateUpdated",
                table: "Purchases");

            migrationBuilder.AddColumn<DateTime>(
                name: "ModificationDate",
                table: "Purchases",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "PurchaseHistories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PurchaseId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseHistories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PurchaseHistoryItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PurchaseHistoryId = table.Column<int>(type: "int", nullable: false),
                    State = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseHistoryItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PurchaseHistoryItems_PurchaseHistories_PurchaseHistoryId",
                        column: x => x.PurchaseHistoryId,
                        principalTable: "PurchaseHistories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseHistoryItems_PurchaseHistoryId",
                table: "PurchaseHistoryItems",
                column: "PurchaseHistoryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PurchaseHistoryItems");

            migrationBuilder.DropTable(
                name: "PurchaseHistories");

            migrationBuilder.DropColumn(
                name: "ModificationDate",
                table: "Purchases");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateUpdated",
                table: "Purchases",
                type: "datetime2",
                nullable: true);
        }
    }
}
