using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace cns.Migrations
{
    public partial class AddPDC_Department : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DomainEmpNumber",
                table: "PDC_Member",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "PDC_Department",
                columns: table => new
                {
                    DepartmentID = table.Column<Guid>(nullable: false),
                    CompCode = table.Column<string>(maxLength: 64, nullable: false),
                    CompName = table.Column<string>(nullable: true),
                    BUCode = table.Column<string>(maxLength: 64, nullable: false),
                    BUName = table.Column<string>(nullable: false),
                    ShowName = table.Column<string>(nullable: true),
                    Creator = table.Column<string>(maxLength: 128, nullable: false),
                    CreatorName = table.Column<string>(maxLength: 128, nullable: false),
                    CreatorDate = table.Column<DateTime>(nullable: false),
                    Modifyer = table.Column<string>(maxLength: 128, nullable: true),
                    ModifyerName = table.Column<string>(maxLength: 128, nullable: true),
                    ModifyerDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PDC_Department", x => x.DepartmentID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PDC_Department");
            

            migrationBuilder.DropColumn(
                name: "DomainEmpNumber",
                table: "PDC_Member");

           
        }
    }
}
