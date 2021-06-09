using Microsoft.EntityFrameworkCore.Migrations;

namespace InventoryPOS.DataStore.Migrations
{
    public partial class fixedtypoerrorSalesInvoicetoSaleInvoiceEntityandNavigationproperties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_SalesInvoices_SaleInvoiceId",
                table: "Payments");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductSales_SalesInvoices_SaleInvoiceId",
                table: "ProductSales");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SalesInvoices",
                table: "SalesInvoices");

            migrationBuilder.DropColumn(
                name: "SalesInvoiceId",
                table: "ProductSales");

            migrationBuilder.RenameTable(
                name: "SalesInvoices",
                newName: "SaleInvoices");

            migrationBuilder.AlterColumn<int>(
                name: "SaleInvoiceId",
                table: "ProductSales",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_SaleInvoices",
                table: "SaleInvoices",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_SaleInvoices_SaleInvoiceId",
                table: "Payments",
                column: "SaleInvoiceId",
                principalTable: "SaleInvoices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductSales_SaleInvoices_SaleInvoiceId",
                table: "ProductSales",
                column: "SaleInvoiceId",
                principalTable: "SaleInvoices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_SaleInvoices_SaleInvoiceId",
                table: "Payments");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductSales_SaleInvoices_SaleInvoiceId",
                table: "ProductSales");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SaleInvoices",
                table: "SaleInvoices");

            migrationBuilder.RenameTable(
                name: "SaleInvoices",
                newName: "SalesInvoices");

            migrationBuilder.AlterColumn<int>(
                name: "SaleInvoiceId",
                table: "ProductSales",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "SalesInvoiceId",
                table: "ProductSales",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_SalesInvoices",
                table: "SalesInvoices",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_SalesInvoices_SaleInvoiceId",
                table: "Payments",
                column: "SaleInvoiceId",
                principalTable: "SalesInvoices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductSales_SalesInvoices_SaleInvoiceId",
                table: "ProductSales",
                column: "SaleInvoiceId",
                principalTable: "SalesInvoices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
