using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace xgca.entity.Migrations
{
    public partial class added_column_AccreditedBy_Company : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AddColumn<string>(
                name: "AccreditedBy",
                schema: "Company",
                table: "Company",
                nullable: true);

            migrationBuilder.UpdateData(
                schema: "Company",
                table: "Company",
                keyColumn: "CompanyId",
                keyValue: 1,
                columns: new[] { "CreatedOn", "Guid", "ModifiedOn" },
                values: new object[] { new DateTime(2020, 9, 1, 15, 3, 27, 331, DateTimeKind.Utc).AddTicks(5656), new Guid("3608a083-fbe5-44d5-afef-c7814100aec7"), new DateTime(2020, 9, 1, 15, 3, 27, 331, DateTimeKind.Utc).AddTicks(6575) });

            migrationBuilder.UpdateData(
                schema: "Company",
                table: "Guest",
                keyColumn: "GuestId",
                keyValue: 1,
                columns: new[] { "CreatedOn", "DeletedOn", "ModifiedOn" },
                values: new object[] { new DateTime(2020, 9, 1, 15, 3, 27, 333, DateTimeKind.Utc).AddTicks(2037), new DateTime(2020, 9, 1, 15, 3, 27, 333, DateTimeKind.Utc).AddTicks(4317), new DateTime(2020, 9, 1, 15, 3, 27, 333, DateTimeKind.Utc).AddTicks(2980) });

            migrationBuilder.UpdateData(
                schema: "General",
                table: "Address",
                keyColumn: "AddressId",
                keyValue: 1,
                columns: new[] { "CreatedOn", "Guid", "ModifiedOn" },
                values: new object[] { new DateTime(2020, 9, 1, 15, 3, 27, 329, DateTimeKind.Utc).AddTicks(9336), new Guid("21716a6c-4dee-422b-b2d9-74bf4b12e242"), new DateTime(2020, 9, 1, 15, 3, 27, 330, DateTimeKind.Utc).AddTicks(303) });

            migrationBuilder.UpdateData(
                schema: "General",
                table: "ContactDetail",
                keyColumn: "ContactDetailId",
                keyValue: 1,
                columns: new[] { "CreatedOn", "Guid", "ModifiedOn" },
                values: new object[] { new DateTime(2020, 9, 1, 15, 3, 27, 330, DateTimeKind.Utc).AddTicks(8102), new Guid("1f5cb3b9-992b-4279-acf7-7f0273abbf03"), new DateTime(2020, 9, 1, 15, 3, 27, 330, DateTimeKind.Utc).AddTicks(9082) });

            migrationBuilder.UpdateData(
                schema: "Settings",
                table: "AddressType",
                keyColumn: "AddressTypeId",
                keyValue: 1,
                columns: new[] { "CreatedOn", "Guid", "ModifiedOn" },
                values: new object[] { new DateTime(2020, 9, 1, 15, 3, 27, 327, DateTimeKind.Utc).AddTicks(5880), new Guid("1e0621b2-b7ea-4d48-8be2-f09980694816"), new DateTime(2020, 9, 1, 15, 3, 27, 327, DateTimeKind.Utc).AddTicks(6884) });

            migrationBuilder.UpdateData(
                schema: "Settings",
                table: "AddressType",
                keyColumn: "AddressTypeId",
                keyValue: 2,
                columns: new[] { "CreatedOn", "Guid", "ModifiedOn" },
                values: new object[] { new DateTime(2020, 9, 1, 15, 3, 27, 329, DateTimeKind.Utc).AddTicks(1797), new Guid("95ec682b-074f-42bb-9fed-d4dbde41e41d"), new DateTime(2020, 9, 1, 15, 3, 27, 329, DateTimeKind.Utc).AddTicks(1824) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccreditedBy",
                schema: "Company",
                table: "Company");


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
