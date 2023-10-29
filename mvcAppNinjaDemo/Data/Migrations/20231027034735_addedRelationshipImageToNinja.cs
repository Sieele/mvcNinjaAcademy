using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace mvcAppNinjaDemo.Data.Migrations
{
    public partial class addedRelationshipImageToNinja : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ImageId",
                table: "Ninjas",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NinjaId",
                table: "Images",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Ninjas_ImageId",
                table: "Ninjas",
                column: "ImageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Ninjas_Images_ImageId",
                table: "Ninjas",
                column: "ImageId",
                principalTable: "Images",
                principalColumn: "ImageId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ninjas_Images_ImageId",
                table: "Ninjas");

            migrationBuilder.DropIndex(
                name: "IX_Ninjas_ImageId",
                table: "Ninjas");

            migrationBuilder.DropColumn(
                name: "ImageId",
                table: "Ninjas");

            migrationBuilder.DropColumn(
                name: "NinjaId",
                table: "Images");
        }
    }
}
