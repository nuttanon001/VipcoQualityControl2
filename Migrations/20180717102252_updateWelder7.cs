using Microsoft.EntityFrameworkCore.Migrations;

namespace VipcoQualityControl.Migrations
{
    public partial class updateWelder7 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "WelderNo1Name",
                table: "RequireHasWelder",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WelderNo2Name",
                table: "RequireHasWelder",
                maxLength: 200,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WelderNo1Name",
                table: "RequireHasWelder");

            migrationBuilder.DropColumn(
                name: "WelderNo2Name",
                table: "RequireHasWelder");
        }
    }
}
