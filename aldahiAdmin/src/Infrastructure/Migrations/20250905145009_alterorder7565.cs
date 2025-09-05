using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FirstCall.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class alterorder7565 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "DeliveryOrders",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "DeliveryOrders");
        }
    }
}
