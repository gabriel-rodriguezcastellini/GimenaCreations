using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GimenaCreations.Data.Migrations
{
    public partial class Supplier : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Website",
                table: "Suppliers",
                newName: "SupplierType");

            migrationBuilder.RenameColumn(
                name: "Phone",
                table: "Suppliers",
                newName: "SupplierId");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Suppliers",
                newName: "State");

            migrationBuilder.RenameColumn(
                name: "Cuit",
                table: "Suppliers",
                newName: "Phone2");

            migrationBuilder.RenameColumn(
                name: "CompanyAddress",
                table: "Suppliers",
                newName: "Phone1");

            migrationBuilder.RenameColumn(
                name: "AfipResponsibility",
                table: "Suppliers",
                newName: "ContactName");

            migrationBuilder.AddColumn<DateTime>(
                name: "AddDate",
                table: "Suppliers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Suppliers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "Agreement",
                table: "Suppliers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "BusinessName",
                table: "Suppliers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "Suppliers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ContactPhone",
                table: "Suppliers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "Image",
                table: "Suppliers",
                type: "varbinary(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AddDate",
                table: "Suppliers");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "Suppliers");

            migrationBuilder.DropColumn(
                name: "Agreement",
                table: "Suppliers");

            migrationBuilder.DropColumn(
                name: "BusinessName",
                table: "Suppliers");

            migrationBuilder.DropColumn(
                name: "City",
                table: "Suppliers");

            migrationBuilder.DropColumn(
                name: "ContactPhone",
                table: "Suppliers");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "Suppliers");

            migrationBuilder.RenameColumn(
                name: "SupplierType",
                table: "Suppliers",
                newName: "Website");

            migrationBuilder.RenameColumn(
                name: "SupplierId",
                table: "Suppliers",
                newName: "Phone");

            migrationBuilder.RenameColumn(
                name: "State",
                table: "Suppliers",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "Phone2",
                table: "Suppliers",
                newName: "Cuit");

            migrationBuilder.RenameColumn(
                name: "Phone1",
                table: "Suppliers",
                newName: "CompanyAddress");

            migrationBuilder.RenameColumn(
                name: "ContactName",
                table: "Suppliers",
                newName: "AfipResponsibility");
        }
    }
}
