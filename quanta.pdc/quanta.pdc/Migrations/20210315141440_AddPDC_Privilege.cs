using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace cns.Migrations
{
    public partial class AddPDC_Privilege : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.CreateTable(
                name: "PDC_Privilege",
                columns: table => new
                {
                    PrivilegeID = table.Column<Guid>(nullable: false),
                    MemberID = table.Column<Guid>(nullable: false),
                    RoleID = table.Column<int>(nullable: false),
                    IsMB = table.Column<bool>(nullable: true),
                    IsMail = table.Column<bool>(nullable: true),
                    Creator = table.Column<string>(maxLength: 128, nullable: false),
                    CreatorName = table.Column<string>(maxLength: 128, nullable: false),
                    CreatorDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PDC_Privilege", x => x.PrivilegeID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PDC_Privilege");

        }
    }
}
