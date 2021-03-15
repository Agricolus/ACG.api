using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;

namespace ACG.api.Migrations
{
    public partial class add_point_geometry_to_machine_location : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Point>(
                name: "Position",
                table: "machines_history",
                type: "geometry",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Position",
                table: "machines_history");
        }
    }
}
