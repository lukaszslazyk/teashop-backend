using Microsoft.EntityFrameworkCore.Migrations;

namespace Teashop.Backend.Migrations
{
    public partial class AddProductNumberToProduct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateSequence<int>(
                name: "ProductNumbers",
                schema: "dbo",
                startValue: 100000L);

            migrationBuilder.AddColumn<int>(
                name: "ProductNumber",
                table: "Products",
                nullable: false,
                defaultValueSql: "NEXT VALUE FOR dbo.ProductNumbers");

            migrationBuilder.CreateIndex(
                name: "IX_Products_ProductNumber",
                table: "Products",
                column: "ProductNumber",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Products_ProductNumber",
                table: "Products");

            migrationBuilder.DropSequence(
                name: "ProductNumbers",
                schema: "dbo");

            migrationBuilder.DropColumn(
                name: "ProductNumber",
                table: "Products");
        }
    }
}
