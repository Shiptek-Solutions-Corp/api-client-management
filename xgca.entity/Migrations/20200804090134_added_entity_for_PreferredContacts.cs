using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace xgca.entity.Migrations
{
    public partial class added_entity_for_PreferredContacts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Guest_Company_CompanyId",
                schema: "Company",
                table: "Guest");

            migrationBuilder.DropIndex(
                name: "IX_Guest_CompanyId",
                schema: "Company",
                table: "Guest");

            migrationBuilder.AlterColumn<byte>(
                name: "IsDeleted",
                schema: "Company",
                table: "Guest",
                nullable: true,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "CreatedBy",
                schema: "Company",
                table: "Guest",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "Guid",
                schema: "Company",
                table: "CompanyGroupResource",
                nullable: false,
                defaultValue: new Guid("821e7024-322c-45e0-963f-2697da01523a"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldDefaultValue: new Guid("acf85556-f111-4f58-a196-b64bcef3b1cb"));

            migrationBuilder.CreateTable(
                name: "PreferredContact",
                schema: "Company",
                columns: table => new
                {
                    PreferredContactId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GuestId = table.Column<int>(nullable: false),
                    CompanyId = table.Column<int>(nullable: false),
                    ContactType = table.Column<int>(nullable: false),
                    Guid = table.Column<Guid>(nullable: false),
                    CreatedBy = table.Column<int>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<int>(nullable: false),
                    ModifiedOn = table.Column<DateTime>(nullable: false),
                    IsDeleted = table.Column<int>(nullable: false, defaultValue: 0),
                    DeletedBy = table.Column<int>(nullable: false),
                    DeletedOn = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PreferredContact", x => x.PreferredContactId);
                    table.ForeignKey(
                        name: "FK_PreferredContact_Company_CompanyId",
                        column: x => x.CompanyId,
                        principalSchema: "Company",
                        principalTable: "Company",
                        principalColumn: "CompanyId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PreferredContact_Guest_GuestId",
                        column: x => x.GuestId,
                        principalSchema: "Company",
                        principalTable: "Guest",
                        principalColumn: "GuestId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PreferredContact_CompanyId",
                schema: "Company",
                table: "PreferredContact",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_PreferredContact_GuestId",
                schema: "Company",
                table: "PreferredContact",
                column: "GuestId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PreferredContact",
                schema: "Company");

            migrationBuilder.AlterColumn<int>(
                name: "IsDeleted",
                schema: "Company",
                table: "Guest",
                type: "int",
                nullable: false,
                oldClrType: typeof(byte));

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                schema: "Company",
                table: "Guest",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<Guid>(
                name: "Guid",
                schema: "Company",
                table: "CompanyGroupResource",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("acf85556-f111-4f58-a196-b64bcef3b1cb"),
                oldClrType: typeof(Guid),
                oldDefaultValue: new Guid("821e7024-322c-45e0-963f-2697da01523a"));

            migrationBuilder.CreateIndex(
                name: "IX_Guest_CompanyId",
                schema: "Company",
                table: "Guest",
                column: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Guest_Company_CompanyId",
                schema: "Company",
                table: "Guest",
                column: "CompanyId",
                principalSchema: "Company",
                principalTable: "Company",
                principalColumn: "CompanyId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
