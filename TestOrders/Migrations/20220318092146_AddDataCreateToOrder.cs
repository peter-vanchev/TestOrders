using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TestOrders.Migrations
{
    public partial class AddDataCreateToOrder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Create",
                table: "OrderDatas");

            migrationBuilder.AddColumn<DateTime>(
                name: "Create",
                table: "Orders",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "MyProperty",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Create",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "MyProperty",
                table: "Orders");

            migrationBuilder.AddColumn<DateTime>(
                name: "Create",
                table: "OrderDatas",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
