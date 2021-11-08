using Microsoft.EntityFrameworkCore.Migrations;

namespace SMSGateway.Server.Migrations
{
    public partial class AddLog : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreditValue",
                table: "AspNetUsers",
                newName: "CostPerSms");

            migrationBuilder.AddColumn<int>(
                name: "SmsCredit",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SmsCredit",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "CostPerSms",
                table: "AspNetUsers",
                newName: "CreditValue");
        }
    }
}
