using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace xgca.entity.Migrations
{
    public partial class remove_user_contact_email_table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserContact",
                schema: "Users");

            migrationBuilder.DropTable(
                name: "UserEmail",
                schema: "Users");

            migrationBuilder.DropTable(
                name: "ContactType",
                schema: "Users");

            migrationBuilder.AddColumn<byte>(
                name: "IsDeleted",
                schema: "Users",
                table: "User",
                nullable: false,
                defaultValue: (byte)0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                schema: "Users",
                table: "User");

            migrationBuilder.CreateTable(
                name: "ContactType",
                schema: "Users",
                columns: table => new
                {
                    ContactTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Label = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ModifiedBy = table.Column<int>(type: "int", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactType", x => x.ContactTypeId);
                });

            migrationBuilder.CreateTable(
                name: "UserEmail",
                schema: "Users",
                columns: table => new
                {
                    EmailId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ModifiedBy = table.Column<int>(type: "int", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<byte>(type: "tinyint", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserEmail", x => x.EmailId);
                    table.ForeignKey(
                        name: "FK_UserEmail_User_UserId",
                        column: x => x.UserId,
                        principalSchema: "Users",
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserContact",
                schema: "Users",
                columns: table => new
                {
                    UserContactId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ContactNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ContactTypeId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<int>(type: "int", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<byte>(type: "tinyint", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
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

            migrationBuilder.CreateIndex(
                name: "IX_UserEmail_UserId",
                schema: "Users",
                table: "UserEmail",
                column: "UserId");
        }
    }
}
