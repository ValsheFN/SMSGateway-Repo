using Microsoft.EntityFrameworkCore.Migrations;

namespace SMSGateway.Server.Migrations
{
    public partial class UpdateHistoryAndLog : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MessageId",
                table: "Logs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MessageId",
                table: "History",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MessageId",
                table: "Logs");

            migrationBuilder.DropColumn(
                name: "MessageId",
                table: "History");
        }
    }
}
