using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TestOrders.Migrations
{
    public partial class AddMazalo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderDatas_AspNetUsers_ApplicationUserId",
                table: "OrderDatas");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderDatas_Restaurants_RestaurantId",
                table: "OrderDatas");

            migrationBuilder.DropIndex(
                name: "IX_OrderDatas_ApplicationUserId",
                table: "OrderDatas");

            migrationBuilder.DropIndex(
                name: "IX_OrderDatas_RestaurantId",
                table: "OrderDatas");

            migrationBuilder.DropColumn(
                name: "MyProperty",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "OrderDatas");

            migrationBuilder.DropColumn(
                name: "RestaurantId",
                table: "OrderDatas");

            migrationBuilder.AddColumn<Guid>(
                name: "DriverId",
                table: "Orders",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_DriverId",
                table: "Orders",
                column: "DriverId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Drivers_DriverId",
                table: "Orders",
                column: "DriverId",
                principalTable: "Drivers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Drivers_DriverId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_DriverId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "DriverId",
                table: "Orders");

            migrationBuilder.AddColumn<int>(
                name: "MyProperty",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "OrderDatas",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "RestaurantId",
                table: "OrderDatas",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrderDatas_ApplicationUserId",
                table: "OrderDatas",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDatas_RestaurantId",
                table: "OrderDatas",
                column: "RestaurantId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDatas_AspNetUsers_ApplicationUserId",
                table: "OrderDatas",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDatas_Restaurants_RestaurantId",
                table: "OrderDatas",
                column: "RestaurantId",
                principalTable: "Restaurants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
