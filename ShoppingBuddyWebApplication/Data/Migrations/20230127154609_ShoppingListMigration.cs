using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShoppingBuddyWebApplication.Data.Migrations
{
    public partial class ShoppingListMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Product_AspNetUsers_OwnerId",
                table: "Product");

            migrationBuilder.AlterColumn<string>(
                name: "OwnerId",
                table: "Product",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddForeignKey(
                name: "FK_Product_AspNetUsers_OwnerId",
                table: "Product",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Product_AspNetUsers_OwnerId",
                table: "Product");

            migrationBuilder.AlterColumn<string>(
                name: "OwnerId",
                table: "Product",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Product_AspNetUsers_OwnerId",
                table: "Product",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
