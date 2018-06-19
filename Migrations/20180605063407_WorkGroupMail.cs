using Microsoft.EntityFrameworkCore.Migrations;

namespace VipcoQualityControl.Migrations
{
    public partial class WorkGroupMail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "WorkGroupQualityControl",
                maxLength: 250,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "WorkGroupQualityControl");
        }
    }
}
