using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EVStation_basedRentalSystem.Services.UserAPI.Migrations
{
    /// <inheritdoc />
    public partial class FixUserTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Role",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "HireDate",
                table: "Staffs");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Admins");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Renters",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Renters");

            migrationBuilder.AddColumn<string>(
                name: "Role",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "HireDate",
                table: "Staffs",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Admins",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
