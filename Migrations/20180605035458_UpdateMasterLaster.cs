using Microsoft.EntityFrameworkCore.Migrations;

namespace VipcoQualityControl.Migrations
{
    public partial class UpdateMasterLaster : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Box",
                table: "MasterProjectList",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UnitNo",
                table: "MasterProjectList",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Box",
                table: "MasterProjectList");

            migrationBuilder.DropColumn(
                name: "UnitNo",
                table: "MasterProjectList");
        }
    }
}
