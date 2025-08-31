using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FirstCall.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class alterProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Product_ProductCategories_ProductDefaultCategoryId",
                table: "Product");

            migrationBuilder.DropForeignKey(
                name: "FK_Product_ProductCategories_ProductParentCategoryId",
                table: "Product");

            migrationBuilder.DropForeignKey(
                name: "FK_Product_ProductCategories_ProductSubCategoryId",
                table: "Product");

            migrationBuilder.DropForeignKey(
                name: "FK_Product_ProductCategories_ProductSubSubCategoryId",
                table: "Product");

            migrationBuilder.DropForeignKey(
                name: "FK_Product_ProductCategories_ProductSubSubSubCategoryId",
                table: "Product");

            migrationBuilder.DropIndex(
                name: "IX_Product_ProductDefaultCategoryId",
                table: "Product");

            migrationBuilder.DropIndex(
                name: "IX_Product_ProductParentCategoryId",
                table: "Product");

            migrationBuilder.DropIndex(
                name: "IX_Product_ProductSubCategoryId",
                table: "Product");

            migrationBuilder.DropIndex(
                name: "IX_Product_ProductSubSubCategoryId",
                table: "Product");

            migrationBuilder.DropIndex(
                name: "IX_Product_ProductSubSubSubCategoryId",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "IsVisible",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "ProductDefaultCategoryId",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "ProductParentCategoryId",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "ProductSubCategoryId",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "ProductSubSubCategoryId",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "ProductSubSubSubCategoryId",
                table: "Product");

            migrationBuilder.RenameColumn(
                name: "Weight",
                table: "Product",
                newName: "StorgePlace");

            migrationBuilder.RenameColumn(
                name: "DescriptionAboutEn",
                table: "Product",
                newName: "Sizes");

            migrationBuilder.RenameColumn(
                name: "DescriptionAboutAr",
                table: "Product",
                newName: "ProductImageUrl4");

            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "Product",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Colors",
                table: "Product",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "KindId",
                table: "Product",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "PackageNumber",
                table: "Product",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProductImageUrl2",
                table: "Product",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProductImageUrl3",
                table: "Product",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Qty",
                table: "Product",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Product_CategoryId",
                table: "Product",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Product_KindId",
                table: "Product",
                column: "KindId");

            migrationBuilder.AddForeignKey(
                name: "FK_Product_Categories_CategoryId",
                table: "Product",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Product_Kinds_KindId",
                table: "Product",
                column: "KindId",
                principalTable: "Kinds",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Product_Categories_CategoryId",
                table: "Product");

            migrationBuilder.DropForeignKey(
                name: "FK_Product_Kinds_KindId",
                table: "Product");

            migrationBuilder.DropIndex(
                name: "IX_Product_CategoryId",
                table: "Product");

            migrationBuilder.DropIndex(
                name: "IX_Product_KindId",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "Colors",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "KindId",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "PackageNumber",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "ProductImageUrl2",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "ProductImageUrl3",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "Qty",
                table: "Product");

            migrationBuilder.RenameColumn(
                name: "StorgePlace",
                table: "Product",
                newName: "Weight");

            migrationBuilder.RenameColumn(
                name: "Sizes",
                table: "Product",
                newName: "DescriptionAboutEn");

            migrationBuilder.RenameColumn(
                name: "ProductImageUrl4",
                table: "Product",
                newName: "DescriptionAboutAr");

            migrationBuilder.AddColumn<bool>(
                name: "IsVisible",
                table: "Product",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "ProductDefaultCategoryId",
                table: "Product",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProductParentCategoryId",
                table: "Product",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProductSubCategoryId",
                table: "Product",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProductSubSubCategoryId",
                table: "Product",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProductSubSubSubCategoryId",
                table: "Product",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Product_ProductDefaultCategoryId",
                table: "Product",
                column: "ProductDefaultCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Product_ProductParentCategoryId",
                table: "Product",
                column: "ProductParentCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Product_ProductSubCategoryId",
                table: "Product",
                column: "ProductSubCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Product_ProductSubSubCategoryId",
                table: "Product",
                column: "ProductSubSubCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Product_ProductSubSubSubCategoryId",
                table: "Product",
                column: "ProductSubSubSubCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Product_ProductCategories_ProductDefaultCategoryId",
                table: "Product",
                column: "ProductDefaultCategoryId",
                principalTable: "ProductCategories",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Product_ProductCategories_ProductParentCategoryId",
                table: "Product",
                column: "ProductParentCategoryId",
                principalTable: "ProductCategories",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Product_ProductCategories_ProductSubCategoryId",
                table: "Product",
                column: "ProductSubCategoryId",
                principalTable: "ProductCategories",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Product_ProductCategories_ProductSubSubCategoryId",
                table: "Product",
                column: "ProductSubSubCategoryId",
                principalTable: "ProductCategories",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Product_ProductCategories_ProductSubSubSubCategoryId",
                table: "Product",
                column: "ProductSubSubSubCategoryId",
                principalTable: "ProductCategories",
                principalColumn: "Id");
        }
    }
}
