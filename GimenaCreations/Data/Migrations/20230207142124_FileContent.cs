using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GimenaCreations.Data.Migrations
{
    public partial class FileContent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Path",
                table: "Files",
                newName: "Name");

            migrationBuilder.AddColumn<byte[]>(
                name: "Content",
                table: "Files",
                type: "varbinary(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Content",
                table: "Files");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Files",
                newName: "Path");
        }
    }
}
