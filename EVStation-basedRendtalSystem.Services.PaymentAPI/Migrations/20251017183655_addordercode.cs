using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EVStation_basedRendtalSystem.Services.PaymentAPI.Migrations
{
    /// <inheritdoc />
    public partial class addordercode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "OrderCode",
                table: "Payments",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrderCode",
                table: "Payments");
        }
    }
}
