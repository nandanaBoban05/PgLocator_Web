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
                name: "Pid",
                table: "Room",
                newName: "Pgid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Pgid",
                table: "Room",
                newName: "Pid");
        }
    }
}
