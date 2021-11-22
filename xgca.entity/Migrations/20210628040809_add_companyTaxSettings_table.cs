using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace xgca.entity.Migrations
{
    public partial class add_companyTaxSettings_table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PricingSettingsDescription",
                schema: "Company",
                table: "Company",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CompanyTaxSettings",
                schema: "Company",
                columns: table => new
                {
                    CompanyTaxSettingsId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyId = table.Column<int>(nullable: false),
                    Guid = table.Column<Guid>(nullable: false, defaultValueSql: "(newid())"),
                    TaxTypeId = table.Column<string>(nullable: true),
                    TaxTypeDescription = table.Column<string>(nullable: true),
                    TaxPercentageRate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsTaxExcempted = table.Column<bool>(nullable: false),
                    Status = table.Column<int>(nullable: false, defaultValue: 1),
                    StatusName = table.Column<string>(nullable: false, defaultValue: "Active"),
                    CreatedBy = table.Column<string>(nullable: false, defaultValue: "Admin"),
                    CreatedOn = table.Column<DateTime>(nullable: false, defaultValueSql: "(getutcdate())"),
                    ModifiedBy = table.Column<string>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyTaxSettings", x => x.CompanyTaxSettingsId);
                    table.ForeignKey(
                        name: "FK_CompanyTaxSettings_Company_CompanyId",
                        column: x => x.CompanyId,
                        principalSchema: "Company",
                        principalTable: "Company",
                        principalColumn: "CompanyId",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_CompanyTaxSettings_CompanyId",
                schema: "Company",
                table: "CompanyTaxSettings",
                column: "CompanyId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CompanyTaxSettings",
                schema: "Company");

            migrationBuilder.DropColumn(
                name: "PricingSettingsDescription",
                schema: "Company",
                table: "Company");

            migrationBuilder.UpdateData(
                schema: "Settings",
                table: "AddressType",
                keyColumn: "AddressTypeId",
                keyValue: 1,
                columns: new[] { "CreatedOn", "Guid", "ModifiedOn" },
                values: new object[] { new DateTime(2021, 6, 24, 5, 11, 42, 669, DateTimeKind.Utc).AddTicks(4098), new Guid("1e0621b2-b7ea-4d48-8be2-f09980694816"), new DateTime(2021, 6, 24, 5, 11, 42, 669, DateTimeKind.Utc).AddTicks(4771) });

            migrationBuilder.UpdateData(
                schema: "Settings",
                table: "AddressType",
                keyColumn: "AddressTypeId",
                keyValue: 2,
                columns: new[] { "CreatedOn", "Guid", "ModifiedOn" },
                values: new object[] { new DateTime(2021, 6, 24, 5, 11, 42, 670, DateTimeKind.Utc).AddTicks(3862), new Guid("95ec682b-074f-42bb-9fed-d4dbde41e41d"), new DateTime(2021, 6, 24, 5, 11, 42, 670, DateTimeKind.Utc).AddTicks(3878) });
        }
    }
}
