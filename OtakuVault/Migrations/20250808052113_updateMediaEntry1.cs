using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OtakuVault.Migrations
{
    /// <inheritdoc />
    public partial class updateMediaEntry1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsLocked",
                table: "MediaEntry",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsLocked",
                table: "MediaEntry");
        }
    }
}
