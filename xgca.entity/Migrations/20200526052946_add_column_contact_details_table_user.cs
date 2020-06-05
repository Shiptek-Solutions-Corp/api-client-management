using Microsoft.EntityFrameworkCore.Migrations;

namespace xgca.entity.Migrations
{
    public partial class add_column_contact_details_table_user : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ContactDetailId",
                schema: "Users",
                table: "User",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_User_ContactDetailId",
                schema: "Users",
                table: "User",
                column: "ContactDetailId");

            migrationBuilder.AddForeignKey(
                name: "FK_User_ContactDetail_ContactDetailId",
                schema: "Users",
                table: "User",
                column: "ContactDetailId",
                principalSchema: "General",
                principalTable: "ContactDetail",
                principalColumn: "ContactDetailId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_ContactDetail_ContactDetailId",
                schema: "Users",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_User_ContactDetailId",
                schema: "Users",
                table: "User");

            migrationBuilder.DropColumn(
                name: "ContactDetailId",
                schema: "Users",
                table: "User");
        }
    }
}
