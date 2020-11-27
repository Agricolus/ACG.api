using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ACG.api.Migrations
{
    public partial class add_point_time_to_machine : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "PTime",
                table: "machines",
                type: "timestamp without time zone",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PTime",
                table: "machines");
        }
    }
}
