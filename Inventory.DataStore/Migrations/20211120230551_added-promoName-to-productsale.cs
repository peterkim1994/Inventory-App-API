using Microsoft.EntityFrameworkCore.Migrations;

namespace InventoryPOS.DataStore.Migrations
{
    public partial class addedpromoNametoproductsale : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PromotionName",
                table: "ProductSales",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PromotionName",
                table: "ProductSales");
        }
    }
}
