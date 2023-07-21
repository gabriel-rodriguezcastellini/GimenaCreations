using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GimenaCreations.Data.Migrations
{
    public partial class LoginLogoutAuditApplicationUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "LoginLogoutAudits");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "LoginLogoutAudits",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_LoginLogoutAudits_ApplicationUserId",
                table: "LoginLogoutAudits",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_LoginLogoutAudits_AspNetUsers_ApplicationUserId",
                table: "LoginLogoutAudits",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LoginLogoutAudits_AspNetUsers_ApplicationUserId",
                table: "LoginLogoutAudits");

            migrationBuilder.DropIndex(
                name: "IX_LoginLogoutAudits_ApplicationUserId",
                table: "LoginLogoutAudits");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "LoginLogoutAudits");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "LoginLogoutAudits",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
