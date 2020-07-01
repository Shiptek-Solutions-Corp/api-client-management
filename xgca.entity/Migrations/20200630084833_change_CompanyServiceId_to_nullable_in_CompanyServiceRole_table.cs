using Microsoft.EntityFrameworkCore.Migrations;

namespace xgca.entity.Migrations
{
    public partial class change_CompanyServiceId_to_nullable_in_CompanyServiceRole_table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompanyServiceRole_CompanyService_CompanyServiceId",
                schema: "Company",
                table: "CompanyServiceRole");

            migrationBuilder.AlterColumn<int>(
                name: "CompanyServiceId",
                schema: "Company",
                table: "CompanyServiceRole",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyServiceRole_CompanyService_CompanyServiceId",
                schema: "Company",
                table: "CompanyServiceRole",
                column: "CompanyServiceId",
                principalSchema: "Company",
                principalTable: "CompanyService",
                principalColumn: "CompanyServiceId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompanyServiceRole_CompanyService_CompanyServiceId",
                schema: "Company",
                table: "CompanyServiceRole");

            migrationBuilder.AlterColumn<int>(
                name: "CompanyServiceId",
                schema: "Company",
                table: "CompanyServiceRole",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyServiceRole_CompanyService_CompanyServiceId",
                schema: "Company",
                table: "CompanyServiceRole",
                column: "CompanyServiceId",
                principalSchema: "Company",
                principalTable: "CompanyService",
                principalColumn: "CompanyServiceId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
