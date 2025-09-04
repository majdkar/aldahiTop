using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FirstCall.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addProduct5456 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StorgePlace",
                table: "Product");

            migrationBuilder.AddColumn<int>(
                name: "WarehousesId",
                table: "Product",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Product_WarehousesId",
                table: "Product",
                column: "WarehousesId");

            migrationBuilder.AddForeignKey(
                name: "FK_Product_Warehousess_WarehousesId",
                table: "Product",
                column: "WarehousesId",
                principalTable: "Warehousess",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Product_Warehousess_WarehousesId",
                table: "Product");

            migrationBuilder.DropIndex(
                name: "IX_Product_WarehousesId",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "WarehousesId",
                table: "Product");

            migrationBuilder.AddColumn<string>(
                name: "StorgePlace",
                table: "Product",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
