using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace mvcAppNinjaDemo.Data.Migrations
{
    public partial class addedRelationshipClanToImage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ClanId",
                table: "Images",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Images_ClanId",
                table: "Images",
                column: "ClanId");

            migrationBuilder.AddForeignKey(
                name: "FK_Images_Clans_ClanId",
                table: "Images",
                column: "ClanId",
                principalTable: "Clans",
                principalColumn: "ClanId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Images_Clans_ClanId",
                table: "Images");

            migrationBuilder.DropIndex(
                name: "IX_Images_ClanId",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "ClanId",
                table: "Images");
        }
    }
}
