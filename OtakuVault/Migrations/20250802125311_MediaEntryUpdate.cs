using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OtakuVault.Migrations
{
    /// <inheritdoc />
    public partial class MediaEntryUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Group",
                table: "MediaEntry",
                newName: "Title");

            migrationBuilder.AddColumn<string>(
                name: "MangaImageFolder",
                table: "MediaEntry",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NovelContent",
                table: "MediaEntry",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VideoUrl",
                table: "MediaEntry",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MangaImageFolder",
                table: "MediaEntry");

            migrationBuilder.DropColumn(
                name: "NovelContent",
                table: "MediaEntry");

            migrationBuilder.DropColumn(
                name: "VideoUrl",
                table: "MediaEntry");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "MediaEntry",
                newName: "Group");
        }
    }
}
