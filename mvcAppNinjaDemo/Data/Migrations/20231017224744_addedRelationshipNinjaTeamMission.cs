using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace mvcAppNinjaDemo.Data.Migrations
{
    public partial class addedRelationshipNinjaTeamMission : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TeamId",
                table: "Ninjas",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TeamId",
                table: "Missions",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Ninjas_TeamId",
                table: "Ninjas",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_Missions_TeamId",
                table: "Missions",
                column: "TeamId",
                unique: true,
                filter: "[TeamId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Missions_Teams_TeamId",
                table: "Missions",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "TeamId");

            migrationBuilder.AddForeignKey(
                name: "FK_Ninjas_Teams_TeamId",
                table: "Ninjas",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "TeamId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Missions_Teams_TeamId",
                table: "Missions");

            migrationBuilder.DropForeignKey(
                name: "FK_Ninjas_Teams_TeamId",
                table: "Ninjas");

            migrationBuilder.DropIndex(
                name: "IX_Ninjas_TeamId",
                table: "Ninjas");

            migrationBuilder.DropIndex(
                name: "IX_Missions_TeamId",
                table: "Missions");

            migrationBuilder.DropColumn(
                name: "TeamId",
                table: "Ninjas");

            migrationBuilder.DropColumn(
                name: "TeamId",
                table: "Missions");
        }
    }
}
