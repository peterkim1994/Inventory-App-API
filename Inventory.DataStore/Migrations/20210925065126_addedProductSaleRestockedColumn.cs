using Microsoft.EntityFrameworkCore.Migrations;

namespace InventoryPOS.DataStore.Migrations
{
    public partial class addedProductSaleRestockedColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Restocked",
                table: "ProductSales",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Restocked",
                table: "ProductSales");
        }
    }
}
