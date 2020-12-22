using Microsoft.EntityFrameworkCore.Migrations;

namespace ACG.api.Migrations
{
    public partial class add_context_broker_subscription_id_to_machine : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CBSubscriptionId",
                table: "machines",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CBSubscriptionId",
                table: "machines");
        }
    }
}
