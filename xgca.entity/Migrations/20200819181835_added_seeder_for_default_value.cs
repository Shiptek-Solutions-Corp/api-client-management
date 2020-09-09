using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace xgca.entity.Migrations
{
    public partial class added_seeder_for_default_value : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "Guid",
                schema: "Company",
                table: "CompanyGroupResource",
                nullable: false,
                defaultValue: new Guid("20ab4a8f-876f-4a3d-839e-537ba35cb549"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldDefaultValue: new Guid("be1c8842-5c07-42bc-9b98-dbae67c3cbd8"));

            migrationBuilder.UpdateData(
                schema: "Company",
                table: "Company",
                keyColumn: "CompanyId",
                keyValue: 1,
                columns: new[] { "CreatedOn", "Guid", "ModifiedOn" },
                values: new object[] { new DateTime(2020, 8, 19, 18, 18, 34, 33, DateTimeKind.Utc).AddTicks(4749), new Guid("3608a083-fbe5-44d5-afef-c7814100aec7"), new DateTime(2020, 8, 19, 18, 18, 34, 33, DateTimeKind.Utc).AddTicks(6599) });

            migrationBuilder.UpdateData(
                schema: "Company",
                table: "Guest",
                keyColumn: "GuestId",
                keyValue: 1,
                columns: new[] { "CreatedOn", "DeletedOn", "ModifiedOn" },
                values: new object[] { new DateTime(2020, 8, 19, 18, 18, 34, 36, DateTimeKind.Utc).AddTicks(9122), new DateTime(2020, 8, 19, 18, 18, 34, 37, DateTimeKind.Utc).AddTicks(3836), new DateTime(2020, 8, 19, 18, 18, 34, 37, DateTimeKind.Utc).AddTicks(1138) });

            migrationBuilder.UpdateData(
                schema: "General",
                table: "ContactDetail",
                keyColumn: "ContactDetailId",
                keyValue: 1,
                columns: new[] { "CreatedOn", "Guid", "ModifiedOn" },
                values: new object[] { new DateTime(2020, 8, 19, 18, 18, 34, 31, DateTimeKind.Utc).AddTicks(9550), new Guid("1f5cb3b9-992b-4279-acf7-7f0273abbf03"), new DateTime(2020, 8, 19, 18, 18, 34, 32, DateTimeKind.Utc).AddTicks(1470) });

            migrationBuilder.InsertData(
                schema: "Settings",
                table: "AddressType",
                columns: new[] { "AddressTypeId", "CreatedBy", "CreatedOn", "Guid", "IsDeleted", "ModifiedBy", "ModifiedOn", "Name" },
                values: new object[,]
                {
                    { 1, 0, new DateTime(2020, 8, 19, 18, 18, 34, 25, DateTimeKind.Utc).AddTicks(6593), new Guid("1e0621b2-b7ea-4d48-8be2-f09980694816"), (byte)0, 0, new DateTime(2020, 8, 19, 18, 18, 34, 25, DateTimeKind.Utc).AddTicks(9639), "Company" },
                    { 2, 0, new DateTime(2020, 8, 19, 18, 18, 34, 28, DateTimeKind.Utc).AddTicks(7050), new Guid("95ec682b-074f-42bb-9fed-d4dbde41e41d"), (byte)0, 0, new DateTime(2020, 8, 19, 18, 18, 34, 28, DateTimeKind.Utc).AddTicks(7092), "Residential" }
                });

            migrationBuilder.UpdateData(
                schema: "General",
                table: "Address",
                keyColumn: "AddressId",
                keyValue: 1,
                columns: new[] { "CreatedOn", "Guid", "ModifiedOn" },
                values: new object[] { new DateTime(2020, 8, 19, 18, 18, 34, 30, DateTimeKind.Utc).AddTicks(1956), new Guid("21716a6c-4dee-422b-b2d9-74bf4b12e242"), new DateTime(2020, 8, 19, 18, 18, 34, 30, DateTimeKind.Utc).AddTicks(3814) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "Company",
                table: "Company",
                keyColumn: "CompanyId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                schema: "Settings",
                table: "AddressType",
                keyColumn: "AddressTypeId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                schema: "General",
                table: "Address",
                keyColumn: "AddressId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                schema: "Settings",
                table: "AddressType",
                keyColumn: "AddressTypeId",
                keyValue: 1);

            migrationBuilder.AlterColumn<Guid>(
                name: "Guid",
                schema: "Company",
                table: "CompanyGroupResource",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("be1c8842-5c07-42bc-9b98-dbae67c3cbd8"),
                oldClrType: typeof(Guid),
                oldDefaultValue: new Guid("20ab4a8f-876f-4a3d-839e-537ba35cb549"));

            migrationBuilder.UpdateData(
                schema: "Company",
                table: "Guest",
                keyColumn: "GuestId",
                keyValue: 1,
                columns: new[] { "CreatedOn", "DeletedOn", "ModifiedOn" },
                values: new object[] { new DateTime(2020, 8, 19, 18, 9, 3, 163, DateTimeKind.Utc).AddTicks(6764), new DateTime(2020, 8, 19, 18, 9, 3, 164, DateTimeKind.Utc).AddTicks(4516), new DateTime(2020, 8, 19, 18, 9, 3, 163, DateTimeKind.Utc).AddTicks(9769) });

            migrationBuilder.InsertData(
                schema: "General",
                table: "Address",
                columns: new[] { "AddressId", "AddressLine", "AddressTypeId", "CityId", "CityName", "CountryId", "CountryName", "CreatedBy", "CreatedOn", "FullAddress", "Guid", "IsDeleted", "Latitude", "Longitude", "ModifiedBy", "ModifiedOn", "StateId", "StateName", "ZipCode" },
                values: new object[] { 1, "None", 1, 1, "None", 1, "Global", 0, new DateTime(2020, 8, 19, 18, 9, 3, 153, DateTimeKind.Utc).AddTicks(8035), "None", new Guid("21716a6c-4dee-422b-b2d9-74bf4b12e242"), (byte)1, "None", "None", 0, new DateTime(2020, 8, 19, 18, 9, 3, 154, DateTimeKind.Utc).AddTicks(935), 1, "None", "None" });

            migrationBuilder.UpdateData(
                schema: "General",
                table: "ContactDetail",
                keyColumn: "ContactDetailId",
                keyValue: 1,
                columns: new[] { "CreatedOn", "Guid", "ModifiedOn" },
                values: new object[] { new DateTime(2020, 8, 19, 18, 9, 3, 158, DateTimeKind.Utc).AddTicks(3451), new Guid("1f5cb3b9-992b-4279-acf7-7f0273abbf03"), new DateTime(2020, 8, 19, 18, 9, 3, 158, DateTimeKind.Utc).AddTicks(5208) });

            migrationBuilder.InsertData(
                schema: "Company",
                table: "Company",
                columns: new[] { "CompanyId", "AddressId", "ClientId", "CompanyName", "ContactDetailId", "CreatedBy", "CreatedOn", "EmailAddress", "Guid", "ImageURL", "IsDeleted", "ModifiedBy", "ModifiedOn", "Status", "TaxExemption", "TaxExemptionStatus", "WebsiteURL" },
                values: new object[] { 1, 1, 1, "None", 1, 0, new DateTime(2020, 8, 19, 18, 9, 3, 159, DateTimeKind.Utc).AddTicks(8645), "None", new Guid("3608a083-fbe5-44d5-afef-c7814100aec7"), "None", (byte)1, 0, new DateTime(2020, 8, 19, 18, 9, 3, 160, DateTimeKind.Utc).AddTicks(1472), (byte)0, (byte)0, (byte)0, "None" });
        }
    }
}
