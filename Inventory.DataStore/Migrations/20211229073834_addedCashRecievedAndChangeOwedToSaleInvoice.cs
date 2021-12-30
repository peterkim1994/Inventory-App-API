using Microsoft.EntityFrameworkCore.Migrations;

namespace InventoryPOS.DataStore.Migrations
{
    public partial class addedCashRecievedAndChangeOwedToSaleInvoice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "ChangeGiven",
                table: "SaleInvoices",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<double>(
                name: "TotalCashRecieved",
                table: "SaleInvoices",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChangeGiven",
                table: "SaleInvoices");

            migrationBuilder.DropColumn(
                name: "TotalCashRecieved",
                table: "SaleInvoices");
        }
    }
}
