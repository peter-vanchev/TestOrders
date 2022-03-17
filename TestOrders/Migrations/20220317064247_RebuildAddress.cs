using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TestOrders.Migrations
{
    public partial class RebuildAddress : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StreetNumber",
                table: "Addresses",
                newName: "Street");

            migrationBuilder.AlterColumn<string>(
                name: "Url",
                table: "Cars",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AddColumn<string>(
                name: "Number",
                table: "Addresses",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Number",
                table: "Addresses");

            migrationBuilder.RenameColumn(
                name: "Street",
                table: "Addresses",
                newName: "StreetNumber");

            migrationBuilder.AlterColumn<string>(
                name: "Url",
                table: "Cars",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);
        }
    }
}
