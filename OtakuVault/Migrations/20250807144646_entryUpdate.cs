using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OtakuVault.Migrations
{
    /// <inheritdoc />
    public partial class entryUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<byte[]>(
                name: "ContentData",
                table: "MediaEntry",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<string>(
                name: "ContentType",
                table: "MediaEntry",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContentData",
                table: "MediaEntry");

            migrationBuilder.DropColumn(
                name: "ContentType",
                table: "MediaEntry");

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
    }
}
