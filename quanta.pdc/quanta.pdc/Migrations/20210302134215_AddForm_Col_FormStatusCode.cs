using Microsoft.EntityFrameworkCore.Migrations;

namespace cns.Migrations
{
    public partial class AddForm_Col_FormStatusCode : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FormStatusCode",
                table: "PDC_Form",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FormStatusCode",
                table: "PDC_Form");
        }
    }
}
