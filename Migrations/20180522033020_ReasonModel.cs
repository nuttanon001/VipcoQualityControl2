using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace VipcoQualityControl.Migrations
{
    public partial class ReasonModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "QualityControlReasonId",
                table: "RequireHasMasterProject",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "QualityControlReason",
                columns: table => new
                {
                    Creator = table.Column<string>(maxLength: 50, nullable: true),
                    CreateDate = table.Column<DateTime>(nullable: true),
                    Modifyer = table.Column<string>(maxLength: 50, nullable: true),
                    ModifyDate = table.Column<DateTime>(nullable: true),
                    QualityControlReasonId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 50, nullable: true),
                    Description = table.Column<string>(maxLength: 250, nullable: true),
                    Remark = table.Column<string>(maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QualityControlReason", x => x.QualityControlReasonId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RequireHasMasterProject_QualityControlReasonId",
                table: "RequireHasMasterProject",
                column: "QualityControlReasonId");

            migrationBuilder.AddForeignKey(
                name: "FK_RequireHasMasterProject_QualityControlReason_QualityControlReasonId",
                table: "RequireHasMasterProject",
                column: "QualityControlReasonId",
                principalTable: "QualityControlReason",
                principalColumn: "QualityControlReasonId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RequireHasMasterProject_QualityControlReason_QualityControlReasonId",
                table: "RequireHasMasterProject");

            migrationBuilder.DropTable(
                name: "QualityControlReason");

            migrationBuilder.DropIndex(
                name: "IX_RequireHasMasterProject_QualityControlReasonId",
                table: "RequireHasMasterProject");

            migrationBuilder.DropColumn(
                name: "QualityControlReasonId",
                table: "RequireHasMasterProject");
        }
    }
}
