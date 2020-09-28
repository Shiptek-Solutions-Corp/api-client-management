using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace xgca.entity.Migrations
{
    public partial class changed_lat_long_max_length : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Longitude",
                schema: "General",
                table: "Address",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Latitude",
                schema: "General",
                table: "Address",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10,
                oldNullable: true);

            migrationBuilder.UpdateData(
                schema: "Company",
                table: "Company",
                keyColumn: "CompanyId",
                keyValue: 1,
                columns: new[] { "CreatedOn", "Guid", "ModifiedOn" },
                values: new object[] { new DateTime(2020, 9, 21, 8, 17, 6, 441, DateTimeKind.Utc).AddTicks(2104), new Guid("3608a083-fbe5-44d5-afef-c7814100aec7"), new DateTime(2020, 9, 21, 8, 17, 6, 441, DateTimeKind.Utc).AddTicks(3498) });

            migrationBuilder.UpdateData(
                schema: "Company",
                table: "Guest",
                keyColumn: "GuestId",
                keyValue: 1,
                columns: new[] { "CreatedOn", "DeletedOn", "ModifiedOn" },
                values: new object[] { new DateTime(2020, 9, 21, 8, 17, 6, 443, DateTimeKind.Utc).AddTicks(1924), new DateTime(2020, 9, 21, 8, 17, 6, 443, DateTimeKind.Utc).AddTicks(4917), new DateTime(2020, 9, 21, 8, 17, 6, 443, DateTimeKind.Utc).AddTicks(3178) });

            migrationBuilder.UpdateData(
                schema: "General",
                table: "Address",
                keyColumn: "AddressId",
                keyValue: 1,
                columns: new[] { "CreatedOn", "Guid", "ModifiedOn" },
                values: new object[] { new DateTime(2020, 9, 21, 8, 17, 6, 439, DateTimeKind.Utc).AddTicks(1553), new Guid("21716a6c-4dee-422b-b2d9-74bf4b12e242"), new DateTime(2020, 9, 21, 8, 17, 6, 439, DateTimeKind.Utc).AddTicks(2808) });

            migrationBuilder.UpdateData(
                schema: "General",
                table: "ContactDetail",
                keyColumn: "ContactDetailId",
                keyValue: 1,
                columns: new[] { "CreatedOn", "Guid", "ModifiedOn" },
                values: new object[] { new DateTime(2020, 9, 21, 8, 17, 6, 440, DateTimeKind.Utc).AddTicks(2612), new Guid("1f5cb3b9-992b-4279-acf7-7f0273abbf03"), new DateTime(2020, 9, 21, 8, 17, 6, 440, DateTimeKind.Utc).AddTicks(3786) });

            migrationBuilder.UpdateData(
                schema: "Settings",
                table: "AddressType",
                keyColumn: "AddressTypeId",
                keyValue: 1,
                columns: new[] { "CreatedOn", "Guid", "ModifiedOn" },
                values: new object[] { new DateTime(2020, 9, 21, 8, 17, 6, 436, DateTimeKind.Utc).AddTicks(4683), new Guid("1e0621b2-b7ea-4d48-8be2-f09980694816"), new DateTime(2020, 9, 21, 8, 17, 6, 436, DateTimeKind.Utc).AddTicks(5927) });

            migrationBuilder.UpdateData(
                schema: "Settings",
                table: "AddressType",
                keyColumn: "AddressTypeId",
                keyValue: 2,
                columns: new[] { "CreatedOn", "Guid", "ModifiedOn" },
                values: new object[] { new DateTime(2020, 9, 21, 8, 17, 6, 438, DateTimeKind.Utc).AddTicks(2107), new Guid("95ec682b-074f-42bb-9fed-d4dbde41e41d"), new DateTime(2020, 9, 21, 8, 17, 6, 438, DateTimeKind.Utc).AddTicks(2129) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Longitude",
                schema: "General",
                table: "Address",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Latitude",
                schema: "General",
                table: "Address",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.UpdateData(
                schema: "Company",
                table: "Company",
                keyColumn: "CompanyId",
                keyValue: 1,
                columns: new[] { "CreatedOn", "Guid", "ModifiedOn" },
                values: new object[] { new DateTime(2020, 9, 2, 17, 7, 9, 517, DateTimeKind.Utc).AddTicks(6055), new Guid("3608a083-fbe5-44d5-afef-c7814100aec7"), new DateTime(2020, 9, 2, 17, 7, 9, 517, DateTimeKind.Utc).AddTicks(7971) });

            migrationBuilder.UpdateData(
                schema: "Company",
                table: "Guest",
                keyColumn: "GuestId",
                keyValue: 1,
                columns: new[] { "CreatedOn", "DeletedOn", "ModifiedOn" },
                values: new object[] { new DateTime(2020, 9, 2, 17, 7, 9, 521, DateTimeKind.Utc).AddTicks(9540), new DateTime(2020, 9, 2, 17, 7, 9, 522, DateTimeKind.Utc).AddTicks(6610), new DateTime(2020, 9, 2, 17, 7, 9, 522, DateTimeKind.Utc).AddTicks(2513) });

            migrationBuilder.UpdateData(
                schema: "General",
                table: "Address",
                keyColumn: "AddressId",
                keyValue: 1,
                columns: new[] { "CreatedOn", "Guid", "ModifiedOn" },
                values: new object[] { new DateTime(2020, 9, 2, 17, 7, 9, 514, DateTimeKind.Utc).AddTicks(1418), new Guid("21716a6c-4dee-422b-b2d9-74bf4b12e242"), new DateTime(2020, 9, 2, 17, 7, 9, 514, DateTimeKind.Utc).AddTicks(3939) });

            migrationBuilder.UpdateData(
                schema: "General",
                table: "ContactDetail",
                keyColumn: "ContactDetailId",
                keyValue: 1,
                columns: new[] { "CreatedOn", "Guid", "ModifiedOn" },
                values: new object[] { new DateTime(2020, 9, 2, 17, 7, 9, 516, DateTimeKind.Utc).AddTicks(1150), new Guid("1f5cb3b9-992b-4279-acf7-7f0273abbf03"), new DateTime(2020, 9, 2, 17, 7, 9, 516, DateTimeKind.Utc).AddTicks(3070) });

            migrationBuilder.UpdateData(
                schema: "Settings",
                table: "AddressType",
                keyColumn: "AddressTypeId",
                keyValue: 1,
                columns: new[] { "CreatedOn", "Guid", "ModifiedOn" },
                values: new object[] { new DateTime(2020, 9, 2, 17, 7, 9, 509, DateTimeKind.Utc).AddTicks(8389), new Guid("1e0621b2-b7ea-4d48-8be2-f09980694816"), new DateTime(2020, 9, 2, 17, 7, 9, 510, DateTimeKind.Utc).AddTicks(346) });

            migrationBuilder.UpdateData(
                schema: "Settings",
                table: "AddressType",
                keyColumn: "AddressTypeId",
                keyValue: 2,
                columns: new[] { "CreatedOn", "Guid", "ModifiedOn" },
                values: new object[] { new DateTime(2020, 9, 2, 17, 7, 9, 512, DateTimeKind.Utc).AddTicks(7434), new Guid("95ec682b-074f-42bb-9fed-d4dbde41e41d"), new DateTime(2020, 9, 2, 17, 7, 9, 512, DateTimeKind.Utc).AddTicks(7474) });
        }
    }
}
