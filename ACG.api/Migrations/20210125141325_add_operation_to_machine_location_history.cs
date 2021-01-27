using Microsoft.EntityFrameworkCore.Migrations;

namespace ACG.api.Migrations
{
    public partial class add_operation_to_machine_location_history : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Operation",
                table: "machines_history",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Operation",
                table: "machines_history");
        }
    }
}
