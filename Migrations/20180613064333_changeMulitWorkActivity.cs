using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace VipcoQualityControl.Migrations
{
    public partial class changeMulitWorkActivity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TypeWorkActivity",
                table: "WorkActivity",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "QualityControlWelding",
                columns: table => new
                {
                    Creator = table.Column<string>(maxLength: 50, nullable: true),
                    CreateDate = table.Column<DateTime>(nullable: true),
                    Modifyer = table.Column<string>(maxLength: 50, nullable: true),
                    ModifyDate = table.Column<DateTime>(nullable: true),
                    QualityControlWeldingId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    WeldingDate = table.Column<DateTime>(nullable: false),
                    MarkNo = table.Column<string>(maxLength: 100, nullable: true),
                    MarkNoPreview = table.Column<string>(maxLength: 100, nullable: true),
                    WelderNo = table.Column<string>(maxLength: 100, nullable: true),
                    ProcessWeld = table.Column<string>(maxLength: 100, nullable: true),
                    JointNo = table.Column<int>(nullable: true),
                    Thickness = table.Column<double>(nullable: true),
                    TestLength = table.Column<double>(nullable: true),
                    FailLength = table.Column<double>(nullable: true),
                    Reject = table.Column<double>(nullable: true),
                    Remark = table.Column<string>(maxLength: 250, nullable: true),
                    ParentQcWeldingId = table.Column<int>(nullable: true),
                    ResponseBy = table.Column<string>(nullable: true),
                    RequireQualityControlId = table.Column<int>(nullable: true),
                    ProjectCodeMasterId = table.Column<int>(nullable: true),
                    QualityControlReasonId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QualityControlWelding", x => x.QualityControlWeldingId);
                    table.ForeignKey(
                        name: "FK_QualityControlWelding_QualityControlWelding_ParentQcWeldingId",
                        column: x => x.ParentQcWeldingId,
                        principalTable: "QualityControlWelding",
                        principalColumn: "QualityControlWeldingId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_QualityControlWelding_QualityControlReason_QualityControlReasonId",
                        column: x => x.QualityControlReasonId,
                        principalTable: "QualityControlReason",
                        principalColumn: "QualityControlReasonId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_QualityControlWelding_RequireQualityControl_RequireQualityControlId",
                        column: x => x.RequireQualityControlId,
                        principalTable: "RequireQualityControl",
                        principalColumn: "RequireQualityControlId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RequireQcMoreWorkActvity",
                columns: table => new
                {
                    Creator = table.Column<string>(maxLength: 50, nullable: true),
                    CreateDate = table.Column<DateTime>(nullable: true),
                    Modifyer = table.Column<string>(maxLength: 50, nullable: true),
                    ModifyDate = table.Column<DateTime>(nullable: true),
                    RequireQcMoreWorkActvityId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RequireQualityControlId = table.Column<int>(nullable: true),
                    WorkActivityId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequireQcMoreWorkActvity", x => x.RequireQcMoreWorkActvityId);
                    table.ForeignKey(
                        name: "FK_RequireQcMoreWorkActvity_RequireQualityControl_RequireQualityControlId",
                        column: x => x.RequireQualityControlId,
                        principalTable: "RequireQualityControl",
                        principalColumn: "RequireQualityControlId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RequireQcMoreWorkActvity_WorkActivity_WorkActivityId",
                        column: x => x.WorkActivityId,
                        principalTable: "WorkActivity",
                        principalColumn: "WorkActivityId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_QualityControlWelding_ParentQcWeldingId",
                table: "QualityControlWelding",
                column: "ParentQcWeldingId");

            migrationBuilder.CreateIndex(
                name: "IX_QualityControlWelding_QualityControlReasonId",
                table: "QualityControlWelding",
                column: "QualityControlReasonId");

            migrationBuilder.CreateIndex(
                name: "IX_QualityControlWelding_RequireQualityControlId",
                table: "QualityControlWelding",
                column: "RequireQualityControlId");

            migrationBuilder.CreateIndex(
                name: "IX_RequireQcMoreWorkActvity_RequireQualityControlId",
                table: "RequireQcMoreWorkActvity",
                column: "RequireQualityControlId");

            migrationBuilder.CreateIndex(
                name: "IX_RequireQcMoreWorkActvity_WorkActivityId",
                table: "RequireQcMoreWorkActvity",
                column: "WorkActivityId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QualityControlWelding");

            migrationBuilder.DropTable(
                name: "RequireQcMoreWorkActvity");

            migrationBuilder.DropColumn(
                name: "TypeWorkActivity",
                table: "WorkActivity");
        }
    }
}
