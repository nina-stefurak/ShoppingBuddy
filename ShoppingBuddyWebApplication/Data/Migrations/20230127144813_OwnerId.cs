using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShoppingBuddyWebApplication.Data.Migrations
{
    public partial class OwnerId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OwnerId",
                table: "Product",
                type: "nvarchar(450)",
                nullable: true,
                defaultValue: "f3257f74-a195-4add-b893-0b857455c465");

            migrationBuilder.CreateIndex(
                name: "IX_Product_OwnerId",
                table: "Product",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Product_AspNetUsers_OwnerId",
                table: "Product",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Product_AspNetUsers_OwnerId",
                table: "Product");

            migrationBuilder.DropIndex(
                name: "IX_Product_OwnerId",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "Product");
        }
    }
}
