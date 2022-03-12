using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TestOrders.Migrations
{
    public partial class AddDateCreateRest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Time",
                table: "OrderStatuses",
                newName: "LastUpdate");

            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "Restaurants",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "OrderStatuses",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Create",
                table: "OrderStatuses",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "RestaurantId",
                table: "OrderStatuses",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "RestaurantViewModel",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Category = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Created = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumner = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Town = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Street = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Number = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserPassword = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConfirmPassword = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RestaurantViewModel", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderStatuses_ApplicationUserId",
                table: "OrderStatuses",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderStatuses_RestaurantId",
                table: "OrderStatuses",
                column: "RestaurantId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderStatuses_AspNetUsers_ApplicationUserId",
                table: "OrderStatuses",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderStatuses_Restaurants_RestaurantId",
                table: "OrderStatuses",
                column: "RestaurantId",
                principalTable: "Restaurants",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderStatuses_AspNetUsers_ApplicationUserId",
                table: "OrderStatuses");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderStatuses_Restaurants_RestaurantId",
                table: "OrderStatuses");

            migrationBuilder.DropTable(
                name: "RestaurantViewModel");

            migrationBuilder.DropIndex(
                name: "IX_OrderStatuses_ApplicationUserId",
                table: "OrderStatuses");

            migrationBuilder.DropIndex(
                name: "IX_OrderStatuses_RestaurantId",
                table: "OrderStatuses");

            migrationBuilder.DropColumn(
                name: "Created",
                table: "Restaurants");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "OrderStatuses");

            migrationBuilder.DropColumn(
                name: "Create",
                table: "OrderStatuses");

            migrationBuilder.DropColumn(
                name: "RestaurantId",
                table: "OrderStatuses");

            migrationBuilder.RenameColumn(
                name: "LastUpdate",
                table: "OrderStatuses",
                newName: "Time");
        }
    }
}
