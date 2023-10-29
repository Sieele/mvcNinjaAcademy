using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace mvcAppNinjaDemo.Data.Migrations
{
    public partial class addedImagePathIsAliveInformationToNinja : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Information",
                table: "Ninjas",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsAlive",
                table: "Ninjas",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NinjaImagePath",
                table: "Ninjas",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Information",
                table: "Ninjas");

            migrationBuilder.DropColumn(
                name: "IsAlive",
                table: "Ninjas");

            migrationBuilder.DropColumn(
                name: "NinjaImagePath",
                table: "Ninjas");
        }
    }
}
