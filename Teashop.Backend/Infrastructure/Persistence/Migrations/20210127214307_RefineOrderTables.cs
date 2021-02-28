using Microsoft.EntityFrameworkCore.Migrations;

namespace Teashop.Backend.Infrastructure.Persistence.Migrations
{
    public partial class RefineOrderTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "ShippingMethods");

            migrationBuilder.AddColumn<double>(
                name: "Fee",
                table: "ShippingMethods",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "ShippingMethodNo",
                table: "ShippingMethods",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "Fee",
                table: "PaymentMethods",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "PaymentMethodNo",
                table: "PaymentMethods",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "TotalPrice",
                table: "Orders",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Fee",
                table: "ShippingMethods");

            migrationBuilder.DropColumn(
                name: "ShippingMethodNo",
                table: "ShippingMethods");

            migrationBuilder.DropColumn(
                name: "Fee",
                table: "PaymentMethods");

            migrationBuilder.DropColumn(
                name: "PaymentMethodNo",
                table: "PaymentMethods");

            migrationBuilder.DropColumn(
                name: "TotalPrice",
                table: "Orders");

            migrationBuilder.AddColumn<double>(
                name: "Price",
                table: "ShippingMethods",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
