using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GimenaCreations.Data.Migrations
{
    public partial class ChangeColumnName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RequireFile",
                table: "OrderItems",
                newName: "RequiresFile");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RequiresFile",
                table: "OrderItems",
                newName: "RequireFile");
        }
    }
}
