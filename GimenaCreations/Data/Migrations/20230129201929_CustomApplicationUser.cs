using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GimenaCreations.Data.Migrations
{
    public partial class CustomApplicationUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AspNetUsersId",
                table: "Orders");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "Orders",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_ApplicationUserId",
                table: "Orders",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_AspNetUsers_ApplicationUserId",
                table: "Orders",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_AspNetUsers_ApplicationUserId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_ApplicationUserId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Orders");

            migrationBuilder.AddColumn<string>(
                name: "AspNetUsersId",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
