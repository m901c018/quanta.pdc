using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace cns.Migrations
{
    public partial class AddForm_Col : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PDC_Member",
                table: "PDC_Form_StageLog",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Stage",
                table: "PDC_Form_StageLog",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Revision",
                table: "PDC_Form",
                maxLength: 256,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ProjectName",
                table: "PDC_Form",
                maxLength: 64,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 64,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "BoardTypeName",
                table: "PDC_Form",
                maxLength: 64,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 64,
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ApplyDate",
                table: "PDC_Form",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PDC_Member",
                table: "PDC_Form_StageLog");

            migrationBuilder.DropColumn(
                name: "Stage",
                table: "PDC_Form_StageLog");

            migrationBuilder.DropColumn(
                name: "ApplyDate",
                table: "PDC_Form");

            migrationBuilder.AlterColumn<string>(
                name: "Revision",
                table: "PDC_Form",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 256);

            migrationBuilder.AlterColumn<string>(
                name: "ProjectName",
                table: "PDC_Form",
                maxLength: 64,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 64);

            migrationBuilder.AlterColumn<string>(
                name: "BoardTypeName",
                table: "PDC_Form",
                maxLength: 64,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 64);
        }
    }
}
