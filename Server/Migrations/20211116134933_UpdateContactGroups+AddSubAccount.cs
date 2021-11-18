using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SMSGateway.Server.Migrations
{
    public partial class UpdateContactGroupsAddSubAccount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContactGroupId",
                table: "Contact");

            migrationBuilder.AddColumn<string>(
                name: "ContactId",
                table: "ContactGroups",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "SubAccount",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    UpdatedByUserId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubAccount", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubAccount_AspNetUsers_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SubAccount_AspNetUsers_UpdatedByUserId",
                        column: x => x.UpdatedByUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_SubAccount_CreatedByUserId",
                table: "SubAccount",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_SubAccount_UpdatedByUserId",
                table: "SubAccount",
                column: "UpdatedByUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SubAccount");

            migrationBuilder.DropColumn(
                name: "ContactId",
                table: "ContactGroups");

            migrationBuilder.AddColumn<int>(
                name: "ContactGroupId",
                table: "Contact",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
