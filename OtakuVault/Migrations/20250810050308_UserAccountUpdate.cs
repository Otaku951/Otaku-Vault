using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OtakuVault.Migrations
{
    /// <inheritdoc />
    public partial class UserAccountUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Balance",
                table: "UserAccount");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Balance",
                table: "UserAccount",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
