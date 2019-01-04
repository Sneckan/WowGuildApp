using Microsoft.EntityFrameworkCore.Migrations;

namespace WowGuildApp.Migrations
{
    public partial class battletag : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BattleTag",
                table: "AspNetUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BattleTag",
                table: "AspNetUsers");
        }
    }
}
