using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace mvcAppNinjaDemo.Data.Migrations
{
    public partial class addedRelationshipClanToNinja : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ClanId",
                table: "Ninjas",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Ninjas_ClanId",
                table: "Ninjas",
                column: "ClanId");

            migrationBuilder.AddForeignKey(
                name: "FK_Ninjas_Clans_ClanId",
                table: "Ninjas",
                column: "ClanId",
                principalTable: "Clans",
                principalColumn: "ClanId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ninjas_Clans_ClanId",
                table: "Ninjas");

            migrationBuilder.DropIndex(
                name: "IX_Ninjas_ClanId",
                table: "Ninjas");

            migrationBuilder.DropColumn(
                name: "ClanId",
                table: "Ninjas");
        }
    }
}
