using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace xgca.entity.Migrations
{
    public partial class updated_seeder_AddressType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "Guid",
                schema: "Company",
                table: "CompanyGroupResource",
                nullable: false,
                defaultValue: new Guid("cf1f204a-f287-4c6c-974e-d48ebe760b6f"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldDefaultValue: new Guid("d3395554-5023-4e17-af81-2d66d0e18f04"));

            migrationBuilder.UpdateData(
                schema: "Company",
                table: "Company",
                keyColumn: "CompanyId",
                keyValue: 1,
                columns: new[] { "CreatedOn", "Guid", "ModifiedOn" },
                values: new object[] { new DateTime(2020, 8, 25, 8, 3, 40, 331, DateTimeKind.Utc).AddTicks(8128), new Guid("3608a083-fbe5-44d5-afef-c7814100aec7"), new DateTime(2020, 8, 25, 8, 3, 40, 331, DateTimeKind.Utc).AddTicks(9471) });

            migrationBuilder.UpdateData(
                schema: "Company",
                table: "Guest",
                keyColumn: "GuestId",
                keyValue: 1,
                columns: new[] { "CreatedOn", "DeletedOn", "ModifiedOn" },
                values: new object[] { new DateTime(2020, 8, 25, 8, 3, 40, 334, DateTimeKind.Utc).AddTicks(749), new DateTime(2020, 8, 25, 8, 3, 40, 334, DateTimeKind.Utc).AddTicks(4134), new DateTime(2020, 8, 25, 8, 3, 40, 334, DateTimeKind.Utc).AddTicks(2192) });

            migrationBuilder.UpdateData(
                schema: "General",
                table: "Address",
                keyColumn: "AddressId",
                keyValue: 1,
                columns: new[] { "CreatedOn", "Guid", "ModifiedOn" },
                values: new object[] { new DateTime(2020, 8, 25, 8, 3, 40, 329, DateTimeKind.Utc).AddTicks(1541), new Guid("21716a6c-4dee-422b-b2d9-74bf4b12e242"), new DateTime(2020, 8, 25, 8, 3, 40, 329, DateTimeKind.Utc).AddTicks(3084) });

            migrationBuilder.UpdateData(
                schema: "General",
                table: "ContactDetail",
                keyColumn: "ContactDetailId",
                keyValue: 1,
                columns: new[] { "CreatedOn", "Guid", "ModifiedOn" },
                values: new object[] { new DateTime(2020, 8, 25, 8, 3, 40, 330, DateTimeKind.Utc).AddTicks(5202), new Guid("1f5cb3b9-992b-4279-acf7-7f0273abbf03"), new DateTime(2020, 8, 25, 8, 3, 40, 330, DateTimeKind.Utc).AddTicks(6892) });

            migrationBuilder.UpdateData(
                schema: "Settings",
                table: "AddressType",
                keyColumn: "AddressTypeId",
                keyValue: 1,
                columns: new[] { "CreatedOn", "Guid", "ModifiedOn" },
                values: new object[] { new DateTime(2020, 8, 25, 8, 3, 40, 324, DateTimeKind.Utc).AddTicks(6276), new Guid("1e0621b2-b7ea-4d48-8be2-f09980694816"), new DateTime(2020, 8, 25, 8, 3, 40, 324, DateTimeKind.Utc).AddTicks(7710) });

            migrationBuilder.UpdateData(
                schema: "Settings",
                table: "AddressType",
                keyColumn: "AddressTypeId",
                keyValue: 2,
                columns: new[] { "CreatedOn", "Guid", "ModifiedOn" },
                values: new object[] { new DateTime(2020, 8, 25, 8, 3, 40, 327, DateTimeKind.Utc).AddTicks(7990), new Guid("95ec682b-074f-42bb-9fed-d4dbde41e41d"), new DateTime(2020, 8, 25, 8, 3, 40, 327, DateTimeKind.Utc).AddTicks(8030) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "Guid",
                schema: "Company",
                table: "CompanyGroupResource",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("d3395554-5023-4e17-af81-2d66d0e18f04"),
                oldClrType: typeof(Guid),
                oldDefaultValue: new Guid("cf1f204a-f287-4c6c-974e-d48ebe760b6f"));

            migrationBuilder.UpdateData(
                schema: "Company",
                table: "Company",
                keyColumn: "CompanyId",
                keyValue: 1,
                columns: new[] { "CreatedOn", "Guid", "ModifiedOn" },
                values: new object[] { new DateTime(2020, 8, 25, 6, 52, 49, 840, DateTimeKind.Utc).AddTicks(7724), new Guid("3608a083-fbe5-44d5-afef-c7814100aec7"), new DateTime(2020, 8, 25, 6, 52, 49, 841, DateTimeKind.Utc).AddTicks(22) });

            migrationBuilder.UpdateData(
                schema: "Company",
                table: "Guest",
                keyColumn: "GuestId",
                keyValue: 1,
                columns: new[] { "CreatedOn", "DeletedOn", "ModifiedOn" },
                values: new object[] { new DateTime(2020, 8, 25, 6, 52, 49, 843, DateTimeKind.Utc).AddTicks(7469), new DateTime(2020, 8, 25, 6, 52, 49, 844, DateTimeKind.Utc).AddTicks(956), new DateTime(2020, 8, 25, 6, 52, 49, 843, DateTimeKind.Utc).AddTicks(8918) });

            migrationBuilder.UpdateData(
                schema: "General",
                table: "Address",
                keyColumn: "AddressId",
                keyValue: 1,
                columns: new[] { "CreatedOn", "Guid", "ModifiedOn" },
                values: new object[] { new DateTime(2020, 8, 25, 6, 52, 49, 837, DateTimeKind.Utc).AddTicks(8615), new Guid("21716a6c-4dee-422b-b2d9-74bf4b12e242"), new DateTime(2020, 8, 25, 6, 52, 49, 838, DateTimeKind.Utc).AddTicks(47) });

            migrationBuilder.UpdateData(
                schema: "General",
                table: "ContactDetail",
                keyColumn: "ContactDetailId",
                keyValue: 1,
                columns: new[] { "CreatedOn", "Guid", "ModifiedOn" },
                values: new object[] { new DateTime(2020, 8, 25, 6, 52, 49, 839, DateTimeKind.Utc).AddTicks(3758), new Guid("1f5cb3b9-992b-4279-acf7-7f0273abbf03"), new DateTime(2020, 8, 25, 6, 52, 49, 839, DateTimeKind.Utc).AddTicks(5383) });

            migrationBuilder.UpdateData(
                schema: "Settings",
                table: "AddressType",
                keyColumn: "AddressTypeId",
                keyValue: 1,
                columns: new[] { "CreatedOn", "Guid", "ModifiedOn" },
                values: new object[] { new DateTime(2020, 8, 25, 6, 52, 49, 834, DateTimeKind.Utc).AddTicks(4601), new Guid("1e0621b2-b7ea-4d48-8be2-f09980694816"), new DateTime(2020, 8, 25, 6, 52, 49, 834, DateTimeKind.Utc).AddTicks(6959) });

            migrationBuilder.UpdateData(
                schema: "Settings",
                table: "AddressType",
                keyColumn: "AddressTypeId",
                keyValue: 2,
                columns: new[] { "CreatedOn", "Guid", "ModifiedOn" },
                values: new object[] { new DateTime(2020, 8, 25, 6, 52, 49, 836, DateTimeKind.Utc).AddTicks(7705), new Guid("95ec682b-074f-42bb-9fed-d4dbde41e41d"), new DateTime(2020, 8, 25, 6, 52, 49, 836, DateTimeKind.Utc).AddTicks(7738) });
        }
    }
}
