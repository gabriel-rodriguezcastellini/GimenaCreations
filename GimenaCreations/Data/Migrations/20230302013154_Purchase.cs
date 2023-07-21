using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GimenaCreations.Data.Migrations
{
    /// <inheritdoc />
    public partial class Purchase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPaid",
                table: "Purchases");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "PurchaseItems");

            migrationBuilder.RenameColumn(
                name: "Reference",
                table: "Purchases",
                newName: "ShippingCompany");

            migrationBuilder.RenameColumn(
                name: "InvoiceNumber",
                table: "Purchases",
                newName: "ShippingCity");

            migrationBuilder.RenameColumn(
                name: "InvoiceExpirationDate",
                table: "Purchases",
                newName: "PurchaseDate");

            migrationBuilder.RenameColumn(
                name: "InvoiceDate",
                table: "Purchases",
                newName: "PaymentDeadline");

            migrationBuilder.AlterColumn<string>(
                name: "SupplierId",
                table: "Suppliers",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "Purchases",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Comments",
                table: "Purchases",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Deadline",
                table: "Purchases",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<decimal>(
                name: "Discount",
                table: "Purchases",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Importance",
                table: "Purchases",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PaymentMethod",
                table: "Purchases",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "Purchases",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RecipientName",
                table: "Purchases",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ShippingAddress",
                table: "Purchases",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "Tax",
                table: "Purchases",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Suppliers_SupplierId",
                table: "Suppliers",
                column: "SupplierId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Purchases_ApplicationUserId",
                table: "Purchases",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Purchases_AspNetUsers_ApplicationUserId",
                table: "Purchases",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Purchases_AspNetUsers_ApplicationUserId",
                table: "Purchases");

            migrationBuilder.DropIndex(
                name: "IX_Suppliers_SupplierId",
                table: "Suppliers");

            migrationBuilder.DropIndex(
                name: "IX_Purchases_ApplicationUserId",
                table: "Purchases");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Purchases");

            migrationBuilder.DropColumn(
                name: "Comments",
                table: "Purchases");

            migrationBuilder.DropColumn(
                name: "Deadline",
                table: "Purchases");

            migrationBuilder.DropColumn(
                name: "Discount",
                table: "Purchases");

            migrationBuilder.DropColumn(
                name: "Importance",
                table: "Purchases");

            migrationBuilder.DropColumn(
                name: "PaymentMethod",
                table: "Purchases");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "Purchases");

            migrationBuilder.DropColumn(
                name: "RecipientName",
                table: "Purchases");

            migrationBuilder.DropColumn(
                name: "ShippingAddress",
                table: "Purchases");

            migrationBuilder.DropColumn(
                name: "Tax",
                table: "Purchases");

            migrationBuilder.RenameColumn(
                name: "ShippingCompany",
                table: "Purchases",
                newName: "Reference");

            migrationBuilder.RenameColumn(
                name: "ShippingCity",
                table: "Purchases",
                newName: "InvoiceNumber");

            migrationBuilder.RenameColumn(
                name: "PurchaseDate",
                table: "Purchases",
                newName: "InvoiceExpirationDate");

            migrationBuilder.RenameColumn(
                name: "PaymentDeadline",
                table: "Purchases",
                newName: "InvoiceDate");

            migrationBuilder.AlterColumn<string>(
                name: "SupplierId",
                table: "Suppliers",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<bool>(
                name: "IsPaid",
                table: "Purchases",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "PurchaseItems",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
