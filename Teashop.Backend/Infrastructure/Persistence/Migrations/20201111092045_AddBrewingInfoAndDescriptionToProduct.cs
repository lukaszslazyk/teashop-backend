using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Teashop.Backend.Infrastructure.Persistence.Migrations
{
    public partial class AddBrewingInfoAndDescriptionToProduct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Products",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "BrewingInfo",
                columns: table => new
                {
                    BrewingInfoId = table.Column<Guid>(nullable: false),
                    WeightInfo = table.Column<string>(nullable: true),
                    TemperatureInfo = table.Column<string>(nullable: true),
                    TimeInfo = table.Column<string>(nullable: true),
                    NumberOfBrewingsInfo = table.Column<string>(nullable: true),
                    ProductId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BrewingInfo", x => x.BrewingInfoId);
                    table.ForeignKey(
                        name: "FK_BrewingInfo_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BrewingInfo_ProductId",
                table: "BrewingInfo",
                column: "ProductId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BrewingInfo");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Products");
        }
    }
}
