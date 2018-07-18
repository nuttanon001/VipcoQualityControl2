using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace VipcoQualityControl.Migrations
{
    public partial class updateWelder3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RequireHasWelder",
                columns: table => new
                {
                    Creator = table.Column<string>(maxLength: 50, nullable: true),
                    CreateDate = table.Column<DateTime>(nullable: true),
                    Modifyer = table.Column<string>(maxLength: 50, nullable: true),
                    ModifyDate = table.Column<DateTime>(nullable: true),
                    RequireHasWelderId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    VTStaus = table.Column<int>(nullable: true),
                    QcStatus = table.Column<int>(nullable: true),
                    WelderProcess = table.Column<int>(nullable: true),
                    WelderDate = table.Column<DateTime>(nullable: true),
                    PercentNDE = table.Column<double>(nullable: false),
                    Remark = table.Column<string>(maxLength: 200, nullable: true),
                    RequireHasMasterProjectId = table.Column<int>(nullable: false),
                    WelderNo1Id = table.Column<int>(nullable: false),
                    WelderNo2Id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequireHasWelder", x => x.RequireHasWelderId);
                    table.ForeignKey(
                        name: "FK_RequireHasWelder_RequireHasMasterProject_RequireHasMasterProjectId",
                        column: x => x.RequireHasMasterProjectId,
                        principalTable: "RequireHasMasterProject",
                        principalColumn: "RequireHasMasterProjectId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WelderNo",
                columns: table => new
                {
                    Creator = table.Column<string>(maxLength: 50, nullable: true),
                    CreateDate = table.Column<DateTime>(nullable: true),
                    Modifyer = table.Column<string>(maxLength: 50, nullable: true),
                    ModifyDate = table.Column<DateTime>(nullable: true),
                    WelderNoId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    WelderNoCode = table.Column<string>(nullable: true),
                    RegisterDate = table.Column<DateTime>(nullable: true),
                    ExprireDate = table.Column<DateTime>(nullable: true),
                    Description = table.Column<string>(maxLength: 200, nullable: true),
                    Remark = table.Column<string>(maxLength: 200, nullable: true),
                    EmpCode = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WelderNo", x => x.WelderNoId);
                });

            migrationBuilder.CreateTable(
                name: "WelderHasProject",
                columns: table => new
                {
                    Creator = table.Column<string>(maxLength: 50, nullable: true),
                    CreateDate = table.Column<DateTime>(nullable: true),
                    Modifyer = table.Column<string>(maxLength: 50, nullable: true),
                    ModifyDate = table.Column<DateTime>(nullable: true),
                    WelderHasProjectId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(maxLength: 200, nullable: true),
                    Remark = table.Column<string>(maxLength: 200, nullable: true),
                    WelderNoId = table.Column<int>(nullable: true),
                    ProjectCodeMasterId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WelderHasProject", x => x.WelderHasProjectId);
                    table.ForeignKey(
                        name: "FK_WelderHasProject_WelderNo_WelderNoId",
                        column: x => x.WelderNoId,
                        principalTable: "WelderNo",
                        principalColumn: "WelderNoId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RequireHasWelder_RequireHasMasterProjectId",
                table: "RequireHasWelder",
                column: "RequireHasMasterProjectId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WelderHasProject_WelderNoId",
                table: "WelderHasProject",
                column: "WelderNoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RequireHasWelder");

            migrationBuilder.DropTable(
                name: "WelderHasProject");

            migrationBuilder.DropTable(
                name: "WelderNo");
        }
    }
}
