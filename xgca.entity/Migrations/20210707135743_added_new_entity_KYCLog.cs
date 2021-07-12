using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace xgca.entity.Migrations
{
    public partial class added_new_entity_KYCLog : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "KYCLog",
                schema: "Settings",
                columns: table => new
                {
                    KYCLogId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyId = table.Column<int>(nullable: false),
                    CompanySectionsId = table.Column<int>(nullable: false),
                    Remarks = table.Column<string>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KYCLog", x => x.KYCLogId);
                    table.ForeignKey(
                        name: "FK_KYCLog_Company_CompanyId",
                        column: x => x.CompanyId,
                        principalSchema: "Company",
                        principalTable: "Company",
                        principalColumn: "CompanyId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_KYCLog_CompanySections_CompanySectionsId",
                        column: x => x.CompanySectionsId,
                        principalSchema: "Company",
                        principalTable: "CompanySections",
                        principalColumn: "CompanySectionsId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                schema: "Settings",
                table: "AddressType",
                keyColumn: "AddressTypeId",
                keyValue: 1,
                columns: new[] { "CreatedOn", "Guid", "ModifiedOn" },
                values: new object[] { new DateTime(2021, 7, 7, 13, 57, 42, 329, DateTimeKind.Utc).AddTicks(509), new Guid("1e0621b2-b7ea-4d48-8be2-f09980694816"), new DateTime(2021, 7, 7, 13, 57, 42, 329, DateTimeKind.Utc).AddTicks(1523) });

            migrationBuilder.UpdateData(
                schema: "Settings",
                table: "AddressType",
                keyColumn: "AddressTypeId",
                keyValue: 2,
                columns: new[] { "CreatedOn", "Guid", "ModifiedOn" },
                values: new object[] { new DateTime(2021, 7, 7, 13, 57, 42, 331, DateTimeKind.Utc).AddTicks(90), new Guid("95ec682b-074f-42bb-9fed-d4dbde41e41d"), new DateTime(2021, 7, 7, 13, 57, 42, 331, DateTimeKind.Utc).AddTicks(124) });

            migrationBuilder.CreateIndex(
                name: "IX_KYCLog_CompanyId",
                schema: "Settings",
                table: "KYCLog",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_KYCLog_CompanySectionsId",
                schema: "Settings",
                table: "KYCLog",
                column: "CompanySectionsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "KYCLog",
                schema: "Settings");

            migrationBuilder.UpdateData(
                schema: "Settings",
                table: "AddressType",
                keyColumn: "AddressTypeId",
                keyValue: 1,
                columns: new[] { "CreatedOn", "Guid", "ModifiedOn" },
                values: new object[] { new DateTime(2021, 6, 28, 4, 8, 9, 202, DateTimeKind.Utc).AddTicks(5977), new Guid("1e0621b2-b7ea-4d48-8be2-f09980694816"), new DateTime(2021, 6, 28, 4, 8, 9, 202, DateTimeKind.Utc).AddTicks(6673) });

            migrationBuilder.UpdateData(
                schema: "Settings",
                table: "AddressType",
                keyColumn: "AddressTypeId",
                keyValue: 2,
                columns: new[] { "CreatedOn", "Guid", "ModifiedOn" },
                values: new object[] { new DateTime(2021, 6, 28, 4, 8, 9, 203, DateTimeKind.Utc).AddTicks(5995), new Guid("95ec682b-074f-42bb-9fed-d4dbde41e41d"), new DateTime(2021, 6, 28, 4, 8, 9, 203, DateTimeKind.Utc).AddTicks(6009) });
        }
    }
}
