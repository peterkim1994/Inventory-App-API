using Microsoft.EntityFrameworkCore.Migrations;

namespace InventoryPOS.DataStore.Migrations
{
    public partial class addedCancelToProductSaleEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Canceled",
                table: "ProductSales",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Canceled",
                table: "ProductSales");
        }
    }
}
