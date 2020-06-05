using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace xgca.entity.Migrations
{
    public partial class add_user_contact_table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserContact",
                schema: "Users",
                columns: table => new
                {
                    UserContactId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(nullable: false),
                    ContactTypeId = table.Column<int>(nullable: false),
                    ContactNumber = table.Column<string>(maxLength: 100, nullable: false),
                    Status = table.Column<byte>(nullable: false, defaultValue: 1),
                    CreatedBy = table.Column<int>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<int>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserContact", x => x.UserContactId);
                    table.ForeignKey(
                        name: "FK_UserContact_ContactType_ContactTypeId",
                        column: x => x.ContactTypeId,
                        principalSchema: "Users",
                        principalTable: "ContactType",
                        principalColumn: "ContactTypeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserContact_User_UserId",
                        column: x => x.UserId,
                        principalSchema: "Users",
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserContact_ContactTypeId",
                schema: "Users",
                table: "UserContact",
                column: "ContactTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_UserContact_UserId",
                schema: "Users",
                table: "UserContact",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserContact",
                schema: "Users");
        }
    }
}
