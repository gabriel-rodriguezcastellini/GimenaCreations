using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GimenaCreations.Data.Migrations
{
    public partial class File : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "RequiresFile",
                table: "OrderItems",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RequiresFile",
                table: "OrderItems");
        }
    }
}
