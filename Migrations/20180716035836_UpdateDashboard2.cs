using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace VipcoQualityControl.Migrations
{
    public partial class UpdateDashboard2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DashBoardDaily",
                columns: table => new
                {
                    Creator = table.Column<string>(maxLength: 50, nullable: true),
                    CreateDate = table.Column<DateTime>(nullable: true),
                    Modifyer = table.Column<string>(maxLength: 50, nullable: true),
                    ModifyDate = table.Column<DateTime>(nullable: true),
                    DashBoardDailyId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DashBoardDate = table.Column<DateTime>(nullable: true),
                    TotalRequire = table.Column<int>(nullable: true),
                    TotalRequirePass = table.Column<int>(nullable: true),
                    TotalRequireFail = table.Column<int>(nullable: true),
                    Top1Name = table.Column<string>(nullable: true),
                    Top1Require = table.Column<int>(nullable: true),
                    Top2Name = table.Column<string>(nullable: true),
                    Top2Require = table.Column<int>(nullable: true),
                    Top3Name = table.Column<string>(nullable: true),
                    Top3Require = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DashBoardDaily", x => x.DashBoardDailyId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DashBoardDaily");
        }
    }
}
