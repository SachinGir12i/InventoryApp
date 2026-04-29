using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InventoryApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedDefaultSupplier : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "PartyCategories",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "PartyCategories",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "Parties",
                columns: new[] { "Id", "Address", "City", "CreatedAt", "CreditLimit", "CreditReminderEnabled", "CurrentBalance", "Email", "IsActive", "Name", "OpeningBalance", "PANNumber", "PartyCategoryId", "Phone", "Remarks", "ReminderDaysInterval", "Type", "UpdatedAt", "VATNumber" },
                values: new object[] { 1, null, null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 0m, false, 0m, null, true, "Default Supplier", 0m, null, 2, null, null, null, 2, null, null });

            migrationBuilder.UpdateData(
                table: "PartyCategories",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "IsActive", "Type" },
                values: new object[] { true, 0 });

            migrationBuilder.UpdateData(
                table: "PartyCategories",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "IsActive", "Type" },
                values: new object[] { true, 0 });

            migrationBuilder.UpdateData(
                table: "PartyCategories",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "IsActive", "Type" },
                values: new object[] { true, 0 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Parties",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "PartyCategories");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "PartyCategories");
        }
    }
}
