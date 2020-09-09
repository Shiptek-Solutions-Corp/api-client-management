using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace xgca.entity.Migrations
{
    public partial class added_entity_PreferredProvider : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        { 
            migrationBuilder.CreateTable(
                name: "PreferredProvider",
                schema: "Company",
                columns: table => new
                {
                    PreferredProviderId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyServiceId = table.Column<string>(nullable: true),
                    CompanyId = table.Column<string>(nullable: true),
                    ServiceId = table.Column<string>(nullable: true),
                    ProfileId = table.Column<int>(nullable: false),
                    Guid = table.Column<Guid>(nullable: false),
                    CreatedBy = table.Column<int>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<int>(nullable: false),
                    ModifiedOn = table.Column<DateTime>(nullable: false),
                    IsDeleted = table.Column<int>(nullable: false),
                    DeletedBy = table.Column<int>(nullable: false),
                    DeletedOn = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PreferredProvider", x => x.PreferredProviderId);
                });

            migrationBuilder.UpdateData(
                schema: "Company",
                table: "Company",
                keyColumn: "CompanyId",
                keyValue: 1,
                columns: new[] { "CreatedOn", "Guid", "ModifiedOn" },
                values: new object[] { new DateTime(2020, 8, 28, 5, 0, 19, 272, DateTimeKind.Utc).AddTicks(1083), new Guid("3608a083-fbe5-44d5-afef-c7814100aec7"), new DateTime(2020, 8, 28, 5, 0, 19, 272, DateTimeKind.Utc).AddTicks(3346) });

            migrationBuilder.UpdateData(
                schema: "Company",
                table: "Guest",
                keyColumn: "GuestId",
                keyValue: 1,
                columns: new[] { "CreatedOn", "DeletedOn", "ModifiedOn" },
                values: new object[] { new DateTime(2020, 8, 28, 5, 0, 19, 276, DateTimeKind.Utc).AddTicks(5952), new DateTime(2020, 8, 28, 5, 0, 19, 277, DateTimeKind.Utc).AddTicks(1922), new DateTime(2020, 8, 28, 5, 0, 19, 276, DateTimeKind.Utc).AddTicks(8480) });

            migrationBuilder.UpdateData(
                schema: "General",
                table: "Address",
                keyColumn: "AddressId",
                keyValue: 1,
                columns: new[] { "CreatedOn", "Guid", "ModifiedOn" },
                values: new object[] { new DateTime(2020, 8, 28, 5, 0, 19, 268, DateTimeKind.Utc).AddTicks(383), new Guid("21716a6c-4dee-422b-b2d9-74bf4b12e242"), new DateTime(2020, 8, 28, 5, 0, 19, 268, DateTimeKind.Utc).AddTicks(2633) });

            migrationBuilder.UpdateData(
                schema: "General",
                table: "ContactDetail",
                keyColumn: "ContactDetailId",
                keyValue: 1,
                columns: new[] { "CreatedOn", "Guid", "ModifiedOn" },
                values: new object[] { new DateTime(2020, 8, 28, 5, 0, 19, 270, DateTimeKind.Utc).AddTicks(1100), new Guid("1f5cb3b9-992b-4279-acf7-7f0273abbf03"), new DateTime(2020, 8, 28, 5, 0, 19, 270, DateTimeKind.Utc).AddTicks(3430) });

            migrationBuilder.UpdateData(
                schema: "Settings",
                table: "AddressType",
                keyColumn: "AddressTypeId",
                keyValue: 1,
                columns: new[] { "CreatedOn", "Guid", "ModifiedOn" },
                values: new object[] { new DateTime(2020, 8, 28, 5, 0, 19, 262, DateTimeKind.Utc).AddTicks(8692), new Guid("1e0621b2-b7ea-4d48-8be2-f09980694816"), new DateTime(2020, 8, 28, 5, 0, 19, 263, DateTimeKind.Utc).AddTicks(1320) });

            migrationBuilder.UpdateData(
                schema: "Settings",
                table: "AddressType",
                keyColumn: "AddressTypeId",
                keyValue: 2,
                columns: new[] { "CreatedOn", "Guid", "ModifiedOn" },
                values: new object[] { new DateTime(2020, 8, 28, 5, 0, 19, 266, DateTimeKind.Utc).AddTicks(2427), new Guid("95ec682b-074f-42bb-9fed-d4dbde41e41d"), new DateTime(2020, 8, 28, 5, 0, 19, 266, DateTimeKind.Utc).AddTicks(2474) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PreferredProvider",
                schema: "Company");

            migrationBuilder.UpdateData(
                schema: "Company",
                table: "Company",
                keyColumn: "CompanyId",
                keyValue: 1,
                columns: new[] { "CreatedOn", "Guid", "ModifiedOn" },
                values: new object[] { new DateTime(2020, 8, 25, 16, 37, 27, 462, DateTimeKind.Utc).AddTicks(7543), new Guid("3608a083-fbe5-44d5-afef-c7814100aec7"), new DateTime(2020, 8, 25, 16, 37, 27, 462, DateTimeKind.Utc).AddTicks(9672) });

            migrationBuilder.UpdateData(
                schema: "Company",
                table: "Guest",
                keyColumn: "GuestId",
                keyValue: 1,
                columns: new[] { "CreatedOn", "DeletedOn", "ModifiedOn" },
                values: new object[] { new DateTime(2020, 8, 25, 16, 37, 27, 466, DateTimeKind.Utc).AddTicks(4562), new DateTime(2020, 8, 25, 16, 37, 27, 466, DateTimeKind.Utc).AddTicks(9920), new DateTime(2020, 8, 25, 16, 37, 27, 466, DateTimeKind.Utc).AddTicks(6703) });

            migrationBuilder.UpdateData(
                schema: "General",
                table: "Address",
                keyColumn: "AddressId",
                keyValue: 1,
                columns: new[] { "CreatedOn", "Guid", "ModifiedOn" },
                values: new object[] { new DateTime(2020, 8, 25, 16, 37, 27, 458, DateTimeKind.Utc).AddTicks(9185), new Guid("21716a6c-4dee-422b-b2d9-74bf4b12e242"), new DateTime(2020, 8, 25, 16, 37, 27, 459, DateTimeKind.Utc).AddTicks(1397) });

            migrationBuilder.UpdateData(
                schema: "General",
                table: "ContactDetail",
                keyColumn: "ContactDetailId",
                keyValue: 1,
                columns: new[] { "CreatedOn", "Guid", "ModifiedOn" },
                values: new object[] { new DateTime(2020, 8, 25, 16, 37, 27, 460, DateTimeKind.Utc).AddTicks(9690), new Guid("1f5cb3b9-992b-4279-acf7-7f0273abbf03"), new DateTime(2020, 8, 25, 16, 37, 27, 461, DateTimeKind.Utc).AddTicks(1871) });

            migrationBuilder.UpdateData(
                schema: "Settings",
                table: "AddressType",
                keyColumn: "AddressTypeId",
                keyValue: 1,
                columns: new[] { "CreatedOn", "Guid", "ModifiedOn" },
                values: new object[] { new DateTime(2020, 8, 25, 16, 37, 27, 453, DateTimeKind.Utc).AddTicks(9493), new Guid("1e0621b2-b7ea-4d48-8be2-f09980694816"), new DateTime(2020, 8, 25, 16, 37, 27, 454, DateTimeKind.Utc).AddTicks(1783) });

            migrationBuilder.UpdateData(
                schema: "Settings",
                table: "AddressType",
                keyColumn: "AddressTypeId",
                keyValue: 2,
                columns: new[] { "CreatedOn", "Guid", "ModifiedOn" },
                values: new object[] { new DateTime(2020, 8, 25, 16, 37, 27, 457, DateTimeKind.Utc).AddTicks(1721), new Guid("95ec682b-074f-42bb-9fed-d4dbde41e41d"), new DateTime(2020, 8, 25, 16, 37, 27, 457, DateTimeKind.Utc).AddTicks(1765) });
        }
    }
}
