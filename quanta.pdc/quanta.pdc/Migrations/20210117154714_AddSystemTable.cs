using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace cns.Migrations
{
    public partial class AddSystemTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PDC_File",
                columns: table => new
                {
                    FileID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    FileFullName = table.Column<string>(maxLength: 256, nullable: true),
                    FileName = table.Column<string>(maxLength: 128, nullable: true),
                    FileExtension = table.Column<string>(maxLength: 10, nullable: true),
                    FileSize = table.Column<long>(nullable: false),
                    FileCategory = table.Column<int>(nullable: false),
                    FileType = table.Column<int>(nullable: false),
                    FunctionName = table.Column<string>(nullable: false),
                    SourceID = table.Column<long>(nullable: false),
                    FileNote = table.Column<string>(nullable: true),
                    FileRemark = table.Column<string>(nullable: true),
                    FileDescription = table.Column<string>(nullable: true),
                    Creator = table.Column<string>(maxLength: 128, nullable: false),
                    CreatorName = table.Column<string>(maxLength: 128, nullable: false),
                    CreatorDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PDC_File", x => x.FileID);
                });

            migrationBuilder.CreateTable(
                name: "PDC_Parameter",
                columns: table => new
                {
                    ParameterID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ParameterGroup = table.Column<string>(nullable: true),
                    ParameterParentID = table.Column<long>(nullable: false),
                    ParameterName = table.Column<string>(maxLength: 256, nullable: true),
                    ParameterType = table.Column<string>(nullable: true),
                    ParameterText = table.Column<string>(maxLength: 256, nullable: true),
                    ParameterValue = table.Column<string>(nullable: true),
                    ParameterDesc = table.Column<string>(nullable: true),
                    ParameterNote = table.Column<string>(nullable: true),
                    OrderNo = table.Column<int>(nullable: false),
                    IsSync = table.Column<bool>(nullable: false),
                    Creator = table.Column<string>(maxLength: 128, nullable: false),
                    CreatorName = table.Column<string>(maxLength: 128, nullable: false),
                    CreatorDate = table.Column<DateTime>(nullable: false),
                    Modifyer = table.Column<string>(maxLength: 128, nullable: true),
                    ModifyerName = table.Column<string>(maxLength: 128, nullable: true),
                    ModifyerDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PDC_Parameter", x => x.ParameterID);
                });

            migrationBuilder.CreateTable(
                name: "PDC_StackupColumn",
                columns: table => new
                {
                    StackupColumnID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ColumnCode = table.Column<string>(maxLength: 64, nullable: false),
                    ColumnName = table.Column<string>(maxLength: 128, nullable: true),
                    ColumnType = table.Column<string>(maxLength: 64, nullable: true),
                    DataType = table.Column<string>(maxLength: 64, nullable: false),
                    MaxLength = table.Column<int>(nullable: false),
                    OrderNo = table.Column<int>(nullable: false),
                    DecimalPlaces = table.Column<int>(nullable: false),
                    ParentColumnID = table.Column<long>(nullable: false),
                    ColumnValue = table.Column<string>(nullable: true),
                    Creator = table.Column<string>(maxLength: 128, nullable: false),
                    CreatorName = table.Column<string>(maxLength: 128, nullable: false),
                    CreatorDate = table.Column<DateTime>(nullable: false),
                    Modifyer = table.Column<string>(maxLength: 128, nullable: true),
                    ModifyerName = table.Column<string>(maxLength: 128, nullable: true),
                    ModifyerDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PDC_StackupColumn", x => x.StackupColumnID);
                });

            migrationBuilder.CreateTable(
                name: "PDC_StackupDetail",
                columns: table => new
                {
                    StackupDetailID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    StackupColumnID = table.Column<long>(nullable: false),
                    DataType = table.Column<string>(maxLength: 64, nullable: false),
                    ColumnType = table.Column<string>(maxLength: 64, nullable: true),
                    IndexNo = table.Column<int>(nullable: false),
                    ColumnValue = table.Column<string>(nullable: true),
                    IsDisabled = table.Column<bool>(nullable: false),
                    Creator = table.Column<string>(maxLength: 128, nullable: false),
                    CreatorName = table.Column<string>(maxLength: 128, nullable: false),
                    CreatorDate = table.Column<DateTime>(nullable: false),
                    Modifyer = table.Column<string>(maxLength: 128, nullable: true),
                    ModifyerName = table.Column<string>(maxLength: 128, nullable: true),
                    ModifyerDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PDC_StackupDetail", x => x.StackupDetailID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PDC_File");

            migrationBuilder.DropTable(
                name: "PDC_Parameter");

            migrationBuilder.DropTable(
                name: "PDC_StackupColumn");

            migrationBuilder.DropTable(
                name: "PDC_StackupDetail");
        }
    }
}
