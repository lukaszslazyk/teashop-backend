using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Teashop.Backend.Infrastructure.Persistence.Migrations
{
    public partial class AddedCreatedAtToCartItem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "CartItems",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "CartItems");
        }
    }
}
