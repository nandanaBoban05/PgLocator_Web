using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PgLocator_web.Migrations
{
    /// <inheritdoc />
    public partial class mediachanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Url",
                table: "Media");

            migrationBuilder.AddColumn<byte[]>(
                name: "FileData",
                table: "Media",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileData",
                table: "Media");

            migrationBuilder.AddColumn<string>(
                name: "Url",
                table: "Media",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
