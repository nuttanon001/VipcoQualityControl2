using Microsoft.EntityFrameworkCore.Migrations;

namespace VipcoQualityControl.Migrations
{
    public partial class UpdateWelder8 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NameEnglish",
                table: "WelderNo",
                maxLength: 250,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NameThai",
                table: "WelderNo",
                maxLength: 250,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NameEnglish",
                table: "WelderNo");

            migrationBuilder.DropColumn(
                name: "NameThai",
                table: "WelderNo");
        }
    }
}
