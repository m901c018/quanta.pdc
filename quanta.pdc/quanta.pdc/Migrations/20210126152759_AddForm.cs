using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace cns.Migrations
{
    public partial class AddForm : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PDC_Form",
                columns: table => new
                {
                    FormID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AppliedFormNo = table.Column<string>(maxLength: 64, nullable: false),
                    FormStatus = table.Column<string>(maxLength: 10, nullable: false),
                    CompCode = table.Column<string>(maxLength: 32, nullable: true),
                    BUCode = table.Column<string>(maxLength: 32, nullable: true),
                    ApplierID = table.Column<string>(nullable: false),
                    PCBLayoutStatus = table.Column<string>(maxLength: 32, nullable: true),
                    IsMB = table.Column<bool>(nullable: false),
                    PCBType = table.Column<string>(nullable: true),
                    ProjectName = table.Column<string>(maxLength: 64, nullable: true),
                    BoardTypeName = table.Column<string>(maxLength: 64, nullable: true),
                    Revision = table.Column<string>(maxLength: 256, nullable: true),
                    Creator = table.Column<string>(maxLength: 128, nullable: false),
                    CreatorName = table.Column<string>(maxLength: 128, nullable: false),
                    CreatorDate = table.Column<DateTime>(nullable: false),
                    Modifyer = table.Column<string>(maxLength: 128, nullable: true),
                    ModifyerName = table.Column<string>(maxLength: 128, nullable: true),
                    ModifyerDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PDC_Form", x => x.FormID);
                });

            migrationBuilder.CreateTable(
                name: "PDC_Form_StageLog",
                columns: table => new
                {
                    StageLogID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    FormID = table.Column<long>(nullable: false),
                    StageName = table.Column<string>(maxLength: 128, nullable: true),
                    Result = table.Column<string>(nullable: true),
                    WorkHour = table.Column<decimal>(nullable: false),
                    Creator = table.Column<string>(maxLength: 128, nullable: false),
                    CreatorName = table.Column<string>(maxLength: 128, nullable: false),
                    CreatorDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PDC_Form_StageLog", x => x.StageLogID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PDC_Form");

            migrationBuilder.DropTable(
                name: "PDC_Form_StageLog");
        }
    }
}
