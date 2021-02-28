using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Teashop.Backend.Infrastructure.Persistence.Migrations
{
    public partial class AddOrderNoAndCreatedAtToOrder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.CreateSequence<int>(
                name: "OrderNumbers",
                schema: "dbo",
                startValue: 100000L);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Orders",
                nullable: false,
                defaultValueSql: "getdate()");

            migrationBuilder.AddColumn<int>(
                name: "OrderNo",
                table: "Orders",
                nullable: false,
                defaultValueSql: "NEXT VALUE FOR dbo.OrderNumbers");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_OrderNo",
                table: "Orders",
                column: "OrderNo",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Orders_OrderNo",
                table: "Orders");

            migrationBuilder.DropSequence(
                name: "OrderNumbers",
                schema: "dbo");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "OrderNo",
                table: "Orders");
        }
    }
}
