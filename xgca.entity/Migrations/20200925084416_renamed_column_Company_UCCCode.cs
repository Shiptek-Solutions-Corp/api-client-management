using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace xgca.entity.Migrations
{
    public partial class renamed_column_Company_UCCCode : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UCCCode",
                schema: "Company",
                table: "Company");

            migrationBuilder.AddColumn<string>(
                name: "CUCC",
                schema: "Company",
                table: "Company",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CUCC",
                schema: "Company",
                table: "Guest",
                nullable: true);

            migrationBuilder.UpdateData(
                schema: "Company",
                table: "Company",
                keyColumn: "CompanyId",
                keyValue: 1,
                columns: new[] { "CreatedOn", "Guid", "ModifiedOn" },
                values: new object[] { new DateTime(2020, 9, 25, 8, 44, 15, 186, DateTimeKind.Utc).AddTicks(3639), new Guid("3608a083-fbe5-44d5-afef-c7814100aec7"), new DateTime(2020, 9, 25, 8, 44, 15, 186, DateTimeKind.Utc).AddTicks(4838) });

            migrationBuilder.UpdateData(
                schema: "Company",
                table: "Guest",
                keyColumn: "GuestId",
                keyValue: 1,
                columns: new[] { "CreatedOn", "DeletedOn", "ModifiedOn" },
                values: new object[] { new DateTime(2020, 9, 25, 8, 44, 15, 188, DateTimeKind.Utc).AddTicks(3906), new DateTime(2020, 9, 25, 8, 44, 15, 188, DateTimeKind.Utc).AddTicks(7091), new DateTime(2020, 9, 25, 8, 44, 15, 188, DateTimeKind.Utc).AddTicks(5235) });

            migrationBuilder.UpdateData(
                schema: "General",
                table: "Address",
                keyColumn: "AddressId",
                keyValue: 1,
                columns: new[] { "CreatedOn", "Guid", "ModifiedOn" },
                values: new object[] { new DateTime(2020, 9, 25, 8, 44, 15, 184, DateTimeKind.Utc).AddTicks(1873), new Guid("21716a6c-4dee-422b-b2d9-74bf4b12e242"), new DateTime(2020, 9, 25, 8, 44, 15, 184, DateTimeKind.Utc).AddTicks(3105) });

            migrationBuilder.UpdateData(
                schema: "General",
                table: "ContactDetail",
                keyColumn: "ContactDetailId",
                keyValue: 1,
                columns: new[] { "CreatedOn", "Guid", "ModifiedOn" },
                values: new object[] { new DateTime(2020, 9, 25, 8, 44, 15, 185, DateTimeKind.Utc).AddTicks(3678), new Guid("1f5cb3b9-992b-4279-acf7-7f0273abbf03"), new DateTime(2020, 9, 25, 8, 44, 15, 185, DateTimeKind.Utc).AddTicks(4919) });

            migrationBuilder.UpdateData(
                schema: "Settings",
                table: "AddressType",
                keyColumn: "AddressTypeId",
                keyValue: 1,
                columns: new[] { "CreatedOn", "Guid", "ModifiedOn" },
                values: new object[] { new DateTime(2020, 9, 25, 8, 44, 15, 181, DateTimeKind.Utc).AddTicks(4628), new Guid("1e0621b2-b7ea-4d48-8be2-f09980694816"), new DateTime(2020, 9, 25, 8, 44, 15, 181, DateTimeKind.Utc).AddTicks(5910) });

            migrationBuilder.UpdateData(
                schema: "Settings",
                table: "AddressType",
                keyColumn: "AddressTypeId",
                keyValue: 2,
                columns: new[] { "CreatedOn", "Guid", "ModifiedOn" },
                values: new object[] { new DateTime(2020, 9, 25, 8, 44, 15, 183, DateTimeKind.Utc).AddTicks(2426), new Guid("95ec682b-074f-42bb-9fed-d4dbde41e41d"), new DateTime(2020, 9, 25, 8, 44, 15, 183, DateTimeKind.Utc).AddTicks(2451) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CUCC",
                schema: "Company",
                table: "Guest");

            migrationBuilder.DropColumn(
                name: "CUCC",
                schema: "Company",
                table: "Company");

            migrationBuilder.AddColumn<string>(
                name: "UCCCode",
                schema: "Company",
                table: "Company",
                type: "nvarchar(max)",
                nullable: true);

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
    }
}
