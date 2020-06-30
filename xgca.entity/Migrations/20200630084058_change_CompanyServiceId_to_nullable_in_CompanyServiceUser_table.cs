using Microsoft.EntityFrameworkCore.Migrations;

namespace xgca.entity.Migrations
{
    public partial class change_CompanyServiceId_to_nullable_in_CompanyServiceUser_table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompanyServiceUser_CompanyService_CompanyServiceId",
                schema: "Company",
                table: "CompanyServiceUser");

            migrationBuilder.AlterColumn<int>(
                name: "CompanyServiceId",
                schema: "Company",
                table: "CompanyServiceUser",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyServiceUser_CompanyService_CompanyServiceId",
                schema: "Company",
                table: "CompanyServiceUser",
                column: "CompanyServiceId",
                principalSchema: "Company",
                principalTable: "CompanyService",
                principalColumn: "CompanyServiceId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompanyServiceUser_CompanyService_CompanyServiceId",
                schema: "Company",
                table: "CompanyServiceUser");

            migrationBuilder.AlterColumn<int>(
                name: "CompanyServiceId",
                schema: "Company",
                table: "CompanyServiceUser",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyServiceUser_CompanyService_CompanyServiceId",
                schema: "Company",
                table: "CompanyServiceUser",
                column: "CompanyServiceId",
                principalSchema: "Company",
                principalTable: "CompanyService",
                principalColumn: "CompanyServiceId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
