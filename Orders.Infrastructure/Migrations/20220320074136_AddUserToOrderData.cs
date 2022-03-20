using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Orders.Infrastructure.Migrations
{
    public partial class AddUserToOrderData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "OrderDatas",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDatas_UserId",
                table: "OrderDatas",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDatas_AspNetUsers_UserId",
                table: "OrderDatas",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderDatas_AspNetUsers_UserId",
                table: "OrderDatas");

            migrationBuilder.DropIndex(
                name: "IX_OrderDatas_UserId",
                table: "OrderDatas");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "OrderDatas");
        }
    }
}
