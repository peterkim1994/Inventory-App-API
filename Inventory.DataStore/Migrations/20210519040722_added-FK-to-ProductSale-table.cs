using Microsoft.EntityFrameworkCore.Migrations;

namespace InventoryPOS.DataStore.Migrations
{
    public partial class addedFKtoProductSaletable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_ProductSales_PromotionId",
                table: "ProductSales",
                column: "PromotionId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductSales_Promotions_PromotionId",
                table: "ProductSales",
                column: "PromotionId",
                principalTable: "Promotions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductSales_Promotions_PromotionId",
                table: "ProductSales");

            migrationBuilder.DropIndex(
                name: "IX_ProductSales_PromotionId",
                table: "ProductSales");
        }
    }
}
