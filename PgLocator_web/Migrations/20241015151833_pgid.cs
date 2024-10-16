using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PgLocator_web.Migrations
{
    /// <inheritdoc />
    public partial class pgid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Reviewteaxt",
                table: "Review",
                newName: "Reviewtext");

            migrationBuilder.RenameColumn(
                name: "Pid",
                table: "Review",
                newName: "Pgid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Reviewtext",
                table: "Review",
                newName: "Reviewteaxt");

            migrationBuilder.RenameColumn(
                name: "Pgid",
                table: "Review",
                newName: "Pid");
        }
    }
}
