using Microsoft.EntityFrameworkCore.Migrations;

namespace VipcoQualityControl.Migrations
{
    public partial class updateWelded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "GradeMaterial1",
                table: "MasterProjectList",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GradeMaterial2",
                table: "MasterProjectList",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "JointNumber",
                table: "MasterProjectList",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Thickness",
                table: "MasterProjectList",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TypeMaterial1",
                table: "MasterProjectList",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TypeMaterial2",
                table: "MasterProjectList",
                maxLength: 200,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GradeMaterial1",
                table: "MasterProjectList");

            migrationBuilder.DropColumn(
                name: "GradeMaterial2",
                table: "MasterProjectList");

            migrationBuilder.DropColumn(
                name: "JointNumber",
                table: "MasterProjectList");

            migrationBuilder.DropColumn(
                name: "Thickness",
                table: "MasterProjectList");

            migrationBuilder.DropColumn(
                name: "TypeMaterial1",
                table: "MasterProjectList");

            migrationBuilder.DropColumn(
                name: "TypeMaterial2",
                table: "MasterProjectList");
        }
    }
}
