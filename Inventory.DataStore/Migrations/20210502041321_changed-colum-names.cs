using Microsoft.EntityFrameworkCore.Migrations;

namespace InventoryPOS.DataStore.Migrations
{
    public partial class changedcolumnames : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SizeName",
                table: "Sizes",
                newName: "Value");

            migrationBuilder.RenameIndex(
                name: "IX_Sizes_SizeName",
                table: "Sizes",
                newName: "IX_Sizes_Value");

            migrationBuilder.RenameColumn(
                name: "Category",
                table: "ItemCategories",
                newName: "Value");

            migrationBuilder.RenameIndex(
                name: "IX_ItemCategories_Category",
                table: "ItemCategories",
                newName: "IX_ItemCategories_Value");

            migrationBuilder.RenameColumn(
                name: "ColourName",
                table: "Colours",
                newName: "Value");

            migrationBuilder.RenameIndex(
                name: "IX_Colours_ColourName",
                table: "Colours",
                newName: "IX_Colours_Value");

            migrationBuilder.RenameColumn(
                name: "BrandName",
                table: "Brands",
                newName: "Value");

            migrationBuilder.RenameIndex(
                name: "IX_Brands_BrandName",
                table: "Brands",
                newName: "IX_Brands_Value");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Value",
                table: "Sizes",
                newName: "SizeName");

            migrationBuilder.RenameIndex(
                name: "IX_Sizes_Value",
                table: "Sizes",
                newName: "IX_Sizes_SizeName");

            migrationBuilder.RenameColumn(
                name: "Value",
                table: "ItemCategories",
                newName: "Category");

            migrationBuilder.RenameIndex(
                name: "IX_ItemCategories_Value",
                table: "ItemCategories",
                newName: "IX_ItemCategories_Category");

            migrationBuilder.RenameColumn(
                name: "Value",
                table: "Colours",
                newName: "ColourName");

            migrationBuilder.RenameIndex(
                name: "IX_Colours_Value",
                table: "Colours",
                newName: "IX_Colours_ColourName");

            migrationBuilder.RenameColumn(
                name: "Value",
                table: "Brands",
                newName: "BrandName");

            migrationBuilder.RenameIndex(
                name: "IX_Brands_Value",
                table: "Brands",
                newName: "IX_Brands_BrandName");
        }
    }
}
