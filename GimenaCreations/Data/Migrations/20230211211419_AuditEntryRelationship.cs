using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GimenaCreations.Data.Migrations
{
    public partial class AuditEntryRelationship : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "AuditEntry");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "AuditEntry",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AuditEntry_ApplicationUserId",
                table: "AuditEntry",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_AuditEntry_AspNetUsers_ApplicationUserId",
                table: "AuditEntry",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuditEntry_AspNetUsers_ApplicationUserId",
                table: "AuditEntry");

            migrationBuilder.DropIndex(
                name: "IX_AuditEntry_ApplicationUserId",
                table: "AuditEntry");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "AuditEntry");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "AuditEntry",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
