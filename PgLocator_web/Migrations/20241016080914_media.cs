using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PgLocator_web.Migrations
{
    /// <inheritdoc />
    public partial class media : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileData",
                table: "Media");

            migrationBuilder.RenameColumn(
                name: "Type",
                table: "Media",
                newName: "FilePath");

            migrationBuilder.RenameColumn(
                name: "Pid",
                table: "Media",
                newName: "Pgid");

            migrationBuilder.RenameColumn(
                name: "Mid",
                table: "Media",
                newName: "MediaId");

            migrationBuilder.CreateIndex(
                name: "IX_Media_Pgid",
                table: "Media",
                column: "Pgid");

            migrationBuilder.AddForeignKey(
                name: "FK_Media_Pg_Pgid",
                table: "Media",
                column: "Pgid",
                principalTable: "Pg",
                principalColumn: "Pgid",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Media_Pg_Pgid",
                table: "Media");

            migrationBuilder.DropIndex(
                name: "IX_Media_Pgid",
                table: "Media");

            migrationBuilder.RenameColumn(
                name: "Pgid",
                table: "Media",
                newName: "Pid");

            migrationBuilder.RenameColumn(
                name: "FilePath",
                table: "Media",
                newName: "Type");

            migrationBuilder.RenameColumn(
                name: "MediaId",
                table: "Media",
                newName: "Mid");

            migrationBuilder.AddColumn<byte[]>(
                name: "FileData",
                table: "Media",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);
        }
    }
}
