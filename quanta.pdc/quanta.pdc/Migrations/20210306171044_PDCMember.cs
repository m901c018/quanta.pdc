using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace cns.Migrations
{
    public partial class PDCMember : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PDC_Member",
                columns: table => new
                {
                    MemberID = table.Column<Guid>(nullable: false),
                    CompCode = table.Column<string>(maxLength: 64, nullable: false),
                    CompName = table.Column<string>(nullable: false),
                    BUCode = table.Column<string>(maxLength: 64, nullable: false),
                    BUName = table.Column<string>(nullable: false),
                    Email = table.Column<string>(nullable: true),
                    RoleID = table.Column<int>(nullable: false),
                    UserName = table.Column<string>(nullable: true),
                    UserEngName = table.Column<string>(nullable: true),
                    Extension = table.Column<string>(nullable: true),
                    PCBType = table.Column<string>(nullable: true),
                    EmpNumber = table.Column<string>(nullable: false),
                    Phone = table.Column<string>(nullable: true),
                    IsEnabled = table.Column<bool>(nullable: false),
                    Creator = table.Column<string>(maxLength: 128, nullable: false),
                    CreatorName = table.Column<string>(maxLength: 128, nullable: false),
                    CreatorDate = table.Column<DateTime>(nullable: false),
                    Modifyer = table.Column<string>(maxLength: 128, nullable: true),
                    ModifyerName = table.Column<string>(maxLength: 128, nullable: true),
                    ModifyerDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PDC_Member", x => x.MemberID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PDC_Member");
        }
    }
}
